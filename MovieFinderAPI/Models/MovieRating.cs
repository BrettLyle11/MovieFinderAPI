namespace MovieFinderAPI.Models
{
    public class MovieRating
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string RatingCompanyName { get; set; }
        public string Score { get; set; }
        public string Summary { get; set; }

        // Navigation properties
        public Movie Movie { get; set; }
        public RatingCompany RatingCompany { get; set; }
    }
}
