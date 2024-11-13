using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFinderAPI.Models
{
    public class Director
    {
        [Key]
        public string DirectorName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

        // Navigation properties
        public ICollection<DirectedBy> DirectedBys { get; set; }
    }
}
