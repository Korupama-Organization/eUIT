-- Import du lieu sinh vien
ALTER TABLE sinh_vien
DROP COLUMN anh_the_url;

COPY sinh_vien
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\sinh_vien.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');


-- Import du lieu mon hoc
COPY mon_hoc
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\danh_muc_mon_hoc.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8');

-- Import du lieu dieu kien mon hoc
copy dieu_kien_mon_hoc
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\dieu_kien_mon_hoc.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

-- Import du lieu giang vien
ALTER TABLE giang_vien
ALTER COLUMN so_dien_thoai TYPE VARCHAR(15);

COPY giang_vien
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\giang_vien.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

    copy thoi_khoa_bieu
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\thoi_khoa_bieu_fixed.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

 copy thoi_khoa_bieu
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\kq_dkhp_with_gv.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8');


    copy bang_diem
    FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\bang_diem.csv'
    WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');


select * from sinh_vien
where nganh_hoc in ('Công nghệ thông tin',  'Công nghệ thông tin - Định hướng Nhật Bản')
and khoa_hoc = 23;

    -- Import du lieu cot ma_lop, mssv, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki vao bang ket_qua_hoc_tap
alter table ket_qua_hoc_tap
alter column ma_lop_goc type VARCHAR(20);

ALTER TABLE ket_qua_hoc_tap
DROP CONSTRAINT ket_qua_hoc_tap_pkey;
-- 1. Drop foreign key cũ
ALTER TABLE ket_qua_hoc_tap
DROP CONSTRAINT ket_qua_hoc_tap_ma_lop_fkey;  -- tên FK cũ, nếu không biết dùng query pg_constraint để tìm

-- 2. Tạo FK mới từ ma_lop_goc
ALTER TABLE ket_qua_hoc_tap
ADD CONSTRAINT ket_qua_hoc_tap_ma_lop_goc_fkey
FOREIGN KEY (ma_lop_goc) REFERENCES bang_diem(ma_lop);

ALTER TABLE ket_qua_hoc_tap
ADD CONSTRAINT ket_qua_hoc_tap_pkey
PRIMARY KEY (ma_lop, mssv, ma_lop_goc);

copy ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki, ghi_chu)
FROM 'D:\eUIT-master\eUIT-master\scripts\database\other_data\ket_qua_hoc_tap_mau.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

DELETE FROM ket_qua_hoc_tap;

copy ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki, ghi_chu)
FROM 'D:\eUIT-master\eUIT-master\scripts\database\other_data\ket_qua_hoc_tap_mau_expanded.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');



INSERT INTO ket_qua_hoc_tap (ma_lop, mssv, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki)
VALUES ('IT001.N11', '21520541', 10, NULL, 10, 9);

select create_student_account(21520541, 'password0541')

select *  from tai_khoan_sinh_vien;

SELECT kq.ma_lop, kq.diem_tong_ket FROM ket_qua_hoc_tap kq WHERE mssv = '21520541';
SELECT * FROM bang_diem
SELECT * from hoc_phi

SELECT * from ket_qua_hoc_tap


DELETE FROM bang_diem;
DELETE FROM ket_qua_hoc_tap;
DELETE FROM hoc_phi;

SELECT auth_authenticate('student', '23520541', 'PasswordA1!'); -- TRUE

SELECT * FROM func_get_student_semester_transcript(23520541, '2024_2025_1');

SELECT * FROM func_get_student_full_transcript(23520545)