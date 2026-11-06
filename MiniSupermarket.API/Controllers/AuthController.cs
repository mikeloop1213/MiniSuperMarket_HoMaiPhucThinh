using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase {
        private readonly SupermarketDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SupermarketDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        //POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request) {
            //thieu tai khoan/mat khau -> 400
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) {
                return BadRequest(new { success = false, message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!" });
            }

            string username = request.Username.Trim();

            //tim tai khoan trong CSDL
            MiniSupermarket.API.Models.User? user;
            try {
                user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == username);
            } catch (Exception ex) {
                // loi ket noi / thieu bang -> 500 kem ly do
                return StatusCode(500, new { success = false, message = "Lỗi cơ sở dữ liệu: " + ex.GetBaseException().Message });
            }

            //sai tai khoan hoac mat khau -> 401
            if (user == null || (user.Password ?? string.Empty).Trim() != request.Password.Trim()) {
                return Unauthorized(new { success = false, message = "Sai tài khoản hoặc mật khẩu!" });
            }

            //tai khoan bi khoa -> 401
            if (!user.IsActive) {
                return Unauthorized(new { success = false, message = "Tài khoản của bạn đã bị khóa! Vui lòng liên hệ Admin." });
            }

            //sinh JWT Token chua ten dang nhap + vai tro
            var token = GenerateJwtToken(user.Username, user.Role);

            //200 OK + token cho WinForms luu vao SessionManager
            return Ok(new {
                success = true,
                token = token,
                role = user.Role,
                username = user.Username,
                fullName = user.FullName
            });
        }

        //ham tao chuoi JWT
        private string GenerateJwtToken(string username, string role) {
            var tokenHandler = new JwtSecurityTokenHandler();
            //khoa bi mat doc tu appsettings.json
            var secret = _configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("Thiếu JwtSettings:Secret trong appsettings.json");
            var key = Encoding.ASCII.GetBytes(secret);
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
}
