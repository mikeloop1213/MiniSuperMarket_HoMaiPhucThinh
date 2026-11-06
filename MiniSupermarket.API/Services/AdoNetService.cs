using System.Data;
using Microsoft.Data.SqlClient;
using MiniSupermarket.API.Dtos;

namespace MiniSupermarket.API.Services
{
    public class AdoNetService
    {
        // chuoi ket noi tu appsettings.json
        private readonly string _connectionString;

        // quy doi diem thuong: dung 50 diem tru 50.000 d
        public const int PointsPerRedeem = 50;
        public const decimal DiscountPerRedeem = 50000m;

        public AdoNetService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Thiếu chuỗi kết nối 'DefaultConnection' trong appsettings.json");
        }

        // them san pham theo ma vach
        public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode)
        {
            // cau lenh SQL dung tham so @Barcode
            const string sql = @"
                SELECT p.ProductId, p.Barcode, p.ProductName, p.Price, p.StockQuantity, p.CategoryId,
                       c.CategoryName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
                WHERE p.Barcode = @Barcode";

            // khoi using tu dong Close/Dispose ket noi ke ca khi co loi
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // truyen tham so an toan, chong SQL Injection
                cmd.Parameters.AddWithValue("@Barcode", barcode.Trim());

                await conn.OpenAsync();

                // SqlDataReader doc du lieu mot chieu
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ProductDto
                        {
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            Barcode = reader.GetString(reader.GetOrdinal("Barcode")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("CategoryName"))
                        };
                    }
                }
            }
            return null;
        }

        public async Task<ShiftReportDto> GetShiftReportAsync(DateTime reportDate, string? cashierUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_BaoCaoDoanhThuCa", conn))
            {
                // bao cho ADO.NET 
                cmd.CommandType = CommandType.StoredProcedure;

                // tham so dau vao cua thu tuc
                cmd.Parameters.AddWithValue("@NgayBaoCao", reportDate.Date);
                cmd.Parameters.AddWithValue("@CashierUsername",
                    string.IsNullOrWhiteSpace(cashierUsername) ? DBNull.Value : (object)cashierUsername.Trim());

                await conn.OpenAsync();

                var report = new ShiftReportDto
                {
                    ReportDate = reportDate.ToString("yyyy-MM-dd"),
                    CashierUsername = cashierUsername,
                    BestSeller = "Chưa có dữ liệu"
                };

                // ket qua tra SqlDataReader
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        report.TotalOrders = reader.GetInt32(reader.GetOrdinal("TotalOrders"));
                        report.TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                        int idx = reader.GetOrdinal("BestSeller");
                        if (!reader.IsDBNull(idx))
                        {
                            report.BestSeller = reader.GetString(idx);
                        }
                    }
                }
                return report;
            }
        }
        //transaction
        public async Task<CheckoutResult> CheckoutAsync(CheckoutRequestDto request, string cashierUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Bat dau giao dich
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // tim khach hang theo sdt
                        int? customerId = null;
                        int currentPoints = 0;
                        string? phone = string.IsNullOrWhiteSpace(request.CustomerPhone) ? null : request.CustomerPhone.Trim();
                        if (phone != null)
                        {
                            using (var cmdFindCustomer = new SqlCommand(@"
                                SELECT CustomerId, RewardPoints
                                FROM Customers WITH (UPDLOCK, ROWLOCK)
                                WHERE PhoneNumber = @Phone", conn, transaction))
                            {
                                cmdFindCustomer.Parameters.AddWithValue("@Phone", phone);
                                using (SqlDataReader reader = await cmdFindCustomer.ExecuteReaderAsync())
                                {
                                    if (await reader.ReadAsync())
                                    {
                                        customerId = reader.GetInt32(0);
                                        currentPoints = reader.GetInt32(1);
                                    }
                                }
                            }
                        }

                        // kiem tra dieu kien doi diem truoc khi tao hoa don
                        if (request.UsePoints)
                        {
                            if (!customerId.HasValue)
                            {
                                transaction.Rollback();
                                return Fail("Khách vãng lai không có điểm tích lũy để đổi. Đã hủy giao dịch.");
                            }
                            if (currentPoints < PointsPerRedeem)
                            {
                                transaction.Rollback();
                                return Fail($"Khách hàng chỉ có {currentPoints} điểm, cần tối thiểu {PointsPerRedeem} điểm để đổi. Đã hủy giao dịch.");
                            }
                        }

                        // tim ma nhan vien lap hoa don theo ten dang nhap
                        int userId;
                        using (var cmdFindUser = new SqlCommand(
                            "SELECT UserId FROM Users WHERE Username = @Username", conn, transaction))
                        {
                            cmdFindUser.Parameters.AddWithValue("@Username", cashierUsername);
                            object? foundUser = await cmdFindUser.ExecuteScalarAsync();
                            if (foundUser == null || foundUser == DBNull.Value)
                            {
                                transaction.Rollback();
                                return Fail($"Không tìm thấy nhân viên '{cashierUsername}'. Đã hủy giao dịch.");
                            }
                            userId = Convert.ToInt32(foundUser);
                        }

                        // tao hoa don
                        int orderId;
                        using (var cmdInsertOrder = new SqlCommand(@"
                            INSERT INTO Orders (OrderDate, UserId, CustomerId, TotalAmount, PaymentMethod, OrderStatus,
                                                DiscountAmount, FinalAmount, PointsUsed, PointsEarned)
                            OUTPUT INSERTED.OrderId
                            VALUES (@OrderDate, @UserId, @CustomerId, 0, @PaymentMethod, @OrderStatus, 0, 0, 0, 0)", conn, transaction))
                        {
                            cmdInsertOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                            cmdInsertOrder.Parameters.AddWithValue("@UserId", userId);
                            cmdInsertOrder.Parameters.AddWithValue("@CustomerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                            cmdInsertOrder.Parameters.AddWithValue("@PaymentMethod", "Tiền mặt");
                            cmdInsertOrder.Parameters.AddWithValue("@OrderStatus", "Hoàn thành");
                            orderId = Convert.ToInt32(await cmdInsertOrder.ExecuteScalarAsync());
                        }

                        decimal totalAmount = 0;

                        // xu ly san pham
                        foreach (var item in request.Items)
                        {
                            if (item.Quantity <= 0)
                            {
                                transaction.Rollback();   //du lieu sai
                                return Fail($"Số lượng của sản phẩm #{item.ProductId} không hợp lệ. Đã hủy hóa đơn.");
                            }

                            string productName;
                            decimal price;
                            int stock;

                            // Doc san pham va khoa dong
                            using (var cmdGetProduct = new SqlCommand(@"
                                SELECT ProductName, Price, StockQuantity
                                FROM Products WITH (UPDLOCK, ROWLOCK)
                                WHERE ProductId = @ProductId", conn, transaction))
                            {
                                cmdGetProduct.Parameters.AddWithValue("@ProductId", item.ProductId);
                                using (SqlDataReader reader = await cmdGetProduct.ExecuteReaderAsync())
                                {
                                    if (!await reader.ReadAsync())
                                    {
                                        reader.Close();
                                        transaction.Rollback();
                                        return Fail($"Không tìm thấy sản phẩm mã #{item.ProductId}. Đã hủy hóa đơn.");
                                    }
                                    productName = reader.GetString(0);
                                    price = reader.GetDecimal(1);
                                    stock = reader.GetInt32(2);
                                }
                            }

                            // kho het hang
                            if (stock < item.Quantity)
                            {
                                transaction.Rollback();
                                return Fail($"Sản phẩm '{productName}' chỉ còn {stock} trong kho, không đủ để bán {item.Quantity}. " +
                                            "Giao dịch đã được hoàn tác (Rollback), hóa đơn không được lưu.");
                            }

                            // tru ton kho
                            using (var cmdUpdateStock = new SqlCommand(
                                "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductId = @ProductId", conn, transaction))
                            {
                                cmdUpdateStock.Parameters.AddWithValue("@Qty", item.Quantity);
                                cmdUpdateStock.Parameters.AddWithValue("@ProductId", item.ProductId);
                                await cmdUpdateStock.ExecuteNonQueryAsync();
                            }

                            // them dong chi tiet hoa don
                            using (var cmdInsertDetail = new SqlCommand(@"
                                INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice)
                                VALUES (@OrderId, @ProductId, @Qty, @Price)", conn, transaction))
                            {
                                cmdInsertDetail.Parameters.AddWithValue("@OrderId", orderId);
                                cmdInsertDetail.Parameters.AddWithValue("@ProductId", item.ProductId);
                                cmdInsertDetail.Parameters.AddWithValue("@Qty", item.Quantity);
                                cmdInsertDetail.Parameters.AddWithValue("@Price", price);
                                await cmdInsertDetail.ExecuteNonQueryAsync();
                            }

                            totalAmount += price * item.Quantity;
                        }

                        // doi diem thuong tru tien hoa don
                        int pointsUsed = 0;
                        decimal discountAmount = 0;
                        if (request.UsePoints)
                        {
                            if (totalAmount < DiscountPerRedeem)
                            {
                                transaction.Rollback();
                                return Fail($"Hóa đơn phải từ {DiscountPerRedeem:N0} đ trở lên mới được dùng {PointsPerRedeem} điểm. Đã hủy giao dịch.");
                            }
                            pointsUsed = PointsPerRedeem;
                            discountAmount = DiscountPerRedeem;
                        }
                        decimal finalAmount = totalAmount - discountAmount;

                        // khach thanh vien duoc tich 1 diem cho moi 10.000 d thuc tra
                        int addedPoints = customerId.HasValue ? (int)(finalAmount / 10000) : 0;

                        // cap nhat tong tien, tien giam va tien thanh toan cua hoa don
                        using (var cmdUpdateTotal = new SqlCommand(@"
                            UPDATE Orders
                            SET TotalAmount = @Total,
                                PointsUsed = @PointsUsed,
                                DiscountAmount = @Discount,
                                FinalAmount = @Final,
                                PointsEarned = @PointsEarned
                            WHERE OrderId = @OrderId", conn, transaction))
                        {
                            cmdUpdateTotal.Parameters.AddWithValue("@Total", totalAmount);
                            cmdUpdateTotal.Parameters.AddWithValue("@PointsUsed", pointsUsed);
                            cmdUpdateTotal.Parameters.AddWithValue("@Discount", discountAmount);
                            cmdUpdateTotal.Parameters.AddWithValue("@Final", finalAmount);
                            cmdUpdateTotal.Parameters.AddWithValue("@PointsEarned", addedPoints);
                            cmdUpdateTotal.Parameters.AddWithValue("@OrderId", orderId);
                            await cmdUpdateTotal.ExecuteNonQueryAsync();
                        }

                        // tru diem da dung, cong diem moi & nang cap hang thanh vien
                        if (customerId.HasValue)
                        {
                            using (var cmdPoints = new SqlCommand(@"
                                UPDATE Customers
                                SET RewardPoints = RewardPoints - @PointsUsed + @Points,
                                    MembershipRank = CASE
                                        WHEN RewardPoints - @PointsUsed + @Points >= 500 THEN N'Kim cương'
                                        WHEN RewardPoints - @PointsUsed + @Points >= 200 THEN N'Vàng'
                                        WHEN RewardPoints - @PointsUsed + @Points >= 80  THEN N'Bạc'
                                        ELSE MembershipRank END
                                WHERE CustomerId = @CustomerId AND RewardPoints >= @PointsUsed", conn, transaction))
                            {
                                cmdPoints.Parameters.AddWithValue("@PointsUsed", pointsUsed);
                                cmdPoints.Parameters.AddWithValue("@Points", addedPoints);
                                cmdPoints.Parameters.AddWithValue("@CustomerId", customerId.Value);
                                int affected = await cmdPoints.ExecuteNonQueryAsync();
                                if (affected == 0)
                                {
                                    transaction.Rollback();
                                    return Fail("Không trừ được điểm thưởng của khách hàng. Giao dịch đã được hoàn tác (Rollback).");
                                }
                            }
                        }

                        // trang thai giao dich thanh cong
                        transaction.Commit();

                        return new CheckoutResult
                        {
                            Success = true,
                            OrderId = orderId,
                            TotalAmount = totalAmount,
                            PointsUsed = pointsUsed,
                            DiscountAmount = discountAmount,
                            FinalAmount = finalAmount,
                            AddedPoints = addedPoints,
                            Message = pointsUsed > 0
                                ? $"Thanh toán thành công! Đã dùng {pointsUsed} điểm, giảm {discountAmount:N0} đ."
                                : "Thanh toán đơn hàng thành công!"
                        };
                    }
                    catch
                    {
                        // loi bat ngo (tu ngat ket noi)
                        try { transaction.Rollback(); } catch { /* transaction co the da bi huy */ }
                        throw;
                    }
                }
            }
        }

        // doanh thu tung ngay trong khoang thoi gian (goi Stored Procedure)
        public async Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            var result = new List<DailyRevenueDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_DoanhThuTheoNgay", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TuNgay", fromDate.Date);
                cmd.Parameters.AddWithValue("@DenNgay", toDate.Date);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new DailyRevenueDto
                        {
                            SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                            TotalOrders = reader.GetInt32(reader.GetOrdinal("TotalOrders")),
                            Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue"))
                        });
                    }
                }
            }
            return result;
        }

        // Ham phu tao ket qua that bai
        private static CheckoutResult Fail(string message) => new CheckoutResult { Success = false, Message = message };


        // tao & cap nhat Stored Procedure sp_BaoCaoDoanhThuCa, sp_DoanhThuTheoNgay

        public async Task EnsureStoredProceduresAsync()
        {
            const string createSp = @"
CREATE OR ALTER PROCEDURE dbo.sp_BaoCaoDoanhThuCa
    @NgayBaoCao      DATE,
    @CashierUsername NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(o.OrderId)               AS TotalOrders,
        ISNULL(SUM(o.FinalAmount), 0)  AS TotalRevenue,
        (
            SELECT TOP 1 p.ProductName + N' (' + CAST(SUM(d.Quantity) AS NVARCHAR(20)) + N' cái)'
            FROM OrderDetails d
            INNER JOIN Orders o2  ON d.OrderId = o2.OrderId
            INNER JOIN Products p ON d.ProductId = p.ProductId
            WHERE CAST(o2.OrderDate AS DATE) = @NgayBaoCao
              AND (@CashierUsername IS NULL OR o2.UserId IN (SELECT u.UserId FROM Users u WHERE u.Username = @CashierUsername))
            GROUP BY p.ProductName
            ORDER BY SUM(d.Quantity) DESC
        )                              AS BestSeller
    FROM Orders o
    WHERE CAST(o.OrderDate AS DATE) = @NgayBaoCao
      AND (@CashierUsername IS NULL OR o.UserId IN (SELECT u.UserId FROM Users u WHERE u.Username = @CashierUsername));
END";
            const string createSpDaily = @"
CREATE OR ALTER PROCEDURE dbo.sp_DoanhThuTheoNgay
    @TuNgay  DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CAST(o.OrderDate AS DATE)          AS SaleDate,
        COUNT(o.OrderId)                   AS TotalOrders,
        ISNULL(SUM(o.FinalAmount), 0)      AS Revenue
    FROM Orders o
    WHERE CAST(o.OrderDate AS DATE) BETWEEN @TuNgay AND @DenNgay
    GROUP BY CAST(o.OrderDate AS DATE)
    ORDER BY SaleDate;
END";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                foreach (string sql in new[] { createSp, createSpDaily })
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}
