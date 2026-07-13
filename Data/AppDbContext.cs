using DharoneAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<DailyTask> DailyTasks => Set<DailyTask>();
    public DbSet<ContactEnquiry> ContactEnquiries => Set<ContactEnquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<StudentProfile>()
            .HasIndex(s => s.EnrollmentId)
            .IsUnique();

        modelBuilder.Entity<StudentProfile>()
            .HasOne(s => s.User)
            .WithOne(u => u.StudentProfile)
            .HasForeignKey<StudentProfile>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DailyTask>()
            .HasOne(d => d.StudentProfile)
            .WithMany(s => s.DailyTasks)
            .HasForeignKey(d => d.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DailyTask>()
            .Property(d => d.HoursSpent)
            .HasPrecision(5, 2);
    }
}
