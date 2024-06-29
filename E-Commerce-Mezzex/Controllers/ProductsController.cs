using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using E_Commerce_Mezzex.Models.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Mezzex.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ApplicationDbContext _context;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _context = context;
        }

        [Authorize(Policy = "CreateProductPolicy")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.VariationTypes = new SelectList(_context.VariationTypes, "Id", "Name");
            await PopulateViewData();
            return View(new Product());
        }

        [Authorize(Policy = "CreateProductPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                HandleTags(product);
                await _productRepository.AddAsync(product);

                await AssignCategoriesToProduct(product);
                await AssignProductRelationshipsToProduct(product);

                // Save paired product details
                if (product.PairedProductId.HasValue)
                {
                    var pairedProduct = await _productRepository.GetByIdAsync(product.PairedProductId.Value);
                    if (pairedProduct != null)
                    {
                        product.PairedProduct = pairedProduct;
                    }
                }

                await _productRepository.UpdateAsync(product);

                // Set productId in TempData
                TempData["ProductId"] = product.Id;

                return Json(new { success = true, productId = product.Id });
            }

            await PopulateViewData();
            return PartialView("Create", product);
        }

        private async Task AssignProductRelationshipsToProduct(Product product)
        {
            var relatedProductIds = Request.Form["RelatedProductId"].ToString().Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
            var crossSellProductIds = Request.Form["CrossSellProductIds"].ToString().Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
            var upSellProductIds = Request.Form["UpSellProductIds"].ToString().Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();

            product.ProductRelationships = new List<ProductRelationship>();

            var allProductIds = relatedProductIds.Concat(crossSellProductIds).Concat(upSellProductIds).Distinct().ToList();

            foreach (var productId in allProductIds)
            {
                var relatedProduct = await _productRepository.GetByIdAsync(productId);
                if (relatedProduct != null)
                {
                    var relationship = new ProductRelationship
                    {
                        MainProductId = product.Id,
                        RelatedProductId = productId,
                        RelatedProductName = relatedProduct.Name,
                        RelatedIsPublish = relatedProduct.IsPublish,
                        RelatedProductPrice = relatedProduct.Price,
                        MainProduct = product,
                        RelatedProductDetails = relatedProduct,
                        IsRelatedProduct = relatedProductIds.Contains(productId),
                        IsCrossSellProduct = crossSellProductIds.Contains(productId),
                        IsUpSellProduct = upSellProductIds.Contains(productId)
                    };

                    product.ProductRelationships.Add(relationship);
                }
            }
        }



        [HttpGet]
        [Authorize(Policy = "CreateProductPolicy")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _categoryRepository.GetSubCategoriesAsync(categoryId);
            return Json(subCategories);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetCategoriesByProductIdAsync(id);
            var brand = await _brandRepository.GetByIdAsync(product.BrandId);

            var productDetails = new ProductDetails()
            {
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                Price = product.Price,
                Brand = brand?.Name,
                NotReturnable = product.NotReturnable,
                DisableBuyButton = product.DisableBuyButton,
                DisableWishlistButton = product.DisableWishlistButton,
                AvailableStartDateTimeUtc = product.AvailableStartDateTimeUtc,
                AvailableEndDateTimeUtc = product.AvailableEndDateTimeUtc,
                Images = product.Images,
                MetaKeywords = product.MetaKeywords,
                MetaDescription = product.MetaDescription,
                MetaTitle = product.MetaTitle,
                TagNames = product.TagNames,
                DisplayOrder = product.DisplayOrder,
                Categories = categories
            };

            return View(productDetails);
        }

        private void HandleTags(Product product)
        {
            var tagNames = Request.Form["TagNames"].ToString()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim())
                .Where(tag => !string.IsNullOrWhiteSpace(tag));
            product.TagNames = string.Join(",", tagNames);
        }

        private async Task AssignCategoriesToProduct(Product product)
        {
            product.Categories.Clear();
            foreach (var categoryId in product.CategoryId)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    product.Categories.Add(category);
                }
            }
        }

        private async Task PopulateViewData()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = BuildCategorySelectList(categories);
            ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");

            // Ensure products include their categories
            var products = await _productRepository.GetAllAsync();

            // Populate ProductViewModel list
            var productList = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                SKU  = product.SKU,
                Price = product.Price
            }).ToList();

            ViewBag.Products = productList;

            var relatedProducts = products.Select(product => new
            {
                Value = product.Id,
                Text = product.Name,
                Published = product.IsPublish
            }).ToList();

            ViewBag.RelatedProducts = relatedProducts;
            ViewBag.CrossSellProducts = relatedProducts;
            ViewBag.UpsellProducts = relatedProducts;
        }




        private List<SelectListItem> BuildCategorySelectList(IEnumerable<Category> categories, int? parentId = null, string prefix = "")
        {
            var categorySelectList = new List<SelectListItem>();

            var filteredCategories = categories.Where(c => c.ParentCategoryId == parentId).ToList();
            foreach (var category in filteredCategories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = $"{prefix}{category.Name}"
                });

                var subcategories = BuildCategorySelectList(categories, category.Id, $"{prefix}{category.Name} >> ");
                categorySelectList.AddRange(subcategories);
            }

            return categorySelectList;
        }
    }
}
