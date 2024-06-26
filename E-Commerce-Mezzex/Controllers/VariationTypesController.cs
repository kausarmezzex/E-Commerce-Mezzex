using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Controllers
{
    public class VariationTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariationTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VariationTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VariationTypes.ToListAsync());
        }

        // GET: VariationTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationType = await _context.VariationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variationType == null)
            {
                return NotFound();
            }

            return View(variationType);
        }

        // GET: VariationTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VariationTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ControlType")] VariationType variationType)
        {
            if (ModelState.IsValid) 
            {
                _context.Add(variationType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Variation Type added successfully!" });
            }

            return Json(new { success = false, message = "Validation failed.", errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        // GET: VariationTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationType = await _context.VariationTypes.FindAsync(id);
            if (variationType == null)
            {
                return NotFound();
            }
            return View(variationType);
        }

        // POST: VariationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] VariationType variationType)
        {
            if (id != variationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariationTypeExists(variationType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(variationType);
        }

        // GET: VariationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationType = await _context.VariationTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variationType == null)
            {
                return NotFound();
            }

            return View(variationType);
        }

        // POST: VariationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variationType = await _context.VariationTypes.FindAsync(id);
            if (variationType != null)
            {
                _context.VariationTypes.Remove(variationType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariationTypeExists(int id)
        {
            return _context.VariationTypes.Any(e => e.Id == id);
        }

    }
}
