using System;
using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public List<int> CategoryId { get; set; } = new List<int>(); // Multiple Category IDs
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public List<int>? RelatedProductId { get; set; } = new List<int>();
        public int BrandId { get; set; }
        public ICollection<Brand> Brands { get; set; } = new List<Brand>();
        public bool NotReturnable { get; set; }
        public bool DisableBuyButton { get; set; }
        public bool IsPublish { get; set; } = true; // Default to checked
        public bool ShowOnHomePage { get; set; }
        public bool AllowCustomerReview { get; set; }
        public bool DisableWishlistButton { get; set; }
        public DateTime? AvailableStartDateTimeUtc { get; set; }
        public DateTime? AvailableEndDateTimeUtc { get; set; }
        public ICollection<Picture> Images { get; set; } = new List<Picture>();
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string TagNames { get; set; }
        public int DisplayOrder { get; set; }

        // New properties
        public ICollection<CustomerReview>? CustomerReviews { get; set; } = new List<CustomerReview>();
        public ProductSpecification? Specifications { get; set; }
        public ICollection<RelatedProduct>? RelatedProducts { get; set; } = new List<RelatedProduct>();
        public ICollection<QuestionAnswer>? QuestionsAnswers { get; set; } = new List<QuestionAnswer>();

        // New collection for Variation Types
        public ICollection<VariationType> VariationTypes { get; set; } = new List<VariationType>();
        public ICollection<VariationValue> VariationValues { get; set; } = new List<VariationValue>();
        public ICollection<ProductVariationValue> ProductVariationValues { get; set; } = new List<ProductVariationValue>();
    }
}