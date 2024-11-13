namespace MovieFinderAPI.Models
{
    public class RatingCompany
    {
        public string RatingCompanyName { get; set; }
        public string RatingScale { get; set; }

        // Navigation properties
        public ICollection<MovieRating> MovieRatings { get; set; }
    }
}
