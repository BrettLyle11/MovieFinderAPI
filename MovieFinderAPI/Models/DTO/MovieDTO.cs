namespace MovieFinderAPI.Models.DTO
{
    public class MovieDTO
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public int DurationMins { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Actors { get; set; } // Comma-separated string
        public string Directors { get; set; } // Comma-separated string
        public string StreamingServices { get; set; } // Comma-separated string
        public string Genres { get; set; } // Comma-separated string
        public string RatingsAndScores { get; set; } // Comma-separated 'Company:Score'
        public string ProductionCompanies { get; set; } // Comma-separated string
    }
}
