CREATE OR REPLACE FUNCTION func_get_student_profile(p_mssv INT)
RETURNS TABLE
(
    mssv INT,
    ho_ten VARCHAR,
    ngay_sinh DATE,
    nganh_hoc VARCHAR,
    khoa_hoc INT,
    lop_sinh_hoat CHAR(10),

    noi_sinh VARCHAR,
    cccd CHAR(12),
    ngay_cap_cccd DATE,
    noi_cap_cccd VARCHAR,
    dan_toc VARCHAR,
    ton_giao VARCHAR,
    so_dien_thoai CHAR(10),
    dia_chi_thuong_tru VARCHAR,
    tinh_thanh_pho VARCHAR,
    phuong_xa TEXT,
    qua_trinh_hoc_tap_cong_tac VARCHAR,
    thanh_tich VARCHAR,
    email_ca_nhan VARCHAR,

    ma_ngan_hang CHAR(4),
    ten_ngan_hang VARCHAR,
    so_tai_khoan VARCHAR,
    chi_nhanh VARCHAR,

    ho_ten_cha VARCHAR,
    quoc_tich_cha VARCHAR,
    dan_toc_cha VARCHAR,
    ton_giao_cha VARCHAR,
    sdt_cha CHAR(10),
    email_cha VARCHAR, 
    dia_chi_thuong_tru_cha VARCHAR,
    cong_viec_cha VARCHAR,
    
    ho_ten_me VARCHAR,
    quoc_tich_me VARCHAR,
    dan_toc_me VARCHAR,
    ton_giao_me VARCHAR,
    sdt_me CHAR(10),
    email_me VARCHAR, 
    dia_chi_thuong_tru_me VARCHAR,
    cong_viec_me VARCHAR,

    ho_ten_ngh VARCHAR,
    quoc_tich_ngh VARCHAR,
    dan_toc_ngh VARCHAR,
    ton_giao_ngh VARCHAR,
    sdt_ngh CHAR(10),
    email_ngh VARCHAR, 
    dia_chi_thuong_tru_ngh VARCHAR,
    cong_viec_ngh VARCHAR,

    thong_tin_nguoi_can_bao_tin VARCHAR,
    so_dien_thoai_bao_tin CHAR(10),

    anh_the_url VARCHAR
)
LANGUAGE sql
AS $$
SELECT
    mssv,
    ho_ten,
    ngay_sinh,
    nganh_hoc,
    khoa_hoc,
    lop_sinh_hoat,

    noi_sinh,
    cccd,
    ngay_cap_cccd,
    noi_cap_cccd,
    dan_toc,
    ton_giao,
    so_dien_thoai,
    dia_chi_thuong_tru,
    tinh_thanh_pho,
    phuong_xa,
    qua_trinh_hoc_tap_cong_tac,
    thanh_tich,
    email_ca_nhan,

    ma_ngan_hang,
    ten_ngan_hang,
    so_tai_khoan,
    chi_nhanh,

    ho_ten_cha,
    quoc_tich_cha,
    dan_toc_cha,
    ton_giao_cha,
    sdt_cha,
    email_cha, 
    dia_chi_thuong_tru_cha,
    cong_viec_cha,

    ho_ten_me,
    quoc_tich_me,
    dan_toc_me,
    ton_giao_me,
    sdt_me,
    email_me, 
    dia_chi_thuong_tru_me,
    cong_viec_me,

    ho_ten_ngh,
    quoc_tich_ngh,
    dan_toc_ngh,
    ton_giao_ngh,
    sdt_ngh,
    email_ngh, 
    dia_chi_thuong_tru_ngh,
    cong_viec_ngh,

    thong_tin_nguoi_can_bao_tin,
    so_dien_thoai_bao_tin,

    anh_the_url

FROM sinh_vien
WHERE mssv = p_mssv;
$$;

SELECT * FROM func_get_student_profile(23520541);
