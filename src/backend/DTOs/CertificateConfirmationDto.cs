namespace eUIT.API.DTOs;

public class CertificateConfirmationDto
{
    public int Mssv { get; set; }
    public string MaChungChi { get; set; } = string.Empty;
    public string LoaiChungChi { get; set; } = string.Empty;
    public DateTime NgayThi { get; set; }
    public string TinhTrang { get; set; } = string.Empty;
    public DateTime NgayDangKy { get; set; }
    public string? GhiChu { get; set; }
}
