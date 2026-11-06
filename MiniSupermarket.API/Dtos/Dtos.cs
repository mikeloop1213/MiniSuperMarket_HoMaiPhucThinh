using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Dtos
{
    // phan trang

    // Ket qua tra ve co phan trang
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } 
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
    }

    // nhom hang

    // DTO tra ve cho client
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // DTO nhan du lieu khi Them / Sua nhom hang
    public class CategoryCreateUpdateDto
    {
        [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên nhóm hàng không vượt quá 100 ký tự")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }

    // san pham

    // DTO tra ve cho client (co ten nhom hang lay tu .Include - TC26)
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    // DTO nhan du lieu khi THEM san pham (client khong gui ProductId)
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Mã vạch không được để trống")]
        [StringLength(50)]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        // [TC20] Gia ban > 0
        [Range(typeof(decimal), "1", "1000000000", ErrorMessage = "Giá bán phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhóm hàng")]
        public int CategoryId { get; set; }
    }

    // DTO nhan du lieu khi SUA san pham
    public class UpdateProductDto : CreateProductDto
    {
        public int ProductId { get; set; }
    }

    // khach hang

    // DTO tra ve cho client
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int RewardPoints { get; set; }
        public string MembershipRank { get; set; } = string.Empty;
    }

    // DTO nhan du lieu khi Them / Sua khach hang
    public class CustomerCreateUpdateDto
    {
        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Điểm tích lũy không được âm")]
        public int RewardPoints { get; set; }

        [StringLength(50)]
        public string? MembershipRank { get; set; }
    }

    // tai khoan & vai tro

    // DTO nhan du lieu dang nhap
    public class LoginRequestDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    // DTO tra ve thong tin tai khoan (KHONG co Password)
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // DTO tao tai khoan
    public class CreateUserDto
    {
        [Required, StringLength(50)] public string Username { get; set; } = string.Empty;
        [Required, StringLength(100)] public string Password { get; set; } = string.Empty;
        [StringLength(100)] public string FullName { get; set; } = string.Empty;
        [StringLength(30)] public string Role { get; set; } = AppRoles.Cashier;
    }

    // DTO vai tro (dung cho ca nhan va tra)
    public class RoleDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên vai trò không được để trống!")]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }

    // thanh toan & bao cao

    // DTO yeu cau thanh toan gui tu WinForms POS
    public class CheckoutRequestDto
    {
        public string CashierUsername { get; set; } = string.Empty;

        [StringLength(15)]
        public string? CustomerPhone { get; set; }

        [Required, MinLength(1, ErrorMessage = "Giỏ hàng không có sản phẩm nào!")]
        public List<CheckoutItemDto> Items { get; set; } = new();

        // true = khach dung 50 diem de tru 50.000 d vao hoa don
        public bool UsePoints { get; set; }
    }

    // DTO mot dong hang trong gio
    public class CheckoutItemDto
    {
        [Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")] public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Ket qua thanh toan tra ve tu AdoNetService
    public class CheckoutResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public int PointsUsed { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public int AddedPoints { get; set; }
    }

    // Ket qua bao cao doanh thu ca (doc tu Stored Procedure - TC34)
    public class ShiftReportDto
    {
        public string ReportDate { get; set; } = string.Empty;
        public string? CashierUsername { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public string BestSeller { get; set; } = string.Empty;
    }

    // DTO chi tiet hoa don tra ve
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public int PointsUsed { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public List<OrderDetailDto> Details { get; set; } = new();
    }

    // mot dong trong danh sach hoa don
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = string.Empty;
        public string CashierName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
    }

    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }

    // bao cao tong quan

    // 4 the chi so tren Dashboard
    public class DashboardSummaryDto
    {
        public decimal TodayRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewCustomers { get; set; }
        public int LowStockProducts { get; set; }
    }

    // doanh thu 1 ngay (ve bieu do cot)
    public class DailyRevenueDto
    {
        public DateTime SaleDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
    }

    // doanh thu tung san pham theo ngay (bieu do cot chong)
    public class ProductDailyRevenueDto
    {
        public DateTime SaleDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    // mat hang ban chay
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public int StockQuantity { get; set; }
    }
}
