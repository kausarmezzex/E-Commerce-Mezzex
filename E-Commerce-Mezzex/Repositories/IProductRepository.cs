using System.Collections.Generic;
using System.Threading.Tasks;
using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId);
        Task AddAsync(Product product);
    }
}
