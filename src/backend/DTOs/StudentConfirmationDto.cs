namespace eUIT.API.DTOs;

public class StudentConfirmationDto
{
    public int Id { get; set; }
    public int Mssv { get; set; }
    public string NgonNgu { get; set; } = string.Empty;
    public string LyDo { get; set; } = string.Empty;
    public string? LyDoKhac { get; set; }
    public DateTime NgayDangKy { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
}
