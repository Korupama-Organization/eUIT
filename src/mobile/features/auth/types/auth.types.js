
/**
 * Auth Types - Định nghĩa các kiểu dữ liệu cho authentication
 */

/**
 * @typedef {Object} LoginRequest
 * @property {'student' | 'lecturer' | 'admin'} role - Vai trò người dùng
 * @property {string} userId - ID người dùng (MSSV cho student, mã GV cho lecturer)
 * @property {string} password - Mật khẩu
 */

/**
 * @typedef {Object} LoginResponse
 * @property {string} accessToken - Access Token (thời hạn 1 giờ)
 * @property {string} refreshToken - Refresh Token (thời hạn 14 ngày)
 * @property {string} accessTokenExpiry - Thời gian hết hạn Access Token
 * @property {string} refreshTokenExpiry - Thời gian hết hạn Refresh Token
 */

/**
 * @typedef {Object} RefreshTokenRequest
 * @property {string} refreshToken - Refresh Token hiện tại
 */

/**
 * @typedef {Object} RefreshTokenResponse
 * @property {string} accessToken - Access Token mới
 * @property {string} accessTokenExpiry - Thời gian hết hạn Access Token mới
 * @property {string?} refreshToken - Refresh Token mới (optional)
 */

/**
 * @typedef {Object} UserProfile
 * @property {string} userId - ID người dùng
 * @property {'student' | 'lecturer' | 'admin'} role - Vai trò
 * @property {string} tokenId - JWT ID
 * @property {string} serverTime - Thời gian server
 */

/**
 * @typedef {Object} ApiError
 * @property {string} message - Thông báo lỗi
 * @property {number?} status - HTTP status code
 */

// Các role được hỗ trợ
export const USER_ROLES = {
  STUDENT: 'student',
  LECTURER: 'lecturer', 
  ADMIN: 'admin'
};

// Các trạng thái auth
export const AUTH_STATUS = {
  IDLE: 'idle',
  LOADING: 'loading',
  AUTHENTICATED: 'authenticated',
  UNAUTHENTICATED: 'unauthenticated',
  ERROR: 'error'
};

// Các loại lỗi auth
export const AUTH_ERRORS = {
  INVALID_CREDENTIALS: 'Invalid credentials',
  NETWORK_ERROR: 'Network error',
  TOKEN_EXPIRED: 'Token expired',
  INVALID_TOKEN: 'Invalid token',
  SERVER_ERROR: 'Server error'
};
