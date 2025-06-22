using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using ShoppingCart.Controllers;
using ShoppingCart.Domain.Models;
using Xunit;

namespace ShoppingCart.IntegrationTests
{
    public class CartControllerTests : IClassFixture<ShoppingCartWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CartControllerTests(ShoppingCartWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Cart_CRUD_Operations_ShouldWorkCorrectly()
        {
            // 1. Create a new cart for a user
            var createCartRequest = new CreateCartRequest { UserId = 123 };
            var createResponse = await _client.PostAsJsonAsync("/api/Cart", createCartRequest);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdCart = await createResponse.Content.ReadFromJsonAsync<Cart>();
            createdCart.Should().NotBeNull();
            createdCart.UserId.Should().Be(123);
            var cartId = createdCart.Id;

            // 2. Get the created cart to verify it exists
            var getResponse = await _client.GetAsync($"/api/Cart/{cartId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var fetchedCart = await getResponse.Content.ReadFromJsonAsync<Cart>();
            fetchedCart.Should().NotBeNull();
            fetchedCart.Id.Should().Be(cartId);
            
            // 3. Add a product to the cart
            var addProductRequest = new AddProductRequest { CartId = cartId, ProductId = 101, Quantity = 2 };
            var addProductResponse = await _client.PostAsJsonAsync("/api/Cart/add-product", addProductRequest);
            addProductResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // 4. Get items from the cart to verify the product was added
            var getItemsResponse = await _client.GetAsync($"/api/Cart/{cartId}/items");
            getItemsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var items = await getItemsResponse.Content.ReadFromJsonAsync<CartItem[]>();
            items.Should().HaveCount(1);
            items[0].ProductId.Should().Be(101);
            items[0].Quantity.Should().Be(2);
            var cartItemId = items[0].Id;

            // 5. Remove the item from the cart
            var deleteItemResponse = await _client.DeleteAsync($"/api/Cart/items/{cartItemId}");
            deleteItemResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 6. Verify the item was removed
            var getItemsAfterDeleteResponse = await _client.GetAsync($"/api/Cart/{cartId}/items");
            var itemsAfterDelete = await getItemsAfterDeleteResponse.Content.ReadFromJsonAsync<CartItem[]>();
            itemsAfterDelete.Should().BeEmpty();

            // 7. Delete the entire cart
            var deleteCartResponse = await _client.DeleteAsync($"/api/Cart/{cartId}");
            deleteCartResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 8. Verify the cart was deleted
            var getDeletedCartResponse = await _client.GetAsync($"/api/Cart/{cartId}");
            getDeletedCartResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
} 