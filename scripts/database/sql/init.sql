-- Import du lieu sinh vien
COPY sinh_vien
FROM 'eUIT/scripts/database/data/sinh_vien.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');


-- Import du lieu mon hoc
COPY mon_hoc
FROM 'D:\eUIT\scripts\database\data\danh_muc_mon_hoc.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8');

-- Import du lieu dieu kien mon hoc
copy select * from dieu_kien_mon_hoc
FROM 'D:\eUIT\scripts\database\data\dieu_kien_mon_hoc.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

-- Import du lieu giang vien
COPY giang_vien
FROM 'D:\eUIT\scripts\database\data\giang_vien.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

    copy thoi_khoa_bieu
FROM 'D:\eUIT\scripts\database\main_data\thoi_khoa_bieu.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

 copy thoi_khoa_bieu
<<<<<<< HEAD
FROM 'D:\eUIT\scripts\database\main_data\kq_dkhp_with_gv.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8')
=======
<<<<<<< HEAD
FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\kq_dkhp_with_gv.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8');


=======
FROM 'D:\eUIT\scripts\database\main_data\kq_dkhp_with_gv.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ';', ENCODING 'UTF8')
>>>>>>> 1ec7209cfc1b816af324f937ce3c21b6c6c60fb9
>>>>>>> fixing

    copy bang_diem
    FROM 'D:\eUIT\scripts\database\main_data\bang_diem.csv'
    WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');


select * from sinh_vien
where nganh_hoc in ('Công nghệ thông tin',  'Công nghệ thông tin - Định hướng Nhật Bản')
and khoa_hoc = 23;

    -- Import du lieu cot ma_lop, mssv, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki vao bang ket_qua_hoc_tap
copy ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki, ghi_chu)
FROM 'D:\eUIT\scripts\database\other_data\ket_qua_hoc_tap_mau.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

copy ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki, ghi_chu)
FROM 'D:\eUIT\scripts\database\other_data\ket_qua_hoc_tap_mau_expanded.csv'
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
DELETE FROM hoc_phi;

copy hoat_dong_ren_luyen FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\hoat_dong_ren_luyen.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

copy tieu_chi FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\tieu_chi.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');

copy chi_tiet_hoat_dong_ren_luyen FROM 'D:\eUIT-master\eUIT-master\scripts\database\main_data\chi_tiet_hoat_dong_ren_luyen.csv'
WITH (FORMAT csv, HEADER true, DELIMITER ',', ENCODING 'UTF8');
<<<<<<< HEAD
-- Tạm thời tắt kiểm tra khóa ngoại
SET session_replication_role = replica;

-- Xóa dữ liệu cũ để tránh trùng lặp
TRUNCATE TABLE ket_qua_hoc_tap, bang_diem, thoi_khoa_bieu, giang_vien, dieu_kien_mon_hoc, mon_hoc, sinh_vien RESTART IDENTITY CASCADE;

-- Import dữ liệu
\copy sinh_vien FROM 'D:/IE307_4/eUIT/scripts/database/main_data/sinh_vien.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';
\copy mon_hoc FROM 'D:/IE307_4/eUIT/scripts/database/main_data/danh_muc_mon_hoc.csv' CSV HEADER DELIMITER ';' ENCODING 'UTF8';
\copy dieu_kien_mon_hoc FROM 'D:/IE307_4/eUIT/scripts/database/main_data/dieu_kien_mon_hoc.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';
\copy giang_vien FROM 'D:/IE307_4/eUIT/scripts/database/main_data/giang_vien.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';
\copy thoi_khoa_bieu FROM 'D:/IE307_4/eUIT/scripts/database/main_data/thoi_khoa_bieu.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';
\copy thoi_khoa_bieu FROM 'D:/IE307_4/eUIT/scripts/database/main_data/kq_dkhp_with_gv.csv' CSV HEADER DELIMITER ';' ENCODING 'UTF8';
\copy bang_diem FROM 'D:/IE307_4/eUIT/scripts/database/main_data/bang_diem.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';
\copy ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki, ghi_chu) FROM 'D:/IE307_4/eUIT/scripts/database/other_data/ket_qua_hoc_tap_mau_expanded.csv' CSV HEADER DELIMITER ',' ENCODING 'UTF8';

-- Thêm dữ liệu mẫu
INSERT INTO ket_qua_hoc_tap (ma_lop, mssv, ma_lop_goc, diem_qua_trinh, diem_giua_ki, diem_thuc_hanh, diem_cuoi_ki)
VALUES ('IT001.N11', '21520541', 'IT001.N11', 10, NULL, 10, 9)
ON CONFLICT (ma_lop, mssv) DO NOTHING;

-- Tạo tài khoản sinh viên nếu chưa có
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM tai_khoan_sinh_vien WHERE mssv = 21520541) THEN
        PERFORM create_student_account(21520541, 'password0541');
    END IF;
END $$;

-- Bật lại kiểm tra khóa ngoại
SET session_replication_role = DEFAULT;

-- Kiểm tra dữ liệu mẫu
SELECT kq.ma_lop, kq.mssv FROM ket_qua_hoc_tap kq LIMIT 5;
=======
>>>>>>> fixing
