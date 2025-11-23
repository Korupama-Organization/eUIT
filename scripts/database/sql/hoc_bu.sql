-- ============================================
-- CLEAN UP - DROP BẢNG CŨ NẾU TỒN TẠI
-- ============================================

DROP TABLE IF EXISTS meetings CASCADE;
DROP TABLE IF EXISTS exams CASCADE;
DROP TABLE IF EXISTS announcements CASCADE;
DROP TABLE IF EXISTS absences CASCADE;
DROP TABLE IF EXISTS makeup_classes CASCADE;

-- ============================================
-- TẠNG 5 BẢNG MỚI (Không Foreign Key)
-- ============================================

-- 1️⃣ Tạo bảng makeup_classes (Báo bù)
CREATE TABLE IF NOT EXISTS makeup_classes (
    id SERIAL PRIMARY KEY,
    giang_vien_id VARCHAR(50) NOT NULL,
    class_id VARCHAR(50) NOT NULL,
    makeup_date TIMESTAMP NOT NULL,
    room_id VARCHAR(50),
    reason TEXT,
    status VARCHAR(20) DEFAULT 'submitted',
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP
);

-- 2️⃣ Tạo bảng absences (Báo nghỉ)
CREATE TABLE IF NOT EXISTS absences (
    id SERIAL PRIMARY KEY,
    giang_vien_id VARCHAR(50) NOT NULL,
    class_id VARCHAR(50) NOT NULL,
    absence_date TIMESTAMP NOT NULL,
    reason TEXT,
    type VARCHAR(20) DEFAULT 'personal',
    status VARCHAR(20) DEFAULT 'submitted',
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP
);

