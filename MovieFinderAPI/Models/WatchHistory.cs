namespace MovieFinderAPI.Models
{
    public class WatchHistory
    {
        public int UserID { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public bool DidFinish { get; set; }

        // Navigation properties
        public MovieFinderUser MovieFinderUser { get; set; }
        public Movie Movie { get; set; }
    }
}
