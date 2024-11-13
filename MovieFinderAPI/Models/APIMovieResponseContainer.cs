using System.Text.Json.Serialization;

namespace MovieFinderAPI.Models
{
    public class APIMovieResponseContainer
    {
        [JsonPropertyName("shows")]
        public List<DirtyAPIMovieResponse> Shows { get; set; } // List of shows/movies

        [JsonPropertyName("hasMore")]
        public bool HasMore { get; set; } // Indicates if there are more results

        [JsonPropertyName("nextCursor")]
        public string NextCursor { get; set; } // Cursor for the next page of results
    }
}