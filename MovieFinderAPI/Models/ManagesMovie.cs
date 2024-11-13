namespace MovieFinderAPI.Models
{
    public class ManagesMovie
    {
        public int AdminID { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // Navigation properties
        public Admin Admin { get; set; }
        public Movie Movie { get; set; }
    }
}
