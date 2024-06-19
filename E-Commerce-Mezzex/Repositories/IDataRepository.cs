using E_Commerce_Mezzex.Models.ViewModel;

namespace E_Commerce_Mezzex.Repositories
{
    public interface IDataRepository
    {
        CategorySettingsViewModel GetCategoryById(int id);
        void UpdateCategory(CategorySettingsViewModel category);

        BrandSettingsViewModel GetBrandById(int id);
        void UpdateBrand(BrandSettingsViewModel brand);

        ProductSettingsViewModel GetProductById(int id);
        void UpdateProduct(ProductSettingsViewModel product);

        List<CategorySettingsViewModel> GetAllCategories();
        List<BrandSettingsViewModel> GetAllBrands();
        List<ProductSettingsViewModel> GetAllProducts();
    }
}
