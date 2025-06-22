using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
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

        public Cart FindById(int id)
        {
            return _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.Id == id);
        }

        public List<Cart> GetAll()
        {
            return _context.Carts.Include(c => c.Items).ToList();
        }
    }
} 