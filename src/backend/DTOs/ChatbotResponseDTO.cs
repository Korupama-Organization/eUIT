namespace eUIT.API.DTOs;

/// <summary>
/// DTO cho phản hồi từ chatbot
/// </summary>
public class ChatbotResponseDTO
{
    /// <summary>
    /// Tin nhắn phản hồi từ chatbot
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// ID của cuộc trò chuyện
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Loại phản hồi (text, data, action, etc.)
    /// </summary>
    public string ResponseType { get; set; } = "text";

    /// <summary>
    /// Dữ liệu đi kèm (nếu có)
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Các hành động được đề xuất
    /// </summary>
    public List<SuggestedAction>? SuggestedActions { get; set; }

    /// <summary>
    /// Thời gian phản hồi
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Hành động được đề xuất cho người dùng
/// </summary>
public class SuggestedAction
{
    /// <summary>
    /// Tiêu đề của hành động
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Mô tả hành động
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Hành động cần thực hiện (API endpoint hoặc action type)
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Tham số cần thiết cho hành động
    /// </summary>
    public Dictionary<string, object>? Parameters { get; set; }
}