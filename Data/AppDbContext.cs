using Microsoft.EntityFrameworkCore;
using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Represents the "Students" table in the database
        public DbSet<Student> Students { get; set; }
    }
}
