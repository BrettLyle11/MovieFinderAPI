namespace MovieFinderAPI.Models
{
    public class Playlist
    {
        public int UserID { get; set; }
        public string PlaylistName { get; set; }
        public int? WatchTime { get; set; }

        // Navigation properties
        public MovieFinderUser MovieFinderUser { get; set; }
    }
}
