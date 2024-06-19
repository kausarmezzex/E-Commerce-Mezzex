using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId);
        Task<List<Category>> GetAllWithSubCategoriesAsync();
        Task<List<Category>> GetCategoriesByProductIdAsync(int productId);
        Task LoadParentCategoryAsync(Category category);
    }
}
