namespace E_Commerce_Mezzex.Models.Domain
{
    public class RelatedProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int RelatedProductId { get; set; }
        public Product Related { get; set; }
    }

}
