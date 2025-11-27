namespace eUIT.API.DTOs;

/// <summary>
/// DTO để trả về thông tin thông báo cho giảng viên
/// </summary>
public class NotificationDTO
{
    /// <summary>
    /// ID thông báo
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tiêu đề thông báo
    /// </summary>
    public string TieuDe { get; set; } = string.Empty;

    /// <summary>
    /// Nội dung chi tiết thông báo
    /// </summary>
    public string NoiDung { get; set; } = string.Empty;

    /// <summary>
    /// Ngày gửi thông báo
    /// </summary>
    public DateTime NgayGui { get; set; }

    /// <summary>
    /// ID người nhận
    /// </summary>
    public int NguoiGuiId { get; set; }
}
