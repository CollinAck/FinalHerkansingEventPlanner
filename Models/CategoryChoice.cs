using System.ComponentModel.DataAnnotations;

namespace FinalHerkansingEventPlanner.Models
{
    public class CategoryChoice
    {
        [Key]
        public int CategoriesChoiceId { get; set; }
        [Required]
        public string CategoryChoiceName { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
