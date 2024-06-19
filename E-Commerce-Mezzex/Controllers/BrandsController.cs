using Microsoft.AspNetCore.Mvc;
using E_Commerce_Mezzex.Models.Domain;
using E_Commerce_Mezzex.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_Mezzex.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    public class BrandsController : Controller
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAllAsync();
            return View(brands);
        }

        public async Task<IActionResult> Details(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [Authorize(Policy = "CreateBrandPolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CreateBrandPolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.CreatedOnUtc = DateTime.UtcNow;
                brand.UpdatedOnUtc = DateTime.UtcNow;
                await _brandRepository.AddAsync(brand);
                return RedirectToAction(nameof(Index));
            }
            return View("Create", brand);
        }
    }
}
