-- ========================================
-- Script tạo bảng refresh_tokens
-- Để quản lý Refresh Token trong hệ thống
-- ========================================

-- Tạo bảng refresh_tokens
CREATE TABLE IF NOT EXISTS refresh_tokens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    token_hash VARCHAR(256) NOT NULL UNIQUE,
    user_id VARCHAR(50) NOT NULL,
    user_role VARCHAR(20) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP WITH TIME ZONE NOT NULL,
    is_revoked BOOLEAN DEFAULT FALSE,
    revoked_at TIMESTAMP WITH TIME ZONE NULL,
    device_info VARCHAR(500) NULL,
    ip_address VARCHAR(45) NULL
);

-- Tạo các index để tối ưu performance
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_token_hash ON refresh_tokens(token_hash);
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_user_id ON refresh_tokens(user_id);
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_expires_at ON refresh_tokens(expires_at);
CREATE INDEX IF NOT EXISTS ix_refresh_tokens_cleanup ON refresh_tokens(is_revoked, expires_at);

-- Tạo function để dọn dẹp token hết hạn (optional)
CREATE OR REPLACE FUNCTION cleanup_expired_refresh_tokens()
RETURNS INTEGER AS $$
DECLARE
    deleted_count INTEGER;
BEGIN
    -- Xóa các token hết hạn từ 30 ngày trước
    DELETE FROM refresh_tokens 
    WHERE expires_at < CURRENT_TIMESTAMP - INTERVAL '30 days'
       OR (is_revoked = true AND revoked_at < CURRENT_TIMESTAMP - INTERVAL '30 days');
    
    GET DIAGNOSTICS deleted_count = ROW_COUNT;
    
    RETURN deleted_count;
END;
$$ LANGUAGE plpgsql;

-- Thêm comment cho function

PRINT 'Tạo bảng refresh_tokens và các index thành công!';