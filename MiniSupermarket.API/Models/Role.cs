using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models {
    [Table("Roles")]
    public class Role {
        // ma dinh danh vai tro
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // ten vai tro
        [Required(ErrorMessage = "Tên vai trò không được để trống!")]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty;

        // mo ta chi tiet chuc nang cua vai tro
        [StringLength(255)]
        public string? Description { get; set; }
    }
}
