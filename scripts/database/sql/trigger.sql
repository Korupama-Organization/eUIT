-- Trigger functions for calculating final scores in the "ket_qua_hoc_tap" table
-- 1) BEFORE INSERT/UPDATE trigger function  ket_qua_hoc_tap
CREATE OR REPLACE FUNCTION calc_diem_tong_ket_coalesce_no_round()
RETURNS trigger AS $$
DECLARE
    w_qt INT;
    w_gk INT;
    w_th INT;
    w_ck INT;
    sumw INT;
    numerator numeric;
BEGIN
    SELECT trong_so_qua_trinh, trong_so_giua_ki, trong_so_thuc_hanh, trong_so_cuoi_ki
    INTO w_qt, w_gk, w_th, w_ck
    FROM bang_diem
    WHERE ma_lop = NEW.ma_lop
    LIMIT 1;

    IF NOT FOUND THEN
        NEW.diem_tong_ket := NULL;
        RETURN NEW;
    END IF;

    -- COALESCE trọng số = 0
    w_qt := COALESCE(w_qt, 0);
    w_gk := COALESCE(w_gk, 0);
    w_th := COALESCE(w_th, 0);
    w_ck := COALESCE(w_ck, 0);

    sumw := w_qt + w_gk + w_th + w_ck;
    IF sumw <> 100 THEN
        NEW.diem_tong_ket := NULL;
        RETURN NEW;
    END IF;

    -- Nếu trọng số>0 mà điểm NULL => không tính
    IF (w_qt > 0 AND NEW.diem_qua_trinh IS NULL)
       OR (w_gk > 0 AND NEW.diem_giua_ki IS NULL)
       OR (w_th > 0 AND NEW.diem_thuc_hanh IS NULL)
       OR (w_ck > 0 AND NEW.diem_cuoi_ki IS NULL) THEN
        NEW.diem_tong_ket := NULL;
        RETURN NEW;
    END IF;

    -- Tính tổng (không dùng ROUND). numerator kiểu numeric để chia chính xác
    numerator := (COALESCE(NEW.diem_qua_trinh,0) * w_qt
                + COALESCE(NEW.diem_giua_ki,0) * w_gk
                + COALESCE(NEW.diem_thuc_hanh,0) * w_th
                + COALESCE(NEW.diem_cuoi_ki,0) * w_ck)::numeric;

    NEW.diem_tong_ket := numerator / 100;  -- gán thẳng; cột NUMERIC(4,2) sẽ lưu theo scale của cột

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_calc_diem_tong_ket ON ket_qua_hoc_tap;
CREATE TRIGGER trg_calc_diem_tong_ket
BEFORE INSERT OR UPDATE ON ket_qua_hoc_tap
FOR EACH ROW
EXECUTE FUNCTION calc_diem_tong_ket_coalesce_no_round();


-- 2) AFTER INSERT/UPDATE trigger function trên bang_diem (batch recalculation)
CREATE OR REPLACE FUNCTION recalc_diem_kq_for_class_coalesce_no_round()
RETURNS trigger AS $$
DECLARE
    w_qt INT;
    w_gk INT;
    w_th INT;
    w_ck INT;
    sumw INT;
    numerator numeric;
BEGIN
    SELECT trong_so_qua_trinh, trong_so_giua_ki, trong_so_thuc_hanh, trong_so_cuoi_ki
    INTO w_qt, w_gk, w_th, w_ck
    FROM bang_diem
    WHERE ma_lop = NEW.ma_lop
    LIMIT 1;

    IF NOT FOUND THEN
        RETURN NEW;
    END IF;

    w_qt := COALESCE(w_qt, 0);
    w_gk := COALESCE(w_gk, 0);
    w_th := COALESCE(w_th, 0);
    w_ck := COALESCE(w_ck, 0);

    sumw := w_qt + w_gk + w_th + w_ck;

    IF sumw <> 100 THEN
        UPDATE ket_qua_hoc_tap
        SET diem_tong_ket = NULL
        WHERE ma_lop = NEW.ma_lop;
        RETURN NEW;
    END IF;

    -- Cập nhật hàng loạt: tính và gán trực tiếp (không dùng ROUND)
    UPDATE ket_qua_hoc_tap
    SET diem_tong_ket = (
          (COALESCE(diem_qua_trinh,0) * w_qt
         + COALESCE(diem_giua_ki,0)   * w_gk
         + COALESCE(diem_thuc_hanh,0) * w_th
         + COALESCE(diem_cuoi_ki,0)   * w_ck)::numeric
        ) / 100
    WHERE ma_lop = NEW.ma_lop
      AND ( (w_qt = 0 OR diem_qua_trinh IS NOT NULL)
            AND (w_gk = 0 OR diem_giua_ki IS NOT NULL)
            AND (w_th = 0 OR diem_thuc_hanh IS NOT NULL)
            AND (w_ck = 0 OR diem_cuoi_ki IS NOT NULL)
          );

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_recalc_after_bang_diem ON bang_diem;
CREATE TRIGGER trg_recalc_after_bang_diem
AFTER INSERT OR UPDATE ON bang_diem
FOR EACH ROW
EXECUTE FUNCTION recalc_diem_kq_for_class_coalesce_no_round();
