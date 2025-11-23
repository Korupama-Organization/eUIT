-- Đồng bộ tất cả sequences
SELECT setval('gxn_sinh_vien_id_seq', (SELECT COALESCE(MAX(id), 0) FROM gxn_sinh_vien));
SELECT setval('phuc_khao_id_seq', (SELECT COALESCE(MAX(id), 0) FROM phuc_khao));
SELECT setval('bang_diem_yeu_cau_id_seq', (SELECT COALESCE(MAX(id), 0) FROM bang_diem_yeu_cau));
SELECT setval('giay_gioi_thieu_id_seq', (SELECT COALESCE(MAX(id), 0) FROM giay_gioi_thieu));

-- Thoát
\q