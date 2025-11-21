using Microsoft.EntityFrameworkCore;
using PROGPOE3.Models;

namespace PROGPOE3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        // DbSets for the entities
        public DbSet<Claim> Claims { get; set; }
        // DbSet for ClaimStatus
        public DbSet<ClaimStatus> ClaimStatuses { get; set; }
        // DbSet for Lecturer
        public DbSet<Lecturer> Lecturers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed ClaimStatuses
            modelBuilder.Entity<ClaimStatus>().HasData(
                new ClaimStatus { Id = 1, StatusName = "Pending" },
                new ClaimStatus { Id = 4, StatusName = "Approved" },
                new ClaimStatus { Id = 5, StatusName = "Rejected" }
            );

            // Decimal precision
            modelBuilder.Entity<Claim>().Property(c => c.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Claim>().Property(c => c.HoursWorked).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Claim>().Property(c => c.HourlyRate).HasColumnType("decimal(18,2)");

            // Default value for DateSubmitted
            modelBuilder.Entity<Claim>().Property(c => c.DateSubmitted).HasDefaultValueSql("GETDATE()");
        }
    }
}
