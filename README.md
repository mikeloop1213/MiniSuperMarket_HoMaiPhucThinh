# 🛒 NGHIÊN CỨU VÀ XÂY DỰNG ỨNG DỤNG QUẢN LÝ CỬA HÀNG BÁN LẺ (SIÊU THỊ MINI ZEN MART)

> **Môn học:** Lập trình Ứng dụng .NET Core (Mã môn: 229162)  
> **Buổi thực hành:** Buổi 4 - THIẾT KẾ KIẾN TRÚC GIAO DIỆN WINFORMS SHELL (SIDEBAR & MAIN WORKSPACE) VÀ ĐIỀU HƯỚNG PHÂN QUYỀN (ADMIN / CASHIER / WAREHOUSE)

---

## 🎯 1. Mục tiêu và Tình huống Thực tế
### Mục tiêu
* **Backend (.NET Core Web API):** Thiết kế và hiện thực hóa bộ API CRUD đầy đủ cho danh mục Nhóm hàng (`CategoriesController`), áp dụng cơ chế xác thực JWT và phân quyền chặt chẽ (`[Authorize(Roles = "Admin,Warehouse")]`).
* **Frontend Client (WinForms Shell):** Xây dựng giao diện `FormCategoryManagement` chuyên nghiệp, kết nối HTTP API thông qua `ApiClientService`, hiển thị danh sách nhóm hàng lên `DataGridView`, hỗ trợ tìm kiếm, thêm, sửa, xóa và kiểm soát quyền thao tác theo Role.
* **Tích hợp UI Shell (Single-Form Architecture):** Nhúng động `FormCategoryManagement` vào `panelMainContent` của `FormMainShell` theo mô hình kiến trúc giao diện đơn khung.

### Tình huống thực tế
Tại **Siêu thị Mini Zen Mart**, việc phân loại sản phẩm theo nhóm hàng (như *Thực phẩm tươi sống*, *Hóa mỹ phẩm*, *Bánh kẹo*, *Nước giải khát*) là yêu cầu tối quan trọng để kiểm soát tồn kho và tính doanh thu. 

Theo quy trình phân quyền:
* Nhân viên **Thu ngân (Cashier)** không có quyền truy cập hay chỉnh sửa danh mục Nhóm hàng.
* Nhân viên **Thủ kho (Warehouse)** và **Quản trị viên (Admin)** có toàn quyền quản lý, thêm mới, cập nhật tên/mô tả nhóm hàng và xóa các nhóm hàng không còn kinh doanh.

---

## 🏗️ 2. Mô hình Kiến trúc Hệ thống (Buổi 4 Integration)

