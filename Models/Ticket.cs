﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalHerkansingEventPlanner.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        [Required]
        [ForeignKey("Participant")] // Verwijst naar deelnemer klasse, om zo een koppeling te leggen.
        public int ParticipantId { get; set; }

        [Required] // Waar of onwaar == "bestelnummer"
        public bool IsPaid { get; set; }

        public Event Event { get; set; } // Een ticket voor een specefiek evenement, om relatie te leggen
        public Participant Participant { get; set; }

        //DIT IS EEN VOORBEELD VOOR EEN PUSH :)
    }
}
