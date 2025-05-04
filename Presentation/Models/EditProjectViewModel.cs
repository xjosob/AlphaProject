using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Presentation.Models
{
    public class EditProjectViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Range(0, 1000000)]
        public decimal? Budget { get; set; }

        [Required]
        [Display(Name = "Client")]
        public string ClientId { get; set; } = null!;

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<Client> Clients { get; set; } = [];
        public IEnumerable<Status> Statuses { get; set; } = [];
    }
}