-- 3️⃣ Tạo bảng announcements (Thông báo)
CREATE TABLE IF NOT EXISTS announcements (
    id SERIAL PRIMARY KEY,
    class_id VARCHAR(50) NOT NULL,
    title VARCHAR(255) NOT NULL,
    content TEXT,
    created_by VARCHAR(50) NOT NULL,
    published_date TIMESTAMP,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 4️⃣ Tạo bảng exams (Lịch thi)
CREATE TABLE IF NOT EXISTS exams (
    id SERIAL PRIMARY KEY,
    class_id VARCHAR(50) NOT NULL,
    exam_date TIMESTAMP NOT NULL,
    room VARCHAR(50),
    invigilators TEXT,
    duration INTEGER,
    semester VARCHAR(10),
    academic_year VARCHAR(10)
);

-- 5️⃣ Tạo bảng meetings (Lịch họp)
CREATE TABLE IF NOT EXISTS meetings (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    meeting_date TIMESTAMP NOT NULL,
    location VARCHAR(255),
    duration INTEGER
);

-- ============================================
-- TẠNG INDEXES ĐỂ TỐI ƯU QUERY
-- ============================================

CREATE INDEX IF NOT EXISTS ix_makeup_giang_vien ON makeup_classes(giang_vien_id);
CREATE INDEX IF NOT EXISTS ix_makeup_class ON makeup_classes(class_id);
CREATE INDEX IF NOT EXISTS ix_makeup_created_at ON makeup_classes(created_at);

CREATE INDEX IF NOT EXISTS ix_absence_giang_vien ON absences(giang_vien_id);
CREATE INDEX IF NOT EXISTS ix_absence_class ON absences(class_id);
CREATE INDEX IF NOT EXISTS ix_absence_created_at ON absences(created_at);

CREATE INDEX IF NOT EXISTS ix_announcement_class ON announcements(class_id);
CREATE INDEX IF NOT EXISTS ix_announcement_created_by ON announcements(created_by);
CREATE INDEX IF NOT EXISTS ix_announcement_published_date ON announcements(published_date);

CREATE INDEX IF NOT EXISTS ix_exam_class ON exams(class_id);
CREATE INDEX IF NOT EXISTS ix_exam_date ON exams(exam_date);
CREATE INDEX IF NOT EXISTS ix_exam_semester_year ON exams(semester, academic_year);

CREATE INDEX IF NOT EXISTS ix_meeting_date ON meetings(meeting_date);

-- ============================================
-- INSERT DỮ LIỆU TEST VÀO 5 BẢNG
-- ============================================

-- 1️⃣ INSERT dữ liệu vào makeup_classes (Báo bù)
INSERT INTO makeup_classes (giang_vien_id, class_id, makeup_date, room_id, reason, status, created_at)
VALUES 
    ('80068', 'IE307.Q11', '2025-12-15 14:00:00', 'A101', 'Có việc gia đình', 'submitted', NOW()),
    ('80068', 'IE307.Q12', '2025-12-16 15:00:00', 'A102', 'Bệnh', 'approved', NOW() - INTERVAL '2 days'),
    ('80067', 'CS202.Q01', '2025-12-20 09:00:00', 'B201', 'Dự án cấp bộ', 'draft', NOW()),
    ('80067', 'CS202.Q02', '2025-12-21 10:30:00', 'B202', 'Họp khoa', 'rejected', NOW() - INTERVAL '1 day');

-- 2️⃣ INSERT dữ liệu vào absences (Báo nghỉ)
INSERT INTO absences (giang_vien_id, class_id, absence_date, reason, type, status, created_at)
VALUES 
    ('80068', 'IE307.Q11', '2025-12-10 14:00:00', 'Bệnh nặng', 'sick', 'approved', NOW()),
    ('80068', 'IE307.Q12', '2025-12-11 15:00:00', 'Việc cá nhân', 'personal', 'submitted', NOW()),
    ('80067', 'CS202.Q01', '2025-12-08 09:00:00', 'Công vụ', 'official', 'approved', NOW() - INTERVAL '3 days'),
    ('80067', 'CS202.Q02', '2025-12-09 10:30:00', 'Khẩn cấp', 'emergency', 'pending', NOW() - INTERVAL '2 days'),
    ('80069', 'EN101.Q05', '2025-12-05 08:00:00', 'Đi công tác', 'official', 'approved', NOW() - INTERVAL '5 days');

-- 3️⃣ INSERT dữ liệu vào announcements (Thông báo)
INSERT INTO announcements (class_id, title, content, created_by, published_date, created_at)
VALUES 
    ('IE307.Q11', 'Thay đổi lịch học', 'Buổi học thứ 5 tuần này dời sang thứ 6. Vui lòng cập nhật lịch biểu.', '80068', NOW(), NOW()),
    ('IE307.Q12', 'Nộp bài tập cuối kỳ', 'Hạn chót nộp bài tập là 15/12/2025 23:59. Không nhận bài nộp muộn.', '80068', NOW() - INTERVAL '1 hour', NOW() - INTERVAL '1 hour'),
    ('CS202.Q01', 'Thi giữa kỳ', 'Thi giữa kỳ sẽ diễn ra vào 20/12/2025. Địa điểm: Phòng 301. Thời gian: 60 phút.', '80067', NOW() - INTERVAL '2 days', NOW() - INTERVAL '2 days'),
    ('CS202.Q02', 'Thang điểm môn học', 'Thang điểm: Quá trình 30%, Giữa kỳ 30%, Cuối kỳ 40%. Tham khảo thêm trên Canvas.', '80067', NOW() - INTERVAL '5 days', NOW() - INTERVAL '5 days'),
    ('EN101.Q05', 'Bài tập thêm', 'Bài tập thêm tuần này được đăng lên Canvas. Làm vào notebook để kiểm tra tuần sau.', '80069', NOW() - INTERVAL '3 days', NOW() - INTERVAL '3 days');

-- 4️⃣ INSERT dữ liệu vào exams (Lịch thi)
INSERT INTO exams (class_id, exam_date, room, invigilators, duration, semester, academic_year)
VALUES 
    ('IE307.Q11', '2025-12-25 08:00:00', 'A301', '80068, 80070', 120, '1', '2024-2025'),
    ('IE307.Q12', '2025-12-25 10:00:00', 'A302', '80068, 80071', 120, '1', '2024-2025'),
    ('CS202.Q01', '2025-12-26 08:00:00', 'B301', '80067, 80072', 90, '1', '2024-2025'),
    ('CS202.Q02', '2025-12-26 10:00:00', 'B302', '80067, 80073', 90, '1', '2024-2025'),
    ('EN101.Q05', '2025-12-27 14:00:00', 'C301', '80069, 80074', 60, '1', '2024-2025'),
    ('EN101.Q06', '2025-12-27 15:30:00', 'C302', '80069, 80075', 60, '1', '2024-2025');

-- 5️⃣ INSERT dữ liệu vào meetings (Lịch họp) - Đã có 5 records rồi
INSERT INTO meetings (title, description, meeting_date, location, duration)
VALUES 
    ('Họp Bộ môn Công nghệ Phần mềm', 'Đánh giá kết quả học tập', '2025-12-22 09:00:00', 'Phòng họp A', 120),
    ('Họp cấp Khoa', 'Kế hoạch tuyển sinh 2026', '2025-12-23 14:00:00', 'Hội trường', 180);

-- ============================================
-- VERIFY - KIỂM TRA DỮ LIỆU
-- ============================================

SELECT '✅ Tables created successfully!' AS status;

SELECT 
    'makeup_classes' AS table_name, COUNT(*) AS total_records FROM makeup_classes
UNION ALL
SELECT 'absences', COUNT(*) FROM absences
UNION ALL
SELECT 'announcements', COUNT(*) FROM announcements
UNION ALL
SELECT 'exams', COUNT(*) FROM exams
UNION ALL
SELECT 'meetings', COUNT(*) FROM meetings;

-- Xem chi tiết dữ liệu
SELECT '📊 MAKEUP DATA:' AS info;
SELECT * FROM makeup_classes ORDER BY created_at DESC;

SELECT '📊 ABSENCE DATA:' AS info;
SELECT * FROM absences ORDER BY created_at DESC;

SELECT '📊 ANNOUNCEMENT DATA:' AS info;
SELECT * FROM announcements ORDER BY published_date DESC;

SELECT '📊 EXAM DATA:' AS info;
SELECT * FROM exams ORDER BY exam_date;

SELECT '📊 MEETING DATA:' AS info;
SELECT * FROM meetings ORDER BY meeting_date;
