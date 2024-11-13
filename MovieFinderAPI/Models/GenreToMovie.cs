using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFinderAPI.Models
{
    public class GenreToMovie
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
    }
}
