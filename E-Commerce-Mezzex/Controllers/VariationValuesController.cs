﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce_Mezzex.Models.Domain;

namespace E_Commerce_Mezzex.Controllers
{
    public class VariationValuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariationValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VariationValues
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VariationValues.Include(v => v.VariationType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VariationValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationValue = await _context.VariationValues
                .Include(v => v.VariationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variationValue == null)
            {
                return NotFound();
            }

            return View(variationValue);
        }

        // GET: VariationValues/Create
        public IActionResult Create()
        {
            ViewData["VariationTypeId"] = new SelectList(_context.VariationTypes, "Id", "Name");
            return View();
        }

        // POST: VariationValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value,VariationTypeId, ProductId")] VariationValue variationValue)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(variationValue);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Variation Value added successfully!" });
            }

            return Json(new { success = false, message = "Validation failed.", errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        // GET: VariationValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationValue = await _context.VariationValues.FindAsync(id);
            if (variationValue == null)
            {
                return NotFound();
            }
            ViewData["VariationTypeId"] = new SelectList(_context.VariationTypes, "Id", "Name", variationValue.VariationTypeId);
            return View(variationValue);
        }

        // POST: VariationValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Value,VariationTypeId")] VariationValue variationValue)
        {
            if (id != variationValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variationValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariationValueExists(variationValue.Id))
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
            ViewData["VariationTypeId"] = new SelectList(_context.VariationTypes, "Id", "Name", variationValue.VariationTypeId);
            return View(variationValue);
        }

        // GET: VariationValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationValue = await _context.VariationValues
                .Include(v => v.VariationType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variationValue == null)
            {
                return NotFound();
            }

            return View(variationValue);
        }

        // POST: VariationValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variationValue = await _context.VariationValues.FindAsync(id);
            if (variationValue != null)
            {
                _context.VariationValues.Remove(variationValue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariationValueExists(int id)
        {
            return _context.VariationValues.Any(e => e.Id == id);
        }
    }
}
