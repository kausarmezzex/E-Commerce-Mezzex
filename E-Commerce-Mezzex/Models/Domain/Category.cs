using System;
using System.Collections.Generic;

namespace E_Commerce_Mezzex.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string ImageUrl { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public bool IncludInTopMenu { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }

        // SEO-related fields
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        // New OrderBy property
        public int DisplayOrder { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
