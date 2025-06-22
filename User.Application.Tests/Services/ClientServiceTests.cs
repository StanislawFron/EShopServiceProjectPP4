using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using User.Application.Services;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;
using User.Domain.Repositories;
using Xunit;

namespace User.Application.Tests.Services
{
    public class ClientServiceTests
    {
        private async Task<DataContext> GetDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dataContext = new DataContext(options);
            await dataContext.Database.EnsureCreatedAsync();
            
            return dataContext;
        }

        [Fact]
        public async Task CreateClientAsync_WithValidData_ShouldCreateClientAndAddress()
        {
            // Arrange
            var dbContext = await GetDataContext();
            var testUser = new Domain.Models.Entities.User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
            dbContext.Users.Add(testUser);
            await dbContext.SaveChangesAsync();
            
            var clientService = new ClientService(dbContext);
            var request = new CreateClientRequest
            {
                Name = "Test Client",
                UserId = 1
            };

            // Act
            var result = await clientService.CreateClientAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Client");
            result.User.Should().NotBeNull();
            result.UserId.Should().Be(1);
            result.Address.Should().NotBeNull();

            var clientInDb = await dbContext.Clients.Include(c => c.Address).Include(c => c.User).FirstOrDefaultAsync(c => c.Id == result.Id);
            clientInDb.Should().NotBeNull();
            clientInDb.Name.Should().Be("Test Client");
            clientInDb.User.Username.Should().Be("testuser");
            clientInDb.Address.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllClientsAsync_ShouldReturnAllClients()
        {
            // Arrange
            var dbContext = await GetDataContext();
            var testUser = new Domain.Models.Entities.User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
            dbContext.Users.Add(testUser);
            var client1 = new Client { Name = "Client1", User = testUser, UserId = 1, Address = new Address(), NIP = "", PhoneNumber = "" };
            var client2 = new Client { Name = "Client2", User = testUser, UserId = 1, Address = new Address(), NIP = "", PhoneNumber = "" };
            dbContext.Clients.AddRange(client1, client2);
            await dbContext.SaveChangesAsync();

            var clientService = new ClientService(dbContext);

            // Act
            var result = await clientService.GetAllClientsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Select(c => c.Name).Should().Contain(new[] { "Client1", "Client2" });
        }
        
        [Fact]
        public async Task UpdateAddressForClientAsync_ShouldUpdateAddress()
        {
            // Arrange
            var dbContext = await GetDataContext();
            var testUser = new Domain.Models.Entities.User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
            dbContext.Users.Add(testUser);
            var client = new Client { Name = "Client1", User = testUser, UserId = 1, Address = new Address { City = "Old City" }, NIP = "", PhoneNumber = "" };
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            var clientService = new ClientService(dbContext);
            var newAddress = new Address
            {
                Street = "New Street",
                StreetNumber = "123",
                City = "New City",
                Country = "New Country"
            };

            // Act
            var result = await clientService.UpdateAddressForClientAsync(client.Id, newAddress);

            // Assert
            result.Should().NotBeNull();
            result.City.Should().Be("New City");
            result.Street.Should().Be("New Street");

            var clientInDb = await dbContext.Clients.Include(c => c.Address).FirstAsync();
            clientInDb.Address.City.Should().Be("New City");
        }

        [Fact]
        public async Task DeleteClientAsync_ShouldRemoveClientButNotAddress()
        {
            // Arrange
            var dbContext = await GetDataContext();
            var testUser = new Domain.Models.Entities.User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
            dbContext.Users.Add(testUser);
            var client = new Client { Id = 1, Name = "ClientToDelete", User = testUser, UserId = 1, Address = new Address(), NIP = "", PhoneNumber = "" };
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            var clientService = new ClientService(dbContext);

            // Act
            var result = await clientService.DeleteClientAsync(1);

            // Assert
            result.Should().BeTrue();
            (await dbContext.Clients.CountAsync()).Should().Be(0);
            (await dbContext.Addresses.CountAsync()).Should().Be(1); // Address should NOT be deleted
        }
    }
} 