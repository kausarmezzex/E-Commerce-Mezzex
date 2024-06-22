using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class ProductVariationValue
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int VariationValueId { get; set; }
        public VariationValue VariationValue { get; set; }
    }
}
