using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

public class AcademicResultDTO
{
    [StringLength(11)]
    public string? HocKy { get; set; }

    [StringLength(8)]
    public string? MaMonHoc { get; set; }

    [StringLength(255)]
    public string? TenMonHoc { get; set; }

    public int? SoTinChi { get; set; }

    public int? TrongSoCuoiKi { get; set; }

    public int? TrongSoQuaTrinh { get; set; }

    public int? TrongSoGiuaKi { get; set; }

    public int? TrongSoThucHanh { get; set; }

    public decimal? DiemQuaTrinh { get; set; }

    public decimal? DiemGiuaKi { get; set; }

    public decimal? DiemThucHanh { get; set; }

    public decimal? DiemCuoiKi { get; set; }

    public decimal? DiemTongKet { get; set; }
}