using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;
using User.Domain.Repositories;

namespace User.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly DataContext _context;

        public ClientService(DataContext context)
        {
            _context = context;
        }

        public async Task<Client> CreateClientAsync(CreateClientRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

            var address = new Address();
            var client = new Client
            {
                Name = request.Name,
                User = user,
                Address = address,
                NIP = string.Empty,
                PhoneNumber = string.Empty
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.Include(c => c.Address).Include(c => c.User).ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _context.Clients.Include(c => c.Address).Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Address?> GetAddressForClientAsync(int clientId)
        {
            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == clientId);
            return client?.Address;
        }

        public async Task<Address> UpdateAddressForClientAsync(int clientId, Address address)
        {
            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new KeyNotFoundException($"Client with ID {clientId} not found.");

            if (client.Address == null)
                client.Address = new Address();

            client.Address.Street = address.Street;
            client.Address.StreetNumber = address.StreetNumber;
            client.Address.City = address.City;
            client.Address.Country = address.Country;

            await _context.SaveChangesAsync();
            return client.Address;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAddressForClientAsync(int clientId)
        {
            var client = await _context.Clients.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == clientId);
            if (client?.Address == null)
            {
                return false;
            }

            _context.Addresses.Remove(client.Address);
            client.Address = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 