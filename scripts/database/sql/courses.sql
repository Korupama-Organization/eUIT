drop Function public.func_get_student_registered_courses(integer);

CREATE OR REPLACE FUNCTION public.func_get_student_registered_courses(p_mssv integer)
RETURNS TABLE(
    ma_lop CHAR(20),
    ma_mon_hoc CHAR(8),
    ten_mon_hoc VARCHAR,
    so_tin_chi INT,
    ma_giang_vien CHAR(5)
)
LANGUAGE sql
AS $function$
SELECT
    kq.ma_lop,
    tkb.ma_mon_hoc,
    mh.ten_mon_hoc_vn,
    tkb.so_tin_chi,
    tkb.ma_giang_vien
FROM ket_qua_hoc_tap kq
JOIN thoi_khoa_bieu tkb ON kq.ma_lop = tkb.ma_lop
JOIN mon_hoc mh ON tkb.ma_mon_hoc = mh.ma_mon_hoc
WHERE kq.mssv = p_mssv
ORDER BY tkb.hoc_ky, tkb.ma_mon_hoc;
$function$;


SELECT * 
FROM public.func_get_student_registered_courses(23520541);



CREATE OR REPLACE FUNCTION public.func_get_prerequisites(p_ma_mon char(8))
RETURNS TABLE(
    ma_mon_hoc_dieu_kien char(8),
    ten_mon_hoc varchar
)
LANGUAGE sql
AS $function$
SELECT 
    dk.ma_mon_hoc_dieu_kien,
    mh.ten_mon_hoc_vn
FROM dieu_kien_mon_hoc dk
JOIN mon_hoc mh ON dk.ma_mon_hoc_dieu_kien = mh.ma_mon_hoc
WHERE dk.ma_mon_hoc = p_ma_mon
ORDER BY mh.ten_mon_hoc_vn;
$function$;


CREATE OR REPLACE FUNCTION func_get_all_courses()
RETURNS TABLE(
    ma_mon_hoc char(8),
    ten_mon_hoc_vn varchar,
    ten_mon_hoc_en varchar,
    con_mo_lop char(5),
    khoa_bo_mon_quan_ly char(5),
    loai_mon_hoc char(4),
    so_tc_ly_thuyet int,
    so_tc_thuc_hanh int
)
AS $$
SELECT * FROM mon_hoc ORDER BY ten_mon_hoc_vn;
$$ LANGUAGE sql;

CREATE OR REPLACE FUNCTION func_get_course_detail(p_ma_mon char(8))
RETURNS TABLE(
    ma_mon_hoc char(8),
    ten_mon_hoc_vn varchar,
    ten_mon_hoc_en varchar,
    con_mo_lop char(5),
    khoa_bo_mon_quan_ly char(5),
    loai_mon_hoc char(4),
    so_tc_ly_thuyet int,
    so_tc_thuc_hanh int
)
AS $$
SELECT * FROM mon_hoc WHERE ma_mon_hoc = p_ma_mon;
$$ LANGUAGE sql;
