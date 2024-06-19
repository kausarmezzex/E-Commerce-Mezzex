using E_Commerce_Mezzex.Models.ViewModel;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    public class SettingsController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;

        public SettingsController(ICategoryRepository categoryRepository, IBrandRepository brandRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var brands = await _brandRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            var model = new SettingsViewModel
            {
                Categories = categories.Select(c => new CategorySettingsViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShowOnHomePage = c.ShowOnHomePage,
                    Published = c.Published,
                    Deleted = c.Deleted,
                    IncludeInTopMenu = c.IncludInTopMenu
                }).ToList(),

                Brands = brands.Select(b => new BrandSettingsViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    ShowOnHomePage = b.ShowOnHomePage,
                    Published = b.Published,
                    Deleted = b.Deleted
                }).ToList(),

                Products = products.Select(p => new ProductSettingsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    NotReturnable = p.NotReturnable,
                    DisableBuyButton = p.DisableBuyButton,
                    ShowOnHomePage = p.ShowOnHomePage,
                    AllowCustomerReview = p.AllowCustomerReview,
                    DisableWishlistButton = p.DisableWishlistButton,
                    IsPublish = p.IsPublish
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategorySettingsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ShowOnHomePage = category.ShowOnHomePage,
                Published = category.Published,
                Deleted = category.Deleted,
                IncludeInTopMenu = category.IncludInTopMenu
            };

            return PartialView("_EditCategoryPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBrand(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            var model = new BrandSettingsViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                ShowOnHomePage = brand.ShowOnHomePage,
                Published = brand.Published,
                Deleted = brand.Deleted
            };

            return PartialView("_EditBrandPartial", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductSettingsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                NotReturnable = product.NotReturnable,
                DisableBuyButton = product.DisableBuyButton,
                ShowOnHomePage = product.ShowOnHomePage,
                AllowCustomerReview = product.AllowCustomerReview,
                DisableWishlistButton = product.DisableWishlistButton,
                IsPublish = product.IsPublish
            };

            return PartialView("_EditProductPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCategory([FromBody] CategorySettingsViewModel category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    existingCategory.ShowOnHomePage = category.ShowOnHomePage;
                    existingCategory.Published = category.Published;
                    existingCategory.Deleted = category.Deleted;
                    existingCategory.IncludInTopMenu = category.IncludeInTopMenu;

                    await _categoryRepository.UpdateAsync(existingCategory);
                    return Ok();
                }
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> SaveBrand([FromBody] BrandSettingsViewModel brand)
        {
            if (ModelState.IsValid)
            {
                var existingBrand = await _brandRepository.GetByIdAsync(brand.Id);
                if (existingBrand != null)
                {
                    existingBrand.Name = brand.Name;
                    existingBrand.ShowOnHomePage = brand.ShowOnHomePage;
                    existingBrand.Published = brand.Published;
                    existingBrand.Deleted = brand.Deleted;

                    await _brandRepository.UpdateAsync(existingBrand);
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct([FromBody] ProductSettingsViewModel product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.NotReturnable = product.NotReturnable;
                    existingProduct.DisableBuyButton = product.DisableBuyButton;
                    existingProduct.ShowOnHomePage = product.ShowOnHomePage;
                    existingProduct.AllowCustomerReview = product.AllowCustomerReview;
                    existingProduct.DisableWishlistButton = product.DisableWishlistButton;
                    existingProduct.IsPublish = product.IsPublish;
                    await _productRepository.UpdateAsync(existingProduct);
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}