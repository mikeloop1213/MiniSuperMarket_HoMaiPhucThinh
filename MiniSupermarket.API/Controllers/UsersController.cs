using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;

using UserEntity = MiniSupermarket.API.Models.User;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.AdminGroup)]
    public class UsersController : ControllerBase
    {
        // danh sach cac vai tro
        private static readonly string[] AllowedRoles = AppRoles.All;

        private readonly SupermarketDbContext _context;

        public UsersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET/api/users (danh sach tai khoan)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .Select(u => new UserDto
                {
                    Id = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Role = u.Role,
                    IsActive = u.IsActive
                })
                .ToListAsync();
            return Ok(users);
        }

        // GET/api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Select(u => new UserDto { Id = u.UserId, Username = u.Username, FullName = u.FullName, Role = u.Role, IsActive = u.IsActive })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }
            return Ok(user);
        }

        // POST/api/users (tao tai khoan moi)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            string username = dto.Username.Trim();
            string role = string.IsNullOrWhiteSpace(dto.Role) ? AppRoles.Cashier : dto.Role.Trim();

            // vai tro trong danh sach cho phep
            if (!AllowedRoles.Contains(role))
            {
                return BadRequest(new { message = "Vai trò không hợp lệ! Chỉ nhận: " + string.Join(", ", AppRoles.All) });
            }

            // Khong cho trung ten dang nhap
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại trong hệ thống!" });
            }

            var newUser = new UserEntity
            {
                Username = username,
                Password = dto.Password.Trim(),
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? username : dto.FullName.Trim(),
                Role = role,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newUser.UserId },
                new { success = true, message = "Tạo tài khoản thành công!", userId = newUser.UserId });
        }

        // PUT/api/users/{id}/toggle-lock (khoa/mo khoa tai khoan)
        [HttpPut("{id:int}/toggle-lock")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            // khong cho Admin tu khoa chinh minh
            if (string.Equals(user.Username, User.Identity?.Name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Không thể tự khóa tài khoản bạn đang đăng nhập!" });
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, isActive = user.IsActive, message = user.IsActive ? "Đã mở khóa tài khoản!" : "Đã khóa tài khoản!" });
        }

        // 5. POST /api/users/{id}/reset-password (dat lai mat khau mac dinh 123456)
        [HttpPost("{id:int}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            user.Password = "123456";
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Đã đặt lại mật khẩu về mặc định 123456!" });
        }
    }
}
