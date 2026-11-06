using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        // ten khach
        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        [StringLength(100, ErrorMessage = "Tên khách hàng tối đa 100 ký tự")]
        public string CustomerName { get; set; } = string.Empty;

        // sdt
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15)]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; } = string.Empty;

        // dia chi
        [StringLength(255)]
        public string? Address { get; set; }

        // diem tich luy
        [Range(0, int.MaxValue)]
        public int RewardPoints { get; set; } = 0;

        // hang thanh vien: Chuan / Bac / Vang / Kim cuong
        [StringLength(50)]
        public string MembershipRank { get; set; } = "Chuẩn";

        // ngay dang ky thanh vien (cot CreatedAt trong CSDL)
        [Column("CreatedAt")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        // khach hang co nhieu hoa don
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
