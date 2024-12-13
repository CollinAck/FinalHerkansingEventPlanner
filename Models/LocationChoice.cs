using System.ComponentModel.DataAnnotations;

namespace FinalHerkansingEventPlanner.Models
{
    public class LocationChoice
    {
        [Key]
        public int LocationChoiceId { get; set; }
        [Required]
        [Display(Name = "Locatie")]

        public string LocationChoiceName { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
