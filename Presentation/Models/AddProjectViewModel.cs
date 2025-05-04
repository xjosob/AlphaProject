using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Presentation.Models
{
    public class AddProjectViewModel
    {
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(0, 1000000)]
        public decimal? Budget { get; set; }

        [Required]
        public string ClientId { get; set; } = null!;

        [Required]
        public int StatusId { get; set; }

        [Required]
        public IEnumerable<Client> Clients { get; set; } = [];
    }
}
