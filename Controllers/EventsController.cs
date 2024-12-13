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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(string? category)
        {
            ViewData["Categories"] = _context.CategoryChoices
            .Select(c => c.CategoryChoiceName)
            .ToList();

            var events = _context.Events
                .Include(e => e.CategoryChoice)
                .Include(e => e.LocationChoice)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                events = events.Where(e => e.CategoryChoice.CategoryChoiceName == category);
            }

            events = events.OrderBy(e => e.DateTime);

            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.CategoryChoice)
                .Include(e => e.LocationChoice)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CategoryChoiceId"] = new SelectList(_context.CategoryChoices, "CategoriesChoiceId", "CategoryChoiceName");
            ViewData["LocationChoiceId"] = new SelectList(_context.LocationChoices, "LocationChoiceId", "LocationChoiceName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Description,DateTime,Cost,MaxParticipants,AvailableSpots,ImageFileName,LocationChoiceId,CategoryChoiceId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryChoiceId"] = new SelectList(_context.CategoryChoices, "CategoriesChoiceId", "CategoryChoiceName", @event.CategoryChoiceId);
            ViewData["LocationChoiceId"] = new SelectList(_context.LocationChoices, "LocationChoiceId", "LocationChoiceName", @event.LocationChoiceId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CategoryChoiceId"] = new SelectList(_context.CategoryChoices, "CategoriesChoiceId", "CategoryChoiceName", @event.CategoryChoiceId);
            ViewData["LocationChoiceId"] = new SelectList(_context.LocationChoices, "LocationChoiceId", "LocationChoiceName", @event.LocationChoiceId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Description,DateTime,Cost,MaxParticipants,AvailableSpots,ImageFileName,LocationChoiceId,CategoryChoiceId")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewData["CategoryChoiceId"] = new SelectList(_context.CategoryChoices, "CategoriesChoiceId", "CategoryChoiceName", @event.CategoryChoiceId);
            ViewData["LocationChoiceId"] = new SelectList(_context.LocationChoices, "LocationChoiceId", "LocationChoiceName", @event.LocationChoiceId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.CategoryChoice)
                .Include(e => e.LocationChoice)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveTicket(int eventId, int participantId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var participant = await _context.Participants.FindAsync(participantId);

            var ticket = new Ticket
            {
                EventId = eventId,
                ParticipantId = participantId,
                IsPaid = false
            };

            @event.AvailableSpots--;

            _context.Tickets.Add(ticket);
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ticket succesvol gereserveerd!";
            return RedirectToAction(nameof(Index));
        }
    }
}
