using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class VariationViewModel
    {
        public VariationValue VariationValue { get; set; }
        public VariationType VariationType { get; set; }
        public int ProductId { get; set; }  // Add this line
    }
}
