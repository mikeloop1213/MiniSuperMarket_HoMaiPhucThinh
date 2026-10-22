using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class CleanSeededDatabase : Migration
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
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RewardPoints = table.Column<int>(type: "int", nullable: false),
                    MembershipRank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Bánh kẹo & Đồ ăn vặt", "Snack, bánh quy, kẹo dẻo" },
                    { 2, "Nước giải khát & Trà", "Nước ngọt, nước khoáng, trà" },
                    { 3, "Sữa & Sản phẩm từ sữa", "Sữa tươi, sữa chua, phô mai" },
                    { 4, "Mì gói & Thực phẩm ăn liền", "Mì ăn liền, phở khô, cháo gói" },
                    { 5, "Gia vị & Dầu ăn", "Nước mắm, hạt nêm, dầu thực vật" },
                    { 6, "Rau củ & Trái cây", "Rau xanh, trái cây, nông sản tươi" },
                    { 7, "Thịt & Hải sản", "Thịt heo, gà, cá, tôm" },
                    { 8, "Đồ hộp & Đóng gói", "Đồ hộp, thực phẩm lưu trữ" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CustomerName", "MembershipRank", "PhoneNumber", "RewardPoints" },
                values: new object[,]
                {
                    { 1, "Q.1, TP.HCM", "Nguyễn Văn A", "Vàng", "0901122334", 150 },
                    { 2, "Q.7, TP.HCM", "Trần Thị B", "Bạc", "0918877665", 50 },
                    { 3, "Q.3, TP.HCM", "Lê Văn C", "Chuẩn", "0983344556", 10 },
                    { 4, "Q.5, TP.HCM", "Phạm Thị D", "Kim cương", "0909988776", 320 },
                    { 5, "Q.10, TP.HCM", "Hoàng Văn E", "Vàng", "0912345678", 80 },
                    { 6, "Q.12, TP.HCM", "Nguyễn Thị F", "Vàng", "0977665544", 210 },
                    { 7, "Q.4, TP.HCM", "Đỗ Văn G", "Bạc", "0966554433", 25 },
                    { 8, "Q.2, TP.HCM", "Mai Thị H", "Chuẩn", "0932456789", 70 },
                    { 9, "Q.8, TP.HCM", "Vũ Văn K", "Vàng", "0922334455", 140 },
                    { 10, "Q.11, TP.HCM", "Lý Thị M", "Bạc", "0944556677", 60 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "893500123001", 1, 18000m, "Oishi Snack Seaweed", 120 },
                    { 2, "893500123002", 1, 22000m, "Bánh quy bơ sữa", 85 },
                    { 3, "893500123003", 1, 15000m, "Kẹo mềm dâu tây", 140 },
                    { 4, "893500123004", 2, 12000m, "Coca Cola 330ml", 200 },
                    { 5, "893500123005", 2, 7000m, "Nước suối Lavie 500ml", 250 },
                    { 6, "893500123006", 2, 16000m, "Trà xanh không độ", 110 },
                    { 7, "893500123007", 3, 23000m, "Sữa tươi Vinamilk", 90 },
                    { 8, "893500123008", 3, 18000m, "Sữa chua ăn hương dâu", 130 },
                    { 9, "893500123009", 3, 52000m, "Phô mai mozzarella", 40 },
                    { 10, "893500123010", 4, 9000m, "Mì Gấu Đỏ 65g", 180 },
                    { 11, "893500123011", 4, 14000m, "Phở khô bò hầm", 100 },
                    { 12, "893500123012", 4, 11000m, "Cháo gói thịt nấm", 160 },
                    { 13, "893500123013", 5, 30000m, "Nước mắm Nam Ngư", 75 },
                    { 14, "893500123014", 5, 12000m, "Muối hạt tiêu", 95 },
                    { 15, "893500123015", 5, 36000m, "Dầu ăn Neptune 1L", 60 },
                    { 16, "893500123016", 6, 15000m, "Rau cải xanh", 80 },
                    { 17, "893500123017", 6, 45000m, "Táo Mỹ", 70 },
                    { 18, "893500123018", 6, 28000m, "Cam sành", 110 },
                    { 19, "893500123019", 7, 92000m, "Thịt heo ba rọi", 45 },
                    { 20, "893500123020", 7, 99000m, "Cá basa tươi", 36 },
                    { 21, "893500123021", 7, 180000m, "Tôm sú thẻ", 28 },
                    { 22, "893500123022", 8, 17000m, "Đậu đóng hộp", 90 },
                    { 23, "893500123023", 8, 21000m, "Nước sốt cà chua", 80 },
                    { 24, "893500123024", 8, 24000m, "Đồ hộp ngũ cốc", 60 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
