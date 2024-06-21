namespace E_Commerce_Mezzex.Models.Domain
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string VariantName { get; set; }  // e.g., "Color"
        public string VariantValue { get; set; } // e.g., "Red", "Blue"
        public decimal? Price { get; set; }
        public ICollection<Picture> Images { get; set; } = new List<Picture>();
        public Product Product { get; set; }
    }
}
