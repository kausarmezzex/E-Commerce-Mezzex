using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class ProductSpecification
    {
        [Key]
        public string Key { get; set; } // Assuming Key is unique
        public string Value { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
    }
}
