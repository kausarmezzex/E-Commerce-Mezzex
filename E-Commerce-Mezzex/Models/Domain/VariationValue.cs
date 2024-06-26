using E_Commerce_Mezzex.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // New attributes
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAdjustment { get; set; }

        [Required]
        public bool UsePercentage { get; set; }
    }
}
