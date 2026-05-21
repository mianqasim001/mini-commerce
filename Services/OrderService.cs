using backend.Models;
using backend.Repositories;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;
        private readonly CartRepository _cartRepo;
        private readonly AppDbContext _context;

        public OrderService(OrderRepository orderRepo, CartRepository cartRepo, AppDbContext context)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _context = context;
        }

        public async Task<Order> CheckoutAsync(int userId, string address)
        {
            // Use a transaction so that if any step fails, the DB rolls back to its original state
            using var transaction = await _orderRepo.BeginTransactionAsync();
            try
            {
                // 1. Get Cart with Items
                var cart = await _cartRepo.GetCartByUserIdAsync(userId);
                if (cart == null || !cart.CartItems.Any())
                    throw new InvalidOperationException("Cannot checkout with an empty cart.");

                // 2. Prepare Order Object
                var order = new Order
                {
                    UserId = userId,
                    TotalAmount = cart.TotalAmount,
                    ShippingAddress = address,
                    OrderStatus = "Confirmed",
                    CreatedAt = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                // 3. Process Items: Check Stock + Snapshot Price + Reduce Inventory
                foreach (var cartItem in cart.CartItems)
                {
                    var product = cartItem.Product;
                    if (product == null) throw new Exception("Product details missing.");

                    if (product.StockQuantity < cartItem.Quantity)
                        throw new Exception($"Insufficient stock for {product.ProductName}.");

                    // REDUCE INVENTORY
                    product.StockQuantity -= cartItem.Quantity;

                    // ADD TO ORDER ITEMS
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = product.Price, // Snapshot price at time of purchase
                        Subtotal = cartItem.Subtotal
                    });
                }

                // 4. Save Order and Items
                await _orderRepo.AddOrderAsync(order);
                
                // 5. Clear the User's Cart
                _context.CartItems.RemoveRange(cart.CartItems);
                cart.TotalAmount = 0;
                cart.TotalItems = 0;

                await _orderRepo.SaveChangesAsync();
                
                // Commit changes to DB
                await transaction.CommitAsync();
                
                return order;
            }
            catch (Exception)
            {
                // If anything goes wrong, undo all changes
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CancelOrderAsync(int orderId, int userId)
        {
            using var transaction = await _orderRepo.BeginTransactionAsync();
            try
            {
                var order = await _orderRepo.GetOrderByIdOnlyAsync(orderId);

        // 1. Validation
        if (order == null) throw new Exception("Order not found.");
        if (order.UserId != userId) throw new Exception("You are not authorized to cancel this order.");
        
        // 2. Business Rule: Prevent cancellation of shipped items
        if (order.OrderStatus == "Shipped" || order.OrderStatus == "Delivered")
            throw new Exception($"Cannot cancel an order that is already {order.OrderStatus}.");

        if (order.OrderStatus == "Cancelled")
            throw new Exception("Order is already cancelled.");

        // 3. Restore Stock Levels
        foreach (var item in order.OrderItems)
        {
        if (item.Product != null)
        {
            item.Product.StockQuantity += item.Quantity; // Restore the items
        }
        }

        // 4. Update Status
        order.OrderStatus = "Cancelled";

        await _orderRepo.SaveChangesAsync();
        await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}