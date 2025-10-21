using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO cho yêu cầu gửi đến chatbot từ sinh viên
/// </summary>
public class ChatbotRequestDTO
{
    /// <summary>
    /// Nội dung câu hỏi hoặc yêu cầu từ sinh viên
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn")]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// ID của cuộc trò chuyện (tùy chọn - sẽ tự động tạo nếu không có)
    /// </summary>
    public string? ConversationId { get; set; }
}