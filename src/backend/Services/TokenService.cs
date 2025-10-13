using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace eUIT.API.Services;

public interface ITokenService
{
    (string accessToken, string refreshToken) CreateToken(string userID, string role);
}
public class TokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;

    public (string accessToken, string refreshToken) CreateToken(string userID, string role)
    {
        // 1. Tạo danh sách các "thông tin" (Claims) để đưa vào access token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userID),
            new Claim(ClaimTypes.Role, role)
        };
        // 2. Lấy key từ appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        // 3. Tạo "chứng thực ký" bằng thuật toán an toàn
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // 4. Mô tả access token: thông tin, ngày hết hạn, chứng thực
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30), // Access Token sẽ hết hạn sau 30 phút
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        // 5. Tạo access token dựa trên bản mô tả
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // 6. Tạo refresh token (random string)
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        
        // 7. Trả về chuỗi token đã được mã hóa
        return (accessToken, refreshToken);
    }

}
