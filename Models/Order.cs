using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("ORDERS")]
    public class Order
    {
        [Key][Column("order_id")] public int OrderId { get; set; }

        [Required][Column("user_id")] public int UserId { get; set; }

        [Required][Column("total_amount")] public decimal TotalAmount { get; set; }

        [Required]
        [Column("shipping_address")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [Column("order_status")]
        public string OrderStatus { get; set; } = "Pending";

        [Required][Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationship: One Order has many OrderItems
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}