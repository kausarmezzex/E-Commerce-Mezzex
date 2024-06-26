﻿using E_Commerce_Mezzex.Models.Domain;
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
            this._context = context;
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
            if (!ModelState.IsValid)
            {
                HandleTags(product);
                await _productRepository.AddAsync(product);

                await AssignCategoriesToProduct(product);
                await AssignRelatedProductsToProduct(product);

                await _productRepository.UpdateAsync(product);

                // Set productId in ViewBag
                TempData["ProductId"] = product.Id;

                return Json(new { success = true, productId = product.Id });
            }

            await PopulateViewData();
            return PartialView("Create", product);
        }


        private async Task AssignRelatedProductsToProduct(Product product)
        {
            if (product.RelatedProductId != null)
            {
                product.RelatedProducts = new List<RelatedProduct>();
                foreach (var relatedProductId in product.RelatedProductId)
                {
                    var relatedProduct = await _productRepository.GetByIdAsync(relatedProductId);
                    if (relatedProduct != null)
                    {
                        product.RelatedProducts.Add(new RelatedProduct
                        {
                            MainProductId = product.Id,
                            RelatedProductId = relatedProductId,
                            RelatedProductName = relatedProduct.Name,
                            RelatedIsPublish = relatedProduct.IsPublish,
                            RelatedProductPrice = relatedProduct.Price
                        });
                    }
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

            var products = await _productRepository.GetAllAsync();

            var relatedProducts = products.Select(product => new
            {
                Value = product.Id,
                Text = product.Name,
                Published = product.IsPublish
            }).ToList();

            ViewBag.RelatedProducts = relatedProducts;
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