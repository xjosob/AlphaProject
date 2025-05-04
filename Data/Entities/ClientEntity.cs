using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Index(nameof(ClientName), IsUnique = true)]
    public class ClientEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ClientName { get; set; } = null!;
        public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
    }
}
