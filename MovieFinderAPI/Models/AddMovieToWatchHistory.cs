namespace MovieFinderAPI.Models
{
    public class AddMovieToWatchHistory
    {
        public int UserId { get; set; }
        public int movieYear { get; set; }
        public string movieName { get; set; }
        public bool didFinish { get; set; }
    }
}
