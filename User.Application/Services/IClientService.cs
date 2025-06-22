using User.Domain.Models.Entities;
using User.Domain.Models.Requests;

namespace User.Application.Services
{
    public interface IClientService
    {
        Task<Client> CreateClientAsync(CreateClientRequest request);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Address?> GetAddressForClientAsync(int clientId);
        Task<Address> UpdateAddressForClientAsync(int clientId, Address address);
        Task<bool> DeleteClientAsync(int id);
        Task<bool> DeleteAddressForClientAsync(int clientId);
    }
} 