using EShopDomain.Models;

namespace EShop.Application.Service
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> UpdateAsync(Product product);
        Task<Product> AddAsync(Product product);
        
        Task<Product> DeleteAsync(int id);
        Product Add(Product product);
    }
}
