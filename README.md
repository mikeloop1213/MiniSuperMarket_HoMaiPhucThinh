# 🛒 NGHIÊN CỨU VÀ XÂY DỰNG ỨNG DỤNG QUẢN LÝ CỬA HÀNG BÁN LẺ (SIÊU THỊ MINI ZEN MART)
> **Môn học:** Lập trình Ứng dụng .NET Core (Mã môn: 229162)  
> **Kiến trúc:** ASP.NET Core Web API (Backend) + WinForms Single-Form Shell (Client POS)  
> **Bảo mật:** JWT Bearer Authentication & Role-Based Access Control (RBAC)  

---

## 🎯 1. Mục tiêu và Tình huống Thực tế
### Mục tiêu
* **Backend (.NET Core Web API):** Xây dựng hệ thống RESTful API chuẩn mực, xử lý logic nghiệp vụ quản lý bán lẻ, xác thực người dùng bằng JWT Token và phân quyền truy cập endpoint chặt chẽ.
* **Frontend Client (WinForms Shell):** Làm chủ kỹ thuật thiết kế giao diện đơn khung hiện đại (**Single-Form Architecture**) sử dụng Left Sidebar Panel và Main Content Panel. Nhúng động các Form con (`TopLevel = false`, `Dock = Fill`) vào vùng chứa trung tâm mà không mở cửa sổ rời rạc.
* **Phân quyền người dùng (RBAC):** Hiện thực hóa cơ chế phân quyền ngay trên giao diện Client và Backend tương ứng với 3 vai trò: **Admin** (Quản trị viên), **Cashier** (Thu ngân), và **Warehouse** (Thủ kho).

### Tình huống thực tế
Tại siêu thị mini **Zen Mart**, nhân viên Thu ngân chỉ được quyền truy cập màn hình POS bán hàng và thông tin khách hàng thân thiết; Thủ kho chỉ được quản lý danh mục nhóm hàng, sản phẩm và kiểm kê tồn kho; trong khi Quản trị viên (Admin) có toàn quyền thao tác hệ thống, xem báo cáo doanh thu tài chính và quản lý tài khoản nhân viên. Màn hình điều khiển trung tâm (`FormMainShell`) tự động thích ứng giao diện dựa trên vai trò của người dùng ngay sau khi đăng nhập.

---

## 🏗️ 2. Mô hình Kiến trúc Hệ thống (Client - Server)

