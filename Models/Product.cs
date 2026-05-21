using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("PRODUCT")]
    public class Product
    {
        [Key][Column("product_id")] public int ProductId { get; set; }
        //relationship
        [Required][Column("category_id")] public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [StringLength(200)]
        [Column("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [Required][Column("price")] public decimal Price { get; set; }

        [Required][Column("stock_quantity")] public int StockQuantity { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Column("image_path")]
        public string ImagePath { get; set; } = string.Empty;
    }
}