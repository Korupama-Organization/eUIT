namespace eUIT.API.Models;

/// <summary>
/// Model cho bảng thong_bao - Quản lý thông báo cho giảng viên
/// </summary>
public class Announcement
{
    /// <summary>
    /// ID tự động tăng (Primary Key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tiêu đề thông báo (character varying 100)
    /// </summary>
    public string TieuDe { get; set; } = string.Empty;

    /// <summary>
    /// Nội dung chi tiết thông báo (text)
    /// </summary>
    public string NoiDung { get; set; } = string.Empty;

    /// <summary>
    /// Ngày tạo thông báo (date)
    /// </summary>
    public DateTime NgayTao { get; set; }

    /// <summary>
    /// Ngày cập nhật gần nhất (date)
    /// </summary>
    public DateTime NgayCapNhat { get; set; }
}