```text
┌─────────────────────────────────────────────────────────────────────────────┐
│                            FormMainShell (WinForms)                         │
├────────────────────┬────────────────────────────────────────────────────────┤
│  panelSidebar      │  panelTopHeader (Breadcrumb & User Info)               │
│  (Width = 230)     ├────────────────────────────────────────────────────────┤
│  - Logo Zen Mart   │                                                        │
│  - btnCategory ───┼─► panelMainContent (Dock = Fill)                       │
│  - btnProduct      │   ┌────────────────────────────────────────────────┐   │
│  - btnPOS          │   │ FormCategoryManagement (Embedded)              │   │
│  - btnCustomer     │   │ - txtSearch, btnSearch, btnAdd, btnEdit, btnDel│   │
│  - btnReports      │   │ - dgvCategories (AutoGenerateColumns = false)  │   │
│  - btnUserManage   │   └────────────────────────────────────────────────┘   │
└────────────────────┴────────────────────────────────────────────────────────┘
                                      │
                         (HTTP GET/POST/PUT/DELETE)
                         (Header: Authorization Bearer <JWT>)
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                       MiniSupermarket.API (.NET Core)                       │
│  [AuthController]                                                           │
│  [CategoriesController] ──► [Authorize(Roles = "Admin,Warehouse")]          │
│     ├── GET    /api/categories         (Lấy danh sách / Tìm kiếm)        │
│     ├── GET    /api/categories/{id}    (Lấy chi tiết)                    │
│     ├── POST   /api/categories         (Thêm nhóm hàng mới)              │
│     ├── PUT    /api/categories/{id}    (Cập nhật nhóm hàng)              │
│     └── DELETE /api/categories/{id}    (Xóa nhóm hàng)                   │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                (Entity Framework Core)
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            SQL Server (ZenMartDb)                           │
│                            [Table: Categories]                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🛠️ 3. Công nghệ Sử dụng & Thành phần Kỹ thuật Buổi 4
* **Backend Framework:** ASP.NET Core 8.0 Web API, Entity Framework Core / LINQ Queries
* **API Standards:** RESTful API Design, JSON Serialization, Async/Await
* **Authentication & Authorization:** JWT Bearer Token Validation, Policy-based / Role-based Routing
* **Frontend UI Framework:** Windows Forms (.NET 8.0)
* **HTTP Communication:** `System.Net.Http.Json`, `HttpClient` với `AuthenticationHeaderValue` Bearer
* **Database Target:** Microsoft SQL Server (`ZenMartDb`), Bảng `Categories` (`CategoryID`, `CategoryName`, `Description`, `IsActive`)

---

## 🔑 4. Bộ Dữ liệu Kiểm thử Phân quyền & Mẫu Nhóm hàng (Buổi 4)

### Tài khoản kiểm thử phân quyền màn hình Danh mục (15 Dòng)

| STT | Username | Mật khẩu | Họ và tên nhân viên | Vai trò (Role) | Quyền hạn trên FormCategoryManagement |
| :---: | :--- | :---: | :--- | :---: | :--- |
| **1** | `admin01` | `123456` | Nguyễn Quản Trị | **Admin** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **2** | `admin02` | `123456` | Trần Giám Đốc | **Admin** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **3** | `cashier01` | `123456` | Lê Thu Ngân | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **4** | `cashier02` | `123456` | Phạm Bán Hàng | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **5** | `cashier03` | `123456` | Hoàng Thu Ngân | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **6** | `cashier04` | `123456` | Vũ Thị Quầy | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **7** | `cashier05` | `123456` | Đỗ Bán Lẻ | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **8** | `ware01` | `123456` | Ngô Quản Kho | **Warehouse** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **9** | `ware02` | `123456` | Bùi Kiểm Kê | **Warehouse** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **10** | `ware03` | `123456` | Dương Thủ Kho | **Warehouse** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **11** | `ware04` | `123456` | Lý Nhập Hàng | **Warehouse** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **12** | `admin_backup` | `123456` | Đặng Hỗ Trợ | **Admin** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **13** | `cashier06` | `123456` | Hồ Ca Chiều | **Cashier** | 🔒 **Bị ẩn nút / Chặn truy cập API** |
| **14** | `ware05` | `123456` | Trương Vận Chuyển | **Warehouse** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |
| **15** | `supervisor` | `123456` | Mai Giám Sát | **Admin** | 🔓 Toàn quyền (Xem, Thêm, Sửa, Xóa) |

### Dữ liệu mẫu danh mục Nhóm hàng (Categories Sample Data)

| Mã Nhóm (ID) | Tên Nhóm hàng (CategoryName) | Mô tả (Description) | Trạng thái |
| :---: | :--- | :--- | :---: |
| **1** | Bánh Kẹo & Snack | Các loại bánh quy, snack khoai tây, kẹo ngọt các loại | Hoạt động |
| **2** | Nước Giải Khát | Nước ngọt có ga, nước ép trái cây, trà đóng chai, nước khoáng | Hoạt động |
| **3** | Hóa Mỹ Phẩm | Dầu gội, sữa tắm, xà phòng, nước lau sàn, nước giặt | Hoạt động |
| **4** | Thực Phẩm Tươi Sống | Rau củ quả tươi, thịt heo, thịt bò, hải sản tươi sống | Hoạt động |
| **5** | Sữa & Sản Phẩm Từ Sữa | Sữa tươi, sữa chua, phô mai, váng sữa các loại | Hoạt động |

---

## 📊 5. Ma trận Phân quyền Chức năng Danh mục (Buổi 4 Focus)

| Hành động / Endpoint API | Thu ngân (`Cashier`) | Thủ kho (`Warehouse`) | Quản trị viên (`Admin`) |
| :--- | :---: | :---: | :---: |
| **Xem Menu "Quản lý Nhóm hàng" (UI)** | 🔒 Ẩn Nút | 🔓 Hiển thị | 🔓 Hiển thị |
| **`GET /api/categories` (Lấy DS)** | ❌ 403 Forbidden | ✅ 200 OK | ✅ 200 OK |
| **`POST /api/categories` (Thêm mới)** | ❌ 403 Forbidden | ✅ 201 Created | ✅ 201 Created |
| **`PUT /api/categories/{id}` (Cập nhật)** | ❌ 403 Forbidden | ✅ 200 OK / 204 No Content | ✅ 200 OK / 204 No Content |
| **`DELETE /api/categories/{id}` (Xóa)** | ❌ 403 Forbidden | ✅ 200 OK / 204 No Content | ✅ 200 OK / 204 No Content |

---

## 📂 6. Cấu trúc Solution Chi tiết Buổi 4

```text
MiniSupermarket/
│
├── MiniSupermarket.API/                  # Dự án Web API (.NET Core 8.0)
│   ├── Controllers/
│   │   ├── AuthController.cs            # API Đăng nhập & Phát hành JWT Token
│   │   └── CategoriesController.cs      # [MỚI - BUỔI 4] API CRUD Danh mục Nhóm hàng
│   ├── Dtos/
│   │   ├── CategoryDto.cs               # DTO trả về thông tin nhóm hàng
│   │   └── CreateUpdateCategoryDto.cs    # DTO dùng cho request Thêm/Sửa
│   ├── Models/
│   │   └── Category.cs                  # Entity Model mapping bảng Categories
│   ├── Services/
│   │   └── JwtTokenService.cs           # Service tạo và mã hóa Token
│   ├── appsettings.json                  # Cấu hình ConnectionString & JWT Secret
│   └── Program.cs                        # Đăng ký Service & Middleware Authentication/Authorization
│
└── MiniSupermarket.WinForms/             # Dự án Client Windows Forms
    ├── Forms/
    │   ├── FormLogin.cs                  # Màn hình Đăng nhập
    │   ├── FormMainShell.cs              # UI Shell chính (Chứa Sidebar & panelMainContent)
    │   └── FormCategoryManagement.cs     # [MỚI - BUỔI 4] Form Quản lý Nhóm hàng nhúng động
    ├── Services/
    │   ├── ApiClientService.cs           # Service gửi request HTTP có chứa Bearer Token
    │   └── SessionManager.cs             # Lưu thông tin phiên làm việc (Token, Role, User)
    └── Program.cs                        # Điểm chạy ứng dụng (Khởi chạy FormLogin)
