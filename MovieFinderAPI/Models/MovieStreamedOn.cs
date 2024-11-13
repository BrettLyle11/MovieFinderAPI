using System.ComponentModel.DataAnnotations.Schema;

namespace MovieFinderAPI.Models
{
    public class MovieStreamedOn
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string StreamingServiceName { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
        public StreamingService StreamingService { get; set; }
    }
}
