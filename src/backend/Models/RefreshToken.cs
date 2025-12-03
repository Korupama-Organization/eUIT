using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUIT.API.Models;

/// <summary>
/// Entity đại diện cho Refresh Token được lưu trong database
/// Mỗi user có thể có nhiều refresh token (đăng nhập trên nhiều thiết bị)
/// </summary>
[Table("auth_refresh_tokens")]
public class RefreshToken
{
    /// <summary>
    /// ID duy nhất của refresh token
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Giá trị token đã được hash (không lưu plain text)
    /// </summary>
    [Required]
    [Column("token_hash")]
    [MaxLength(256)]
    public string TokenHash { get; set; } = string.Empty;

    /// <summary>
    /// ID của người dùng sở hữu token này
    /// </summary>
    [Required]
    [Column("user_id")]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Vai trò của người dùng (student, lecturer, admin)
    /// </summary>
    [Required]
    [Column("user_role")]
    [MaxLength(20)]
    public string UserRole { get; set; } = string.Empty;

    /// <summary>
    /// Thời gian tạo token
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời gian hết hạn của token
    /// </summary>
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Token có bị thu hồi không (revoked)
    /// </summary>
    [Column("is_revoked")]
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// Thời gian thu hồi token (nếu có)
    /// </summary>
    [Column("revoked_at")]
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Thông tin thiết bị/trình duyệt (optional)
    /// </summary>
    [Column("device_info")]
    [MaxLength(500)]
    public string? DeviceInfo { get; set; }

    /// <summary>
    /// Địa chỉ IP khi tạo token (optional)
    /// </summary>
    [Column("ip_address")]
    [MaxLength(45)] // IPv6 cần tối đa 45 ký tự
    public string? IpAddress { get; set; }

    /// <summary>
    /// Kiểm tra xem token có còn hợp lệ không
    /// </summary>
    public bool IsValid => !IsRevoked && DateTime.UtcNow < ExpiresAt;

    /// <summary>
    /// Đánh dấu token là đã bị thu hồi
    /// </summary>
    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}