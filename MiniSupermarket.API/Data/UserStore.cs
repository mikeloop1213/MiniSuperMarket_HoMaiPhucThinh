using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    public static class UserStore
    {
        public static readonly List<UserAccount> Users = new()
        {
            new UserAccount { Id = 1, Username = "admin01", Password = "123456", FullName = "Nguyễn Quản Trị", Role = "Admin", IsActive = true },
            new UserAccount { Id = 2, Username = "admin02", Password = "123456", FullName = "Trần Giám Đốc", Role = "Admin", IsActive = true },
            new UserAccount { Id = 3, Username = "cashier01", Password = "123456", FullName = "Lê Thu Ngân", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 4, Username = "cashier02", Password = "123456", FullName = "Phạm Bán Hàng", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 5, Username = "cashier03", Password = "123456", FullName = "Hoàng Thu Ngân", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 6, Username = "cashier04", Password = "123456", FullName = "Vũ Thị Quầy", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 7, Username = "cashier05", Password = "123456", FullName = "Đỗ Bán Lẻ", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 8, Username = "ware01", Password = "123456", FullName = "Ngô Quản Kho", Role = "Warehouse", IsActive = true },
            new UserAccount { Id = 9, Username = "ware02", Password = "123456", FullName = "Bùi Kiểm Kê", Role = "Warehouse", IsActive = true },
            new UserAccount { Id = 10, Username = "ware03", Password = "123456", FullName = "Dương Thủ Kho", Role = "Warehouse", IsActive = true },
            new UserAccount { Id = 11, Username = "ware04", Password = "123456", FullName = "Lý Nhập Hàng", Role = "Warehouse", IsActive = true },
            new UserAccount { Id = 12, Username = "admin_backup", Password = "123456", FullName = "Đặng Hỗ Trợ", Role = "Admin", IsActive = true },
            new UserAccount { Id = 13, Username = "cashier06", Password = "123456", FullName = "Hồ Ca Chiều", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 14, Username = "ware05", Password = "123456", FullName = "Trương Vận Chuyển", Role = "Warehouse", IsActive = true },
            new UserAccount { Id = 15, Username = "supervisor", Password = "123456", FullName = "Mai Giám Sát", Role = "Admin", IsActive = true },

            // Fallback tài khoản mặc định
            new UserAccount { Id = 16, Username = "admin", Password = "123456", FullName = "Quản Trị Viên Mặc Định", Role = "Admin", IsActive = true },
            new UserAccount { Id = 17, Username = "cashier", Password = "123456", FullName = "Thu Ngân Mặc Định", Role = "Cashier", IsActive = true },
            new UserAccount { Id = 18, Username = "warehouse", Password = "123456", FullName = "Thủ Kho Mặc Định", Role = "Warehouse", IsActive = true }
        };
    }
}