```text
┌─────────────────────────────────────────────────────────────────────────────┐
│                            FormMainShell (WinForms)                         │
├────────────────────┬────────────────────────────────────────────────────────┤
│  panelSidebar      │  panelTopHeader (Breadcrumb & User Info)               │
│  (Width = 230)     ├────────────────────────────────────────────────────────┤
│  - Logo Zen Mart   │                                                        │
│  - btnPOS          │  panelMainContent (Dock = Fill)                        │
│  - btnCategory     │                                                        │
│  - btnProduct      │  [ VÙNG NHÚNG ĐỘNG CÁC FORM CON NGHIỆP VỤ ]             │
│  - btnCustomer     │   - FormPOS                                            │
│  - btnReports      │   - FormCategoryManagement                             │
│  - btnUserManage   │   - FormProductManagement                              │
│  ───────────────   │   - FormCustomerManagement                             │
│  - btnLogout       │   - FormUserManagement / FormQuickReport              │
└────────────────────┴────────────────────────────────────────────────────────┘
                                      │
                                (HTTP / JSON)
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                       MiniSupermarket.API (.NET Core)                       │
│  [AuthController]  [CategoriesController]  [ProductsController]             │
│  [OrdersController] [CustomersController]   [UsersController]              │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            SQL Server (ZenMartDb)                           │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🛠️ 3. Công nghệ Sử dụng
* **Ngôn ngữ lập trình:** C# (.NET 8.0)
* **Backend:** ASP.NET Core Web API, Controllers, LINQ, Entity Framework Core / ADO.NET
* **Bảo mật & Xác thực:** `Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`
* **Frontend:** Windows Forms (.NET 8.0), `System.Net.Http.Json`, Custom UI Panel Layout
* **Cơ sở dữ liệu:** Microsoft SQL Server (`ZenMartDb`)
* **Công cụ kiểm thử & Quản lý:** Swagger UI (Swashbuckle), Visual Studio 2022

---

## 🔑 4. Bộ Dữ liệu Tài khoản Mẫu & Phân quyền (15 Dòng Kiểm thử)

| STT | Username | Mật khẩu | Họ và tên nhân viên | Vai trò (Role) | Màn hình mặc định | Phạm vi chức năng cho phép |
| :---: | :--- | :---: | :--- | :---: | :--- | :--- |
| **1** | `admin01` | `123456` | Nguyễn Quản Trị | **Admin** | `FormCategoryManagement` | Toàn quyền toàn bộ hệ thống |
| **2** | `admin02` | `123456` | Trần Giám Đốc | **Admin** | `FormCategoryManagement` | Toàn quyền toàn bộ hệ thống |
| **3** | `cashier01` | `123456` | Lê Thu Ngân | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **4** | `cashier02` | `123456` | Phạm Bán Hàng | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **5** | `cashier03` | `123456` | Hoàng Thu Ngân | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **6** | `cashier04` | `123456` | Vũ Thị Quầy | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **7** | `cashier05` | `123456` | Đỗ Bán Lẻ | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **8** | `ware01` | `123456` | Ngô Quản Kho | **Warehouse** | `FormProductManagement` | Sản phẩm, Nhóm hàng, Nhập xuất kho |
| **9** | `ware02` | `123456` | Bùi Kiểm Kê | **Warehouse** | `FormProductManagement` | Sản phẩm, Nhóm hàng, Nhập xuất kho |
| **10** | `ware03` | `123456` | Dương Thủ Kho | **Warehouse** | `FormProductManagement` | Sản phẩm, Nhóm hàng, Nhập xuất kho |
| **11** | `ware04` | `123456` | Lý Nhập Hàng | **Warehouse** | `FormProductManagement` | Sản phẩm, Nhóm hàng, Nhập xuất kho |
| **12** | `admin_backup` | `123456` | Đặng Hỗ Trợ | **Admin** | `FormCategoryManagement` | Toàn quyền toàn bộ hệ thống |
| **13** | `cashier06` | `123456` | Hồ Ca Chiều | **Cashier** | `FormPOS` | Màn hình POS, Khách hàng thành viên |
| **14** | `ware05` | `123456` | Trương Vận Chuyển | **Warehouse** | `FormProductManagement` | Sản phẩm, Nhóm hàng, Nhập xuất kho |
| **15** | `supervisor` | `123456` | Mai Giám Sát | **Admin** | `FormCategoryManagement` | Toàn quyền toàn bộ hệ thống |

---

## 📊 5. Ma trận Phân quyền Chức năng (Role Responsibility Matrix)

| Chức năng / Form | Admin | Cashier (Thu ngân) | Warehouse (Thủ kho) |
| :--- | :---: | :---: | :---: |
| 🛒 **Màn hình Bán hàng Barcode (FormPOS)** | 🔓 Có | 🔓 Có | 🔒 Không |
| 📁 **Quản lý Nhóm hàng (FormCategoryManagement)** | 🔓 Có | 🔒 Không | 🔓 Có |
| 📦 **Quản lý Sản phẩm & Tồn kho (FormProductManagement)** | 🔓 Có | 🔒 Không | 🔓 Có |
| 👥 **Quản lý Khách hàng & Tích điểm (FormCustomerManagement)** | 🔓 Có | 🔓 Có | 🔒 Không |
| 📊 **Báo cáo Doanh thu & Ca bán (FormQuickReport)** | 🔓 Có | 🔒 Không | 🔒 Không |
| 🛡️ **Quản trị Tài khoản & Phân quyền (FormUserManagement)** | 🔓 Có | 🔒 Không | 🔒 Không |

---

## 📂 6. Cấu trúc Solution

```text
MiniSupermarket/
│
├── MiniSupermarket.API/              # Dự án Web API (Backend .NET Core)
│   ├── Controllers/                  # AuthController, CategoriesController, ProductsController, CustomersController, OrdersController, UsersController
│   ├── Models/ / Dtos/               # Data Transfer Objects & Entity Models
│   ├── Services/                     # JwtTokenService, PasswordHasher
│   ├── appsettings.json              # Chuỗi kết nối DB ZenMartDb & Cấu hình JwtSettings:Secret
│   └── Program.cs                    # Cấu hình JWT Bearer Middleware, CORS & Swagger Authorize
│
└── MiniSupermarket.WinForms/         # Dự án Windows Forms Client (Frontend POS Shell)
    ├── Forms/
    │   ├── FormLogin.cs              # Màn hình Đăng nhập hệ thống
    │   ├── FormMainShell.cs          # Khung giao diện chính (Sidebar + Dynamic Workspace)
    │   ├── FormPOS.cs                # Màn hình Bán hàng POS & Quét mã vạch
    │   ├── FormCategoryManagement.cs # Màn hình Quản lý Danh mục Nhóm hàng
    │   ├── FormProductManagement.cs  # Màn hình Quản lý Sản phẩm & Kho hàng
    │   ├── FormCustomerManagement.cs # Màn hình Quản lý Khách hàng & Tích điểm
    │   ├── FormQuickReport.cs        # Màn hình Báo cáo Doanh thu Nhanh
    │   └── FormUserManagement.cs     # Màn hình Quản trị Tài khoản Người dùng
    ├── Services/
    │   ├── ApiClientService.cs       # HttpClient dùng chung đính kèm Bearer Token
    │   └── SessionManager.cs         # Lưu trữ JwtToken, CurrentUsername, CurrentRole toàn cục
    └── Program.cs                    # Điều hướng ứng dụng (Khởi chạy FormLogin đầu tiên)
