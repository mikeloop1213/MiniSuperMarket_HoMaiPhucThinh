CREATE OR ALTER PROCEDURE dbo.sp_DoanhThuTheoNgay
    @TuNgay  DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;
    -- gom nhom doanh thu theo tung ngay
    SELECT
        CAST(o.OrderDate AS DATE)          AS SaleDate,
        COUNT(o.OrderId)                   AS TotalOrders,
        ISNULL(SUM(o.FinalAmount), 0)      AS Revenue
    FROM Orders o
    WHERE CAST(o.OrderDate AS DATE) BETWEEN @TuNgay AND @DenNgay
    GROUP BY CAST(o.OrderDate AS DATE)
    ORDER BY SaleDate;
END
GO
