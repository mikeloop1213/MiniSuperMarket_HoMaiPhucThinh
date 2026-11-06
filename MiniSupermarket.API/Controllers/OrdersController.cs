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
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;
        private readonly AdoNetService _ado;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(SupermarketDbContext context, AdoNetService ado, ILogger<OrdersController> logger)
        {
            _context = context;
            _ado = ado;
            _logger = logger;
        }

        // POST/api/orders/checkout (thanh toan gio hang tu man hinh POS)
        [HttpPost("checkout")]
        [Authorize(Roles = AppRoles.AdminOrCashier)]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            if (request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { success = false, message = "Giỏ hàng không có sản phẩm nào!" });
            }

            // lay ten thu ngan tu Token
            string cashier = User.Identity?.Name ?? request.CashierUsername;

            try
            {
                // tao hoa don + tru kho + tich diem
                CheckoutResult result = await _ado.CheckoutAsync(request, cashier);

                if (!result.Success)
                {
                    // het hang / sai du lieu
                    return BadRequest(new { success = false, message = result.Message });
                }

                return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, new
                {
                    success = true,
                    orderId = result.OrderId,
                    totalAmount = result.TotalAmount,
                    pointsUsed = result.PointsUsed,
                    discountAmount = result.DiscountAmount,
                    finalAmount = result.FinalAmount,
                    addedPoints = result.AddedPoints,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                // loi he thong
                _logger.LogError(ex, "Lỗi khi thanh toán");
                return StatusCode(500, new { success = false, message = "Lỗi máy chủ khi thanh toán, giao dịch đã được hoàn tác!" });
            }
        }

        // GET /api/orders?fromDate=..&toDate=..&keyword=.. (danh sach hoa don khach da mua)
        [HttpGet]
        [Authorize(Roles = AppRoles.AdminOrCashier)]
        public async Task<IActionResult> GetOrders([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? keyword)
        {
            DateTime to = (toDate ?? DateTime.Today).Date;
            DateTime from = (fromDate ?? to).Date;
            if (from > to)
            {
                return BadRequest(new { message = "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!" });
            }
            DateTime toExclusive = to.AddDays(1);

            var query = _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate >= from && o.OrderDate < toExclusive);

            // tim theo ten / sdt khach hang
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(o => o.Customer != null &&
                    (o.Customer.CustomerName.Contains(kw) || o.Customer.PhoneNumber.Contains(kw)));
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderSummaryDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CashierUsername = o.User != null ? o.User.Username : string.Empty,
                    CashierName = o.User != null ? o.User.FullName : string.Empty,
                    CustomerName = o.Customer != null ? o.Customer.CustomerName : "Khách vãng lai",
                    CustomerPhone = o.Customer != null ? o.Customer.PhoneNumber : null,
                    ItemCount = o.OrderDetails.Sum(d => d.Quantity),
                    TotalAmount = o.TotalAmount,
                    DiscountAmount = o.DiscountAmount,
                    FinalAmount = o.FinalAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderStatus = o.OrderStatus
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET/api/orders/{id} (xem chi tiet 1 hoa don)
        [HttpGet("{id:int}")]
        [Authorize(Roles = AppRoles.AdminOrCashier)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy hóa đơn!" });
            }

            var dto = new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CashierUsername = order.User != null ? order.User.Username : string.Empty,
                CustomerName = order.Customer?.CustomerName,
                CustomerPhone = order.Customer?.PhoneNumber,
                TotalAmount = order.TotalAmount,
                PointsUsed = order.PointsUsed,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                Details = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product != null ? d.Product.ProductName : string.Empty,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    LineTotal = d.UnitPrice * d.Quantity
                }).ToList()
            };
            return Ok(dto);
        }

        // bao cao doanh thu ca
        [HttpGet("report")]
        [Authorize(Roles = AppRoles.AdminOrCashier)]
        public async Task<IActionResult> GetQuickReport([FromQuery] DateTime? date, [FromQuery] string? cashier)
        {
            DateTime targetDate = date?.Date ?? DateTime.Today;

            try
            {
                ShiftReportDto report = await _ado.GetShiftReportAsync(targetDate, cashier);

                return Ok(new
                {
                    reportDate = report.ReportDate,
                    cashier = report.CashierUsername,
                    totalOrders = report.TotalOrders,
                    totalRevenue = report.TotalRevenue,
                    bestSeller = report.BestSeller
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chạy Stored Procedure báo cáo");
                return StatusCode(500, new { message = "Không chạy được báo cáo. Kiểm tra Stored Procedure sp_BaoCaoDoanhThuCa!" });
            }
        }
    }
}
