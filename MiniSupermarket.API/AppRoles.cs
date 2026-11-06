namespace MiniSupermarket.API
{
    // ten vai tro dung trong bang Users.Role va JWT
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Cashier = "Cashier";
        public const string Warehouse = "Warehouse";

        // nhom quyen dung trong [Authorize(Roles = ...)]
        public const string AdminGroup = Admin;
        public const string AdminOrCashier = Admin + "," + Cashier;
        public const string AdminOrWarehouse = Admin + "," + Warehouse;

        public static readonly string[] All = { Admin, Cashier, Warehouse };
    }
}
