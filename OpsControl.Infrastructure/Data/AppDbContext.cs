using Microsoft.EntityFrameworkCore;
using OpsControl.Domain.Entities;

namespace OpsControl.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor:
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Incident> Incidents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incident>()
                .Property(incident => incident.Status)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }
}
