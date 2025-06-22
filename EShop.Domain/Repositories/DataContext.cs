using EShopDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
