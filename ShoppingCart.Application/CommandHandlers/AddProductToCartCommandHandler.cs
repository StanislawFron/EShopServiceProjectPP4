using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {
        private readonly ICartAdder _cartAdder;

        public AddProductToCartCommandHandler(ICartAdder cartAdder)
        {
            _cartAdder = cartAdder;
        }

        public Task Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            _cartAdder.AddProductToCart(request.CartId, request.ProductId, request.Quantity);
            return Task.CompletedTask;
        }
    }
}
