﻿using FinalHerkansingEventPlanner.Models;

namespace FinalHerkansingEventPlanner.ViewModels
{
    public class TicketsPaidAndUnpaidViewModel
    {
        public List<Ticket> PaidTickets { get; set; }
        public List<Ticket> UnpaidTickets { get; set; }
    }
}
