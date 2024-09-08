using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; } = Guid.NewGuid(); // Automatically generates a new GUID

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string? Description { get; set; } // Optional

        [Required]
        [Column(TypeName = "decimal(10,2)")] // Decimal type with precision 10 and scale 2
        public decimal PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")] // Decimal type with precision 10 and scale 2
        public decimal SalePrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow; // Set default to current UTC time

        public DateTime? UpdateDate { get; set; } // Nullable
    }
}
