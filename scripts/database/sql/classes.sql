CREATE TABLE classes (
    class_id VARCHAR(20) PRIMARY KEY,
    class_name VARCHAR(255) NOT NULL,
    course_name VARCHAR(255) NOT NULL,
    giang_vien_id VARCHAR(20) NOT NULL,
    number_of_students INTEGER,
    schedule VARCHAR(255),
    room VARCHAR(50),
    semester VARCHAR(10),
    academic_year VARCHAR(10)
);

-- Tạo index giúp truy vấn nhanh theo giảng viên
CREATE INDEX idx_classes_giang_vien ON classes(giang_vien_id);

-- Seed dữ liệu test (nếu muốn)
INSERT INTO classes (class_id, class_name, course_name, giang_vien_id, number_of_students, schedule, room, semester, academic_year)
VALUES
    ('IE307.Q11', 'Lớp IE307.Q11', 'IoT Embedded', '80068', 40, 'Mon 7h00-9h00', 'A209', '1', '2025-2026'),
    ('CS202.Q01', 'Lớp CS202.Q01', 'CTDLGT', '80067', 60, 'Tue 9h00-11h00', 'B101', '2', '2025-2026'),
    ('EN101.Q05', 'Lớp EN101.Q05', 'English 1', '80069', 45, 'Wed 8h00-10h00', 'C202', '1', '2025-2026');
