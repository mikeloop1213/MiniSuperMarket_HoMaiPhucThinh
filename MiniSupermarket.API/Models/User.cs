using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        // ten dang nhap
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; } = string.Empty;

        // mat khau (cot PasswordHash trong CSDL)
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        [Column("PasswordHash", TypeName = "varchar(255)")]
        public string Password { get; set; } = string.Empty;

        // ho ten nhan vien
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // vai tro
        [Required]
        [StringLength(30)]
        public string Role { get; set; } = AppRoles.Cashier;

        // so dien thoai nhan vien
        [StringLength(15)]
        [Column(TypeName = "varchar(15)")]
        public string? PhoneNumber { get; set; }

        // ngay tao tai khoan
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // false = tai khoan bi khoa, khong cho dang nhap
        public bool IsActive { get; set; } = true;
    }
}
