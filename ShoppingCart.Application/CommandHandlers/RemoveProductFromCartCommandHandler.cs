using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
    {
        private readonly ICartRemover _cartRemover;

        public RemoveProductFromCartCommandHandler(ICartRemover cartRemover)
        {
            _cartRemover = cartRemover;
        }

        public Task Handle(RemoveProductFromCartCommand request, CancellationToken cancellationToken)
        {
            _cartRemover.RemoveProductFromCart(request.CartId, request.ProductId);
            return Task.CompletedTask;
        }
    }
}
