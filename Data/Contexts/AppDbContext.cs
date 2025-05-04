using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<UserEntity>(options)
    {
        public virtual DbSet<ClientEntity> Clients { get; set; }
        public virtual DbSet<StatusEntity> Statuses { get; set; }
        public virtual DbSet<ProjectEntity> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<StatusEntity>()
                .HasData(
                    new StatusEntity { Id = 1, StatusName = "Started" },
                    new StatusEntity { Id = 2, StatusName = "Completed" }
                );

            builder.Entity<ProjectEntity>(b =>
            {
                b.Property(p => p.EndDate).HasColumnType("date").IsRequired(false);
            });
        }
    }
}
