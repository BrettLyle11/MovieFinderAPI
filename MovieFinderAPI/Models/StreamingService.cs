using System.ComponentModel.DataAnnotations;

namespace MovieFinderAPI.Models
{
    public class StreamingService
    {
        [Key]
        public string StreamingServiceName { get; set; }

        public ICollection<MovieStreamedOn> MovieStreamedOns { get; set; }
    }
}
