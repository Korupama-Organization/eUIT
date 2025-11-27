namespace eUIT.API.Models;

/// <summary>
/// Model cho bảng thong_bao_giang_vien - Quản lý thông báo cá nhân cho giảng viên
/// </summary>
public class Notification
{
    /// <summary>
    /// ID thông báo (Primary Key)
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
    /// ID người nhận (giảng viên)
    /// </summary>
    public int NguoiGuiId { get; set; }
}
