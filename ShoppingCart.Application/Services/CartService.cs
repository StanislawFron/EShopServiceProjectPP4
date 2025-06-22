using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.Services
{
    public class CartService : ICartAdder, ICartRemover, ICartReader
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public void AddProductToCart(int cartId, int productId)
        {
            var cart = _repository.FindById(cartId);
            if (cart == null)
            {
                cart = new Cart { Id = cartId, UserId = Guid.NewGuid() }; // Tymczasowo, UserId powinien przychodzić z tokenu JWT
                _repository.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }
            _repository.Update(cart);
        }

        public void RemoveProductFromCart(int cartId, int productId)
        {
            var cart = _repository.FindById(cartId);
            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    cart.Items.Remove(cartItem);
                    _repository.Update(cart);
                }
            }
        }

        public Cart GetCart(int cartId)
        {
            return _repository.FindById(cartId);
        }

        public List<Cart> GetAllCarts()
        {
            return _repository.GetAll();
        }
    }
}
