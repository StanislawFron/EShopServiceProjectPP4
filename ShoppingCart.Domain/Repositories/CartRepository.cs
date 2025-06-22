using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Domain.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public Cart FindById(int cartId)
        {
            return _context.Carts
                .Where(c => c.Id == cartId)
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = _context.CartItems.Where(i => i.CartId == c.Id).ToList()
                })
                .FirstOrDefault();
        }

        public void Add(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public List<Cart> GetAll()
        {
            return _context.Carts.ToList();
        }
    }
} 