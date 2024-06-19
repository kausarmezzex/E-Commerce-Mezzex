using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using E_Commerce_Mezzex.Models.ViewModel;

namespace E_Commerce_Mezzex.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        // GET: Create product view
        [Authorize(Policy = "CreateProductPolicy")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categorySelectList = BuildCategorySelectList(categories);
            ViewBag.Categories = categorySelectList;
            ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View(new Product());
        }

        // Generate category select list with hierarchy
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

        // POST: Add product to the database
        [Authorize(Policy = "CreateProductPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                // Handle tags
                var tagNames = Request.Form["TagNames"].ToString()
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(tag => tag.Trim())
                    .Where(tag => !string.IsNullOrWhiteSpace(tag));
                product.TagNames = string.Join(",", tagNames);

                // Add product to the database
                await _productRepository.AddAsync(product);

                // Assign selected categories to the product
                product.Categories.Clear();
                foreach (var categoryId in product.CategoryId)
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }

                // Update product to include categories
                await _productRepository.UpdateAsync(product);

                return Json(new { success = true, productId = product.Id });
            }

            // Reload the view if model state is invalid
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.Brands = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return PartialView("Create", product);
        }

      

        // GET: Get subcategories based on category id
        [HttpGet]
        [Authorize(Policy = "CreateProductPolicy")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _categoryRepository.GetSubCategoriesAsync(categoryId);
            return Json(subCategories);
        }

        // GET: Display list of products
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // GET: Display details of a product
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
                Brand = brand.Name,
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
    }
}
