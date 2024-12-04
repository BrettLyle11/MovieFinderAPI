namespace MovieFinderAPI.Models
{
    public class UpdatePlaylistTime
    {
        public int UserId { get; set; }
        public string playlistName { get; set; }
        public int duration { get; set; }
    }
}
