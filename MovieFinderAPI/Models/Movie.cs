using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MovieFinderAPI.Models
{
    public class Movie
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public int? DurationMins { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        // Navigation properties
        public ICollection<MovieStreamedOn> MovieStreamedOns { get; set; }
        public ICollection<GenreToMovie> Genres{ get; set; }
        public ICollection<MovieRating> MovieRatings { get; set; }
        public ICollection<DirectedBy> DirectedBys { get; set; }
        public ICollection<ProducedBy> ProducedBys { get; set; }
        public ICollection<ActedIn> ActedIns { get; set; }
        public ICollection<WatchHistory> WatchHistories { get; set; }
        public ICollection<ManagesMovie> ManagedByAdmins { get; set; }
    }
}
