CREATE OR ALTER PROCEDURE dbo.sp_BaoCaoDoanhThuCa
    @NgayBaoCao      DATE,
    @CashierUsername NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    -- hoa don
    SELECT
        COUNT(o.OrderId)               AS TotalOrders,
        ISNULL(SUM(o.FinalAmount), 0)  AS TotalRevenue,
        (
            -- Mat hang ban chay
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
END
GO
