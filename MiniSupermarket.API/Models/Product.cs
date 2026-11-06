using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Products")]
    public class Product
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        // ma vach
        [Required(ErrorMessage = "Mã vạch sản phẩm không được trống")]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Barcode { get; set; } = string.Empty;

        // ten san pham
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        // gia ban phai > 0
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "1", "1000000000", ErrorMessage = "Giá bán phải lớn hơn 0")]
        public decimal Price { get; set; }

        // ton kho
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; }

        // khoa ngoai tro toi Categories.CategoryId
        public int CategoryId { get; set; }

        // lay ten nhom hang
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // san pham xuat hien trong nhieu dong chi tiet hoa don
        [JsonIgnore]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
