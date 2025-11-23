-- ===============================================
-- BẢNG THỜI KHÓA BIỂU (đã chỉnh sửa chuẩn)
-- ===============================================
DROP TABLE IF EXISTS thoi_khoa_bieu CASCADE;

CREATE TABLE thoi_khoa_bieu (
    hoc_ky CHAR(11) NOT NULL,
    ma_mon_hoc CHAR(8) NOT NULL,
    ma_lop CHAR(20) NOT NULL,
    so_tin_chi INT,
    ma_giang_vien CHAR(5) NOT NULL,
    thu CHAR(2),
    tiet_bat_dau INT,
    tiet_ket_thuc INT,
    cach_tuan INT,
    ngay_bat_dau DATE,
    ngay_ket_thuc DATE,
    phong_hoc VARCHAR(10),
    si_so INT,
    hinh_thuc_giang_day CHAR(5),
    ghi_chu VARCHAR(255),
    
   
    PRIMARY KEY (ma_lop, ma_giang_vien),
    
    FOREIGN KEY (ma_mon_hoc) REFERENCES mon_hoc(ma_mon_hoc),
    FOREIGN KEY (ma_giang_vien) REFERENCES giang_vien(ma_giang_vien)
);
-- ===============================================
-- BẢNG LỊCH THI (đã chỉnh sửa để khớp với TKB)
-- ===============================================
DROP TABLE IF EXISTS lich_thi CASCADE;

CREATE TABLE lich_thi (
    ma_mon_hoc CHAR(8) NOT NULL,
    ma_lop CHAR(20) NOT NULL,
    ma_giang_vien CHAR(5) NOT NULL,
    ngay_thi DATE NOT NULL,
    ca_thi INT NOT NULL,
    phong_thi VARCHAR(10) NOT NULL,
    ghi_chu VARCHAR(255),
    -- ✅ Khóa chính (1 lớp thi duy nhất 1 ca, 1 giảng viên)
    PRIMARY KEY (ma_lop, ma_giang_vien, ca_thi),
    -- ✅ Khóa ngoại khớp với bảng thời khóa biểu đã chỉnh
    FOREIGN KEY (ma_lop, ma_giang_vien) 
        REFERENCES thoi_khoa_bieu(ma_lop, ma_giang_vien)
        ON DELETE CASCADE,
    FOREIGN KEY (ma_mon_hoc) 
        REFERENCES mon_hoc(ma_mon_hoc)
);

