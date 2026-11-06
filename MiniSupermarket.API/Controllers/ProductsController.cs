using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using MiniSupermarket.API.Models;
using MiniSupermarket.API.Services;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;
        private readonly AdoNetService _ado;

        public ProductsController(SupermarketDbContext context, AdoNetService ado)
        {
            _context = context;
            _ado = ado;
        }

        // GET/api/products?page=1&pageSize=10 (danh sach san pham co phan trang)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Chuan hoa tham so
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _context.Products.AsNoTracking();

            int totalItems = await query.CountAsync();

            var items = await query
                .Include(p => p.Category)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Barcode = p.Barcode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
                })
                .ToListAsync();

            return Ok(new PagedResult<ProductDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = items
            });
        }

        // GET /api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Barcode = p.Barcode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm!" });
            }
            return Ok(product);
        }

        // GET/api/products/barcode/{barcode} (ADO.NET)
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new { message = "Mã vạch không hợp lệ!" });
            }

            var product = await _ado.GetProductByBarcodeAsync(barcode);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm có mã vạch này!" });
            }
            return Ok(product);
        }

        // GET /api/products/search?keyword=...&categoryId=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int? categoryId)
        {
            // IQueryable: chua chay SQL, cac dieu kien duoc ghep dan
            var query = _context.Products.AsNoTracking().Include(p => p.Category).AsQueryable();

            // Tim gan dung theo ten hoac ma vach
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(p => p.ProductName.Contains(kw) || p.Barcode.Contains(kw));
            }

            // Loc theo nhom hang (0 hoac null = tat ca)
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var result = await query
                .OrderBy(p => p.ProductId)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Barcode = p.Barcode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET /api/products/low-stock?threshold=10 (san pham sap het hang)
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        {
            if (threshold < 0)
            {
                return BadRequest(new { message = "Ngưỡng tồn kho không hợp lệ!" });
            }

            var items = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.StockQuantity <= threshold)
                .OrderBy(p => p.StockQuantity)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Barcode = p.Barcode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
                })
                .ToListAsync();

            return Ok(items);
        }

        // POST /api/products (them san pham)
        [HttpPost]
        [Authorize(Roles = AppRoles.AdminOrWarehouse)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            // kiem tra ma vach

            string barcode = dto.Barcode.Trim();

            // kiem tra ma vach bi trung
            if (await _context.Products.AnyAsync(p => p.Barcode == barcode))
            {
                return BadRequest(new { message = "Mã vạch này đã tồn tại cho một sản phẩm khác!" });   // 400
            }

            // kiem tra nhom hang ton tai
            if (!await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId))
            {
                return BadRequest(new { message = "Nhóm hàng không tồn tại!" });
            }

            var product = new Product
            {
                Barcode = barcode,
                ProductName = dto.ProductName.Trim(),
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // /api/products/{id}
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId },
                new { success = true, productId = product.ProductId });
        }

        // PUT /api/products/{id} (cap nhat san pham)
        [HttpPut("{id:int}")]
        [Authorize(Roles = AppRoles.AdminOrWarehouse)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần cập nhật!" });
            }

            // Kiem tra barcode trung neu doi ma vach
            string barcode = dto.Barcode.Trim();
            if (barcode != product.Barcode &&
                await _context.Products.AnyAsync(p => p.Barcode == barcode && p.ProductId != id))
            {
                return BadRequest(new { message = "Mã vạch này đã bị trùng!" });
            }

            if (!await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId))
            {
                return BadRequest(new { message = "Nhóm hàng không tồn tại!" });
            }

            product.Barcode = barcode;
            product.ProductName = dto.ProductName.Trim();
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // DELETE /api/products/{id} (xoa san pham)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.AdminGroup)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa!" });
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (DbUpdateException)
            {
                // san pham da co trong OrderDetails (khoa ngoai Restrict)
                return BadRequest(new { message = "Không thể xóa: sản phẩm đã phát sinh hóa đơn bán hàng!" });
            }
        }
    }
}
