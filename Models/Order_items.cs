using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    [Table("ORDER_ITEM")]
    public class OrderItem
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("price")] // Snapshots the price at time of purchase
        public decimal Price { get; set; }

        [Required]
        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")] [JsonIgnore] public virtual Order? Order { get; set; }
        [ForeignKey("ProductId")] public virtual Product? Product { get; set; }
    }
}