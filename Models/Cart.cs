using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("CART")]
    public class Cart
    {
        [Key]
        [Column("cart_id")]
        public int CartId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("total_items")]
        public int TotalItems { get; set; } = 0;

        [Required]
        [Column("total_amount")]
        public decimal TotalAmount { get; set; } = 0;

        [Required]
        [StringLength(50)]
        [Column("cart_status")]
        public string CartStatus { get; set; } = "Active"; // Active, Abandoned, Converted

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}