```

---

## 🚀 7. Hướng dẫn Cài đặt, Chạy và Kiểm thử Dự án

### Bước 1: Khởi tạo Cơ sở dữ liệu SQL Server
1. Mở SQL Server Management Studio (SSMS).
2. Mở file `ZenMartDb.sql` trong thư mục dự án và nhấn **Execute (F5)** để khởi tạo Database `ZenMartDb` cùng toàn bộ bảng và dữ liệu mẫu 15 dòng cho mỗi bảng.

### Bước 2: Chạy Backend Web API
1. Mở Solution `MiniSupermarket.sln` bằng Visual Studio 2022.
2. Nhấp chuột phải vào project **`MiniSupermarket.API`** chọn **Set as Startup Project**.
3. Cấu hình chuỗi kết nối `ConnectionStrings` trong `appsettings.json` cho phù hợp với SQL Server của bạn.
4. Nhấn **F5** để khởi chạy. Trình duyệt sẽ mở giao diện **Swagger UI**.
5. Thử nghiệm API `POST /api/auth/login` để lấy JWT Token, bấm nút **Authorize** ở góc trên Swagger và dán `Bearer <Token>` để test các endpoint bảo mật.

### Bước 3: Chạy Frontend WinForms Client
1. Đảm bảo thuộc tính `SessionManager.ApiBaseUrl` trong WinForms chỉ đúng cổng URL của Web API đang chạy (ví dụ: `https://localhost:7123/api/`).
2. Nhấp chuột phải vào project **`MiniSupermarket.WinForms`** chọn **Debug -> Start new instance**.
3. Thực hiện kiểm thử phân quyền theo kịch bản:
   * **Đăng nhập `cashier01` / `123456`:** Sidebar chỉ hiển thị nút *Bán hàng (POS)* và *Khách hàng*. Màn hình mặc định là `FormPOS`.
   * **Đăng nhập `ware01` / `123456`:** Sidebar chỉ hiển thị nút *Quản lý Danh mục* và *Sản phẩm*. Màn hình mặc định là `FormCategoryManagement`.
   * **Đăng nhập `admin01` / `123456`:** Sidebar hiển thị toàn bộ 6 nút chức năng hệ thống.

---

## 👨‍💻 8. Thông tin Tác giả
* **Họ và tên sinh viên:** Hồ Mai Phúc Thịnh  
* **Mã sinh viên:** 2124110119  
* **Lớp học phần:** CCQ2411D  
* **Đề tài:** Nghiên cứu và Xây dựng Ứng dụng Quản lý Cửa hàng Bán lẻ sử dụng .NET Core Web API — Siêu thị Mini Zen Mart  