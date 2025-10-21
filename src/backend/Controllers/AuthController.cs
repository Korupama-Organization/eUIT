using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


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

        // Store refresh token securely via DB function (store only hash in DB)
        await using var issueCmd = connection.CreateCommand();
        issueCmd.CommandText = "SELECT issue_refresh_token(@role::auth_user_role, @userId, @token_plain, @ttl::interval)";

        var pRole2 = issueCmd.CreateParameter();
        pRole2.ParameterName = "role";
        pRole2.Value = role;
        issueCmd.Parameters.Add(pRole2);

        var pUser2 = issueCmd.CreateParameter();
        pUser2.ParameterName = "userId";
        pUser2.Value = userId;
        issueCmd.Parameters.Add(pUser2);

        var pTokenPlain = issueCmd.CreateParameter();
        pTokenPlain.ParameterName = "token_plain";
        pTokenPlain.Value = refreshToken;
        issueCmd.Parameters.Add(pTokenPlain);

        var pTtl = issueCmd.CreateParameter();
        pTtl.ParameterName = "ttl";
        pTtl.Value = "7 days"; // will be cast to interval in SQL
        issueCmd.Parameters.Add(pTtl);

        // Execute - returns UUID of issued token (ignored here)
        await issueCmd.ExecuteScalarAsync();

    return Ok(new { accessToken, refreshToken });
}

    [HttpPost("refresh")]
public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO dto)
{
    if (string.IsNullOrEmpty(dto.RefreshToken))
        return BadRequest(new { message = "Missing refresh token" });

    var refreshToken = dto.RefreshToken;

    // 1️ Kiểm tra token đầu vào bằng hàm DB (trả về token_id, user_role, user_id)
    await using var validateCmd = _context.Database.GetDbConnection().CreateCommand();
    validateCmd.CommandText = "SELECT token_id, user_role, user_id FROM validate_refresh_token(@token_plain)";

    var vTokenParam = validateCmd.CreateParameter();
    vTokenParam.ParameterName = "token_plain";
    vTokenParam.Value = refreshToken;
    validateCmd.Parameters.Add(vTokenParam);

    await using var reader = await validateCmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        return Unauthorized(new { message = "Invalid or expired refresh token" });
    }

    var tokenId = reader.GetGuid(0);
    var dbRole = reader.GetString(1);
    var dbUserId = reader.GetString(2);

    // 2 Tạo token mới
    var (newAccessToken, newRefreshToken) = _tokenService.CreateToken(dbUserId, dbRole);

    // 3 Loai bỏ token cũ và phát hành token mới qua hàm DB
    await using var revokeCmd = _context.Database.GetDbConnection().CreateCommand();
    revokeCmd.CommandText = "SELECT revoke_refresh_token_by_id(@id)";
    var rid = revokeCmd.CreateParameter();
    rid.ParameterName = "id";
    rid.Value = tokenId;
    revokeCmd.Parameters.Add(rid);
    await revokeCmd.ExecuteNonQueryAsync();

    await using var issueCmd2 = _context.Database.GetDbConnection().CreateCommand();
    issueCmd2.CommandText = "SELECT issue_refresh_token(@role::auth_user_role, @userId, @token_plain, @ttl::interval)";
    var irRole = issueCmd2.CreateParameter(); irRole.ParameterName = "role"; irRole.Value = dbRole; issueCmd2.Parameters.Add(irRole);
    var irUser = issueCmd2.CreateParameter(); irUser.ParameterName = "userId"; irUser.Value = dbUserId; issueCmd2.Parameters.Add(irUser);
    var irToken = issueCmd2.CreateParameter(); irToken.ParameterName = "token_plain"; irToken.Value = newRefreshToken; issueCmd2.Parameters.Add(irToken);
    var irTtl = issueCmd2.CreateParameter(); irTtl.ParameterName = "ttl"; irTtl.Value = "7 days"; issueCmd2.Parameters.Add(irTtl);
    await issueCmd2.ExecuteScalarAsync();

    // 4 Trả token mới về cho client
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
