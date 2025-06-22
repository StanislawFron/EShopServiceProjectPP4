using EShop.Domain.Repositories;
using EShopDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Seeders
{
    public class EShopSeeder(DataContext context) : IEShopSeeder
    {
        public async Task Seed()
        {
            if (!context.Categories.Any())
            {
                var categories = new List<ProductCategory>
                {
                    new ProductCategory { Name = "Laptops", CreatedBy = 1, UpdatedBy = 1 },
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                var category = await context.Categories
                        .Where(x => x.Name == "Laptops").FirstOrDefaultAsync();

                var products = new List<Product>
                {
                    new Product { Name = "Thinkpad", Price = 10300.12M, Category = category, CreatedBy = 1, UpdatedBy = 1 },
                    new Product { Name = "Dell", Price = 8000.00M, Category = category, CreatedBy = 1, UpdatedBy = 1 },
                    new Product { Name = "Acer", Price = 4500.99M, Category = category, CreatedBy = 1, UpdatedBy = 1 }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}