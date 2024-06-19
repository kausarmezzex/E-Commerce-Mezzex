namespace E_Commerce_Mezzex.Models.Domain
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

        // Seo Related Field 
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        // New OrderBy property
        public int DisplayOrder { get; set; }
    }
}