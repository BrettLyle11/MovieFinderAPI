namespace MovieFinderAPI.Models
{
    public class ManagesUser
    {
        public int AdminID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        public Admin Admin { get; set; }
        public MovieFinderUser MovieFinderUser { get; set; }
    }
}
