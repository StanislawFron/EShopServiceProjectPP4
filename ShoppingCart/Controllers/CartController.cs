using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Services;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    // DTOs for requests
    public class AddProductRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveProductRequest
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateCartRequest
    {
        public int UserId { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request)
        {
            if (request.UserId <= 0)
            {
                return BadRequest("UserId is required and must be greater than 0.");
            }
            var cart = await _cartService.CreateCartAsync(request.UserId);
            return CreatedAtAction(nameof(GetCart), new { cartId = cart.Id }, cart);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var result = await _cartService.DeleteCartAsync(cartId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartItemId)
        {
            var result = await _cartService.RemoveItemFromCartAsync(cartItemId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductRequest request)
        {
            if (request.CartId <= 0 || request.ProductId <= 0 || request.Quantity <= 0)
                return BadRequest("cartId, productId and quantity are required and must be greater than 0.");
            
            await _cartService.AddProductToCartAsync(request.CartId, request.ProductId, request.Quantity);
            return Ok();
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int cartId)
        {
            var result = await _cartService.GetCartAsync(cartId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("{cartId}/items")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            var result = await _cartService.GetCartItemsAsync(cartId);
            return Ok(result);
        }

        [HttpGet("{cartId}/items/{cartItemId}")]
        public async Task<IActionResult> GetCartItem(int cartId, int cartItemId)
        {
            var result = await _cartService.GetCartItemAsync(cartId, cartItemId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            var result = await _cartService.GetAllCartsAsync();
            return Ok(result);
        }
    }
}
