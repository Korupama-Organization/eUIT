namespace eUIT.API.DTOs;

/// <summary>
/// DTO để nhận tham số tìm kiếm sinh viên
/// Tất cả các trường đều optional để hỗ trợ tìm kiếm linh hoạt
/// </summary>
public class StudentSearchRequestDTO
{
    /// <summary>
    /// Từ khóa tìm kiếm (tìm theo MSSV hoặc họ tên)
    /// </summary>
    public string? Keyword { get; set; }
    
    /// <summary>
    /// Tìm theo mã số sinh viên cụ thể
    /// </summary>
    public int? Mssv { get; set; }
    
    /// <summary>
    /// Tìm theo họ tên (tìm kiếm gần đúng)
    /// </summary>
    public string? HoTen { get; set; }
    
    /// <summary>
    /// Tìm theo lớp sinh hoạt
    /// </summary>
    public string? LopSinhHoat { get; set; }
    
    /// <summary>
    /// Tìm theo ngành học
    /// </summary>
    public string? NganhHoc { get; set; }
    
    /// <summary>
    /// Tìm theo khóa học
    /// </summary>
    public int? KhoaHoc { get; set; }
    
    /// <summary>
    /// Số lượng kết quả trên mỗi trang (mặc định 20)
    /// </summary>
    public int PageSize { get; set; } = 20;
    
    /// <summary>
    /// Số trang hiện tại (mặc định 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;
}
