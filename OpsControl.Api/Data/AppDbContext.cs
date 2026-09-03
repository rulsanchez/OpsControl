using Microsoft.EntityFrameworkCore;
using OpsControl.Api.Models;

namespace OpsControl.Api.Data
{
    public class AppDbContext:DbContext
    {
        /// <summary>
        /// Constructor:
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Incident> Incidents { get; set; }
    }
}
