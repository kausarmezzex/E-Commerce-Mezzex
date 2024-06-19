using E_Commerce_Mezzex.Data;
using E_Commerce_Mezzex.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Mezzex.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }


        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId)
        {
            return await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
        }

        public async Task<List<Category>> GetAllWithSubCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null) // Fetch only root categories
                .ToListAsync();
        }
        public async Task<List<Category>> GetCategoriesByProductIdAsync(int productId)
        {
            return await _context.Products
                .Where(p => p.Id == productId)
                .SelectMany(p => p.Categories)
                .ToListAsync();
        }
        public async Task LoadParentCategoryAsync(Category category)
        {
            if (category.ParentCategoryId.HasValue)
            {
                category.ParentCategory = await GetByIdAsync(category.ParentCategoryId.Value);
            }
        }

    }
}
