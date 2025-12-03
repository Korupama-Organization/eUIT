using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO để tạo mới thông tin nghỉ dạy
/// </summary>
public class AbsenceCreateDTO
{
    [Required(ErrorMessage = "Mã lớp là bắt buộc")]
    [MaxLength(20, ErrorMessage = "Mã lớp không được vượt quá 20 ký tự")]
    public string MaLop { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mã giảng viên là bắt buộc")]
    [MaxLength(5, ErrorMessage = "Mã giảng viên không được vượt quá 5 ký tự")]
    public string MaGiangVien { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Lý do nghỉ là bắt buộc")]
    [MaxLength(200, ErrorMessage = "Lý do không được vượt quá 200 ký tự")]
    public string LyDo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ngày nghỉ là bắt buộc")]
    public DateTime NgayNghi { get; set; }
    
    [MaxLength(20, ErrorMessage = "Tình trạng không được vượt quá 20 ký tự")]
    public string TinhTrang { get; set; } = "Chờ duyệt"; // Mặc định
}
