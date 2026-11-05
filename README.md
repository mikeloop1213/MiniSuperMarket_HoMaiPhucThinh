# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)
> **Môn học:** Lập trình Ứng dụng .NET Core (Mã môn: 229162)  
> **Buổi thực hành:** Buổi 3 - Tích hợp SQL Server và Entity Framework Core Code-First

---
## 🏗️ 1. Mô hình Kiến trúc Hệ thống (Client - Server)
Dự án được xây dựng theo mô hình phân tầng hiện đại, tách biệt hoàn toàn giữa Backend và Frontend:
* **`MiniSupermarket.API` (Backend):** Dự án ASP.NET Core Web API xử lý logic nghiệp vụ, quản lý dữ liệu vĩnh viễn trên SQL Server thông qua EF Core Code-First, hỗ trợ xác thực JWT và phân quyền RBAC.
* **`MiniSupermarket.WinForms` (Frontend Client):** Ứng dụng Windows Forms đóng vai trò là máy trạm POS, tích hợp các màn hình Đăng nhập (`FormLogin`), Quản lý danh mục (`FormCategoryManagement`), Quản lý khách hàng (`FormCustomerManagement`), tự động đính kèm `Bearer Token` vào các HTTP Request.

---
## 🛠️ 2. Công nghệ Sử dụng
* **Ngôn ngữ:** C# (.NET 8.0)
* **Backend:** ASP.NET Core Web API, Controllers, LINQ (Async/Await)
* **ORM & Database:** Entity Framework Core Code-First, Microsoft SQL Server
* **Bảo mật & Xác thực:** `Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`
* **Frontend:** Windows Forms (.NET 8.0), `System.Net.Http.Json`
* **Công cụ kiểm thử & Quản lý CSDL:** Swagger UI (Swashbuckle), EF Core Migrations, SQL Server Management Studio (SSMS)

---
## 📂 3. Cấu trúc Solution
```text
MiniSupermarket/
│
├── MiniSupermarket.API/          # Dự án Web API (Backend)
│   ├── Controllers/              # AuthController.cs, CategoriesController.cs, CustomersController.cs
│   ├── Data/                     # SupermarketDbContext.cs (EF Core DbContext & Data Seeding)
│   ├── Migrations/               # Mã quản lý phiên bản cơ sở dữ liệu EF Core
│   ├── Models/                   # Category.cs, Product.cs, Customer.cs
│   ├── appsettings.json          # Chuỗi kết nối ConnectionStrings & JwtSettings:Secret
│   └── Program.cs                # Cấu hình DbContext, JWT Bearer Middleware & Swagger
│
└── MiniSupermarket.WinForms/     # Dự án Windows Forms (Frontend Client)
    ├── FormLogin.cs              # Màn hình Đăng nhập hệ thống
    ├── FormCategoryManagement.cs # Giao diện Quản lý danh mục
    ├── FormCustomerManagement.cs # Giao diện Quản lý khách hàng thân thiết
    ├── SessionManager.cs         # Lưu trữ Token & CurrentRole toàn cục
    └── Program.cs                # Điều hướng ứng dụng
```

---
## 🗄️ 4. Cơ sở Dữ liệu & Entity Models
Hệ thống sử dụng EF Core Code-First để tự động ánh xạ các Model C# thành bảng trong SQL Server:
* **`Categories` (Danh mục):** `CategoryId` (PK), `CategoryName`, `Description`, Quan hệ 1-N với `Products`.
* **`Products` (Sản phẩm):** `ProductId` (PK), `Barcode`, `ProductName`, `Price`, `StockQuantity`, `CategoryId` (FK).
* **`Customers` (Khách hàng):** `CustomerId` (PK), `CustomerName`, `PhoneNumber`, `Address`, `RewardPoints`, `MembershipRank`.

---
## 🔑 5. Tài khoản và Danh sách Endpoints
### Tài khoản mẫu
* **Admin:** `admin` / `123456` (Toàn quyền quản trị)
* **Cashier:** `cashier` / `123456` (Quyền thu ngân/nhân viên)

### Danh sách Endpoints chính
#### Authentication API
* `POST /api/auth/login`: Đăng nhập, nhận JWT Token và Role (Public)

#### Categories API
* `GET /api/categories`: Lấy danh sách danh mục (Lấy từ SQL Server)
* `GET /api/categories/{id}`: Lấy chi tiết danh mục theo ID
* `GET /api/categories/search?keyword=...`: Tìm kiếm danh mục theo từ khóa
* `POST /api/categories`: Thêm mới danh mục
* `PUT /api/categories/{id}`: Cập nhật danh mục
* `DELETE /api/categories/{id}`: Xóa danh mục (Yêu cầu quyền **Admin**)

#### Customers API (Bài tập mở rộng)
* `GET /api/customers`: Lấy danh sách khách hàng thân thiết
* `GET /api/customers/{id}`: Lấy chi tiết khách hàng theo ID
* `GET /api/customers/search?keyword=...`: Tìm kiếm khách hàng theo tên hoặc SĐT
* `POST /api/customers`: Thêm mới khách hàng
* `PUT /api/customers/{id}`: Cập nhật thông tin & hạng thẻ khách hàng
* `DELETE /api/customers/{id}`: Xóa thông tin khách hàng

---
## 🚀 6. Hướng dẫn Chạy và Kiểm thử Dự án
### Bước 1: Cấu hình và Cập nhật Cơ sở dữ liệu (Migrations)
1. Mở file `appsettings.json` trong `MiniSupermarket.API` và điều chỉnh chuỗi kết nối `DefaultConnection` phù hợp với thông số SQL Server trên máy.
2. Mở cửa sổ **Package Manager Console** (Tools -> NuGet Package Manager -> Package Manager Console).
3. Đảm bảo ô **Default project** chọn `MiniSupermarket.API`.
4. Thực thi các lệnh Migrations:
   ```powershell
   Add-Migration InitialCreateDatabase
   Update-Database
   ```
   *(Hệ thống sẽ tự động khởi tạo CSDL SQL Server và nạp dữ liệu mồi ban đầu).*

### Bước 2: Chạy Backend (Web API)
1. Đặt **`MiniSupermarket.API`** làm **Startup Project**.
2. Nhấn **F5** để khởi chạy.
3. Kiểm thử trên giao diện Swagger UI với các chức năng CRUD danh mục và khách hàng.

### Bước 3: Chạy Frontend (WinForms Client)
1. Đảm bảo thuộc tính `SessionManager.ApiBaseUrl` khớp với URL HTTPS của API đang chạy.
2. Nhấp chuột phải vào **`MiniSupermarket.WinForms`** chọn **Debug -> Start new instance**.
3. Thử nghiệm thao tác thêm, sửa, xóa danh mục và khách hàng. Tắt ứng dụng và mở lại để kiểm chứng dữ liệu đã được lưu trữ vĩnh viễn vào SQL Server.

---
## 👨‍💻 7. Tác giả
* **Họ tên sinh viên:** Hồ Mai Phúc Thịnh
* **Mã sinh viên:** 2124110119
* **Lớp học phần:** CCQ2411D