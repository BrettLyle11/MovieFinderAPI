using MovieFinderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class Mapper
{
    public static APIMovieResponseClean ConvertJsonToAPIMovieResponse(string json)
    {
        try
        {
            // Step 1: Deserialize JSON into ApiResponse
            var apiResponse = JsonSerializer.Deserialize<DirtyAPIMovieResponse>(json);

            // Step 2: Map to APIMovieResponse using the MapToAPIMovieResponse function
            return MapToAPIMovieResponse(apiResponse);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return null;
        }
    }

    public static APIMovieResponseClean MapToAPIMovieResponse(DirtyAPIMovieResponse dirtyResponse)
    {
        if (dirtyResponse == null)
        {
            return null;
        }

        return new APIMovieResponseClean
        {
            FirstAirYear = dirtyResponse.ReleaseYear ?? dirtyResponse.FirstAirYear ?? 0,
            originalTitle = dirtyResponse.OriginalTitle ?? dirtyResponse.Title,
            genres = dirtyResponse.Genres?.Select(g => g.Name).ToList(),
            directors = dirtyResponse.Directors ?? new List<string>(),
            duration = dirtyResponse.Runtime ?? 0,
            cast = dirtyResponse.Cast ?? new List<string>(),
            overview = dirtyResponse.Overview,
            streamingServicesName = dirtyResponse.StreamingOptions
                ?.Where(option => option.Key == "ca") // Filter for Canada
                .SelectMany(static option => option.Value.Select(static so => so.Service?.Name))
                .Distinct()
                .ToList(),
            verticalPosterW480 = dirtyResponse.ImageSet?.VerticalPoster?.W480,
            rating = dirtyResponse.Rating ?? null,
            creators = dirtyResponse.Creators ?? new List<string>()
        };
    }
}
