using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public ProductsController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET /api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Select(p => new
                {
                    p.ProductId,
                    p.Barcode,
                    p.ProductName,
                    p.Price,
                    p.StockQuantity,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
                })
                .ToListAsync();

            return Ok(products);
        }

        // 2. GET /api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm!" });
            }

            return Ok(new
            {
                product.ProductId,
                product.Barcode,
                product.ProductName,
                product.Price,
                product.StockQuantity,
                product.CategoryId,
                CategoryName = product.Category != null ? product.Category.CategoryName : string.Empty
            });
        }

        // 3. GET /api/products/barcode/{barcode}
        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return BadRequest(new { message = "Mã vạch không hợp lệ!" });
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Barcode == barcode.Trim());

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm có mã vạch này!" });
            }

            return Ok(new
            {
                product.ProductId,
                product.Barcode,
                product.ProductName,
                product.Price,
                product.StockQuantity,
                product.CategoryId,
                CategoryName = product.Category != null ? product.Category.CategoryName : string.Empty
            });
        }

        // 4. GET /api/products/search
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int? categoryId)
        {
            var query = _context.Products.Include(p => p.Category).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(p => p.ProductName.Contains(kw) || p.Barcode.Contains(kw));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var result = await query.Select(p => new
            {
                p.ProductId,
                p.Barcode,
                p.ProductName,
                p.Price,
                p.StockQuantity,
                p.CategoryId,
                CategoryName = p.Category != null ? p.Category.CategoryName : string.Empty
            }).ToListAsync();

            return Ok(result);
        }

        // 5. POST /api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Barcode) || string.IsNullOrWhiteSpace(dto.ProductName))
            {
                return BadRequest(new { message = "Mã vạch và tên sản phẩm không được trống!" });
            }

            // Kiểm tra trùng barcode
            if (await _context.Products.AnyAsync(p => p.Barcode == dto.Barcode.Trim()))
            {
                return BadRequest(new { message = "Mã vạch này đã tồn tại cho một sản phẩm khác!" });
            }

            var product = new Product
            {
                Barcode = dto.Barcode.Trim(),
                ProductName = dto.ProductName.Trim(),
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, productId = product.ProductId });
        }

        // 6. PUT /api/products/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần cập nhật!" });
            }

            // Kiểm tra barcode trùng nếu đổi
            if (!string.IsNullOrWhiteSpace(dto.Barcode) && dto.Barcode.Trim() != product.Barcode)
            {
                if (await _context.Products.AnyAsync(p => p.Barcode == dto.Barcode.Trim() && p.ProductId != id))
                {
                    return BadRequest(new { message = "Mã vạch này đã bị trùng!" });
                }
                product.Barcode = dto.Barcode.Trim();
            }

            product.ProductName = dto.ProductName.Trim();
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // 7. DELETE /api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa!" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }

    public class CreateProductDto
    {
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
