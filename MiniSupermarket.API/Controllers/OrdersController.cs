using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        // Lưu trữ lịch sử hóa đơn trong bộ nhớ phục vụ báo cáo nhanh
        private static readonly List<OrderRecord> _ordersHistory = new()
        {
            new OrderRecord
            {
                OrderId = 1001,
                OrderDate = DateTime.Today.AddHours(8).AddMinutes(30),
                CashierUsername = "cashier01",
                CustomerPhone = "0903456789",
                TotalAmount = 145000,
                Items = new()
                {
                    new OrderItemRecord { ProductId = 1, ProductName = "Snack khoai tây Lay's", Quantity = 3, UnitPrice = 16000 },
                    new OrderItemRecord { ProductId = 2, ProductName = "Nước ngọt Coca-Cola Sleek", Quantity = 5, UnitPrice = 11000 },
                    new OrderItemRecord { ProductId = 5, ProductName = "Mì Hảo Hảo tôm chua cay", Quantity = 9, UnitPrice = 4500 }
                }
            },
            new OrderRecord
            {
                OrderId = 1002,
                OrderDate = DateTime.Today.AddHours(9).AddMinutes(15),
                CashierUsername = "cashier02",
                CustomerPhone = "0918765432",
                TotalAmount = 230000,
                Items = new()
                {
                    new OrderItemRecord { ProductId = 6, ProductName = "Cà phê sữa Nescafé 3in1", Quantity = 6, UnitPrice = 15000 },
                    new OrderItemRecord { ProductId = 11, ProductName = "Sữa tắm dưỡng ẩm Romano", Quantity = 2, UnitPrice = 38000 },
                    new OrderItemRecord { ProductId = 4, ProductName = "Sữa tươi tiệt trùng Vinamilk", Quantity = 6, UnitPrice = 10000 }
                }
            },
            new OrderRecord
            {
                OrderId = 1003,
                OrderDate = DateTime.Today.AddHours(10).AddMinutes(45),
                CashierUsername = "cashier01",
                CustomerPhone = "",
                TotalAmount = 85000,
                Items = new()
                {
                    new OrderItemRecord { ProductId = 7, ProductName = "Cơm nắm cá hồi nướng Onigiri", Quantity = 3, UnitPrice = 18000 },
                    new OrderItemRecord { ProductId = 3, ProductName = "Nước khoáng thiên nhiên La Vie", Quantity = 5, UnitPrice = 6000 }
                }
            }
        };

        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. POST /api/orders/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new { message = "Giỏ hàng không có sản phẩm nào!" });
            }

            decimal totalAmount = 0;
            var orderItems = new List<OrderItemRecord>();

            // Cập nhật tồn kho sản phẩm trong CSDL
            foreach (var item in request.Items)
            {
                var prod = await _context.Products.FindAsync(item.ProductId);
                if (prod != null)
                {
                    if (prod.StockQuantity >= item.Quantity)
                    {
                        prod.StockQuantity -= item.Quantity;
                    }
                    else
                    {
                        prod.StockQuantity = 0;
                    }
                    totalAmount += item.Quantity * item.UnitPrice;
                    orderItems.Add(new OrderItemRecord
                    {
                        ProductId = prod.ProductId,
                        ProductName = prod.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }
                else
                {
                    totalAmount += item.Quantity * item.UnitPrice;
                    orderItems.Add(new OrderItemRecord
                    {
                        ProductId = item.ProductId,
                        ProductName = $"Sản phẩm #{item.ProductId}",
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }
            }

            // Tích điểm cho khách hàng nếu có SĐT
            if (!string.IsNullOrWhiteSpace(request.CustomerPhone))
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == request.CustomerPhone.Trim());
                if (customer != null)
                {
                    int addedPoints = (int)(totalAmount / 10000); // 10k VND = 1 điểm
                    customer.RewardPoints += addedPoints;
                    if (customer.RewardPoints >= 500) customer.MembershipRank = "Kim cương";
                    else if (customer.RewardPoints >= 200) customer.MembershipRank = "Vàng";
                    else if (customer.RewardPoints >= 80) customer.MembershipRank = "Bạc";
                }
            }

            await _context.SaveChangesAsync();

            int nextOrderId = _ordersHistory.Count > 0 ? _ordersHistory.Max(o => o.OrderId) + 1 : 1001;
            var orderRecord = new OrderRecord
            {
                OrderId = nextOrderId,
                OrderDate = DateTime.Now,
                CashierUsername = request.CashierUsername,
                CustomerPhone = request.CustomerPhone,
                TotalAmount = totalAmount,
                Items = orderItems
            };
            _ordersHistory.Add(orderRecord);

            return Ok(new
            {
                success = true,
                orderId = nextOrderId,
                totalAmount = totalAmount,
                message = "Thanh toán đơn hàng thành công!"
            });
        }

        // 2. GET /api/orders/report
        [HttpGet("report")]
        public IActionResult GetQuickReport([FromQuery] DateTime? date)
        {
            DateTime targetDate = date?.Date ?? DateTime.Today;

            var dayOrders = _ordersHistory
                .Where(o => o.OrderDate.Date == targetDate)
                .ToList();

            int totalOrders = dayOrders.Count;
            decimal totalRevenue = dayOrders.Sum(o => o.TotalAmount);

            // Tìm mặt hàng bán chạy nhất
            var bestSellerGroup = dayOrders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .Select(g => new { ProductName = g.Key, TotalSold = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.TotalSold)
                .FirstOrDefault();

            string bestSeller = bestSellerGroup != null 
                ? $"{bestSellerGroup.ProductName} ({bestSellerGroup.TotalSold} cái)" 
                : "Chưa có dữ liệu";

            return Ok(new
            {
                reportDate = targetDate.ToString("yyyy-MM-dd"),
                totalOrders = totalOrders,
                totalRevenue = totalRevenue,
                bestSeller = bestSeller
            });
        }
    }

    public class CheckoutRequestDto
    {
        public string CashierUsername { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public List<CheckoutItemDto> Items { get; set; } = new();
    }

    public class CheckoutItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderRecord
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemRecord> Items { get; set; } = new();
    }

    public class OrderItemRecord
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
