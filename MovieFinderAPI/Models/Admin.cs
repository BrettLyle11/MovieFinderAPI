namespace MovieFinderAPI.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        // Navigation properties
        public ICollection<ManagesUser> ManagesUsers { get; set; }
        public ICollection<ManagesMovie> ManagesMovies { get; set; }
    }
}
