using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFinderAPI.Models
{

    public class ActedIn
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string ActorName { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}
