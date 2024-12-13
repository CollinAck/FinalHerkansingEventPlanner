using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace FinalHerkansingEventPlanner.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
