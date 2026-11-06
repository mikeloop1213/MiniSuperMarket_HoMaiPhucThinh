using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Dtos;
using MiniSupermarket.API.Services;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AppRoles.AdminGroup)]
    public class ReportsController : ControllerBase
    {
        // nguong canh bao sap het hang
        private const int LowStockThreshold = 10;

        private readonly SupermarketDbContext _context;
        private readonly AdoNetService _ado;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(SupermarketDbContext context, AdoNetService ado, ILogger<ReportsController> logger)
        {
            _context = context;
            _ado = ado;
            _logger = logger;
        }

        // GET /api/reports/dashboard-summary (4 the chi so tong quan)
        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            var todayOrders = _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow);

            var summary = new DashboardSummaryDto
            {
                TodayRevenue = await todayOrders.SumAsync(o => (decimal?)o.FinalAmount) ?? 0,
                TotalOrders = await _context.Orders.AsNoTracking().CountAsync(),
                NewCustomers = await _context.Customers.AsNoTracking()
                    .CountAsync(c => c.CreatedDate >= today && c.CreatedDate < tomorrow),
                LowStockProducts = await _context.Products.AsNoTracking()
                    .CountAsync(p => p.StockQuantity <= LowStockThreshold)
            };

            return Ok(summary);
        }

        // GET /api/reports/revenue-by-date?fromDate=2026-11-01&toDate=2026-11-30
        [HttpGet("revenue-by-date")]
        public async Task<IActionResult> GetRevenueByDate([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            DateTime to = (toDate ?? DateTime.Today).Date;
            DateTime from = (fromDate ?? to.AddDays(-6)).Date;

            if (from > to)
            {
                return BadRequest(new { message = "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!" });
            }
            if ((to - from).TotalDays > 366)
            {
                return BadRequest(new { message = "Khoảng thời gian tối đa là 1 năm!" });
            }

            try
            {
                var rows = await _ado.GetDailyRevenueAsync(from, to);

                // bo sung nhung ngay khong co doanh thu de bieu do lien mach
                var byDate = rows.ToDictionary(r => r.SaleDate.Date);
                var result = new List<DailyRevenueDto>();
                for (DateTime d = from; d <= to; d = d.AddDays(1))
                {
                    result.Add(byDate.TryGetValue(d, out var row)
                        ? row
                        : new DailyRevenueDto { SaleDate = d, TotalOrders = 0, Revenue = 0 });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chạy Stored Procedure sp_DoanhThuTheoNgay");
                return StatusCode(500, new { message = "Không lấy được doanh thu theo ngày. Kiểm tra Stored Procedure sp_DoanhThuTheoNgay!" });
            }
        }

        // GET /api/reports/revenue-by-product?fromDate=..&toDate=.. (doanh thu tung san pham theo ngay)
        [HttpGet("revenue-by-product")]
        public async Task<IActionResult> GetRevenueByProduct([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            DateTime to = (toDate ?? DateTime.Today).Date;
            DateTime from = (fromDate ?? to.AddDays(-6)).Date;

            if (from > to)
            {
                return BadRequest(new { message = "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!" });
            }

            DateTime toExclusive = to.AddDays(1);

            var rows = await _context.OrderDetails
                .AsNoTracking()
                .Where(d => d.Order!.OrderDate >= from && d.Order.OrderDate < toExclusive)
                .GroupBy(d => new { Day = d.Order!.OrderDate.Date, d.ProductId, d.Product!.ProductName })
                .Select(g => new ProductDailyRevenueDto
                {
                    SaleDate = g.Key.Day,
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(d => d.Quantity),
                    Revenue = g.Sum(d => d.Quantity * d.UnitPrice)
                })
                .OrderBy(r => r.SaleDate)
                .ToListAsync();

            return Ok(rows);
        }

        // GET /api/reports/top-products?top=5 (mat hang ban chay nhat)
        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int top = 5)
        {
            if (top < 1 || top > 50)
            {
                return BadRequest(new { message = "Số lượng mặt hàng cần thống kê phải từ 1 đến 50!" });
            }

            var topSales = await _context.OrderDetails
                .AsNoTracking()
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(top)
                .ToListAsync();

            var ids = topSales.Select(x => x.ProductId).ToList();
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => ids.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            var result = topSales.Select(x => new TopProductDto
            {
                ProductId = x.ProductId,
                ProductName = products.TryGetValue(x.ProductId, out var p) ? p.ProductName : $"SP #{x.ProductId}",
                CategoryName = p?.Category?.CategoryName ?? string.Empty,
                StockQuantity = p?.StockQuantity ?? 0,
                TotalQuantity = x.TotalQuantity,
                TotalRevenue = x.TotalRevenue
            }).ToList();

            return Ok(result);
        }
    }
}
