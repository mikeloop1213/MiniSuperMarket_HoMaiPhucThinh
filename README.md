# 🛒 Mini ZenMart – Hệ thống Quản lý Bán lẻ & Tồn kho Siêu thị Mini

Đồ án môn học xây dựng phần mềm quản lý siêu thị mini gồm **Web API (ASP.NET Core 8)** làm backend và **ứng dụng WinForms (.NET 8)** làm giao diện cho nhân viên. Hệ thống hỗ trợ bán hàng tại quầy (POS), quản lý danh mục, sản phẩm, tồn kho, khách hàng thân thiết, phân quyền theo vai trò và báo cáo doanh thu.

---

## 📌 Mục lục

1. [Tính năng chính](#-tính-năng-chính)
2. [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
3. [Cấu trúc thư mục](#-cấu-trúc-thư-mục)
4. [Cài đặt & chạy chương trình](#-cài-đặt--chạy-chương-trình)
5. [Tài khoản đăng nhập mẫu](#-tài-khoản-đăng-nhập-mẫu)
6. [Phân quyền theo vai trò](#-phân-quyền-theo-vai-trò)
7. [Danh sách API](#-danh-sách-api)
8. [Cơ sở dữ liệu](#-cơ-sở-dữ-liệu)
9. [Xử lý lỗi thường gặp](#-xử-lý-lỗi-thường-gặp)

---

## ✨ Tính năng chính

### Bán hàng tại quầy (POS)
- Quét / nhập **mã vạch** để thêm sản phẩm vào giỏ, tự **cộng dồn số lượng** khi quét trùng.
- Tự tính **tổng tiền** và **tiền thối lại**, có nút mệnh giá nhanh (50.000 – 500.000) và phím tắt **F9** để thanh toán.
- Tra cứu khách hàng thành viên theo **số điện thoại**, hiển thị hạng thành viên và điểm tích lũy.
- **Đổi điểm thưởng:** khách có từ 50 điểm được chọn *"Dùng 50 điểm trừ 50.000 đ vào hóa đơn"*.
- Thanh toán được bọc trong **một SqlTransaction**: lưu hóa đơn, trừ tồn kho, trừ / cộng điểm. Nếu kho không đủ hàng thì **tự động Rollback**, không lưu hóa đơn rác.

### Quản lý dữ liệu
- **Danh mục nhóm hàng:** thêm, sửa, xóa, tìm kiếm.
- **Sản phẩm & tồn kho:** danh sách có **phân trang**, tìm theo tên / mã vạch, lọc theo nhóm hàng.
- **Cảnh báo hàng sắp hết:** dòng có tồn kho ≤ 10 được **tô nền đỏ**, kèm huy hiệu đếm số mặt hàng sắp hết.
- **Khách hàng:** quản lý thông tin, điểm tích lũy, hạng thành viên (Chuẩn / Bạc / Vàng / Kim cương).
- **Tài khoản & vai trò:** tạo tài khoản, khóa / mở khóa, đặt lại mật khẩu.

### Báo cáo & thống kê
- **Dashboard** với 4 thẻ chỉ số: doanh thu hôm nay, tổng số hóa đơn, khách hàng mới, số mặt hàng sắp hết.
- **Biểu đồ cột** doanh thu theo ngày trong khoảng thời gian tùy chọn (Từ ngày → Đến ngày).
- **Top 5 sản phẩm bán chạy nhất.**
- **Báo cáo doanh thu ca** bằng Stored Procedure.
- **Xuất Excel (.xlsx) / CSV** danh mục sản phẩm và doanh thu.

---

## 🧰 Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Backend | ASP.NET Core Web API (.NET 8) |
| ORM | Entity Framework Core 8 (Code First, Migrations, Seed Data) |
| Truy cập dữ liệu nhanh | ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`, `SqlTransaction`) |
| Cơ sở dữ liệu | SQL Server / LocalDB |
| Bảo mật | JWT Bearer Token, phân quyền `[Authorize(Roles = ...)]` |
| Tài liệu & kiểm thử API | Swagger UI (Swashbuckle) |
| Giao diện | Windows Forms (.NET 8) |
| Biểu đồ | `WinForms.DataVisualization` (`System.Windows.Forms.DataVisualization.Charting`) |
| Gọi API | `HttpClient` dùng chung qua `ApiClientService` |
| IDE | Visual Studio 2022 |

---

## 📁 Cấu trúc thư mục

```
MiniSuperMarket_HoMaiPhucThinh/
├── MiniSuperMarket_HoMaiPhucThinh-buoi4.sln
│
├── MiniSupermarket.API/                  # Backend Web API
│   ├── Controllers/
│   │   ├── AuthController.cs             # Đăng nhập, cấp JWT
│   │   ├── CategoriesController.cs       # Nhóm hàng
│   │   ├── ProductsController.cs         # Sản phẩm, mã vạch, phân trang, hàng sắp hết
│   │   ├── CustomersController.cs        # Khách hàng
│   │   ├── OrdersController.cs           # Thanh toán, chi tiết hóa đơn, báo cáo ca
│   │   ├── ReportsController.cs          # Dashboard, biểu đồ, Top 5
│   │   ├── UsersController.cs            # Tài khoản nhân viên
│   │   └── RolesController.cs            # Vai trò
│   ├── Data/SupermarketDbContext.cs      # DbContext, quan hệ, Seed Data
│   ├── Models/                           # Entity: User, Category, Product, Customer, Order, OrderDetail, Role
│   ├── Dtos/Dtos.cs                      # Các lớp DTO nhận / trả dữ liệu
│   ├── Services/AdoNetService.cs         # ADO.NET: mã vạch, Stored Procedure, Transaction
│   ├── Database/                         # Script SQL (Stored Procedure, chuyển đổi role)
│   ├── Migrations/                       # EF Core Migrations
│   ├── AppRoles.cs                       # Hằng số tên vai trò
│   ├── appsettings.json                  # Chuỗi kết nối, khóa JWT
│   └── Program.cs                        # Đăng ký dịch vụ, CORS, Swagger, JWT
│
└── MiniSupermarket.WinForms/             # Ứng dụng giao diện
    ├── FormLogin.cs                      # Đăng nhập
    ├── FormMainShell.cs                  # Khung chính: menu trái, tiêu đề, vùng làm việc
    ├── FormDashboard.cs                  # Thẻ KPI, biểu đồ, Top 5
    ├── FormPOS.cs                        # Quầy bán hàng
    ├── FormCategoryManagement.cs         # Danh mục
    ├── FormProductManagement.cs          # Sản phẩm & tồn kho
    ├── FormCustomerManagement.cs         # Khách hàng
    ├── FormUserManagement.cs             # Tài khoản
    ├── FormRoleManagement.cs             # Vai trò
    ├── FormQuickReport.cs                # Báo cáo doanh thu ca
    ├── ExportHelper.cs                   # Xuất Excel / CSV
    └── SessionManager.cs                 # Lưu phiên đăng nhập + ApiClientService
```

---

## 🚀 Cài đặt & chạy chương trình

### Yêu cầu
- Visual Studio 2022 (workload **ASP.NET** và **.NET Desktop Development**)
- .NET 8 SDK
- SQL Server hoặc SQL Server Express LocalDB
- SQL Server Management Studio (khuyến nghị)

### Bước 1 – Cấu hình chuỗi kết nối
Mở `MiniSupermarket.API/appsettings.json` và chỉnh lại tên server / database nếu cần:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ZenMart_HoMaiPhucThinh;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### Bước 2 – Tạo cơ sở dữ liệu
- **Database mới:** chỉ cần chạy API, chương trình sẽ tự gọi `Migrate()` để tạo bảng và nạp dữ liệu mẫu (15 nhóm hàng, 15 sản phẩm, 15 khách hàng). Hoặc chạy thủ công trong *Package Manager Console*:
  ```powershell
  Update-Database
  ```
- **Database đã có sẵn bảng:** API tự nhận biết và đánh dấu migration là đã áp dụng, **không tạo lại hay xóa bảng**. Nếu dữ liệu vai trò đang là tiếng Việt ("Thu ngân", "Quản lý kho"...), chạy script `MiniSupermarket.API/Database/ChuyenVe3Role.sql` trong SSMS để đổi về Admin / Cashier / Warehouse / Customer.

Stored Procedure `sp_BaoCaoDoanhThuCa` và `sp_DoanhThuTheoNgay` được tự động tạo / cập nhật mỗi lần API khởi động.

### Bước 3 – Chạy cùng lúc 2 project
1. Chuột phải vào **Solution** → **Configure Startup Projects...**
2. Chọn **Multiple startup projects**, đặt cả `MiniSupermarket.API` và `MiniSupermarket.WinForms` là **Start**. Nên để API chạy trước.
3. Bấm **Start (F5)**.

API chạy tại `http://localhost:5000` (Swagger: `http://localhost:5000/swagger`). WinForms gọi API qua địa chỉ này.

### Bước 4 – Kiểm thử API trên Swagger
1. Gọi `POST /api/auth/login` với tài khoản mẫu để lấy `token`.
2. Bấm nút ổ khóa **Authorize**, dán chuỗi token (có hoặc không kèm chữ `Bearer`).
3. Gọi các API cần đăng nhập. Chưa đăng nhập sẽ nhận **401 Unauthorized**, sai quyền sẽ nhận **403 Forbidden**.

---

## 🔑 Tài khoản đăng nhập mẫu

Mật khẩu mặc định của tất cả tài khoản: **`123456`**

| Tài khoản | Vai trò | Ghi chú |
|---|---|---|
| `admin` | Admin | Toàn quyền |
| `cashier` | Cashier | Thu ngân |
| `warehouse` | Warehouse | Thủ kho |
| `customer` | Customer | Khách hàng |

Khi khởi động, API tự tạo 4 tài khoản trên nếu database chưa có (không ghi đè tài khoản đã tồn tại).

---

## 🛡 Phân quyền theo vai trò

| Chức năng | Admin | Cashier | Warehouse | Customer |
|---|:---:|:---:|:---:|:---:|
| Dashboard, báo cáo doanh thu | ✅ | ❌ | ❌ | ❌ |
| Bán hàng POS, đổi điểm | ✅ | ✅ | ❌ | ❌ |
| Xem danh mục / sản phẩm | ✅ | ✅ (chỉ xem) | ✅ | ❌ |
| Thêm / sửa danh mục, sản phẩm | ✅ | ❌ | ✅ | ❌ |
| Xóa danh mục, sản phẩm | ✅ | ❌ | ❌ | ❌ |
| Xem khách hàng | ✅ | ✅ (chỉ xem) | ❌ | ❌ |
| Thêm / sửa / xóa khách hàng | ✅ | ❌ | ❌ | ❌ |
| Quản trị tài khoản, vai trò | ✅ | ❌ | ❌ | ❌ |
| Xuất Excel / CSV | ✅ | ✅ | ✅ | ❌ |

- Phân quyền được kiểm tra ở **cả hai phía**: API dùng `[Authorize(Roles = ...)]`, còn WinForms ẩn / hiện nút menu theo vai trò.
- Thu ngân bấm Thêm / Sửa / Xóa sẽ nhận thông báo *"Chỉ Admin mới có quyền chỉnh sửa dữ liệu!"*.

---

## 🌐 Danh sách API

Tất cả API (trừ đăng nhập) đều yêu cầu header `Authorization: Bearer <token>`.

| Phương thức | Đường dẫn | Mô tả |
|---|---|---|
| POST | `/api/auth/login` | Đăng nhập, trả về JWT |
| GET | `/api/categories` | Danh sách nhóm hàng |
| GET | `/api/categories/search?keyword=` | Tìm nhóm hàng |
| POST / PUT / DELETE | `/api/categories/{id}` | Thêm / sửa / xóa nhóm hàng |
| GET | `/api/products?page=1&pageSize=10` | Sản phẩm có phân trang |
| GET | `/api/products/search?keyword=&categoryId=` | Tìm kiếm, lọc theo nhóm |
| GET | `/api/products/barcode/{barcode}` | Tra cứu mã vạch (ADO.NET) |
| GET | `/api/products/low-stock?threshold=10` | Hàng sắp hết |
| POST / PUT / DELETE | `/api/products/{id}` | Thêm / sửa / xóa sản phẩm |
| GET | `/api/customers/by-phone/{phone}` | Tra cứu khách theo SĐT |
| POST | `/api/orders/checkout` | Thanh toán (Transaction) |
| GET | `/api/orders/{id}` | Chi tiết hóa đơn |
| GET | `/api/orders/report?date=` | Báo cáo doanh thu ca (Stored Procedure) |
| GET | `/api/reports/dashboard-summary` | 4 thẻ chỉ số Dashboard |
| GET | `/api/reports/revenue-by-date?fromDate=&toDate=` | Doanh thu theo ngày |
| GET | `/api/reports/top-products?top=5` | Top sản phẩm bán chạy |

Mã trạng thái trả về: **200** thành công, **201** tạo mới, **204** cập nhật / xóa xong, **400** dữ liệu sai, **401** chưa đăng nhập, **403** không đủ quyền, **404** không tìm thấy.

---

## 🗄 Cơ sở dữ liệu

| Bảng | Nội dung |
|---|---|
| `Users` | Tài khoản nhân viên (Username, PasswordHash, FullName, Role, IsActive...) |
| `Roles` | Danh sách vai trò |
| `Categories` | Nhóm hàng |
| `Products` | Sản phẩm, mã vạch, giá bán, tồn kho |
| `Customers` | Khách hàng thành viên, điểm tích lũy, hạng |
| `Orders` | Hóa đơn: tổng tiền, tiền giảm, tiền thực thu, điểm dùng / tích |
| `OrderDetails` | Chi tiết từng dòng hàng trong hóa đơn |

**Quan hệ chính:** Category 1–N Product · Order 1–N OrderDetail · Product 1–N OrderDetail · Customer 1–N Order · User 1–N Order

**Quy tắc tích điểm:** mỗi 10.000 đ thực trả được 1 điểm. Hạng thành viên tự nâng: ≥ 80 điểm Bạc, ≥ 200 điểm Vàng, ≥ 500 điểm Kim cương.

---

## 🩹 Xử lý lỗi thường gặp

| Hiện tượng | Cách xử lý |
|---|---|
| Đăng nhập báo *"Sai tài khoản hoặc mật khẩu"* | Kiểm tra bảng `Users` có đúng tài khoản, `IsActive = 1`; kiểm tra `appsettings.json` trỏ đúng database |
| *"Máy chủ báo lỗi (500)"* hoặc *"Invalid column name"* | Cấu trúc bảng không khớp với Model, xem lại tên cột trong database |
| WinForms báo *"Lỗi kết nối đến Server"* | API chưa chạy hoặc không chạy ở cổng `5000` |
| Biểu đồ không hiển thị | Restore NuGet để tải gói `WinForms.DataVisualization` |
| Xuất Excel báo không ghi được tệp | Đóng tệp đang mở trong Excel rồi xuất lại |
| Sửa code nhưng chương trình không đổi | Bấm **Stop** rồi **Start** lại; chọn **Yes to All** khi VS hỏi tải lại file |

---

## 👨‍💻 6. Tác giả
* **Họ tên sinh viên:** Hồ Mai Phúc Thịnh
* **Mã sinh viên:** 2124110119
* **Lớp học phần:** CCQ2411D
