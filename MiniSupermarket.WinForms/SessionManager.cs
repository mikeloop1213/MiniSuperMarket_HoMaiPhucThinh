using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSupermarket.WinForms {
    public static class SessionManager {
        // Lưu trữ JWT Token nhận từ Server
        public static string JwtToken { get; set; } = string.Empty;

        // Lưu trữ tên tài khoản người dùng
        public static string CurrentUsername { get; set; } = string.Empty;

        // Lưu trữ họ tên đầy đủ người dùng
        public static string CurrentFullName { get; set; } = string.Empty;

        // Lưu trữ vai trò người dùng (Admin / Cashier / Warehouse) để phân quyền giao diện
        public static string CurrentRole { get; set; } = string.Empty;

        public static void ClearSession() {
            JwtToken = string.Empty;
            CurrentUsername = string.Empty;
            CurrentFullName = string.Empty;
            CurrentRole = string.Empty;
        }
    }

    public static class ApiClientService {
        private static readonly HttpClient _client = new HttpClient {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };

        // HttpClient dùng chung được cấu hình tự động Bearer Token
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

        // Hàm gọi API đăng nhập lấy Token và thông tin User
        public static async Task<bool> LoginAsync(string username, string password) {
            var loginObj = new { Username = username, Password = password };
            var response = await Client.PostAsJsonAsync("auth/login", loginObj);
            if (response.IsSuccessStatusCode) {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;
                SessionManager.JwtToken = root.GetProperty("token").GetString() ?? string.Empty;
                SessionManager.CurrentRole = root.GetProperty("role").GetString() ?? string.Empty;
                SessionManager.CurrentUsername = root.TryGetProperty("username", out var u) ? u.GetString() ?? username : username;
                SessionManager.CurrentFullName = root.TryGetProperty("fullName", out var fn) ? fn.GetString() ?? username : username;
                return true;
            }
            return false;
        }

        // Hàm gọi API lấy dữ liệu có gắn kèm Bearer Token bảo mật
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

    // Các DTO dùng chung cho các Form
    public class ProductDto {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CartItemDto {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    public class UserDto {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
