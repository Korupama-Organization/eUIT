using eUIT.API.DTOs;
using eUIT.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace eUIT.API.Services;

/// <summary>
/// Service xử lý logic chatbot và tích hợp Gemini AI
/// </summary>
public class ChatbotService : IChatbotService
{
    private readonly eUITDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatbotService> _logger;
    private readonly string _geminiApiKey;
    private readonly string _geminiApiUrl;



    public ChatbotService(
        eUITDbContext context,
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ChatbotService> logger)
    {
        _context = context;
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        
        _geminiApiKey = _configuration["GeminiAI:ApiKey"] ?? throw new InvalidOperationException("Gemini API key not configured");
        _geminiApiUrl = _configuration["GeminiAI:ApiUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
    }

    public async Task<ChatbotResponseDTO> ProcessMessageAsync(ChatbotRequestDTO request, int studentId)
    {
        var conversationId = request.ConversationId ?? Guid.NewGuid().ToString();
        
        try
        {
            // Bước 1: Phân tích ý định từ tin nhắn
            var intentAnalysis = await AnalyzeIntentAsync(request.Message);
            
            // Bước 2: Xử lý dựa trên ý định được nhận diện
            var response = await ProcessIntentAsync(intentAnalysis, studentId, conversationId);

            // Bước 3: Post-process để trả về câu trả lời cô đọng/định dạng thân thiện
            response = PostProcessResponse(intentAnalysis, request.Message, response);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message for student {StudentId}", studentId);
            
            return new ChatbotResponseDTO
            {
                Message = "Xin lỗi, tôi không thể hiểu yêu cầu của bạn. Bạn có thể diễn đạt lại câu hỏi được không?",
                ConversationId = conversationId,
                ResponseType = "error"
            };
        }
    }

    public async Task<IntentAnalysisDTO> AnalyzeIntentAsync(string message)
    {
        try
        {
            _logger.LogInformation("Analyzing intent for message: {Message}", message);
            
            var prompt = BuildIntentAnalysisPrompt(message);
            _logger.LogDebug("Generated prompt for Gemini: {Prompt}", prompt);
            
            var geminiResponse = await CallGeminiApiAsync(prompt);
            _logger.LogDebug("Gemini response: {Response}", geminiResponse);
            
            var result = ParseIntentFromResponse(geminiResponse, message);
            _logger.LogInformation("Intent analysis result: Intent={Intent}, Confidence={Confidence}", 
                result.Intent, result.Confidence);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing intent for message: {Message}", message);
            
            return new IntentAnalysisDTO
            {
                Intent = ChatbotIntent.Unknown.ToString(),
                Confidence = 0.0,
                NormalizedQuery = message,
                Language = "vi"
            };
        }
    }

    private async Task<ChatbotResponseDTO> ProcessIntentAsync(IntentAnalysisDTO intentAnalysis, int studentId, string conversationId)
    {
        _logger.LogInformation("Processing intent: {Intent} with confidence: {Confidence}", 
            intentAnalysis.Intent, intentAnalysis.Confidence);
        
        // Map intent string to enum with case-insensitive parsing
        var intent = MapStringToIntent(intentAnalysis.Intent);
        
        _logger.LogInformation("Mapped to enum: {Intent}", intent);

        return intent switch
        {
            ChatbotIntent.NextClass => await HandleNextClassIntent(studentId, conversationId),
            ChatbotIntent.AcademicResults => await HandleAcademicResultsIntent(studentId, conversationId),
            ChatbotIntent.Gpa => await HandleGpaIntent(studentId, conversationId),
            ChatbotIntent.StudentCard => await HandleStudentCardIntent(studentId, conversationId),
            ChatbotIntent.Greeting => HandleGreetingIntent(conversationId),
            ChatbotIntent.Help => HandleHelpIntent(conversationId),
            ChatbotIntent.Schedule => await HandleScheduleIntent(studentId, conversationId),
            ChatbotIntent.GeneralInquiry => HandleGeneralInquiryIntent(conversationId),
            _ => HandleUnknownIntent(intentAnalysis.NormalizedQuery, conversationId)
        };
    }

    private ChatbotIntent MapStringToIntent(string intentString)
    {
        if (string.IsNullOrEmpty(intentString))
            return ChatbotIntent.Unknown;

        // Normalize intent string
        var normalized = intentString.Trim().ToLowerInvariant();

        return normalized switch
        {
            "nextclass" or "next_class" or "lớp học" => ChatbotIntent.NextClass,
            "academicresults" or "academic_results" or "kết quả học tập" => ChatbotIntent.AcademicResults,
            "gpa" or "điểm gpa" => ChatbotIntent.Gpa,
            "studentcard" or "student_card" or "thẻ sinh viên" => ChatbotIntent.StudentCard,
            "greeting" or "chào hỏi" => ChatbotIntent.Greeting,
            "help" or "trợ giúp" => ChatbotIntent.Help,
            "schedule" or "thời khóa biểu" => ChatbotIntent.Schedule,
            "generalinquiry" or "general_inquiry" or "câu hỏi chung" => ChatbotIntent.GeneralInquiry,
            "fees" or "học phí" => ChatbotIntent.Fees,
            _ => ChatbotIntent.Unknown
        };
    }

    /// <summary>
    /// Post-process response to produce concise, user-friendly messages based on intent analysis and selection rules
    /// </summary>
    private ChatbotResponseDTO PostProcessResponse(IntentAnalysisDTO intentAnalysis, string userMessage, ChatbotResponseDTO response)
    {
        try
        {
            // Only process if we have a classes/next_class type intent
            var normalizedIntent = intentAnalysis.Intent?.ToLowerInvariant() ?? string.Empty;
            if (!(normalizedIntent.Contains("next") || normalizedIntent.Contains("class") || normalizedIntent.Contains("lớp")))
                return response;

            // If Data includes structured fields from DB, prefer those
            if (response.Data != null)
            {
                try
                {
                    // dynamic mapping
                    var dict = response.Data as System.Text.Json.JsonElement?;
                    // If Data is a typed object (e.g., NextClassInfo), use reflection
                    var room = string.Empty;
                    var subject = string.Empty;
                    var time = string.Empty;

                    // Try reflection on typed object
                    var dataObj = response.Data;
                    var type = dataObj.GetType();
                    var roomProp = type.GetProperty("phong_hoc") ?? type.GetProperty("PhongHoc") ?? type.GetProperty("room");
                    var subjectProp = type.GetProperty("ten_mon_hoc_vn") ?? type.GetProperty("TenMonHoc") ?? type.GetProperty("subject") ?? type.GetProperty("ten_mon_hoc");
                    var dateProp = type.GetProperty("ngay_hoc") ?? type.GetProperty("NgayHoc");
                    var startProp = type.GetProperty("tiet_bat_dau");
                    var endProp = type.GetProperty("tiet_ket_thuc");
                    var teacherProp = type.GetProperty("ten_giang_vien") ?? type.GetProperty("TenGiangVien") ?? type.GetProperty("teacher");

                    if (roomProp != null)
                        room = roomProp.GetValue(dataObj)?.ToString() ?? string.Empty;
                    if (subjectProp != null)
                        subject = subjectProp.GetValue(dataObj)?.ToString() ?? string.Empty;
                    if (dateProp != null && startProp != null && endProp != null)
                    {
                        var dateVal = dateProp.GetValue(dataObj);
                        var startVal = startProp.GetValue(dataObj);
                        var endVal = endProp.GetValue(dataObj);
                        time = dateVal is DateTime dt ? $"{dt:dd/MM} (Tiết {startVal}-{endVal})" : (startVal != null ? $"Tiết {startVal}-{endVal}" : string.Empty);
                    }
                    var teacher = teacherProp != null ? teacherProp.GetValue(dataObj)?.ToString() ?? string.Empty : string.Empty;

                    // Determine requested info_type if present
                    var infoType = string.Empty;
                    if (intentAnalysis.Parameters != null && intentAnalysis.Parameters.TryGetValue("info_type", out var it))
                        infoType = it?.ToString()?.ToLowerInvariant() ?? string.Empty;

                    // Selection rules: prefer DB values and tailor format
                    if (!string.IsNullOrEmpty(infoType))
                    {
                        if (infoType.Contains("room"))
                        {
                            var msg = !string.IsNullOrEmpty(room) ? $"Phòng học: {room}." : response.Message.Split('\n').FirstOrDefault() ?? response.Message;
                            response.Message = msg;
                            return response;
                        }
                        if (infoType.Contains("time"))
                        {
                            var msg = !string.IsNullOrEmpty(time) ? $"Thời gian: {time}." : response.Message.Split('\n').FirstOrDefault() ?? response.Message;
                            response.Message = msg;
                            return response;
                        }
                        if (infoType.Contains("subject"))
                        {
                            var msg = !string.IsNullOrEmpty(subject) ? $"Môn học: {subject}." : response.Message.Split('\n').FirstOrDefault() ?? response.Message;
                            response.Message = msg;
                            return response;
                        }
                        if (infoType.Contains("teacher"))
                        {
                            var msg = !string.IsNullOrEmpty(teacher) ? $"Giảng viên: {teacher}." : response.Message.Split('\n').FirstOrDefault() ?? response.Message;
                            response.Message = msg;
                            return response;
                        }
                    }

                    // Default: return concise one-line summary if possible
                    if (!string.IsNullOrEmpty(subject) || !string.IsNullOrEmpty(room) || !string.IsNullOrEmpty(time))
                    {
                        var parts = new List<string>();
                        if (!string.IsNullOrEmpty(subject)) parts.Add(subject);
                        if (!string.IsNullOrEmpty(time)) parts.Add(time);
                        if (!string.IsNullOrEmpty(room)) parts.Add($"Phòng {room}");
                        response.Message = string.Join(" — ", parts) + ".";
                        return response;
                    }
                }
                catch { /* ignore reflection errors and fallback to original message */ }
            }

            // If no structured data, attempt to pick first sentence from response.Message
            var first = response.Message?.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            if (!string.IsNullOrEmpty(first))
            {
                response.Message = first.Trim() + ".";
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PostProcessResponse");
            return response;
        }
    }

    private async Task<ChatbotResponseDTO> HandleNextClassIntent(int studentId, string conversationId)
    {
        try
        {
            var nextClassResult = await _context.Database.SqlQuery<NextClassInfo>(
                $"SELECT * FROM func_get_next_class({studentId})")
                .FirstOrDefaultAsync();

            if (nextClassResult == null)
            {
                return new ChatbotResponseDTO
                {
                    Message = "Hiện tại bạn không có lớp học nào sắp tới trong thời gian gần nhất.",
                    ConversationId = conversationId,
                    ResponseType = "info"
                };
            }

            var message = $"🕐 **Lớp học tiếp theo của bạn:**\n\n" +
                         $"📚 **Môn học:** {nextClassResult.ten_mon_hoc_vn}\n" +
                         $"🏷️ **Mã lớp:** {nextClassResult.ma_lop}\n" +
                         $"📅 **Ngày học:** {nextClassResult.ngay_hoc:dd/MM/yyyy} ({nextClassResult.thu})\n" +
                         $"⏰ **Tiết:** {nextClassResult.tiet_bat_dau} - {nextClassResult.tiet_ket_thuc}\n" +
                         $"🏢 **Phòng:** {nextClassResult.phong_hoc}\n" +
                         $"👨‍🏫 **Giảng viên:** {nextClassResult.ten_giang_vien}";

            return new ChatbotResponseDTO
            {
                Message = message,
                ConversationId = conversationId,
                ResponseType = "data",
                Data = nextClassResult,
                SuggestedActions = new List<SuggestedAction>
                {
                    new() { Title = "Xem thời khóa biểu", Action = "view_schedule", Description = "Xem toàn bộ thời khóa biểu" },
                    new() { Title = "Xem phòng học", Action = "view_room", Description = "Thông tin chi tiết phòng học" }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting next class for student {StudentId}", studentId);
            throw;
        }
    }

    private async Task<ChatbotResponseDTO> HandleGpaIntent(int studentId, string conversationId)
    {
        try
        {
            var gpaResult = await _context.Database.SqlQuery<QuickGpa>(
                $"SELECT * FROM func_calculate_gpa({studentId})")
                .FirstOrDefaultAsync();

            if (gpaResult == null)
            {
                return new ChatbotResponseDTO
                {
                    Message = "Không tìm thấy thông tin điểm GPA của bạn.",
                    ConversationId = conversationId,
                    ResponseType = "info"
                };
            }

            var message = $"📊 **Thông tin GPA của bạn:**\n\n" +
                         $"🎯 **GPA hiện tại:** {gpaResult.gpa:F2}\n" +
                         $"📚 **Số tín chỉ tích lũy:** {gpaResult.so_tin_chi_tich_luy}\n\n";

            // Đánh giá GPA
            var evaluation = gpaResult.gpa switch
            {
                >= 3.6f => "🌟 Xuất sắc! Hãy tiếp tục duy trì!",
                >= 3.2f => "👏 Khá tốt! Bạn đang trên đường thành công!",
                >= 2.5f => "💪 Trung bình! Hãy cố gắng hơn nữa!",
                _ => "⚠️ Cần cải thiện! Hãy tham khảo học vụ để có kế hoạch học tập phù hợp."
            };

            message += evaluation;

            return new ChatbotResponseDTO
            {
                Message = message,
                ConversationId = conversationId,
                ResponseType = "data",
                Data = gpaResult,
                SuggestedActions = new List<SuggestedAction>
                {
                    new() { Title = "Xem kết quả chi tiết", Action = "view_academic_results", Description = "Xem kết quả học tập từng môn" },
                    new() { Title = "Lời khuyên học tập", Action = "study_advice", Description = "Nhận lời khuyên cải thiện kết quả học tập" }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting GPA for student {StudentId}", studentId);
            throw;
        }
    }

    private async Task<ChatbotResponseDTO> HandleAcademicResultsIntent(int studentId, string conversationId)
    {
        try
        {
            var results = await _context.Database.SqlQuery<AcademicResultQueryResult>(
                $"SELECT * FROM chi_tiet_ket_qua_hoc_tap({studentId})")
                .ToListAsync();

            if (results == null || results.Count == 0)
            {
                return new ChatbotResponseDTO
                {
                    Message = "Không tìm thấy kết quả học tập của bạn.",
                    ConversationId = conversationId,
                    ResponseType = "info"
                };
            }

            // Nhóm theo học kỳ
            var groupedResults = results.GroupBy(r => r.hoc_ky).OrderByDescending(g => g.Key);
            
            var message = new StringBuilder("📋 **Kết quả học tập của bạn:**\n\n");
            
            foreach (var semester in groupedResults.Take(2)) // Chỉ hiển thị 2 học kỳ gần nhất
            {
                message.AppendLine($"**🗓️ {semester.Key}**");
                foreach (var subject in semester.Take(5)) // Tối đa 5 môn mỗi kỳ
                {
                    var grade = subject.diem_tong_ket?.ToString("F1") ?? "Chưa có";
                    message.AppendLine($"• {subject.ten_mon_hoc}: **{grade}**");
                }
                message.AppendLine();
            }

            if (results.Count > 10)
            {
                message.AppendLine("_Để xem đầy đủ kết quả, vui lòng sử dụng tính năng 'Xem chi tiết'_");
            }

            return new ChatbotResponseDTO
            {
                Message = message.ToString(),
                ConversationId = conversationId,
                ResponseType = "data",
                Data = results,
                SuggestedActions = new List<SuggestedAction>
                {
                    new() { Title = "Xem tất cả kết quả", Action = "view_all_results", Description = "Xem toàn bộ kết quả học tập" },
                    new() { Title = "Thống kê theo học kỳ", Action = "semester_stats", Description = "Phân tích kết quả theo từng học kỳ" }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting academic results for student {StudentId}", studentId);
            throw;
        }
    }

    private async Task<ChatbotResponseDTO> HandleStudentCardIntent(int studentId, string conversationId)
    {
        try
        {
            var studentInfo = await _context.Database.SqlQuery<CardInfoResult>(
                $"SELECT * FROM func_get_student_card_info({studentId})")
                .FirstOrDefaultAsync();

            if (studentInfo == null)
            {
                return new ChatbotResponseDTO
                {
                    Message = "Không tìm thấy thông tin thẻ sinh viên của bạn.",
                    ConversationId = conversationId,
                    ResponseType = "info"
                };
            }

            var message = $"🎓 **Thông tin thẻ sinh viên:**\n\n" +
                         $"👤 **Họ tên:** {studentInfo.ho_ten}\n" +
                         $"🆔 **MSSV:** {studentInfo.mssv}\n" +
                         $"📅 **Khóa học:** {studentInfo.khoa_hoc}\n" +
                         $"🎯 **Ngành học:** {studentInfo.nganh_hoc}";

            return new ChatbotResponseDTO
            {
                Message = message,
                ConversationId = conversationId,
                ResponseType = "data",
                Data = studentInfo,
                SuggestedActions = new List<SuggestedAction>
                {
                    new() { Title = "Cập nhật thông tin", Action = "update_info", Description = "Cập nhật thông tin cá nhân" },
                    new() { Title = "Tải ảnh thẻ", Action = "download_card", Description = "Tải ảnh thẻ sinh viên" }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student card info for student {StudentId}", studentId);
            throw;
        }
    }

    private ChatbotResponseDTO HandleGreetingIntent(string conversationId)
    {
        var greetings = new[]
        {
            "Xin chào! Tôi là trợ lý ảo của eUIT. Tôi có thể giúp bạn tra cứu thông tin học tập, lịch học, và nhiều thông tin khác. Bạn cần hỗ trợ gì?",
            "Chào bạn! Tôi ở đây để hỗ trợ bạn với các thông tin học tập. Hãy hỏi tôi bất cứ điều gì bạn muốn biết!",
            "Hello! Tôi có thể giúp bạn kiểm tra điểm số, lịch học, hay bất kỳ thông tin nào khác. Bạn muốn biết điều gì?"
        };

        var random = new Random();
        var message = greetings[random.Next(greetings.Length)];

        return new ChatbotResponseDTO
        {
            Message = message,
            ConversationId = conversationId,
            ResponseType = "greeting",
            SuggestedActions = new List<SuggestedAction>
            {
                new() { Title = "Lớp học tiếp theo", Action = "next_class", Description = "Kiểm tra lớp học sắp tới" },
                new() { Title = "Điểm GPA", Action = "check_gpa", Description = "Xem điểm trung bình tích lũy" },
                new() { Title = "Kết quả học tập", Action = "academic_results", Description = "Xem kết quả học tập" }
            }
        };
    }

    private ChatbotResponseDTO HandleHelpIntent(string conversationId)
    {
        var message = "🤖 **Tôi có thể giúp bạn:**\n\n" +
                     "📅 **Lịch học:** Kiểm tra lớp học tiếp theo, thời khóa biểu\n" +
                     "📊 **Kết quả học tập:** Xem GPA, điểm từng môn, kết quả học tập\n" +
                     "🎓 **Thông tin sinh viên:** Thông tin thẻ sinh viên, thông tin cá nhân\n" +
                     "💰 **Học phí:** Tra cứu học phí, lịch thanh toán\n" +
                     "❓ **Hướng dẫn:** Hướng dẫn các thủ tục, quy định\n\n" +
                     "Bạn chỉ cần hỏi bằng ngôn ngữ tự nhiên, tôi sẽ hiểu và trả lời!";

        return new ChatbotResponseDTO
        {
            Message = message,
            ConversationId = conversationId,
            ResponseType = "help",
            SuggestedActions = new List<SuggestedAction>
            {
                new() { Title = "Ví dụ câu hỏi", Action = "example_questions", Description = "Xem các câu hỏi mẫu" },
                new() { Title = "Liên hệ hỗ trợ", Action = "contact_support", Description = "Thông tin liên hệ khi cần hỗ trợ" }
            }
        };
    }

    private Task<ChatbotResponseDTO> HandleScheduleIntent(int studentId, string conversationId)
    {
        // Tạm thời trả về message thông báo chưa implement
        return Task.FromResult(new ChatbotResponseDTO
        {
            Message = "📅 Tính năng xem thời khóa biểu đang được phát triển. Hiện tại bạn có thể hỏi về lớp học tiếp theo bằng cách nói: 'Lớp học tiếp theo là gì?'",
            ConversationId = conversationId,
            ResponseType = "info",
            SuggestedActions = new List<SuggestedAction>
            {
                new() { Title = "Lớp học tiếp theo", Action = "next_class", Description = "Kiểm tra lớp học sắp tới" }
            }
        });
    }

    private ChatbotResponseDTO HandleGeneralInquiryIntent(string conversationId)
    {
        return new ChatbotResponseDTO
        {
            Message = "❓ **Tôi có thể hướng dẫn bạn về:**\n\n" +
                     "📝 **Đăng ký môn học:** Liên hệ phòng đào tạo hoặc sử dụng hệ thống online\n" +
                     "📊 **Quy định thi cử:** Kiểm tra quy chế đào tạo của trường\n" +
                     "📋 **Thủ tục học vụ:** Liên hệ phòng đào tạo để được hướng dẫn chi tiết\n" +
                     "📞 **Liên hệ:** Phòng đào tạo - Tòa nhà A, tầng 2\n\n" +
                     "Bạn có câu hỏi cụ thể nào khác không?",
            ConversationId = conversationId,
            ResponseType = "info",
            SuggestedActions = new List<SuggestedAction>
            {
                new() { Title = "Thông tin liên hệ", Action = "contact_info", Description = "Xem thông tin liên hệ các phòng ban" },
                new() { Title = "Quy định học vụ", Action = "regulations", Description = "Xem các quy định về học vụ" }
            }
        };
    }

    private ChatbotResponseDTO HandleUnknownIntent(string originalMessage, string conversationId)
    {
        var message = "🤔 Tôi chưa hiểu rõ yêu cầu của bạn. Bạn có thể:\n\n" +
                     "• Diễn đạt lại câu hỏi một cách khác\n" +
                     "• Sử dụng các từ khóa như: 'lịch học', 'điểm số', 'GPA', 'kết quả học tập'\n" +
                     "• Hỏi trực tiếp: 'Lớp học tiếp theo là gì?', 'GPA của tôi bao nhiêu?'\n\n" +
                     "Hoặc bạn có thể chọn một trong các gợi ý bên dưới.";

        return new ChatbotResponseDTO
        {
            Message = message,
            ConversationId = conversationId,
            ResponseType = "clarification",
            SuggestedActions = new List<SuggestedAction>
            {
                new() { Title = "Lớp học tiếp theo", Action = "next_class", Description = "Kiểm tra lớp học sắp tới" },
                new() { Title = "Điểm GPA", Action = "check_gpa", Description = "Xem điểm trung bình tích lũy" },
                new() { Title = "Trợ giúp", Action = "help", Description = "Xem hướng dẫn sử dụng" }
            }
        };
    }

    private string BuildIntentAnalysisPrompt(string message)
    {
        return $@"
Bạn là một AI assistant chuyên phân tích ý định của sinh viên trong hệ thống quản lý đào tạo eUIT.

Nhiệm vụ: Phân tích câu hỏi của sinh viên và xác định ý định chính.

Các ý định có thể nhận diện với ví dụ cụ thể:

1. NextClass: Hỏi về lớp học tiếp theo, giờ học sắp tới
   Ví dụ: ""Lớp học tiếp theo là gì?"", ""Hôm nay tôi có học không?"", ""Buổi học sắp tới khi nào?""

2. AcademicResults: Hỏi về kết quả học tập, điểm số các môn  
   Ví dụ: ""Kết quả học tập của tôi"", ""Xem điểm các môn"", ""Bảng điểm học kỳ""

3. Gpa: Hỏi về GPA, điểm trung bình tích lũy
   Ví dụ: ""GPA của tôi là bao nhiêu?"", ""Điểm trung bình"", ""Điểm tích lũy hiện tại""

4. StudentCard: Hỏi về thông tin cá nhân, thẻ sinh viên
   Ví dụ: ""Thông tin sinh viên"", ""Thẻ sinh viên của tôi"", ""MSSV của tôi""

5. Schedule: Hỏi về thời khóa biểu tổng thể  
   Ví dụ: ""Thời khóa biểu tuần này"", ""Lịch học cả tuần"", ""Xem lịch học""

6. Fees: Hỏi về học phí, công nợ
   Ví dụ: ""Học phí kỳ này"", ""Tiền học"", ""Công nợ học phí""

7. GeneralInquiry: Câu hỏi chung về quy định, thủ tục
   Ví dụ: ""Cách đăng ký môn học"", ""Quy định thi cử"", ""Thủ tục xin nghỉ học""

8. Help: Yêu cầu trợ giúp, hướng dẫn
   Ví dụ: ""Trợ giúp"", ""Hướng dẫn"", ""Tôi cần hỗ trợ""

9. Greeting: Chào hỏi
   Ví dụ: ""Xin chào"", ""Hello"", ""Chào bạn"", ""Hi""

10. Unknown: Không xác định được ý định hoặc câu hỏi không liên quan

Câu hỏi của sinh viên: ""{message}""

Hãy phân tích và trả về JSON theo format chính xác sau:
{{
  ""intent"": ""<tên_ý_định_từ_danh_sách_trên>"",
  ""confidence"": <số_từ_0.0_đến_1.0>,
  ""parameters"": {{}},
  ""normalized_query"": ""<câu_hỏi_đã_được_chuẩn_hóa>""
}}

Chỉ trả về JSON, không có text khác.
";
    }

    private async Task<string> CallGeminiApiAsync(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_geminiApiUrl}?key={_geminiApiKey}")
        {
            Content = content
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Parse Gemini response to extract text
        var geminiResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
        var text = geminiResponse
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "";
    }

    private IntentAnalysisDTO ParseIntentFromResponse(string geminiResponse, string originalMessage)
    {
        try
        {
            _logger.LogDebug("Parsing Gemini response: {Response}", geminiResponse);
            
            // Xử lý trường hợp Gemini trả về text có chứa JSON
            var cleanedResponse = geminiResponse.Trim();
            if (cleanedResponse.StartsWith("```json"))
            {
                var startIndex = cleanedResponse.IndexOf("{");
                var endIndex = cleanedResponse.LastIndexOf("}") + 1;
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    cleanedResponse = cleanedResponse.Substring(startIndex, endIndex - startIndex);
                }
            }
            
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
            
            var intent = jsonResponse.TryGetProperty("intent", out var intentProp) 
                ? intentProp.GetString() ?? "Unknown" 
                : "Unknown";
                
            var confidence = jsonResponse.TryGetProperty("confidence", out var confProp) 
                ? confProp.GetDouble() 
                : 0.0;
                
            var normalizedQuery = jsonResponse.TryGetProperty("normalized_query", out var queryProp) 
                ? queryProp.GetString() ?? originalMessage 
                : originalMessage;

            // Fallback intent recognition nếu Gemini không nhận diện được
            if (intent == "Unknown" || confidence < 0.3)
            {
                intent = FallbackIntentDetection(originalMessage);
                confidence = 0.5;
            }
            
            return new IntentAnalysisDTO
            {
                Intent = intent,
                Confidence = confidence,
                Parameters = new Dictionary<string, object>(),
                NormalizedQuery = normalizedQuery,
                Language = "vi"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing Gemini response: {Response}", geminiResponse);
            
            // Fallback khi parse JSON thất bại
            var fallbackIntent = FallbackIntentDetection(originalMessage);
            
            return new IntentAnalysisDTO
            {
                Intent = fallbackIntent,
                Confidence = 0.5,
                Parameters = new Dictionary<string, object>(),
                NormalizedQuery = originalMessage,
                Language = "vi"
            };
        }
    }

    private string FallbackIntentDetection(string message)
    {
        var lowerMessage = message.ToLower();
        
        // Simple keyword-based intent detection
        if (lowerMessage.Contains("lớp") || lowerMessage.Contains("học tiếp theo") || lowerMessage.Contains("hôm nay"))
            return "NextClass";
            
        if (lowerMessage.Contains("gpa") || lowerMessage.Contains("điểm trung bình") || lowerMessage.Contains("tích lũy"))
            return "Gpa";
            
        if (lowerMessage.Contains("kết quả") || lowerMessage.Contains("điểm") || lowerMessage.Contains("bảng điểm"))
            return "AcademicResults";
            
        if (lowerMessage.Contains("thẻ sinh viên") || lowerMessage.Contains("thông tin") || lowerMessage.Contains("mssv"))
            return "StudentCard";
            
        if (lowerMessage.Contains("xin chào") || lowerMessage.Contains("hello") || lowerMessage.Contains("chào"))
            return "Greeting";
            
        if (lowerMessage.Contains("trợ giúp") || lowerMessage.Contains("hướng dẫn") || lowerMessage.Contains("help"))
            return "Help";
            
        return "Unknown";
    }





    // Inner classes để map với database functions (copy từ StudentController)
    private class NextClassInfo
    {
        public string ma_lop { get; set; } = string.Empty;
        public string ten_mon_hoc_vn { get; set; } = string.Empty;
        public string thu { get; set; } = string.Empty;
        public int tiet_bat_dau { get; set; }
        public int tiet_ket_thuc { get; set; }
        public string phong_hoc { get; set; } = string.Empty;
        public DateTime ngay_hoc { get; set; }
        public string ten_giang_vien { get; set; } = string.Empty;
    }

    private class QuickGpa
    {
        public float gpa { get; set; }
        public int so_tin_chi_tich_luy { get; set; }
    }

    private class AcademicResultQueryResult
    {
        public string? hoc_ky { get; set; }
        public string? ma_mon_hoc { get; set; }
        public string? ten_mon_hoc { get; set; }
        public int? so_tin_chi { get; set; }
        public int? trong_so_qua_trinh { get; set; }
        public int? trong_so_giua_ki { get; set; }
        public int? trong_so_thuc_hanh { get; set; }
        public int? trong_so_cuoi_ki { get; set; }
        public decimal? diem_qua_trinh { get; set; }
        public decimal? diem_giua_ki { get; set; }
        public decimal? diem_thuc_hanh { get; set; }
        public decimal? diem_cuoi_ki { get; set; }
        public decimal? diem_tong_ket { get; set; }
    }

    private class CardInfoResult
    {
        public int mssv { get; set; }
        public string ho_ten { get; set; } = string.Empty;
        public int khoa_hoc { get; set; }
        public string nganh_hoc { get; set; } = string.Empty;
        public string? anh_the_url { get; set; }
    }
}