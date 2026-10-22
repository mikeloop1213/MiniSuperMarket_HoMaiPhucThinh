using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniSupermarket.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration) {
            _configuration = configuration;
        }

        // Endpoint Đăng nhập: POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request) {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) {
                return BadRequest(new { success = false, message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!" });
            }

            var user = UserStore.Users.FirstOrDefault(u => 
                u.Username.Equals(request.Username.Trim(), StringComparison.OrdinalIgnoreCase) && 
                u.Password == request.Password.Trim());

            if (user == null) {
                return Unauthorized(new { success = false, message = "Sai tài khoản hoặc mật khẩu!" });
            }

            if (!user.IsActive) {
                return Unauthorized(new { success = false, message = "Tài khoản của bạn đã bị khóa! Vui lòng liên hệ Admin." });
            }

            var token = GenerateJwtToken(user.Username, user.Role);
            return Ok(new { 
                success = true, 
                token = token, 
                role = user.Role,
                username = user.Username,
                fullName = user.FullName
            });
        }

        private string GenerateJwtToken(string username, string role) {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Lấy khóa bí mật từ appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!");
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequestDto {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
