namespace MovieFinderAPI.Models.DTO
{
    public class MovieUpdateDTO
    {
        public string OriginalTitle { get; set; }
        public int OriginalYear { get; set; }
        public Dictionary<string, object> UpdatedFields { get; set; } = new Dictionary<string, object>();
        public Changes Changes { get; set; } = new Changes();
        public string AdminUsername { get; set; }
        public int AdminId { get; set; }
    }

    public class Changes
    {
        public Dictionary<string, List<string>> Added { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Removed { get; set; } = new Dictionary<string, List<string>>();
    }
}
