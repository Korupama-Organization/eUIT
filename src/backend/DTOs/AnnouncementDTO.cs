namespace eUIT.API.DTOs;

/// <summary>
/// DTO để trả về thông tin thông báo cho client
/// </summary>
public class AnnouncementDTO
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
    /// Nội dung chi tiết
    /// </summary>
    public string NoiDung { get; set; } = string.Empty;

    /// <summary>
    /// Ngày tạo thông báo
    /// </summary>
    public DateTime NgayTao { get; set; }

    /// <summary>
    /// Ngày cập nhật gần nhất
    /// </summary>
    public DateTime NgayCapNhat { get; set; }
}
