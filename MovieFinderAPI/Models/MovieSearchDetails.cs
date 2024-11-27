namespace MovieFinderAPI.Models
{
    public class MovieSearchDetails
    {
        public string? Title { get; set; }                     // Title of the movie
        public int? Year { get; set; }                         // Release year of the movie
        public int? DurationMins { get; set; }                 // Duration in minutes
        public string? Description { get; set; }               // Description of the movie
        public string? Image { get; set; }                     // URL to the movie's image/poster
        public string? Actors { get; set; }                    // Comma-separated actor names
        public string? Directors { get; set; }                 // Comma-separated director names
        public string? StreamingServices { get; set; }         // Comma-separated streaming service names
        public string? Genres { get; set; }                    // Comma-separated genres
        public string? RatingsAndScores { get; set; }          // Comma-separated 'RatingCompany:Score' pairs

        public string? ProductionCompanies { get; set; }
    }
}
