namespace MovieFinderAPI.Models
{
    public class UpdatePlaylistNameRequest
    {
        public int UserId { get; set; }
        public string CurrentPlaylistName { get; set; }
        public string NewPlaylistName { get; set; }
    }
}
