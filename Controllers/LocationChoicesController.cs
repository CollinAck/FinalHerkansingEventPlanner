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
    public class LocationChoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationChoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LocationChoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocationChoices.ToListAsync());
        }

        // GET: LocationChoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationChoice = await _context.LocationChoices
                .FirstOrDefaultAsync(m => m.LocationChoiceId == id);
            if (locationChoice == null)
            {
                return NotFound();
            }

            return View(locationChoice);
        }

        // GET: LocationChoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationChoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationChoiceId,LocationChoiceName")] LocationChoice locationChoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationChoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locationChoice);
        }

        // GET: LocationChoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationChoice = await _context.LocationChoices.FindAsync(id);
            if (locationChoice == null)
            {
                return NotFound();
            }
            return View(locationChoice);
        }

        // POST: LocationChoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationChoiceId,LocationChoiceName")] LocationChoice locationChoice)
        {
            if (id != locationChoice.LocationChoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationChoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationChoiceExists(locationChoice.LocationChoiceId))
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
            return View(locationChoice);
        }

        // GET: LocationChoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationChoice = await _context.LocationChoices
                .FirstOrDefaultAsync(m => m.LocationChoiceId == id);
            if (locationChoice == null)
            {
                return NotFound();
            }

            return View(locationChoice);
        }

        // POST: LocationChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationChoice = await _context.LocationChoices.FindAsync(id);
            if (locationChoice != null)
            {
                _context.LocationChoices.Remove(locationChoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationChoiceExists(int id)
        {
            return _context.LocationChoices.Any(e => e.LocationChoiceId == id);
        }
    }
}
