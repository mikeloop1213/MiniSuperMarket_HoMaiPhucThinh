namespace MiniSupermarket.API.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Admin", "Cashier", "Warehouse"
        public bool IsActive { get; set; } = true;
    }
}
