using Microsoft.AspNetCore.Mvc;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // 1. GET /api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var userDtos = UserStore.Users.Select(u => new
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToList();

            return Ok(userDtos);
        }

        // 2. GET /api/users/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }
            return Ok(new
            {
                user.Id,
                user.Username,
                user.FullName,
                user.Role,
                user.IsActive
            });
        }

        // 3. POST /api/users
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Tên đăng nhập và mật khẩu không được trống!" });
            }

            if (UserStore.Users.Any(u => u.Username.Equals(dto.Username.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại trong hệ thống!" });
            }

            int nextId = UserStore.Users.Count > 0 ? UserStore.Users.Max(u => u.Id) + 1 : 1;
            var newUser = new UserAccount
            {
                Id = nextId,
                Username = dto.Username.Trim(),
                Password = dto.Password.Trim(),
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? dto.Username.Trim() : dto.FullName.Trim(),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Cashier" : dto.Role.Trim(),
                IsActive = true
            };

            UserStore.Users.Add(newUser);
            return Ok(new { success = true, message = "Tạo tài khoản thành công!", userId = newUser.Id });
        }

        // 4. PUT /api/users/{id}/toggle-lock
        [HttpPut("{id:int}/toggle-lock")]
        public IActionResult ToggleLock(int id)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            user.IsActive = !user.IsActive;
            return Ok(new { success = true, isActive = user.IsActive, message = user.IsActive ? "Đã mở khóa tài khoản!" : "Đã khóa tài khoản!" });
        }

        // 5. POST /api/users/{id}/reset-password
        [HttpPost("{id:int}/reset-password")]
        public IActionResult ResetPassword(int id)
        {
            var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            user.Password = "123456";
            return Ok(new { success = true, message = "Đã đặt lại mật khẩu về mặc định 123456!" });
        }
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
    }
}
