using ShoppingCart.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(int cartId);
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task<CartItem> GetCartItemAsync(int cartId, int cartItemId);
        Task AddProductToCartAsync(int cartId, int productId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int cartItemId);
        Task<Cart> CreateCartAsync(int userId);
        Task<bool> DeleteCartAsync(int cartId);
    }
} 