using Domain.Models;

namespace Presentation.Models
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = null!;
        public IEnumerable<Client> Clients { get; set; } = null!;
    }
}
