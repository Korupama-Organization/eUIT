drop FUNCTION IF EXISTS func_total_conduct(INT);


CREATE OR REPLACE FUNCTION func_total_conduct(p_mssv INT)
RETURNS TABLE(
    mssv INT,
    tong_diem_ren_luyen NUMERIC
)
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p_mssv AS mssv,
        COALESCE(SUM(c.diem * c.he_so_tham_gia), 0)::NUMERIC AS tong_diem_ren_luyen
    FROM chi_tiet_hoat_dong_ren_luyen c
    WHERE c.mssv = p_mssv
    GROUP BY p_mssv;
END;
$$ LANGUAGE plpgsql;




drop Function IF EXISTS func_conduct_list(INT);

CREATE OR REPLACE FUNCTION func_conduct_list(p_mssv INT)
RETURNS TABLE(
    ma_hoat_dong INT,
    ten_hoat_dong VARCHAR,
    ma_tieu_chi CHAR(5),
    ten_tieu_chi VARCHAR,
    he_so_tham_gia INT,
    diem INT,
    tong_diem INT,
    ghi_chu VARCHAR
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
    hdr.ma_hoat_dong AS "ma_hoat_dong",
    hdr.ten_hoat_dong AS "ten_hoat_dong",
    hdr.tieu_chi AS "ma_tieu_chi",
    tc.ten_tieu_chi AS "ten_tieu_chi",
    ctr.he_so_tham_gia AS "he_so_tham_gia",
    ctr.diem AS "diem",
    (ctr.he_so_tham_gia * ctr.diem) AS "tong_diem",
    ctr.ghi_chu AS "ghi_chu"
FROM chi_tiet_hoat_dong_ren_luyen ctr
JOIN hoat_dong_ren_luyen hdr 
    ON ctr.ma_hoat_dong = hdr.ma_hoat_dong
JOIN tieu_chi tc
    ON hdr.tieu_chi = tc.tieu_chi
WHERE ctr.mssv = p_mssv;

END;
$$ LANGUAGE plpgsql;
