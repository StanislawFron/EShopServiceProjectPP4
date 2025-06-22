using ShoppingCart.Domain.Models;
using System.Collections.Generic;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartRepository
    {
        Cart FindById(int cartId);
        void Add(Cart cart);
        void Update(Cart cart);
        List<Cart> GetAll();
    }
} 