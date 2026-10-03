# 🛒 HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)
> **Môn học:** Lập trình Ứng dụng .NET Core (Mã môn: 229162)  
> **Buổi thực hành:** Buổi 2 - Bảo mật Web API bằng JWT, phân quyền theo vai trò và màn hình Đăng nhập cho WinForms

---
## 🏗️ 1. Mô hình Kiến trúc Hệ thống (Client - Server)
Dự án được xây dựng theo mô hình phân tầng hiện đại, tách biệt hoàn toàn giữa Backend và Frontend:
* **`MiniSupermarket.API` (Backend):** Dự án ASP.NET Core Web API chịu trách nhiệm xử lý logic nghiệp vụ, quản lý dữ liệu, thực hiện xác thực người dùng (Authentication), cấp phát JWT Token và phân quyền RBAC (Role-Based Access Control) qua `[Authorize]`.
* **`MiniSupermarket.WinForms` (Frontend Client):** Ứng dụng Windows Forms đóng vai trò là máy trạm POS, tích hợp màn hình Đăng nhập (`FormLogin`), quản lý phiên đăng nhập với `SessionManager` và tự động đính kèm `Bearer Token` vào mọi HTTP Request.

---
## 🛠️ 2. Công nghệ Sử dụng
* **Ngôn ngữ:** C# (.NET 8.0)
* **Backend:** ASP.NET Core Web API, Controllers, In-Memory Data, LINQ
* **Bảo mật & Xác thực:** `Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`
* **Frontend:** Windows Forms (.NET 8.0), `System.Net.Http.Json`
* **Công cụ kiểm thử:** Swagger UI (Swashbuckle)

---
## 📂 3. Cấu trúc Solution
```text
MiniSupermarket/
│
├── MiniSupermarket.API/          # Dự án Web API (Backend)
│   ├── Controllers/              # AuthController.cs (JWT Login), CategoriesController.cs (CRUD + Auth)
│   ├── appsettings.json          # Cấu hình chuỗi bí mật JwtSettings:Secret
│   └── Program.cs                # Cấu hình JWT Bearer Middleware & Swagger Authorize
│
└── MiniSupermarket.WinForms/     # Dự án Windows Forms (Frontend Client)
    ├── FormLogin.cs              # Màn hình Đăng nhập hệ thống
    ├── FormCategoryManagement.cs # Giao diện Quản lý danh mục (Gửi kèm Token)
    ├── SessionManager.cs         # Lưu trữ Token & CurrentRole toàn cục
    └── Program.cs                # Điều hướng chạy FormLogin đầu tiên
```

---
## 🔑 4. Tài khoản và Phân quyền API
### Tài khoản mẫu
* **Admin:** `admin` / `123456` (Toàn quyền: xem, thêm, sửa, xóa danh mục)
* **Cashier:** `cashier` / `123456` (Quyền hạn chế: xem, thêm, sửa danh mục; **không có quyền xóa**)

### Danh sách Endpoint chính
* `POST /api/auth/login`: Đăng nhập, nhận JWT Token và Role (Public)
* `GET /api/categories`: Lấy danh sách danh mục (Yêu cầu đăng nhập)
* `POST /api/categories`: Thêm mới danh mục (Yêu cầu đăng nhập)
* `DELETE /api/categories/{id}`: Xóa danh mục (Chỉ dành cho **Admin**)

---
## 🚀 5. Hướng dẫn Chạy và Kiểm thử Dự án
### Bước 1: Chạy phía Backend (Web API)
1. Mở Solution bằng Visual Studio 2022.
2. Nhấp chuột phải vào project **`MiniSupermarket.API`** chọn **Set as Startup Project**.
3. Nhấn **F5** để chạy. Trình duyệt sẽ mở giao diện Swagger UI.
4. Thử nghiệm đăng nhập tại `POST /api/auth/login` để nhận Token, bấm nút **Authorize** ở góc trên Swagger và dán cú pháp `Bearer <Token>` để kiểm thử các API bị khóa.

### Bước 2: Chạy phía Frontend (WinForms Client)
1. Đảm bảo thuộc tính `SessionManager.ApiBaseUrl` khớp với cổng `https://localhost:XXXX/api/` của Web API đang chạy.
2. Nhấp chuột phải vào project **`MiniSupermarket.WinForms`** chọn **Debug -> Start new instance**.
3. Thử nghiệm các chức năng:
   * Đăng nhập tài khoản `cashier` / `123456`: Thử thực hiện thao tác Xóa để nhận thông báo lỗi từ chối quyền (HTTP 403 Forbidden).
   * Đăng xuất và đăng nhập lại tài khoản `admin` / `123456`: Thực hiện xóa danh mục thành công.

---
## 👨‍💻 6. Tác giả
* **Họ tên sinh viên:** Hồ Mai Phúc Thịnh
* **Mã sinh viên:** 2124110119
* **Lớp học phần:** CCQ2411D