using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RewardPoints = table.Column<int>(type: "int", nullable: false),
                    MembershipRank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PointsUsed = table.Column<int>(type: "int", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PointsEarned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Bánh kẹo & Đồ ăn vặt", "Snack khoai tây, bánh quy, kẹo dẻo và đồ ăn nhẹ" },
                    { 2, "Nước giải khát có ga & Trà", "Nước ngọt, nước tăng lực, trà đóng chai các loại" },
                    { 3, "Nước khoáng & Nước tinh khiết", "Nước suối đóng chai, nước khoáng thiên nhiên" },
                    { 4, "Sữa & Sản phẩm từ sữa", "Sữa tươi tiệt trùng, sữa chua uống, phô mai que" },
                    { 5, "Mì gói & Thực phẩm ăn liền", "Mì ly, phở khô, cháo gói và xúc xích tiệt trùng" },
                    { 6, "Cà phê & Đồ uống hòa tan", "Cà phê lon, cà phê đen/sữa đóng gói, trà hòa tan" },
                    { 7, "Bánh mì & Thức ăn nhanh", "Bánh mì sandwich, hamburger, cơm nắm Onigiri" },
                    { 8, "Kem & Đồ tráng miệng lạnh", "Kem que, kem ốc quế, sữa chua giải nhiệt" },
                    { 9, "Đồ hộp & Chế biến sẵn", "Cá hộp, pate gan, thịt hộp và sốt cà đóng hộp" },
                    { 10, "Gia vị & Dầu ăn tiện lợi", "Nước mắm, hạt nêm, tương ớt, dầu ăn chai nhỏ" },
                    { 11, "Hóa mỹ phẩm & Chăm sóc cá nhân", "Dầu gội, sữa tắm, bàn chải và kem đánh răng" },
                    { 12, "Khăn giấy & Vệ sinh cá nhân", "Khăn ướt tiệt trùng, khăn giấy lụa, túi rác mini" },
                    { 13, "Văn phòng phẩm & Tiện ích nhỏ", "Bút bi, sổ tay ghi chú mini, bật lửa, kéo" },
                    { 14, "Vật dụng cá nhân & Y tế mini", "Khẩu trang kháng khuẩn, băng cá nhân, áo mưa tiện lợi" },
                    { 15, "Trái cây & Đồ tươi ăn liền", "Trái cây gọt sẵn, salad đóng hộp, rau củ quả tươi" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CreatedAt", "CustomerName", "MembershipRank", "PhoneNumber", "RewardPoints" },
                values: new object[,]
                {
                    { 1, "Số 2 đường số 5, phường Hiệp Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Thị Lan", "Vàng", "0903456789", 250 },
                    { 2, "Số 934 tỉnh lộ 43, phường Tam Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Văn Hùng", "Bạc", "0918765432", 95 },
                    { 3, "Số 17 Chân Lý, phường Thủ Đức, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Thị Mai", "Chuẩn", "0987654321", 20 },
                    { 4, "Số 81 Hoàng Cầm, phường Linh Xuân, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phạm Minh Quân", "Kim cương", "0908112233", 420 },
                    { 5, "Số 325 Nguyễn Văn Tăng, phường Long Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàng Anh Tú", "Vàng", "0912334455", 180 },
                    { 6, "Số 29 Lê Văn Việt, phường Tăng Nhơn Phú, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bùi Thị Hạnh", "Vàng", "0977554433", 320 },
                    { 7, "Số 183 Đỗ Xuân Hợp, phường Phước Long, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đỗ Quốc Khánh", "Bạc", "0966443322", 40 },
                    { 8, "Số 239 khu phố Long Thuận, phường Long Phước, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Võ Thị Nhi", "Chuẩn", "0934567788", 120 },
                    { 9, "Số 1341 Nguyễn Duy Trinh, phường Long Trường, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Hoàng Nam", "Vàng", "0922334455", 260 },
                    { 10, "Số 10 đường D2, phường An Khánh, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lý Thị Hồng", "Bạc", "0944556677", 90 },
                    { 11, "Số 8 Hồ Thị Nhung, khu phố 4, phường Bình Trưng, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phan Thanh Tuấn", "Vàng", "0938123456", 310 },
                    { 12, "Số 560 Trương Gia Mô, phường Cát Lái, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đặng Thu Thảo", "Kim cương", "0979888999", 550 },
                    { 13, "Số 69 đường Linh Đông, phường Hiệp Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vũ Trọng Phúc", "Chuẩn", "0909654321", 15 },
                    { 14, "Số 719 và 721 Quốc lộ 13, phường Hiệp Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trương Mỹ Linh", "Bạc", "0917223344", 85 },
                    { 15, "Số 707 đường Tô Ngọc Vân, phường Tam Bình, Thành phố Thủ Đức, Thành phố Hồ Chí Minh", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hà Quốc Bảo", "Vàng", "0988112244", 160 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Quản trị viên toàn hệ thống, toàn quyền thao tác", "Admin" },
                    { 2, "Nhân viên thu ngân, chỉ bán hàng tại quầy POS", "Cashier" },
                    { 3, "Quản lý nhóm hàng, sản phẩm và tồn kho", "Warehouse" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Quản Trị", true, "123456", null, "Admin", "admin01" },
                    { 2, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Giám Đốc", true, "123456", null, "Admin", "admin02" },
                    { 3, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Thu Ngân", true, "123456", null, "Cashier", "cashier01" },
                    { 4, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phạm Bán Hàng", true, "123456", null, "Cashier", "cashier02" },
                    { 5, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàng Thu Ngân", true, "123456", null, "Cashier", "cashier03" },
                    { 6, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vũ Thị Quầy", true, "123456", null, "Cashier", "cashier04" },
                    { 7, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đỗ Bán Lẻ", true, "123456", null, "Cashier", "cashier05" },
                    { 8, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ngô Quản Kho", true, "123456", null, "Warehouse", "ware01" },
                    { 9, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bùi Kiểm Kê", true, "123456", null, "Warehouse", "ware02" },
                    { 10, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dương Thủ Kho", true, "123456", null, "Warehouse", "ware03" },
                    { 11, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lý Nhập Hàng", true, "123456", null, "Warehouse", "ware04" },
                    { 12, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đặng Hỗ Trợ", true, "123456", null, "Admin", "admin_backup" },
                    { 13, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hồ Ca Chiều", true, "123456", null, "Cashier", "cashier06" },
                    { 14, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trương Vận Chuyển", true, "123456", null, "Warehouse", "ware05" },
                    { 15, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mai Giám Sát", true, "123456", null, "Admin", "supervisor" },
                    { 16, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quản Trị Viên Mặc Định", true, "123456", null, "Admin", "admin" },
                    { 17, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thu Ngân Mặc Định", true, "123456", null, "Cashier", "cashier" },
                    { 18, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thủ Kho Mặc Định", true, "123456", null, "Warehouse", "warehouse" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "893500123001", 1, 16000m, "Snack khoai tây Lay's vị tự nhiên 56g", 120 },
                    { 2, "893500123002", 2, 11000m, "Nước ngọt Coca-Cola Sleek lon 320ml", 200 },
                    { 3, "893500123003", 3, 6000m, "Nước khoáng thiên nhiên La Vie 500ml", 250 },
                    { 4, "893500123004", 4, 10000m, "Sữa tươi tiệt trùng Vinamilk ít đường 180ml", 150 },
                    { 5, "893500123005", 5, 4500m, "Mì Hảo Hảo tôm chua cay 75g", 300 },
                    { 6, "893500123006", 6, 15000m, "Cà phê sữa Nescafé 3in1 lon 180ml", 80 },
                    { 7, "893500123007", 7, 18000m, "Cơm nắm cá hồi nướng Onigiri 100g", 45 },
                    { 8, "893500123008", 8, 19000m, "Kem que sô cô la Cornetto 80ml", 60 },
                    { 9, "893500123009", 9, 22000m, "Cá nục sốt cà Ba Cô Gái hộp 155g", 75 },
                    { 10, "893500123010", 10, 14000m, "Tương ớt Chin-su chai tiện lợi 250g", 90 },
                    { 11, "893500123011", 11, 38000m, "Sữa tắm dưỡng ẩm Romano Classic 150g", 50 },
                    { 12, "893500123012", 12, 15000m, "Khăn ướt kháng khuẩn Caryn 20 tờ", 110 },
                    { 13, "893500123013", 13, 5000m, "Bút bi gel Thiên Long xanh 0.5mm", 140 },
                    { 14, "893500123014", 14, 12000m, "Khẩu trang y tế 4 lớp (gói 10 cái)", 160 },
                    { 15, "893500123015", 15, 20000m, "Trái cây gọt sẵn ổi ruột hồng 200g", 35 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                table: "Products",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
