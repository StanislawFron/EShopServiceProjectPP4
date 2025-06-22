using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Tests
{
    public class CartServiceTests
    {
        private DataContext _context;
        private CartService _sut;

        private DbContextOptions<DataContext> _options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        public CartServiceTests()
        {
            _context = new DataContext(_options);
            _sut = new CartService(_context);
        }

        [Fact]
        public async Task CreateCartAsync_ShouldCreateCart_WhenUserHasNoCart()
        {
            // Arrange
            var userId = 1;

            // Act
            var createdCart = await _sut.CreateCartAsync(userId);

            // Assert
            Assert.NotNull(createdCart);
            Assert.Equal(userId, createdCart.UserId);
            var cartInDb = await _context.Carts.FindAsync(createdCart.Id);
            Assert.NotNull(cartInDb);
        }

        [Fact]
        public async Task CreateCartAsync_ShouldThrowException_WhenCartForUserExists()
        {
            // Arrange
            var userId = 1;
            await _sut.CreateCartAsync(userId);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateCartAsync(userId));
        }
        
        [Fact]
        public async Task AddProductToCartAsync_ShouldAddNewItem_WhenItemNotInCart()
        {
            // Arrange
            var cart = await _sut.CreateCartAsync(1);
            var productId = 101;
            var quantity = 2;

            // Act
            await _sut.AddProductToCartAsync(cart.Id, productId, quantity);

            // Assert
            var cartInDb = await _context.Carts.Include(c => c.Items).FirstAsync(c => c.Id == cart.Id);
            var newItem = cartInDb.Items.FirstOrDefault();
            Assert.Single(cartInDb.Items);
            Assert.Equal(productId, newItem.ProductId);
            Assert.Equal(quantity, newItem.Quantity);
        }
        
        [Fact]
        public async Task AddProductToCartAsync_ShouldIncreaseQuantity_WhenItemAlreadyInCart()
        {
            // Arrange
            var cart = await _sut.CreateCartAsync(1);
            var productId = 101;
            await _sut.AddProductToCartAsync(cart.Id, productId, 2);

            // Act
            await _sut.AddProductToCartAsync(cart.Id, productId, 3);
            
            // Assert
            var cartInDb = await _context.Carts.Include(c => c.Items).FirstAsync(c => c.Id == cart.Id);
            var item = cartInDb.Items.First();
            Assert.Single(cartInDb.Items);
            Assert.Equal(5, item.Quantity);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var cart = await _sut.CreateCartAsync(1);
            await _sut.AddProductToCartAsync(cart.Id, 101, 1);
            var itemToRemove = (await _context.CartItems.FirstAsync());

            // Act
            var result = await _sut.RemoveItemFromCartAsync(itemToRemove.Id);

            // Assert
            Assert.True(result);
            var itemsInDb = await _context.CartItems.CountAsync();
            Assert.Equal(0, itemsInDb);
        }
        
        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            // Arrange
            var nonExistentItemId = 999;

            // Act
            var result = await _sut.RemoveItemFromCartAsync(nonExistentItemId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetCartItemsAsync_ShouldReturnOnlyItemsForGivenCart()
        {
            // Arrange
            var cart1 = await _sut.CreateCartAsync(1);
            var cart2 = await _sut.CreateCartAsync(2);
            await _sut.AddProductToCartAsync(cart1.Id, 101, 1);
            await _sut.AddProductToCartAsync(cart1.Id, 102, 2);
            await _sut.AddProductToCartAsync(cart2.Id, 201, 3);
            
            // Act
            var cart1Items = (await _sut.GetCartItemsAsync(cart1.Id)).ToList();

            // Assert
            Assert.Equal(2, cart1Items.Count);
            Assert.Contains(cart1Items, i => i.ProductId == 101);
            Assert.Contains(cart1Items, i => i.ProductId == 102);
        }
    }
} 