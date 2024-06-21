namespace E_Commerce_Mezzex.Models.Domain
{
    public class RelatedProduct
    {
        public int Id { get; set; } // Unique identifier for this relationship
        public int MainProductId { get; set; } // ID of the main product
        public int RelatedProductId { get; set; } // ID of the related product
        public string RelatedProductName { get; set; }
        public bool RelatedIsPublish {  get; set; }
        public decimal RelatedProductPrice { get; set; }

        // Navigation properties
        public Product MainProduct { get; set; } // Navigation property to the main product
        public Product RelatedProductDetails { get; set; } // Navigation property to the related product
    }

}
