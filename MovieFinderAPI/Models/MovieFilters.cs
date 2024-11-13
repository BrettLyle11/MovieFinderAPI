namespace MovieFinderAPI.Models
{
    public class MovieFilters
    {
        public int? DurationMins { get; set; }               // Duration in minutes (optional)
        public int? MovieYear { get; set; }                  // Release year (optional)
        public string? MovieTitle { get; set; }              // Title or partial title (optional)
        public string? ActorNames { get; set; }              // Comma-separated actor names (optional)
        public string? DirectorNames { get; set; }           // Comma-separated director names (optional)
        public string? StreamingPlatforms { get; set; }      // Comma-separated streaming platform names (optional)
        public string? RatingCompanies { get; set; }         // Comma-separated 'RatingCompany:MinScore' pairs (optional)
        public string? Genres { get; set; }                  // Comma-separated genres (optional)
    }
}
