using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace eUIT.API.DTOs;

public class ConductTotalDTO
{
    public int Mssv { get; set; }

    [Column("tong_diem_ren_luyen")]
    public int TongDiemRenLuyen { get; set; }
}

[Keyless]

public class ConductDetailDTO
{
    public int MaHoatDong { get; set; }
    public string TenHoatDong { get; set; } = string.Empty;
    public string MaTieuChi { get; set; } = string.Empty;
    public string TenTieuChi { get; set; } = string.Empty;
    public int HeSoThamGia { get; set; }
    public int Diem { get; set; }
    public int TongDiem { get; set; }
    public string? GhiChu { get; set; }
}
