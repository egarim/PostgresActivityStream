using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ultra.ActivityStream
{
    public class ActivityStreamDbContext : DbContext
    {
        private readonly string _connectionString;

        public ActivityStreamDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations if needed
        }
    }
}