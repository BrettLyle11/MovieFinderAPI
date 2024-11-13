namespace MovieFinderAPI.Models
{
    public class APIMovieResponseClean
    {
        public int FirstAirYear { get; set; } // Release year
        public string originalTitle { get; set; } // Title
        public List<string>? genres { get; set; } // List of genres
        public List<string>? directors { get; set; } // List of directors
        public int? duration { get; set; }
        public List<string>? cast { get; set; } // List of cast
        public string? overview { get; set; } // Description
        public List<string>? streamingServicesName { get; set; } // List of streaming services with name and image
        public string? verticalPosterW480 { get; set; } // W480 version of the vertical poster image
        public List<string>? creators { get; set; } // List of creators
        public int? rating { get; set; }

    }

}
