using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO để tạo mới thông báo
/// </summary>
public class AnnouncementCreateDTO
{
    /// <summary>
    /// Tiêu đề thông báo (bắt buộc, tối đa 100 ký tự)
    /// </summary>
    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MaxLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
    public string TieuDe { get; set; } = string.Empty;

    /// <summary>
    /// Nội dung chi tiết (bắt buộc)
    /// </summary>
    [Required(ErrorMessage = "Nội dung là bắt buộc")]
    public string NoiDung { get; set; } = string.Empty;

    /// <summary>
    /// Ngày tạo (mặc định là ngày hiện tại nếu không cung cấp)
    /// </summary>
    public DateTime? NgayTao { get; set; }

    /// <summary>
    /// Ngày cập nhật (mặc định là ngày hiện tại nếu không cung cấp)
    /// </summary>
    public DateTime? NgayCapNhat { get; set; }
}
