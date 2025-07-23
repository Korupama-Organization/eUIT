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