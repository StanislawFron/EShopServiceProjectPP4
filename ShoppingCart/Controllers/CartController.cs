using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Queries;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Repositories;

namespace ShoppingCart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DataContext _context;

        public CartController(IMediator mediator, DataContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartCommand command)
        {
            if (command.CartId <= 0 || command.ProductId <= 0 || command.Quantity <= 0)
                return BadRequest("cart_id, product_id i quantity są wymagane i muszą być większe od 0");
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("remove-product")]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductFromCartCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int cartId)
        {
            var query = new GetCartQuery { CartId = cartId };
            var result = await _mediator.Send(query);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            var query = new GetAllCartsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
