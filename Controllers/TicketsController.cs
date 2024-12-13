using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalHerkansingEventPlanner.Data;
using FinalHerkansingEventPlanner.ViewModels;

namespace FinalHerkansingEventPlanner.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ManageTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .Where(t => !t.IsPaid)
                .ToListAsync();

            return View(tickets);
        }

        public async Task<IActionResult> TicketsPaidAndUnpaid()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .ToListAsync();

            var viewModel = new TicketsPaidAndUnpaidViewModel
            {
                PaidTickets = tickets.Where(t => t.IsPaid).ToList(),
                UnpaidTickets = tickets.Where(t => !t.IsPaid).ToList()
            };

            return View(viewModel);
        }


        public async Task<IActionResult> MarkAsPaid(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

            ticket.IsPaid = true;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageTickets));
        }


    }
}
