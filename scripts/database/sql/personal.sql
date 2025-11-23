CREATE TABLE lich_ca_nhan (
    mssv INT NOT NULL,
    ngay DATE NOT NULL,
    noi_dung VARCHAR(255) NOT NULL,
    ghi_chu VARCHAR(255),
    PRIMARY KEY (mssv, ngay)
);

drop Function if exists func_get_personal_schedule(INT);

CREATE OR REPLACE FUNCTION func_get_personal_schedule(p_mssv INT)
RETURNS TABLE(
    ngay DATE,
    noi_dung VARCHAR,
    ghi_chu VARCHAR
)
AS $$
BEGIN
    RETURN QUERY
    SELECT lc.ngay, lc.noi_dung, lc.ghi_chu
    FROM lich_ca_nhan lc
    WHERE lc.mssv = p_mssv
      AND lc.ngay >= CURRENT_DATE
    ORDER BY ngay;
END;
$$ LANGUAGE plpgsql;

