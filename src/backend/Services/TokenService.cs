using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Services;

public interface ITokenService
{
    /// <summary>
    /// Tạo Access Token (ngắn hạn - 1 giờ)
    /// </summary>
    string CreateAccessToken(string userId, string role);

    /// <summary>
    /// Tạo Refresh Token (dài hạn - 14 ngày) và lưu vào database
    /// </summary>
    Task<RefreshToken> CreateRefreshTokenAsync(string userId, string role, string? deviceInfo = null, string? ipAddress = null);

    /// <summary>
    /// Tạo cả Access và Refresh Token
    /// </summary>
    Task<LoginResponseDto> CreateTokenPairAsync(string userId, string role, string? deviceInfo = null, string? ipAddress = null);

    /// <summary>
    /// Xác thực Refresh Token và tạo Access Token mới
    /// </summary>
    Task<RefreshTokenResponseDto?> RefreshAccessTokenAsync(string refreshTokenValue);

    /// <summary>
    /// Thu hồi Refresh Token (đăng xuất)
    /// </summary>
    Task<bool> RevokeRefreshTokenAsync(string refreshTokenValue);

    /// <summary>
    /// Thu hồi tất cả Refresh Token của một user (đăng xuất khỏi tất cả thiết bị)
    /// </summary>
    Task<int> RevokeAllUserTokensAsync(string userId);

    /// <summary>
    /// Dọn dẹp các token hết hạn (chạy định kỳ)
    /// </summary>
    Task<int> CleanupExpiredTokensAsync();
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly Data.eUITDbContext _dbContext;
    private readonly ILogger<TokenService> _logger;

    // Cấu hình thời gian sống của token
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromHours(1);      // 1 giờ
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(14);     // 14 ngày

    public TokenService(IConfiguration config, Data.eUITDbContext dbContext, ILogger<TokenService> logger)
    {
        _config = config;
        _dbContext = dbContext;
        _logger = logger;
    }

    public string CreateAccessToken(string userId, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role),
            new Claim("token_type", "access"), // Phân biệt với refresh token
            new Claim("jti", Guid.NewGuid().ToString()) // JWT ID để tracking
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(AccessTokenLifetime), // Sử dụng UTC
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
<<<<<<< HEAD
         var accessToken = tokenHandler.WriteToken(token);      

    return accessToken;
=======
        return tokenHandler.WriteToken(token);
>>>>>>> 8779d70e32343167d947973cebbcd1fad990106d
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(string userId, string role, string? deviceInfo = null, string? ipAddress = null)
    {
        // Tạo refresh token value ngẫu nhiên
        var tokenBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }
        var tokenValue = Convert.ToBase64String(tokenBytes);

        // Hash token trước khi lưu database (bảo mật)
        var tokenHash = HashToken(tokenValue);

        var refreshToken = new RefreshToken
        {
            TokenHash = tokenHash,
            UserId = userId,
            UserRole = role,
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenLifetime),
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        // Gán tokenValue để trả về (không lưu trong DB)
        refreshToken.TokenHash = tokenValue; // Tạm thời gán để trả về

        _logger.LogInformation("Created refresh token for user {UserId}", userId);
        return refreshToken;
    }

    public async Task<LoginResponseDto> CreateTokenPairAsync(string userId, string role, string? deviceInfo = null, string? ipAddress = null)
    {
        var accessToken = CreateAccessToken(userId, role);
        var refreshToken = await CreateRefreshTokenAsync(userId, role, deviceInfo, ipAddress);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.TokenHash, // Đây là tokenValue, không phải hash
            AccessTokenExpiry = DateTime.UtcNow.Add(AccessTokenLifetime),
            RefreshTokenExpiry = refreshToken.ExpiresAt
        };
    }

    public async Task<RefreshTokenResponseDto?> RefreshAccessTokenAsync(string refreshTokenValue)
    {
        var tokenHash = HashToken(refreshTokenValue);
        
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        if (refreshToken == null || !refreshToken.IsValid)
        {
            _logger.LogWarning("Invalid or expired refresh token used");
            return null;
        }

        // Tạo access token mới
        var newAccessToken = CreateAccessToken(refreshToken.UserId, refreshToken.UserRole);

        _logger.LogInformation("Refreshed access token for user {UserId}", refreshToken.UserId);

        return new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            AccessTokenExpiry = DateTime.UtcNow.Add(AccessTokenLifetime)
            // Không tạo refresh token mới, giữ nguyên cái cũ
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshTokenValue)
    {
        var tokenHash = HashToken(refreshTokenValue);
        
        var refreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        if (refreshToken == null)
            return false;

        refreshToken.Revoke();
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Revoked refresh token for user {UserId}", refreshToken.UserId);
        return true;
    }

    public async Task<int> RevokeAllUserTokensAsync(string userId)
    {
        var userTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in userTokens)
        {
            token.Revoke();
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Revoked {Count} refresh tokens for user {UserId}", userTokens.Count, userId);
        return userTokens.Count;
    }

    public async Task<int> CleanupExpiredTokensAsync()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30); // Xóa token đã hết hạn 30 ngày trước

        var expiredTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.ExpiresAt < cutoffDate || (rt.IsRevoked && rt.RevokedAt < cutoffDate))
            .ToListAsync();

        _dbContext.RefreshTokens.RemoveRange(expiredTokens);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Cleaned up {Count} expired refresh tokens", expiredTokens.Count);
        return expiredTokens.Count;
    }

    /// <summary>
    /// Hash token bằng SHA256 để bảo mật
    /// </summary>
    private static string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hashedBytes);
    }
}