```

---

## 🚀 7. Hướng dẫn Lập trình & Thực thi Bài tập Buổi 4

### Bước 1: Hiện thực Backend API (`CategoriesController.cs`)
1. Tạo `CategoriesController` kế thừa `ControllerBase` với route `[route("api/[controller]")]`.
2. Khai báo thuộc tính `[Authorize(Roles = "Admin,Warehouse")]` trên Controller để chặn các truy cập trái phép.
3. Cài đặt các Action Method Async:
   * `GetCategories(string? keyword)`: Lấy danh sách nhóm hàng, hỗ trợ lọc theo tên.
   * `GetCategoryById(int id)`: Lấy chi tiết 1 nhóm hàng theo ID.
   * `CreateCategory(CreateUpdateCategoryDto dto)`: Thêm mới nhóm hàng.
   * `UpdateCategory(int id, CreateUpdateCategoryDto dto)`: Cập nhật nhóm hàng.
   * `DeleteCategory(int id)`: Xóa nhóm hàng (hoặc chuyển `IsActive = false`).

### Bước 2: Thiết kế & Nhúng Form WinForms (`FormCategoryManagement.cs`)
1. Tạo Form `FormCategoryManagement` với các Control:
   * `txtSearch`, `btnSearch`, `btnAdd`, `btnEdit`, `btnDelete`, `btnRefresh`.
   * `dgvCategories` (DataGridView cấu hình `AutoGenerateColumns = false`, map thuộc tính `DataPropertyName`).
2. Viết hàm `LoadCategoriesAsync()` gọi `ApiClientService.GetAsync<List<CategoryDto>>("api/categories")` và gán vào `dgvCategories.DataSource`.
3. Bắt sự kiện Click nút `btnCategory` tại `FormMainShell`:
   ```csharp
   private void btnCategory_Click(object sender, EventArgs e)
   {
       OpenChildForm(new FormCategoryManagement());
   }
   ```

### Bước 3: Kiểm thử Kịch bản Chức năng & Phân quyền (Buổi 4)
1. **Kiểm thử Thu ngân (`cashier01` / `123456`):**
   * Đăng nhập thành công, kiểm tra thanh Sidebar: Nút `btnCategory` bị ẩn hoặc vô hiệu hóa.
   * Thử dùng Postman gửi request `GET https://localhost:7123/api/categories` kèm Token của Cashier ➔ Kết quả phải trả về **`403 Forbidden`**.
2. **Kiểm thử Thủ kho (`ware01` / `123456`) & Admin (`admin01` / `123456`):**
   * Đăng nhập thành công ➔ Nhấp nút **Quản lý Nhóm hàng**. Form `FormCategoryManagement` được nhúng trực tiếp vào giữa giao diện.
   * Thực hiện thêm mới nhóm hàng *"Nước Giải Khát Nhập Khẩu"*, sau đó sửa tên và xóa ➔ Kiểm tra dữ liệu cập nhật tức thì trên `DataGridView` và trong CSDL SQL Server.

---

## 👨‍💻 8. Thông tin Tác giả
* **Họ và tên sinh viên:** Hồ Mai Phúc Thịnh  
* **Mã sinh viên:** 2124110119  
* **Lớp học phần:** CCQ2411D  
* **Đề tài:** Nghiên cứu và Xây dựng Ứng dụng Quản lý Cửa hàng Bán lẻ sử dụng .NET Core Web API — Siêu thị Mini Zen Mart  
