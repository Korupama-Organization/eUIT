using eUIT.API.DTOs;

namespace eUIT.API.Services;

/// <summary>
/// Interface định nghĩa các phương thức cho ChatbotService
/// </summary>
public interface IChatbotService
{
    /// <summary>
    /// Xử lý tin nhắn từ sinh viên và trả về phản hồi
    /// </summary>
    /// <param name="request">Yêu cầu từ sinh viên</param>
    /// <param name="studentId">ID sinh viên</param>
    /// <returns>Phản hồi từ chatbot</returns>
    Task<ChatbotResponseDTO> ProcessMessageAsync(ChatbotRequestDTO request, int studentId);

    /// <summary>
    /// Phân tích ý định từ tin nhắn sử dụng Gemini AI
    /// </summary>
    /// <param name="message">Tin nhắn cần phân tích</param>
    /// <returns>Kết quả phân tích ý định</returns>
    Task<IntentAnalysisDTO> AnalyzeIntentAsync(string message);


}