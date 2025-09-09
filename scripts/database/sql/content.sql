CREATE TABLE van_ban (
    ten_van_ban VARCHAR(255) PRIMARY KEY,
    file_url VARCHAR(255) NOT NULL, -- Đường dẫn tương đối, ví dụ: /Regulations/QuyCheHocVu_2025.pdf
    ngay_ban_hanh DATE
);

CREATE TABLE bai_viet (
    tieu_de TEXT PRIMARY KEY,       -- Tiêu đề của bài viết
    file_url_md VARCHAR(255) NOT NULL,    -- Đường dẫn tới file .md, ví dụ: /Posts/thong-bao-nghi-le-2025.md
    ngay_dang TIMESTAMPTZ DEFAULT NOW()  -- Ngày đăng bài
);

