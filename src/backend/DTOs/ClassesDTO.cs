using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/// <summary>
/// DTO cho lớp học tiếp theo của sinh viên
/// </summary>
public class NextClassDto
{
    public string MaLop { get; set; } = string.Empty;
    public string TenLop { get; set; } = string.Empty;
    public string Thu { get; set; } = string.Empty;
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public string PhongHoc { get; set; } = string.Empty;
    public DateTime NgayHoc { get; set; }
    public string TenGiangVien { get; set; } = string.Empty;
}

public class FullScheduleDto
{
    public string MaLop { get; set; } = string.Empty;

    public string TenLop { get; set; } = string.Empty;
    public string Thu { get; set; } = string.Empty;

    public int TietBatDau { get; set; }

    public int TietKetThuc { get; set; }

    public string PhongHoc { get; set; } = string.Empty;

    public DateTime NgayHoc { get; set; }

    public string TenGiangVien { get; set; } = string.Empty;
}