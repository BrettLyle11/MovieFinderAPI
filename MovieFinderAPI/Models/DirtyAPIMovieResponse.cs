using System.Text.Json.Serialization;

namespace MovieFinderAPI.Models
{
    public class DirtyAPIMovieResponse
    {
        [JsonPropertyName("itemType")]
        public string? ItemType { get; set; }

        [JsonPropertyName("showType")]
        public string? ShowType { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("imdbId")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("tmdbId")]
        public string? TmdbId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("overview")]
        public string? Overview { get; set; }

        [JsonPropertyName("releaseYear")]
        public int? ReleaseYear { get; set; }

        [JsonPropertyName("firstAirYear")]
        public int? FirstAirYear { get; set; }

        [JsonPropertyName("lastAirYear")]
        public int? LastAirYear { get; set; }

        [JsonPropertyName("originalTitle")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("genres")]
        public List<GenreIncluded>? Genres { get; set; }

        [JsonPropertyName("directors")]
        public List<string>? Directors { get; set; }

        [JsonPropertyName("creators")]
        public List<string>? Creators { get; set; }

        [JsonPropertyName("cast")]
        public List<string>? Cast { get; set; }

        [JsonPropertyName("rating")]
        public int? Rating { get; set; }

        [JsonPropertyName("seasonCount")]
        public int? SeasonCount { get; set; }

        [JsonPropertyName("episodeCount")]
        public int? EpisodeCount { get; set; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [JsonPropertyName("imageSet")]
        public ImageSet? ImageSet { get; set; }

        [JsonPropertyName("streamingOptions")]
        public Dictionary<string, List<StreamingOption>>? StreamingOptions { get; set; }
        [JsonPropertyName("seasons")]
        public List<Seasons>? Seasons { get; set; }
    }

    public class GenreIncluded
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class ImageSet
    {
        [JsonPropertyName("verticalPoster")]
        public VerticalPoster? VerticalPoster { get; set; }

        [JsonPropertyName("horizontalPoster")]
        public HorizontalPoster? HorizontalPoster { get; set; }

        [JsonPropertyName("verticalBackdrop")]
        public VerticalBackdrop? VerticalBackdrop { get; set; }

        [JsonPropertyName("horizontalBackdrop")]
        public HorizontalBackdrop? HorizontalBackdrop { get; set; }
    }

    public class VerticalPoster
    {
        [JsonPropertyName("w240")]
        public string? W240 { get; set; }

        [JsonPropertyName("w360")]
        public string? W360 { get; set; }

        [JsonPropertyName("w480")]
        public string? W480 { get; set; }

        [JsonPropertyName("w600")]
        public string? W600 { get; set; }

        [JsonPropertyName("w720")]
        public string? W720 { get; set; }
    }

    public class HorizontalPoster
    {
        [JsonPropertyName("w360")]
        public string? W360 { get; set; }

        [JsonPropertyName("w480")]
        public string? W480 { get; set; }

        [JsonPropertyName("w720")]
        public string? W720 { get; set; }

        [JsonPropertyName("w1080")]
        public string? W1080 { get; set; }

        [JsonPropertyName("w1440")]
        public string? W1440 { get; set; }
    }

    public class VerticalBackdrop
    {
        [JsonPropertyName("w240")]
        public string? W240 { get; set; }

        [JsonPropertyName("w360")]
        public string? W360 { get; set; }

        [JsonPropertyName("w480")]
        public string? W480 { get; set; }

        [JsonPropertyName("w600")]
        public string? W600 { get; set; }

        [JsonPropertyName("w720")]
        public string? W720 { get; set; }
    }

    public class HorizontalBackdrop
    {
        [JsonPropertyName("w360")]
        public string? W360 { get; set; }

        [JsonPropertyName("w480")]
        public string? W480 { get; set; }

        [JsonPropertyName("w720")]
        public string? W720 { get; set; }

        [JsonPropertyName("w1080")]
        public string? W1080 { get; set; }

        [JsonPropertyName("w1440")]
        public string? W1440 { get; set; }
    }

    public class StreamingOption
    {
        [JsonPropertyName("service")]
        public Service Service { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("addon")]
        public Addon Addon { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("videoLink")]
        public string? VideoLink { get; set; }

        [JsonPropertyName("quality")]
        public string? Quality { get; set; }

        [JsonPropertyName("audios")]
        public List<Locale> Audios { get; set; }

        [JsonPropertyName("subtitles")]
        public List<Subtitle> Subtitles { get; set; }

        [JsonPropertyName("price")]
        public Price Price { get; set; }

        [JsonPropertyName("expiresSoon")]
        public bool ExpiresSoon { get; set; }

        [JsonPropertyName("expiresOn")]
        public object ExpiresOn { get; set; }

        [JsonPropertyName("availableSince")]
        public int AvailableSince { get; set; }
    }

    public class Seasons
    {
        [JsonPropertyName("itemType")]
        public string? ItemType { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("firstAirYear")]
        public int FirstAirYear { get; set; }

        [JsonPropertyName("lastAirYear")]
        public int LastAirYear { get; set; }

        [JsonPropertyName("streamingOptions")]
        public Dictionary<string, List<StreamingOption>> StreamingOptions { get; set; }

        [JsonPropertyName("episodes")]
        public List<Episode> Episodes { get; set; }
    }

    public class Episode
    {
        [JsonPropertyName("itemType")]
        public string? ItemType { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("airYear")]
        public int AirYear { get; set; }

        [JsonPropertyName("streamingOptions")]
        public Dictionary<string, List<StreamingOption>> StreamingOptions { get; set; }
    }

    public class Service
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("homePage")]
        public string HomePage { get; set; }

        [JsonPropertyName("themeColorCode")]
        public string ThemeColorCode { get; set; }

        [JsonPropertyName("imageSet")]
        public ServiceImageSet ImageSet { get; set; }
    }

    public class ServiceImageSet
    {
        [JsonPropertyName("lightThemeImage")]
        public string LightThemeImage { get; set; }

        [JsonPropertyName("darkThemeImage")]
        public string DarkThemeImage { get; set; }

        [JsonPropertyName("whiteImage")]
        public string WhiteImage { get; set; }
    }

    public class Addon
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("homePage")]
        public string HomePage { get; set; }

        [JsonPropertyName("themeColorCode")]
        public string ThemeColorCode { get; set; }

        [JsonPropertyName("imageSet")]
        public ServiceImageSet ImageSet { get; set; }
    }

    public class Locale
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }
    }

    public class Subtitle
    {
        [JsonPropertyName("closedCaptions")]
        public bool ClosedCaptions { get; set; }

        [JsonPropertyName("locale")]
        public Locale Locale { get; set; }
    }

    public class Price
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("formatted")]
        public string Formatted { get; set; }
    }
}
