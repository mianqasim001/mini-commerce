namespace backend.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        // This can now come directly from the DB column we just added
        public decimal TotalItemPrice { get; set; } 
    }
}