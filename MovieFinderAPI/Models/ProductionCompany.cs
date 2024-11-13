using System.ComponentModel.DataAnnotations;

namespace MovieFinderAPI.Models
{
    public class ProductionCompany
    {
        public string ProductionCompanyName { get; set; }

        // Navigation properties
        public ICollection<ProducedBy> ProducedBys { get; set; }
    }
}
