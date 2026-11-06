using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms {
    // lop tinh luu phien dang nhap
    public static class SessionManager {
        // luu tru JWT Token nhan tu Server
        public static string JwtToken { get; set; } = string.Empty;

        // luu tru ten tai khoan nguoi dung
        public static string CurrentUsername { get; set; } = string.Empty;

        // luu tru ho ten nguoi dung
        public static string CurrentFullName { get; set; } = string.Empty;

        // luu tru vai tro nguoi dung
        public static string CurrentRole { get; set; } = string.Empty;

        // ten vai tro trong CSDL
        public const string RoleAdmin = "Admin";
        public const string RoleCashier = "Cashier";
        public const string RoleWarehouse = "Warehouse";

        // quy doi vai tro ve nhom quyen giao dien: ADMIN / WAREHOUSE / CASHIER
        public static string ToRoleKey(string role)
        {
            string r = (role ?? string.Empty).Trim();
            if (r.Equals(RoleAdmin, StringComparison.OrdinalIgnoreCase)) return "ADMIN";
            if (r.Equals(RoleWarehouse, StringComparison.OrdinalIgnoreCase)) return "WAREHOUSE";
            if (r.Equals(RoleCashier, StringComparison.OrdinalIgnoreCase)) return "CASHIER";
            return string.Empty;
        }

        public static bool IsAdmin => ToRoleKey(CurrentRole) == "ADMIN";
        public static bool IsCashier => ToRoleKey(CurrentRole) == "CASHIER";
        public static bool IsWarehouse => ToRoleKey(CurrentRole) == "WAREHOUSE";

        // thu ngan chi duoc xem, khong duoc them / sua / xoa
        public static bool BlockCashierEdit(IWin32Window? owner)
        {
            if (!IsCashier) return false;
            MessageBox.Show(owner, "Chỉ Admin mới có quyền chỉnh sửa dữ liệu!\nTài khoản Thu ngân chỉ được xem thông tin.",
                "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        // xoa toan bo thong tin phien khi Dang xuat
        public static void ClearSession() {
            PosDraft.Clear();
            JwtToken = string.Empty;
            CurrentUsername = string.Empty;
            CurrentFullName = string.Empty;
            CurrentRole = string.Empty;
        }
    }

    // gio hang POS dang lam do (giu lai khi chuyen sang man hinh khac roi quay lai)
    public static class PosDraft {
        public static List<CartItemDto> Cart { get; } = new();
        public static string CustomerPhone { get; set; } = string.Empty;
        public static bool UsePoints { get; set; }

        public static void Clear() {
            Cart.Clear();
            CustomerPhone = string.Empty;
            UsePoints = false;
        }
    }

    // lop goi API tap trung
    public static class ApiClientService {
        // Dung 1 HttpClient static cho ca ung dung
        private static readonly HttpClient _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };
        // xac thuc Token dang nhap
        public static HttpClient Client {
            get {
                if (!string.IsNullOrEmpty(SessionManager.JwtToken)) {

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
                } else {

                    _client.DefaultRequestHeaders.Authorization = null;
                }
                return _client;
            }
        }

        // ham goi API dang nhap lay Token va thong tin User
        public static async Task<bool> LoginAsync(string username, string password) {
            var loginObj = new { Username = username, Password = password };
            var response = await Client.PostAsJsonAsync("auth/login", loginObj);
            if (response.IsSuccessStatusCode) {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;
                // luu token & vai tro vao bien tinh
                SessionManager.JwtToken = root.GetProperty("token").GetString() ?? string.Empty;
                SessionManager.CurrentRole = root.GetProperty("role").GetString() ?? string.Empty;
                SessionManager.CurrentUsername = root.TryGetProperty("username", out var u) ? u.GetString() ?? username : username;
                SessionManager.CurrentFullName = root.TryGetProperty("fullName", out var fn) ? fn.GetString() ?? username : username;
                return true;
            }
            return false;
        }

        public static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response, string defaultMessage) {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return "Phiên làm việc hết hạn hoặc chưa đăng nhập! Vui lòng đăng nhập lại.";
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return "Tài khoản của bạn không có quyền thực hiện chức năng này!";
            try {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("message", out var msg) && msg.ValueKind == JsonValueKind.String)
                    return msg.GetString() ?? defaultMessage;

                if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object) {
                    var lines = new List<string>();
                    foreach (var field in errors.EnumerateObject())
                        foreach (var e in field.Value.EnumerateArray())
                            lines.Add("- " + e.GetString());
                    if (lines.Count > 0) return string.Join(Environment.NewLine, lines);
                }
            } catch { /* Noi dung khong phai JSON -> dung thong bao mac dinh */ }
            return defaultMessage;
        }

        // ham goi API GET co gan kem Bearer Token bao mat, tra ve chuoi JSON
        public static async Task<string> GetDataWithTokenAsync(string endpoint) {
            var response = await Client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsStringAsync();
            } else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {

                throw new Exception("Phiên làm việc hết hạn hoặc chưa đăng nhập!");
            }
            throw new Exception("Lỗi khi gọi dữ liệu từ Server.");
        }
    }

    // DTO hien thi san pham len DataGridView
    public class ProductDto {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    // ket qua phan trang nhan tu GET api/products?page=..&pageSize=..
    public class PagedResultDto<T> {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();
    }

    // mot dong hang trong gio POS
    public class CartItemDto {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    // DTO tai khoan nhan vien
    public class UserDto {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    // 4 the chi so Dashboard
    public class DashboardSummaryDto {
        public decimal TodayRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int NewCustomers { get; set; }
        public int LowStockProducts { get; set; }
    }

    // doanh thu theo ngay
    public class DailyRevenueDto {
        public DateTime SaleDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
    }

    // doanh thu tung san pham theo ngay
    public class ProductDailyRevenueDto {
        public DateTime SaleDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    // mat hang ban chay
    public class TopProductDto {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public int StockQuantity { get; set; }
    }
}
