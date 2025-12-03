drop Function IF EXISTS func_get_student_exam_schedule(INT);

CREATE OR REPLACE FUNCTION get_exam_schedule(
    p_mssv INT,
    p_hoc_ky CHAR(11)
)
RETURNS TABLE(
    ma_lop CHAR(20),
    ma_mon_hoc CHAR(8),
    ten_mon_hoc_vn VARCHAR(255),
    ten_giang_vien VARCHAR(100),
    ngay_thi DATE,
    ca_thi INT,
    phong_thi VARCHAR(10),
    ghi_chu VARCHAR(255)
)
LANGUAGE sql
AS $$
    SELECT
        lt.ma_lop,
        lt.ma_mon_hoc,
        mh.ten_mon_hoc_vn,
        gv.ho_ten AS ten_giang_vien,
        lt.ngay_thi,
        lt.ca_thi,
        lt.phong_thi,
        lt.ghi_chu
    FROM ket_qua_hoc_tap kqht
    JOIN lich_thi lt
        ON kqht.ma_lop = lt.ma_lop
    JOIN thoi_khoa_bieu tkb
        ON lt.ma_lop = tkb.ma_lop
       AND lt.ma_giang_vien = tkb.ma_giang_vien
    LEFT JOIN mon_hoc mh
        ON lt.ma_mon_hoc = mh.ma_mon_hoc
    LEFT JOIN giang_vien gv
        ON lt.ma_giang_vien = gv.ma_giang_vien
    WHERE kqht.mssv = p_mssv
      AND tkb.hoc_ky = p_hoc_ky
    ORDER BY lt.ngay_thi, lt.ca_thi;
$$;

select * from get_exam_schedule(23520541, '2025_2026_1')

