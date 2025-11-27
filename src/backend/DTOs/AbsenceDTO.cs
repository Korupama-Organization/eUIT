namespace eUIT.API.DTOs;

/// <summary>
/// DTO để trả về thông tin nghỉ dạy cho client
/// </summary>
public class AbsenceDTO
{
    public int Id { get; set; }
    
    public string MaLop { get; set; } = string.Empty;
    
    public string MaGiangVien { get; set; } = string.Empty;
    
    public string LyDo { get; set; } = string.Empty;
    
    public DateTime NgayNghi { get; set; }
    
    public string TinhTrang { get; set; } = string.Empty;
}
