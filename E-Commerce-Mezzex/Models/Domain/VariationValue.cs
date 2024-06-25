using E_Commerce_Mezzex.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class VariationValue
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        [Required]
        public int VariationTypeId { get; set; }
        public VariationType VariationType { get; set; } // Navigation property

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public ICollection<Picture> Images { get; set; } = new List<Picture>();

        public ICollection<ProductVariationValue> ProductVariationValues { get; set; } = new List<ProductVariationValue>();
    }
}
