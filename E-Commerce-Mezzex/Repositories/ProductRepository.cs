using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_Mezzex.Data;
using E_Commerce_Mezzex.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Mezzex.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId)
        {
            return await _dbSet.Where(p => p.BrandId == brandId).ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Images).ToListAsync();
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            foreach (var categoryId in product.CategoryId)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    product.Categories.Add(category);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
