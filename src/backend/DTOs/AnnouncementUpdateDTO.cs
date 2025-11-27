using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO để cập nhật thông báo
/// Tất cả các trường đều optional (nullable) để cho phép cập nhật từng phần
/// </summary>
public class AnnouncementUpdateDTO
{
    /// <summary>
    /// Tiêu đề mới (optional, tối đa 100 ký tự)
    /// </summary>
    [MaxLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
    public string? TieuDe { get; set; }

    /// <summary>
    /// Nội dung mới (optional)
    /// </summary>
    public string? NoiDung { get; set; }

    /// <summary>
    /// Ngày tạo mới (optional - thường không cập nhật)
    /// </summary>
    public DateTime? NgayTao { get; set; }

    /// <summary>
    /// Ngày cập nhật mới (optional - tự động set khi cập nhật)
    /// </summary>
    public DateTime? NgayCapNhat { get; set; }
}
