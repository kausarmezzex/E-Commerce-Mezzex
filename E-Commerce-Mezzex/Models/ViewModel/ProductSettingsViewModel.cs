namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class ProductSettingsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool NotReturnable { get; set; }
        public bool DisableBuyButton { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool AllowCustomerReview { get; set; }
        public bool DisableWishlistButton { get; set; }
        public bool IsPublish { get; set; }
    }
}
