using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

/*
Database Function mà DTO này tương ứng:
CREATE OR REPLACE FUNCTION func_get_student_full_transcript_detail(
    p_mssv INT
)
RETURNS TABLE (
    hoc_ky CHAR(11),
    ma_mon_hoc CHAR(8),
    ten_mon_hoc VARCHAR(255),
    so_tin_chi INT,
    trong_so_qua_trinh INT,
    trong_so_giua_ki INT,
    trong_so_thuc_hanh INT,
    trong_so_cuoi_ki INT,
    diem_qua_trinh NUMERIC(4,2),
    diem_giua_ki NUMERIC(4,2),
    diem_thuc_hanh NUMERIC(4,2),
    diem_cuoi_ki NUMERIC(4,2),
    diem_tong_ket NUMERIC(4,2)
)
LANGUAGE sql
AS $$
WITH 
CacLopGocDaHoc AS (
    SELECT DISTINCT ma_lop_goc FROM ket_qua_hoc_tap WHERE mssv = p_mssv
)
SELECT
    tkb.hoc_ky,
    tkb.ma_mon_hoc,
    mh.ten_mon_hoc_vn,
    tkb.so_tin_chi,
    dtk.trong_so_qua_trinh,
    dtk.trong_so_giua_ki,
    dtk.trong_so_thuc_hanh,
    dtk.trong_so_cuoi_ki,
    dtk.diem_qua_trinh,
    dtk.diem_giua_ki,
    dtk.diem_thuc_hanh,
    dtk.diem_cuoi_ki,
    dtk.diem_tong_ket
FROM 
    CacLopGocDaHoc AS clgdh
JOIN thoi_khoa_bieu AS tkb ON clgdh.ma_lop_goc = tkb.ma_lop
JOIN mon_hoc AS mh ON tkb.ma_mon_hoc = mh.ma_mon_hoc
JOIN LATERAL func_get_student_subject_grade(p_mssv, clgdh.ma_lop_goc) AS dtk ON TRUE
ORDER BY tkb.hoc_ky, tkb.ma_mon_hoc;
$$;

*/

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