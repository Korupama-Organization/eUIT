-- ===============================
-- TABLE: cai_dat_nguoi_dung
-- ===============================

CREATE TABLE IF NOT EXISTS cai_dat_nguoi_dung (
    mssv INT PRIMARY KEY,
    che_do_toi BOOLEAN DEFAULT FALSE,
    cap_nhat_ket_qua_hoc_tap BOOLEAN DEFAULT TRUE,
    thong_bao_nghi_lop BOOLEAN DEFAULT TRUE,
    thong_bao_hoc_bu BOOLEAN DEFAULT TRUE,
    lich_thi BOOLEAN DEFAULT TRUE,
    thong_bao_moi BOOLEAN DEFAULT TRUE,
    cap_nhat_trang_thai_thu_tuc_hanh_chinh BOOLEAN DEFAULT TRUE,
    bat_thong_bao_email BOOLEAN DEFAULT TRUE,
    ngay_tao TIMESTAMP DEFAULT NOW(),
    ngay_cap_nhat TIMESTAMP DEFAULT NOW(),
    FOREIGN KEY (mssv) REFERENCES sinh_vien(mssv)
);

-- ===============================
-- FUNCTION: update timestamp
-- ===============================

CREATE OR REPLACE FUNCTION update_settings_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW.ngay_cap_nhat = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- ===============================
-- TRIGGER: tự động cập nhật thời gian
-- ===============================

DROP TRIGGER IF EXISTS trg_update_settings ON cai_dat_nguoi_dung;

CREATE TRIGGER trg_update_settings
BEFORE UPDATE ON cai_dat_nguoi_dung
FOR EACH ROW
EXECUTE FUNCTION update_settings_timestamp();
