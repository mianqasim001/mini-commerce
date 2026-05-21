using backend.DTOs;
using backend.Models;
using backend.Repositories;
using backend.Data;

namespace backend.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepo;
        private readonly AppDbContext _context;

        public CartService(CartRepository cartRepo, AppDbContext context)
        {
            _cartRepo = cartRepo;
            _context = context;
        }

        // Convert Cart Entity to DTO for the Frontend
        public async Task<CartDto?> GetCartDtoAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return null;

            return new CartDto
            {
                CartId = cart.CartId,
                TotalItemsCount = cart.TotalItems,
                Subtotal = cart.TotalAmount,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown",
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    TotalItemPrice = ci.Subtotal,
                    ImagePath = ci.Product?.ImagePath ?? "/uploads/default.png"
                }).ToList(),
                Discount = 140 // Hardcoded as per your requirement
            };
        }

        // Incremental Add (e.g. clicking "Add to Cart" on a product card)
        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartStatus = "Active" };
                _cartRepo.AddCart(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var item = await _cartRepo.GetCartItemAsync(cart.CartId, productId);
            int currentInCart = item?.Quantity ?? 0;

            if (product.StockQuantity < (currentInCart + quantity))
                throw new InvalidOperationException("Insufficient stock.");

            if (item != null)
            {
                item.Quantity += quantity;
                item.Subtotal = item.Quantity * product.Price;
            }
            else
            {
                _cartRepo.AddCartItem(new CartItem { CartId = cart.CartId, ProductId = productId, Quantity = quantity, Subtotal = quantity * product.Price });
            }

            await _cartRepo.SaveChangesAsync();
            await SyncCartTotalsAsync(cart);
        }

        // Set Exact Quantity (e.g. typing "5" in the cart page or using +/- buttons)
        public async Task UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0) { await RemoveFromCartAsync(userId, productId); return; }

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found");
            if (product.StockQuantity < quantity) throw new InvalidOperationException("Insufficient stock.");

            var item = await _cartRepo.GetCartItemAsync(cart.CartId, productId);
            if (item == null) throw new Exception("Item not in cart");

            item.Quantity = quantity; // We assign the exact value
            item.Subtotal = item.Quantity * product.Price;

            await _cartRepo.SaveChangesAsync();
            await SyncCartTotalsAsync(cart);
        }

        // Remove a single product
        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return;

            var item = await _cartRepo.GetCartItemAsync(cart.CartId, productId);
            if (item != null)
            {
                _cartRepo.RemoveCartItem(item);
                await _cartRepo.SaveChangesAsync();
                await SyncCartTotalsAsync(cart);
            }
        }

        // Wipe the entire cart (Used after checkout or "Empty Cart" button)
        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null && cart.CartItems.Any())
            {
                _cartRepo.RemoveCartItems(cart.CartItems);
                await _cartRepo.SaveChangesAsync();
                await SyncCartTotalsAsync(cart);
            }
        }

        // Recalculates the Total Amount and Total Count for the main Cart table
        private async Task SyncCartTotalsAsync(Cart cart)
        {
            cart.TotalItems = cart.CartItems.Sum(i => i.Quantity);
            cart.TotalAmount = cart.CartItems.Sum(i => i.Subtotal);
            cart.UpdatedAt = DateTime.Now;
            await _cartRepo.SaveChangesAsync();
        }
    }
}