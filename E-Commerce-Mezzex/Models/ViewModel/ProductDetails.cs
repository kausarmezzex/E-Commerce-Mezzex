using E_Commerce_Mezzex.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace E_Commerce_Mezzex.Models.ViewModel
{
    public class ProductDetails
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public bool NotReturnable { get; set; }
        public bool DisableBuyButton { get; set; }
        public bool DisableWishlistButton { get; set; }
        public DateTime? AvailableStartDateTimeUtc { get; set; }
        public DateTime? AvailableEndDateTimeUtc { get; set; }
        public ICollection<Picture> Images { get; set; } = new List<Picture>();
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string TagNames { get; set; }
        public int DisplayOrder { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public string GetCategoryHierarchy()
        {
            var hierarchy = new List<string>();

            foreach (var category in Categories)
            {
                var currentCategory = category;
                var categoryHierarchy = new List<string>();

                while (currentCategory != null)
                {
                    categoryHierarchy.Insert(0, currentCategory.Name);
                    currentCategory = currentCategory.ParentCategory;
                }

                hierarchy.Add(string.Join(" >> ", categoryHierarchy));
            }

            return string.Join(", ", hierarchy);
        }
    }
}
