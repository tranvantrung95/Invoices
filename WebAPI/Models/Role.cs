using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid(); // Automatically generates a new GUID

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow; // Set default to current UTC time

        public DateTime? UpdateDate { get; set; } // Nullable

        [JsonIgnore] // Prevent circular reference when serializing
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
