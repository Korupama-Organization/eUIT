using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO đơn giản cho tin nhắn chatbot - chỉ cần message
/// </summary>
public class SimpleChatRequestDTO
{
    /// <summary>
    /// Nội dung tin nhắn từ sinh viên
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn")]
    [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// DTO đơn giản cho phản hồi - chỉ trả về message
/// </summary>
public class SimpleChatResponseDTO
{
    /// <summary>
    /// Tin nhắn phản hồi từ chatbot
    /// </summary>
    public string Message { get; set; } = string.Empty;
}