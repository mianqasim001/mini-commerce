using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class CartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        // Fetch cart with all items and product details included
        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Find a specific item within a cart
        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        // Standard CRUD operations
        public void AddCart(Cart cart) => _context.Carts.Add(cart);
        public void AddCartItem(CartItem item) => _context.CartItems.Add(item);
        public void RemoveCartItem(CartItem item) => _context.CartItems.Remove(item);
        
        // Remove a collection of items (Used for Clear Cart)
        public void RemoveCartItems(IEnumerable<CartItem> items) => _context.CartItems.RemoveRange(items);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}