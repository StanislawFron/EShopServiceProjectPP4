using EShop.Domain.Repositories;
using EShopDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly DataContext _context;

        public ProductCategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
            => await _context.Categories.ToListAsync();

        public async Task<ProductCategory?> GetByIdAsync(int id)
            => await _context.Categories.FindAsync(id);

        public async Task<ProductCategory> CreateAsync(ProductCategory category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateAsync(ProductCategory category)
        {
            var existing = await _context.Categories.FindAsync(category.Id);
            if (existing == null) return false;

            existing.Name = category.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
