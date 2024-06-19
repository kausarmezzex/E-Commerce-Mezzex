namespace E_Commerce_Mezzex.Models.Domain
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public ICollection<Picture> Images { get; set; } = new List<Picture>();
    }
}
