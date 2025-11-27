namespace eUIT.API.DTOs;

/// <summary>
/// DTO rút gọn để trả về danh sách sinh viên trong kết quả tìm kiếm
/// Chỉ chứa thông tin cơ bản nhất để hiển thị trong danh sách
/// Sử dụng cho endpoint GET /api/GiangVien/students/search
/// </summary>
public class StudentSearchDTO
{
    /// <summary>
    /// Mã số sinh viên
    /// </summary>
    public int Mssv { get; set; }
    
    /// <summary>
    /// Họ tên sinh viên
    /// </summary>
    public string HoTen { get; set; } = string.Empty;
    
    /// <summary>
    /// Lớp sinh hoạt
    /// </summary>
    public string LopSinhHoat { get; set; } = string.Empty;
    
    /// <summary>
    /// Ngành học
    /// </summary>
    public string NganhHoc { get; set; } = string.Empty;
    
    /// <summary>
    /// Khóa học
    /// </summary>
    public int KhoaHoc { get; set; }
    
    /// <summary>
    /// Số điện thoại
    /// </summary>
    public string SoDienThoai { get; set; } = string.Empty;
    
    /// <summary>
    /// Email cá nhân
    /// </summary>
    public string EmailCaNhan { get; set; } = string.Empty;
    
    /// <summary>
    /// Ảnh thẻ URL
    /// </summary>
    public string AnhTheUrl { get; set; } = string.Empty;
}
