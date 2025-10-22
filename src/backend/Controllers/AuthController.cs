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
public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO refreshRequest)
{
    if (refreshRequest == null || string.IsNullOrEmpty(refreshRequest.RefreshToken))
        return BadRequest(new { message = "Missing refresh token" });

    await using var connection = _context.Database.GetDbConnection();
    await connection.OpenAsync();

    Guid tokenId;
    string dbRole;
    string dbUserId;

    // 1️ Validate refresh token trong DB
    await using (var validateCmd = connection.CreateCommand())
    {
        validateCmd.CommandText = "SELECT * FROM validate_refresh_token(@token_plain)";
        var vp = validateCmd.CreateParameter();
        vp.ParameterName = "token_plain";
        vp.Value = refreshRequest.RefreshToken;
        validateCmd.Parameters.Add(vp);

        await using var reader = await validateCmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        tokenId = reader.GetGuid(reader.GetOrdinal("token_id"));
        dbRole = reader.GetString(reader.GetOrdinal("user_role"));
        dbUserId = reader.GetString(reader.GetOrdinal("user_id"));
    }

    // 2️ Tạo mới access/refresh token
    var (newAccessToken, newRefreshToken) = _tokenService.CreateToken(dbUserId, dbRole);

    // 3️ Thu hồi refresh token cũ
    await using (var revokeCmd = connection.CreateCommand())
    {
        revokeCmd.CommandText = "SELECT revoke_refresh_token_by_id(@id)";
        var idp = revokeCmd.CreateParameter();
        idp.ParameterName = "id";
        idp.Value = tokenId;
        revokeCmd.Parameters.Add(idp);
        await revokeCmd.ExecuteNonQueryAsync();
    }

    // 4️ Lưu refresh token mới
    await using (var issueCmd = connection.CreateCommand())
    {
        issueCmd.CommandText =
            "SELECT issue_refresh_token(@role::auth_user_role, @userId, @token_plain, @ttl::interval)";

        var irRole = issueCmd.CreateParameter();
        irRole.ParameterName = "role";
        irRole.Value = dbRole;
        issueCmd.Parameters.Add(irRole);

        var irUser = issueCmd.CreateParameter();
        irUser.ParameterName = "userId";
        irUser.Value = dbUserId;
        issueCmd.Parameters.Add(irUser);

        var irToken = issueCmd.CreateParameter();
        irToken.ParameterName = "token_plain";
        irToken.Value = newRefreshToken;
        issueCmd.Parameters.Add(irToken);

        var irTtl = issueCmd.CreateParameter();
        irTtl.ParameterName = "ttl";
        irTtl.Value = "7 days";
        issueCmd.Parameters.Add(irTtl);

        await issueCmd.ExecuteNonQueryAsync();
    }

    // 5️ Trả về token mới cho client
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
