namespace MovieFinderAPI.Models
{
    public class ProducedBy
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string ProductionCompanyName { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
        public ProductionCompany ProductionCompany { get; set; }
    }
}
