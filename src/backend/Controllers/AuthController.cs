using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data.Common;
using eUIT.API.DTOs;
using eUIT.API.Data;
using eUIT.API.Services;

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class AuthController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(eUITDbContext context, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Đăng nhập và nhận cặp Access Token + Refresh Token
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            // Kiểm tra input cơ bản
            var role = (loginRequest.role ?? string.Empty).Trim().ToLower();
            if (role != "student" && role != "lecturer" && role != "admin")
                return BadRequest(new { error = "Invalid role" });

            var userId = loginRequest.userId ?? string.Empty;
            var password = loginRequest.password ?? string.Empty;

            // Xác thực với database
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT auth_authenticate(@role::auth_user_role, @userId, @password)";

            var pRole = cmd.CreateParameter();
            pRole.ParameterName = "role";
            pRole.Value = role;
            cmd.Parameters.Add(pRole);

            var pUser = cmd.CreateParameter();
            pUser.ParameterName = "userId";
            pUser.Value = userId;
            cmd.Parameters.Add(pUser);

            var pPass = cmd.CreateParameter();
            pPass.ParameterName = "password";
            pPass.Value = password;
            cmd.Parameters.Add(pPass);

            var scalar = await cmd.ExecuteScalarAsync();
            var isAuth = scalar is bool b && b;

            if (!isAuth)
            {
                _logger.LogWarning("Failed login attempt for user {UserId} with role {Role}", userId, role);
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Thu thập thông tin thiết bị và IP
            var deviceInfo = Request.Headers.UserAgent.ToString();
            var ipAddress = GetClientIpAddress();

            // Tạo cặp token
            var tokenPair = await _tokenService.CreateTokenPairAsync(userId, role, deviceInfo, ipAddress);

            _logger.LogInformation("User {UserId} logged in successfully", userId);

            return Ok(tokenPair);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {UserId}", loginRequest.userId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Làm mới Access Token bằng Refresh Token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            var result = await _tokenService.RefreshAccessTokenAsync(request.RefreshToken);

            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Đăng xuất - Thu hồi Refresh Token hiện tại
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            }

            _logger.LogInformation("User {UserId} logged out", userId);
            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Đăng xuất khỏi tất cả thiết bị - Thu hồi tất cả Refresh Token
    /// </summary>
    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
                return Unauthorized();

            var revokedCount = await _tokenService.RevokeAllUserTokensAsync(userId);

            _logger.LogInformation("User {UserId} logged out from all devices ({Count} tokens revoked)", userId, revokedCount);
            return Ok(new { message = $"Logged out from {revokedCount} devices successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout all");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Lấy thông tin profile của người dùng hiện tại
    /// </summary>
    [HttpGet("profile")]
    [Authorize] 
    public IActionResult GetProfile()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var tokenType = User.FindFirst("token_type")?.Value;
            var jti = User.FindFirst("jti")?.Value;

            if (userId == null || role == null)
                return Unauthorized();

            // Kiểm tra xem có phải Access Token không (không phải Refresh Token)
            if (tokenType != "access")
                return Unauthorized(new { message = "Invalid token type" });

            return Ok(new { 
                UserId = userId, 
                Role = role,
                TokenId = jti,
                ServerTime = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Endpoint dành cho admin - Dọn dẹp các token hết hạn
    /// </summary>
    [HttpPost("cleanup-tokens")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CleanupExpiredTokens()
    {
        try
        {
            var deletedCount = await _tokenService.CleanupExpiredTokensAsync();
            return Ok(new { message = $"Cleaned up {deletedCount} expired tokens" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token cleanup");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Lấy địa chỉ IP thực của client (xử lý proxy, load balancer)
    /// </summary>
    private string GetClientIpAddress()
    {
        // Kiểm tra các header thường được sử dụng bởi proxy/load balancer
        var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                       Request.Headers["X-Real-IP"].FirstOrDefault() ??
                       Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        // Nếu có multiple IP trong X-Forwarded-For, lấy cái đầu tiên
        if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Contains(','))
        {
            ipAddress = ipAddress.Split(',')[0].Trim();
        }

        return ipAddress ?? "unknown";
    }
}