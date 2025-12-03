-- =====================================================================
-- HÀM: get_full_schedule
-- Mục đích: Trả về lịch học chi tiết của sinh viên trong học kỳ
-- Tham số:
--   - mssv: Mã số sinh viên
--   - hoc_ky: Học kỳ cần tra cứu (ví dụ: '2025_2026_1')
-- Trả về: Danh sách các buổi học từ tuần hiện tại trở về sau
-- =====================================================================

CREATE OR REPLACE FUNCTION get_full_schedule(p_mssv INT, p_hoc_ky CHAR(11))
RETURNS TABLE (
    ma_lop CHAR(20),
    ten_mon_hoc_vn VARCHAR(255),
    thu CHAR(2),
    tiet_bat_dau INT,
    tiet_ket_thuc INT,
    phong_hoc VARCHAR(10),
    ngay_hoc DATE,
    ten_giang_vien VARCHAR(100)
)
LANGUAGE sql
AS $$
    -- Bước 1: Lấy thông tin lịch học của các lớp sinh viên đã đăng ký
    WITH ScheduleInfo AS (
        SELECT 
            tkb.ma_lop,
            tkb.thu,
            tkb.tiet_bat_dau,
            tkb.tiet_ket_thuc,
            tkb.phong_hoc,
            tkb.ngay_bat_dau,
            tkb.ngay_ket_thuc,
            COALESCE(NULLIF(tkb.cach_tuan, 0), 1) AS cach_tuan,
            mh.ten_mon_hoc_vn,
            gv.ho_ten AS ten_giang_vien,
            -- Chuyển đổi thứ sang day of week (0 = Chủ nhật, 1 = Thứ 2, ...)
            CASE 
                WHEN tkb.thu = '2' THEN 1
                WHEN tkb.thu = '3' THEN 2
                WHEN tkb.thu = '4' THEN 3
                WHEN tkb.thu = '5' THEN 4
                WHEN tkb.thu = '6' THEN 5
                WHEN tkb.thu = '7' THEN 6
                WHEN tkb.thu = '8' THEN 0
                ELSE NULL
            END AS dow
        FROM ket_qua_hoc_tap AS kqht
        INNER JOIN thoi_khoa_bieu AS tkb ON kqht.ma_lop = tkb.ma_lop
        LEFT JOIN mon_hoc AS mh ON tkb.ma_mon_hoc = mh.ma_mon_hoc
        LEFT JOIN giang_vien AS gv ON tkb.ma_giang_vien = gv.ma_giang_vien
        WHERE kqht.mssv = p_mssv
          AND tkb.hoc_ky = p_hoc_ky
          AND tkb.hinh_thuc_giang_day != 'HT2'  -- Loại bỏ lớp thực hành online
          AND tkb.ngay_bat_dau IS NOT NULL
          AND tkb.ngay_ket_thuc IS NOT NULL
    ),
    -- Bước 2: Tính ngày bắt đầu tuần hiện tại (Thứ 2)
    WeekStart AS (
        SELECT (CURRENT_DATE - ((EXTRACT(DOW FROM CURRENT_DATE)::int + 6) % 7) * INTERVAL '1 day')::date AS start_date
    ),
    -- Bước 3: Sinh các ngày học cụ thể cho mỗi lớp
    ClassDates AS (
        SELECT 
            si.ma_lop,
            si.thu,
            si.tiet_bat_dau,
            si.tiet_ket_thuc,
            si.phong_hoc,
            si.ten_mon_hoc_vn,
            si.ten_giang_vien,
            d::date AS ngay_hoc
        FROM ScheduleInfo AS si
        CROSS JOIN WeekStart AS ws
        CROSS JOIN LATERAL generate_series(
            GREATEST(si.ngay_bat_dau, ws.start_date),  -- Bắt đầu từ tuần hiện tại
            si.ngay_ket_thuc,
            INTERVAL '1 day'
        ) AS d
        WHERE EXTRACT(DOW FROM d) = si.dow  -- Đúng thứ trong tuần
          AND ((d::date - si.ngay_bat_dau) / 7) % si.cach_tuan = 0  -- Đúng tuần học
    )
    -- Bước 4: Trả về kết quả sắp xếp theo ngày và tiết
    SELECT 
        ma_lop,
        ten_mon_hoc_vn,
        thu,
        tiet_bat_dau,
        tiet_ket_thuc,
        phong_hoc,
        ngay_hoc,
        ten_giang_vien
    FROM ClassDates
    ORDER BY ngay_hoc, tiet_bat_dau, ma_lop;
$$;

-- =====================================================================
-- TESTS
-- =====================================================================

-- Test 1: Kiểm tra các lớp sinh viên đăng ký
SELECT ma_lop FROM ket_qua_hoc_tap WHERE mssv = 23520541 ORDER BY ma_lop;

-- Test 2: Kiểm tra lịch học đầy đủ
SELECT * FROM get_full_schedule(23520541, '2025_2026_1') ORDER BY ngay_hoc, tiet_bat_dau;

-- Test 3: Kiểm tra chỉ lớp IE104
SELECT ma_lop, ngay_hoc, tiet_bat_dau 
FROM get_full_schedule(23520541, '2025_2026_1') 
WHERE ma_lop LIKE '%IE104%'
ORDER BY ngay_hoc, tiet_bat_dau;

DELETE FROM thoi_khoa_bieu
WHERE ctid IN (
    SELECT ctid FROM (
        SELECT 
            ctid,
            ROW_NUMBER() OVER(
                PARTITION BY 
                    hoc_ky, ma_mon_hoc, ma_lop, ma_giang_vien,
                    thu, tiet_bat_dau, tiet_ket_thuc
                ORDER BY ctid
            ) AS rn
        FROM thoi_khoa_bieu
    ) AS x
    WHERE rn > 1
);
