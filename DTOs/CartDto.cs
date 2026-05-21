using System.Collections.Generic; // Required for List
using System.Linq;                // Required for .Sum()

namespace backend.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        
        // This matches the "total_items" column in your CART table
        public int TotalItemsCount { get; set; } 
        
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

        // We use the 'TotalAmount' column from your DB as the Subtotal
        public decimal Subtotal { get; set; } 

        public decimal Discount { get; set; }

        // The final price shown to the user (e.g., Rs. 2,060)
        public decimal Total => Subtotal - Discount;
    }
}