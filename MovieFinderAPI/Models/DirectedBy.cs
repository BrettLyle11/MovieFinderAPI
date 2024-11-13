using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFinderAPI.Models
{
    public class DirectedBy
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string DirectorName { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
        public Director Director { get; set; }
    }
}
