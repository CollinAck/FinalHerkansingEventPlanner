using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalHerkansingEventPlanner.Data;
using FinalHerkansingEventPlanner.Models;

namespace FinalHerkansingEventPlanner.Controllers
{
    public class CategoryChoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryChoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryChoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoryChoices.ToListAsync());
        }

        // GET: CategoryChoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryChoice = await _context.CategoryChoices
                .FirstOrDefaultAsync(m => m.CategoriesChoiceId == id);
            if (categoryChoice == null)
            {
                return NotFound();
            }

            return View(categoryChoice);
        }

        // GET: CategoryChoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryChoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriesChoiceId,CategoryChoiceName")] CategoryChoice categoryChoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryChoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryChoice);
        }

        // GET: CategoryChoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryChoice = await _context.CategoryChoices.FindAsync(id);
            if (categoryChoice == null)
            {
                return NotFound();
            }
            return View(categoryChoice);
        }

        // POST: CategoryChoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriesChoiceId,CategoryChoiceName")] CategoryChoice categoryChoice)
        {
            if (id != categoryChoice.CategoriesChoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryChoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryChoiceExists(categoryChoice.CategoriesChoiceId))
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
            return View(categoryChoice);
        }

        // GET: CategoryChoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryChoice = await _context.CategoryChoices
                .FirstOrDefaultAsync(m => m.CategoriesChoiceId == id);
            if (categoryChoice == null)
            {
                return NotFound();
            }

            return View(categoryChoice);
        }

        // POST: CategoryChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryChoice = await _context.CategoryChoices.FindAsync(id);
            if (categoryChoice != null)
            {
                _context.CategoryChoices.Remove(categoryChoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryChoiceExists(int id)
        {
            return _context.CategoryChoices.Any(e => e.CategoriesChoiceId == id);
        }
    }
}
