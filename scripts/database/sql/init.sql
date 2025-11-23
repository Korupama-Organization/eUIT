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
