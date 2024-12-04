namespace MovieFinderAPI.Models
{
    public class AddMovieToPlaylist
    {
        public int UserId { get; set; }
        public string playlistName { get; set; }
        public int movieYear { get; set; }
        public string movieName { get; set; }
    }
}
