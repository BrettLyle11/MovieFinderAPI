using System.ComponentModel.DataAnnotations;
namespace MovieFinderAPI.Models

{
    public class Actor
    {
        [Key]
        public string ActorName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

        // Navigation properties
        public ICollection<ActedIn> ActedIns { get; set; }
    }
}
