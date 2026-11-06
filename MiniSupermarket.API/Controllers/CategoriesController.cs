using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public CategoriesController(SupermarketDbContext context)
        {
            _context = context;
        }

        //GET /api/categories -> lay toan bo nhom hang
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryId)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToListAsync();
            return Ok(list);
        }

        //GET /api/categories/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _context.Categories
                .AsNoTracking()
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDto { CategoryId = c.CategoryId, CategoryName = c.CategoryName, Description = c.Description })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound(new { message = "Không tìm thấy nhóm hàng trong CSDL!" });   // 404
            }
            return Ok(dto);
        }

        //GET /api/categories/search?keyword=... -> tim gan dung theo ten
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm!" });   // 400
            }

            string kw = keyword.Trim();
            //LINQ .Contains() -> SQL: WHERE CategoryName LIKE N'%kw%'
            var result = await _context.Categories
                .AsNoTracking()
                .Where(c => c.CategoryName.Contains(kw))
                .Select(c => new CategoryDto { CategoryId = c.CategoryId, CategoryName = c.CategoryName, Description = c.Description })
                .ToListAsync();

            return Ok(result);
        }

        //POST /api/categories -> them nhom hang
        [HttpPost]
        [Authorize(Roles = AppRoles.AdminOrWarehouse)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateUpdateDto dto)
        {
            //[ApiController] da tu kiem tra [Required]/[StringLength] tren DTO va tra 400 neu sai

            var entity = new Category
            {
                CategoryName = dto.CategoryName.Trim(),
                Description = dto.Description?.Trim()
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            var result = new CategoryDto { CategoryId = entity.CategoryId, CategoryName = entity.CategoryName, Description = entity.Description };
            return CreatedAtAction(nameof(GetById), new { id = entity.CategoryId }, result);
        }

        //PUT /api/categories/{id} -> cap nhat nhom hang
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.AdminOrWarehouse)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateUpdateDto dto)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return NotFound(new { message = "Không tìm thấy nhóm hàng cần sửa!" });
            }

            cat.CategoryName = dto.CategoryName.Trim();
            cat.Description = dto.Description?.Trim();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE /api/categories/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]//chi Admin co quyen duoc xoa
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return NotFound(new { message = "Không tìm thấy nhóm hàng cần xóa!" });
            }

            try
            {
                _context.Categories.Remove(cat);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                //san pham trong nhom da co trong hoa don -> khong xoa duoc
                return BadRequest(new { message = "Không thể xóa: nhóm hàng có sản phẩm đã phát sinh hóa đơn bán hàng!" });
            }
        }

        //kiem tra quyen Admin
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> GetAdminDashboard()
        {
            int totalCategories = await _context.Categories.CountAsync();
            int totalProducts = await _context.Products.CountAsync();
            return Ok(new
            {
                message = "Chào mừng Admin! Bạn có toàn quyền quản trị hệ thống siêu thị mini.",
                totalCategories,
                totalProducts
            });
        }

        //kiem tra quyen ban hang (Admin va Cashier deu goi duoc)
        [HttpGet("staff-pos")]
        [Authorize(Roles = AppRoles.AdminOrCashier)]
        public async Task<IActionResult> GetStaffPos()
        {
            int inStock = await _context.Products.CountAsync(p => p.StockQuantity > 0);
            return Ok(new { message = "Màn hình POS Thu ngân sẵn sàng phục vụ bán hàng.", productsInStock = inStock });
        }
    }
}
