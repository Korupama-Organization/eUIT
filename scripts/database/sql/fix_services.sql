-- GIẤY XÁC NHẬN SINH VIÊN
CREATE TABLE IF NOT EXISTS gxn_sinh_vien (
    id SERIAL PRIMARY KEY,
    mssv INT NOT NULL,
    ngon_ngu VARCHAR(50) NOT NULL,
    ly_do VARCHAR(255) NOT NULL,
    ly_do_khac TEXT,
    ngay_dang_ky DATE,
    trang_thai VARCHAR(20),
    ghi_chu TEXT,
    FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);

-- PHÚC KHẢO
CREATE TABLE IF NOT EXISTS phuc_khao (
    id SERIAL PRIMARY KEY,
    mssv INT NOT NULL,
    ma_lop VARCHAR(20),
    ma_mon_hoc VARCHAR(20),
    ngay_dang_ky DATE,
    ly_do TEXT,
    trang_thai VARCHAR(20),
    FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);

-- BẢNG ĐIỂM YÊU CẦU
CREATE TABLE IF NOT EXISTS bang_diem_yeu_cau (
    id SERIAL PRIMARY KEY,
    mssv INT NOT NULL,
    loai_bang_diem VARCHAR(255),
    ngon_ngu VARCHAR(50),
    so_luong INT,
    ngay_dang_ky DATE,
    trang_thai VARCHAR(20),
    chi_phi NUMERIC(12,2),
    FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);

-- GIẤY GIỚI THIỆU
CREATE TABLE IF NOT EXISTS giay_gioi_thieu (
    id SERIAL PRIMARY KEY,
    mssv INT NOT NULL,
    muc_dich TEXT,
    ngay_dang_ky DATE,
    FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);
