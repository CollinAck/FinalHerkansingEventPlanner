﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalHerkansingEventPlanner.Data;
using FinalHerkansingEventPlanner.Models;
using FinalHerkansingEventPlanner.ViewModels;

namespace FinalHerkansingEventPlanner.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TicketsPerEvent(int participantId)
        {
            // Standaard op 1, zodat dummy data overeenkomt
            // Zie de Db + events index
            participantId = 1;

            var participant = await _context.Participants
                .Include(p => p.Tickets)
                .ThenInclude(t => t.Event)
                .FirstOrDefaultAsync(p => p.ParticipantId == participantId);

            var viewModel = participant.Tickets
                .GroupBy(t => t.Event)
                // g. staat voor group, oftewel verwijst naar 'GroupBy' van bovenstaande
                .OrderBy(g => g.Key.DateTime)
                .Select(g => new ParticipantTicketsViewModel
                {
                    EventName = g.Key.Name,
                    EventDate = g.Key.DateTime,
                    Tickets = g.ToList()
                })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

            var eventToUpdate = ticket.Event;
            if (eventToUpdate != null)
            {
                eventToUpdate.AvailableSpots++;
                _context.Events.Update(eventToUpdate);
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            TempData["CancelMessage"] = "Je reservering is succesvol geannuleerd.";
            return RedirectToAction(nameof(TicketsPerEvent), new { participantId = ticket.ParticipantId });
        }

    }
}
