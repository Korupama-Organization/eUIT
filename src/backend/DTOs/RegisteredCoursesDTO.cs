using System.ComponentModel.DataAnnotations;

public class RegisteredCourseDto
{
    public string MaLop { get; set; } = string.Empty;
    public string MaMonHoc { get; set; } = string.Empty;
    public string TenMonHoc { get; set; } = string.Empty;
    public int SoTinChi { get; set; }
    public string MaGiangVien { get; set; } = string.Empty;
}
