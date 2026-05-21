namespace backend.DTOs
{
    // 1. The one your GET request uses (MISSING IN YOUR ERROR)
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        // Using string? (nullable) allows the GET request to work even if 
        // the product hasn't been assigned a category name yet.
        public string? CategoryName { get; set; }
         // This will return the path saved in the DB (e.g., "/uploads/filename.jpg")
        public string ImagePath { get; set; } = string.Empty;
    }

    // 2. The one your POST request uses
    public class ProductCreateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }  // ✅ Moved here (was in wrong class)
        public IFormFile? ImageFile { get; set; }
    }

    // 3. The one your PUT request uses
    public class ProductUpdateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }  // ✅ Add this
        public IFormFile? ImageFile { get; set; }
    }
}