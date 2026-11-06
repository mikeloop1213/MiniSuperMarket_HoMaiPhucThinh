using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase {
        private readonly SupermarketDbContext _context;

        public RolesController(SupermarketDbContext context) {
            _context = context;
        }

        // GET/api/roles
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var roles = await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName, Description = r.Description })
                .ToListAsync();
            return Ok(roles);
        }

        // GET/api/roles/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) {
            var role = await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == id)
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName, Description = r.Description })
                .FirstOrDefaultAsync();
            if (role == null) {
                return NotFound(new { message = "Không tìm thấy vai trò!" });
            }
            return Ok(role);
        }

        // POST/api/roles
        [HttpPost]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Create([FromBody] RoleDto dto) {
            var entity = new Role { RoleName = dto.RoleName.Trim(), Description = dto.Description?.Trim() };
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        // PUT/api/roles/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDto dto) {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) {
                return NotFound(new { message = "Không tìm thấy vai trò cần sửa!" });
            }

            role.RoleName = dto.RoleName.Trim();
            role.Description = dto.Description?.Trim();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE/api/roles/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Delete(int id) {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) {
                return NotFound(new { message = "Không tìm thấy vai trò cần xóa!" });
            }

            // Khong cho xoa vai tro dang co nhan vien su dung
            if (await _context.Users.AnyAsync(u => u.Role == role.RoleName)) {
                return BadRequest(new { message = "Không thể xóa: đang có tài khoản sử dụng vai trò này!" });
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
