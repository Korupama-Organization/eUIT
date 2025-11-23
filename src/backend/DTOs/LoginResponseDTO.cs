using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs
{
    /// <summary>
    /// DTO cho kết quả trả về khi đăng nhập thành công
    /// Bao gồm Access Token (ngắn hạn - 1 giờ) và Refresh Token (dài hạn - 14 ngày)
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Access Token - "Thẻ khóa phòng" có thời hạn ngắn (1 giờ)
        /// Dùng để truy cập các API được bảo vệ
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token - "Chứng minh thư" có thời hạn dài (14 ngày)
        /// Chỉ dùng để làm mới Access Token tại endpoint /auth/refresh
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian hết hạn của Access Token (UTC)
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }

        /// <summary>
        /// Thời gian hết hạn của Refresh Token (UTC)
        /// </summary>
        public DateTime RefreshTokenExpiry { get; set; }
    }

    /// <summary>
    /// DTO cho yêu cầu làm mới Access Token
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Refresh Token hiện tại để đổi lấy Access Token mới
        /// </summary>
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO cho kết quả trả về khi làm mới token thành công
    /// </summary>
    public class RefreshTokenResponseDto
    {
        /// <summary>
        /// Access Token mới
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token mới (optional - có thể giữ nguyên cái cũ)
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Thời gian hết hạn của Access Token mới
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }
    }
}
