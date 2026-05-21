using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("CATEGORY")]
    public class Category
    {
        [Key][Column("category_id")] public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("category_type")]
        public string CategoryType { get; set; } = string.Empty;

        // This links the Category to all Products assigned to it
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}