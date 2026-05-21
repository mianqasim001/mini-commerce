using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("USER")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)] // Prevents 1-letter names or massive strings
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress] // Automatically validates the @ and .com format
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        [Column("address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        // Metadata - we usually don't validate these as the system sets them
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}