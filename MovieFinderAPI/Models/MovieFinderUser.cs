namespace MovieFinderAPI.Models
{
    public class MovieFinderUser
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FavouriteGenre { get; set; }

        // Navigation properties
        public ICollection<WatchHistory> WatchHistories { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<ManagesUser> ManagedByAdmins { get; set; }
    }
}
