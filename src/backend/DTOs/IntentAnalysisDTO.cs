namespace eUIT.API.DTOs;

/// <summary>
/// DTO cho kết quả phân tích ý định từ Gemini AI
/// </summary>
public class IntentAnalysisDTO
{
    /// <summary>
    /// Ý định được nhận diện (next_class, academic_results, gpa, student_info, general_inquiry, etc.)
    /// </summary>
    public string Intent { get; set; } = string.Empty;

    /// <summary>
    /// Độ tin cậy của việc nhận diện ý định (0.0 - 1.0)
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Các thông số được trích xuất từ câu hỏi
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Câu hỏi gốc đã được chuẩn hóa
    /// </summary>
    public string NormalizedQuery { get; set; } = string.Empty;

    /// <summary>
    /// Ngôn ngữ được phát hiện
    /// </summary>
    public string Language { get; set; } = "vi";
}

/// <summary>
/// Enum định nghĩa các ý định có thể nhận diện
/// </summary>
public enum ChatbotIntent
{
    NextClass,          // Hỏi về lớp học tiếp theo
    AcademicResults,    // Hỏi về kết quả học tập
    Gpa,               // Hỏi về điểm GPA
    StudentCard,       // Hỏi thông tin thẻ sinh viên
    Schedule,          // Hỏi về thời khóa biểu
    Fees,              // Hỏi về học phí
    GeneralInquiry,    // Câu hỏi chung
    Help,              // Yêu cầu trợ giúp
    Greeting,          // Chào hỏi
    Unknown            // Không xác định được ý định
}