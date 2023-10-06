using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reactivities.Domain;

namespace Reactivities.Persistence
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ILogger<DataContext> _logger;
        private readonly IConfiguration _config;

        public DataContext(DbContextOptions options, ILogger<DataContext> logger, IConfiguration config) : base(options)
        {
            _logger = logger;
            _config = config;
        }

        public DbSet<Activity> Activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnections"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}