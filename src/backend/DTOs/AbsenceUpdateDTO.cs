using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO để cập nhật thông tin nghỉ dạy
/// </summary>
public class AbsenceUpdateDTO
{
    [MaxLength(20)]
    public string? MaLop { get; set; }
    
    [MaxLength(5)]
    public string? MaGiangVien { get; set; }
    
    [MaxLength(200)]
    public string? LyDo { get; set; }
    
    public DateTime? NgayNghi { get; set; }
    
    [MaxLength(20)]
    public string? TinhTrang { get; set; }
}
