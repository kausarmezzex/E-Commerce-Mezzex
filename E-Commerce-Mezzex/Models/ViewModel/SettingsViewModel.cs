namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class SettingsViewModel
    {
        public List<CategorySettingsViewModel> Categories { get; set; } = new List<CategorySettingsViewModel>();
        public List<BrandSettingsViewModel> Brands { get; set; } = new List<BrandSettingsViewModel>();
        public List<ProductSettingsViewModel> Products { get; set; } = new List<ProductSettingsViewModel>();
    }
}
