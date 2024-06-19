using Microsoft.AspNetCore.Mvc;
using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System;

namespace E_Commerce_Mezzex.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllWithSubCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Policy = "CreateCategoryPolicy")]
        public async Task<IActionResult> Create()
        {
            var userClaims = User.Claims;
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            // Log or inspect the claims and roles
            Console.WriteLine("User Claims: " + string.Join(", ", userClaims.Select(c => c.Type + ":" + c.Value)));
            Console.WriteLine("User Roles: " + string.Join(", ", userRoles));

            var categories = await _categoryRepository.GetAllAsync();
            var categorySelectList = BuildCategorySelectList(categories);
            ViewBag.Categories = categorySelectList;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CreateCategoryPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;

                if (category.ParentCategoryId.HasValue)
                {
                    var parentCategory = await _categoryRepository.GetByIdAsync(category.ParentCategoryId.Value);
                    if (parentCategory == null)
                    {
                        ModelState.AddModelError("ParentCategoryId", "Invalid parent category.");
                        var categories = await _categoryRepository.GetAllAsync();
                        ViewBag.Categories = BuildCategorySelectList(categories);
                        return View("Create", category);
                    }
                    parentCategory.SubCategories.Add(category);
                    await _categoryRepository.UpdateAsync(parentCategory);
                }
                else
                {
                    await _categoryRepository.AddAsync(category);
                }
                return RedirectToAction(nameof(Index));
            }

            var categoriesList = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = BuildCategorySelectList(categoriesList);
            return View("Create", category);
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
