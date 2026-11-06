using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // Thoi diem lap hoa don
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // nhan vien lap hoa don (khoa ngoai toi Users)
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        // khoa ngoai toi Customers
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        // hinh thuc thanh toan
        [Required]
        [StringLength(30)]
        public string PaymentMethod { get; set; } = "Tiền mặt";

        // trang thai hoa don
        [Required]
        [StringLength(30)]
        public string OrderStatus { get; set; } = "Hoàn thành";

        // tong tien hoa don
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "100000000000")]
        public decimal TotalAmount { get; set; }

        // so diem thuong da dung de tru tien
        [Range(0, int.MaxValue)]
        public int PointsUsed { get; set; }

        // so tien duoc giam nho doi diem
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "100000000000")]
        public decimal DiscountAmount { get; set; }

        // so tien khach thuc tra = TotalAmount - DiscountAmount
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "100000000000")]
        public decimal FinalAmount { get; set; }

        // so diem tich them sau khi thanh toan
        [Range(0, int.MaxValue)]
        public int PointsEarned { get; set; }

        // danh sach chi tiet hoa don
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
