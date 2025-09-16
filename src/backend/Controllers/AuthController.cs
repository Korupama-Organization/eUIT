using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


using System.Data.Common;
using eUIT.API.DTOs;
using eUIT.API.Data;
using eUIT.API.Services;
using eUIT.API.Models;

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")] // Đường dẫn sẽ là /api/auth
public class AuthController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(eUITDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        // Kiểm tra input cơ bản
        var role = (loginRequest.role ?? string.Empty).Trim().ToLower();
        if (role != "student" && role != "lecturer" && role != "admin")
            return BadRequest(new { error = "Invalid role" });

        var userId = loginRequest.userId ?? string.Empty;
        var password = loginRequest.password ?? string.Empty;

        // Sử dụng kết nối ADO.NET có tham số để tránh SQL injection và để ép kiểu enum bên phía Postgres
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
            return Unauthorized(new { message = "Invalid credentials" });
        }
        var (accessToken, refreshToken) = _tokenService.CreateToken(loginRequest.userId, loginRequest.role);
        // Lưu refresh token vào DB
            _context.RefreshTokens.Add(new RefreshToken
    {
        UserId = loginRequest.userId,
        Token = refreshToken,
        ExpiryDate = DateTime.UtcNow.AddDays(7)
    });
    await _context.SaveChangesAsync();

    return Ok(new { accessToken, refreshToken });
}

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        // 1️ Kiểm tra refresh token trong DB
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        // 2️ Lấy thông tin user (ví dụ: role, userId) từ DB
        var userId = storedToken.UserId;
        var role = storedToken.Role; // Nếu có cột Role, hoặc join bảng User

        // 3️ Sinh ra cặp token mới
        var (newAccessToken, newRefreshToken) = _tokenService.CreateToken(userId, role);

        // 4️ Lưu refresh token mới vào DB, xoá/disable cái cũ
        storedToken.Token = newRefreshToken;
        storedToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        // 5️ Trả về cho client
        return Ok(new
        {
            accessToken = newAccessToken,
            refreshToken = newRefreshToken
        });
    }

    
    [HttpGet("profile")]
    [Authorize] 
    public IActionResult GetProfile()
    {
        // Lấy thông tin người dùng đã được giải mã từ token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userId == null || role == null)
        {
            return Unauthorized();
        }

        return Ok(new { UserId = userId, Role = role });                                                            
    }
}
