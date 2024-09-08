using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid(); // Automatically generates a new GUID

        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string EmailAddress { get; set; }

        [StringLength(2048)]
        public string? PhotoUrl { get; set; } // Optional

        [Required]
        public string PasswordHash { get; set; } // Store password hash, not plaintext

        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [JsonIgnore] // Prevent circular reference when serializing
        public virtual Role Role { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow; // Set default to current UTC time

        public DateTime? UpdateDate { get; set; } // Nullable
    }
}
