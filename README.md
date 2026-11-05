# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)
> **Môn học:** Lập trình Ứng dụng .NET Core (Mã môn: 229162)  
> **Buổi thực hành:** Buổi 3 - Tích hợp SQL Server và Entity Framework Core Code-First

---
## 🏗️ 1. Mô hình Kiến trúc Hệ thống (Client - Server - Database)
Dự án được xây dựng theo mô hình phân tầng hiện đại, tách biệt hoàn toàn giữa Backend, Frontend và Cơ sở dữ liệu:
* **`MiniSupermarket.API` (Backend):** Dự án ASP.NET Core Web API chịu trách nhiệm xử lý logic nghiệp vụ và cung cấp các RESTful API chuẩn hóa. Dữ liệu không còn lưu tạm trong bộ nhớ (In-Memory) mà được lưu trữ bền vững trong SQL Server thông qua Entity Framework Core.
* **SQL Server (Database):** Cơ sở dữ liệu được sinh tự động từ các lớp Model theo phương pháp **Code-First** (viết code C# trước, EF Core tạo bảng sau) bằng cơ chế Migrations.
* **`MiniSupermarket.WinForms` (Frontend Client):** Ứng dụng Windows Forms đóng vai trò là máy trạm POS tại quầy, sử dụng `HttpClient` để gọi dữ liệu từ API qua mạng và hiển thị trực quan lên `DataGridView`.
---
## 🛠️ 2. Công nghệ Sử dụng
* **Ngôn ngữ:** C# (.NET 8.0)
* **Backend:** ASP.NET Core Web API, Controllers, Entity Framework Core 8 (Code-First), LINQ, async/await
* **Cơ sở dữ liệu:** Microsoft SQL Server, SQL Server Management Studio (SSMS)
* **Thư viện NuGet:** `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.EntityFrameworkCore.Design`
* **Frontend:** Windows Forms (.NET 8.0), `System.Net.Http.Json`
* **Công cụ kiểm thử:** Swagger UI
---
## 📂 3. Cấu trúc Solution
```text
MiniSupermarketSystem/
│
├── MiniSupermarket.API/                # Dự án Web API (Backend)
│   ├── Controllers/                    # CategoriesController (CRUD & Search) truy vấn qua DbContext
│   ├── Data/                           # Chứa SupermarketDbContext.cs (DbSet<Category>, cấu hình Fluent API)
│   ├── Migrations/                     # Các file Migration do EF Core sinh ra để tạo/cập nhật CSDL
│   ├── Models/                         # Chứa lớp thực thể Category.cs (Data Annotations: [Key], [Required]...)
│   ├── appsettings.json                # Chứa chuỗi kết nối ConnectionStrings tới SQL Server
│   └── Program.cs                      # Đăng ký DbContext (AddDbContext + UseSqlServer) và Middleware
│
└── MiniSupermarket.WinForms/           # Dự án Windows Forms (Frontend Client)
    └── FormCategoryManagement.cs       # Giao diện quản lý danh mục CRUD
```

🚀 4. Hướng dẫn Chạy và Kiểm thử Dự án
Bước 1: Cấu hình kết nối SQL Server
Mở file appsettings.json trong project MiniSupermarket.API và chỉnh lại chuỗi kết nối cho khớp với SQL Server trên máy:

"ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=MiniSupermarketDB;Trusted_Connection=True;TrustServerCertificate=True" }


Bước 2: Tạo Cơ sở dữ liệu bằng Migrations (Code-First)
Mở Tools -> NuGet Package Manager -> Package Manager Console, chọn Default project là MiniSupermarket.API.


Chạy lệnh Add-Migration InitialCreate để EF Core sinh file Migration từ các lớp Model.


Chạy lệnh Update-Database để tạo database MiniSupermarketDB và bảng Categories trong SQL Server. Có thể mở SSMS để kiểm tra kết quả.


Bước 3: Chạy phía Backend (Web API)
Nhấp chuột phải vào project MiniSupermarket.API chọn Set as Startup Project.


Nhấn F5 để chạy. Trình duyệt sẽ tự động mở giao diện Swagger UI để kiểm tra các phương thức GET, POST, PUT, DELETE. Dữ liệu thêm/sửa/xóa sẽ được lưu thật vào SQL Server (không mất khi tắt chương trình).


Bước 4: Chạy phía Frontend (WinForms Client)
Đảm bảo cổng (Port) trong ApiClientService hoặc HttpClient của WinForms khớp với cổng https://localhost:XXXXX của Web API đang chạy.

Nhấp chuột phải vào project MiniSupermarket.WinForms chọn Debug -> Start new instance.

Thử nghiệm các chức năng: Tải danh sách, Thêm mới, Sửa, Xóa và Tìm kiếm nhóm hàng, sau đó mở bảng Categories trong SSMS để đối chiếu dữ liệu.

👨‍💻 5. Tác giả
Họ tên sinh viên: Hồ Mai Phúc Thịnh
Mã sinh viên: 2124110119
Lớp học phần: CCQ2411D
