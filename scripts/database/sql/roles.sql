CREATE TABLE tai_khoan_sinh_vien
(
	mssv INT PRIMARY KEY, -- Ma so sinh vien
	mat_khau VARCHAR(50) NOT NULL, -- Mat khau tai khoan
	ngay_tao DATE NOT NULL, -- Ngay tao tai khoan
	ngay_cap_nhat DATE NOT NULL, -- Ngay cap nhat tai khoan
	tinh_trang VARCHAR(20) NOT NULL, -- Tinh trang tai khoan (Hoat dong, Khong hoat dong)
	FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);

CREATE TABLE tai_khoan_giang_vien
(
	ma_giang_vien char(5) PRIMARY KEY, -- Ma so giang vien
	mat_khau VARCHAR(50) NOT NULL, -- Mat khau tai khoan
	ngay_tao DATE NOT NULL, -- Ngay tao tai khoan
	ngay_cap_nhat DATE NOT NULL, -- Ngay cap nhat tai khoan
	tinh_trang VARCHAR(20) NOT NULL, -- Tinh trang tai khoan (Hoat dong, Khong hoat dong)
	FOREIGN KEY (ma_giang_vien) REFERENCES giang_vien(ma_giang_vien)
);

CREATE TABLE tai_khoan_quan_tri_dtdh
(
	dtdh_admin SERIAL PRIMARY KEY, -- Ma so tai khoan quan tri
	mat_khau VARCHAR(50) NOT NULL, -- Mat khau tai khoan
	ngay_tao DATE NOT NULL, -- Ngay tao tai khoan
	ngay_cap_nhat DATE NOT NULL, -- Ngay cap nhat tai khoan
	tinh_trang VARCHAR(20) NOT NULL -- Tinh trang tai khoan (Hoat dong, Khong hoat dong)
);

CREATE TABLE tai_khoan_quan_tri_ctsv
(
	ctsv_admin SERIAL PRIMARY KEY, -- Ma so tai khoan quan tri
	mat_khau VARCHAR(50) NOT NULL, -- Mat khau tai khoan
	ngay_tao DATE NOT NULL, -- Ngay tao tai khoan
	ngay_cap_nhat DATE NOT NULL, -- Ngay cap nhat tai khoan
	tinh_trang VARCHAR(20) NOT NULL -- Tinh trang tai khoan (Hoat dong, Khong hoat dong)
);

--Roles
CREATE ROLE sinh_vien_role
	LOGIN
	PASSWORD 'sinhvien'
	VALID UNTIL '2025-12-31';

CREATE ROLE giang_vien_role
	LOGIN	
	PASSWORD 'giangvien'
	VALID UNTIL '2025-12-31';

CREATE ROLE quan_tri_dtdh_role
	LOGIN
	PASSWORD 'quantridtdh'
	VALID UNTIL '2025-12-31';

CREATE ROLE quan_tri_ctsv_role
	LOGIN
	PASSWORD 'quantricttsv'
	VALID UNTIL '2025-12-31';

--Privileges
-- Sinh vien chi co the xem thong tin cua minh tren cac bang: sinh_vien, tai_khoan_sinh_vien, hoc_phi, dang_ky_gui_xe, xac_nhan_chung_chi
GRANT SELECT ON sinh_vien TO sinh_vien_role;
GRANT SELECT ON tai_khoan_sinh_vien TO sinh_vien_role;
GRANT SELECT ON hoc_phi TO sinh_vien_role;
GRANT SELECT ON dang_ky_gui_xe TO sinh_vien_role;
GRANT SELECT ON xac_nhan_chung_chi TO sinh_vien_role;
-- Giang vien co the xem danh sach sinh vien, lop hoc, thoi khoa bieu, diem danh, diem thi; cap nhat diem thi

