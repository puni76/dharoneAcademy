using DharoneAcademy.Models;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Users.AnyAsync())
            return;

        var admin = new AppUser
        {
            FullName = "Academy Admin",
            Email = "admin@dharoneacademy.com",
            Phone = "8217415668",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        var studentUser = new AppUser
        {
            FullName = "Aanya Sharma",
            Email = "student@dharoneacademy.com",
            Phone = "9876543210",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = "Student",
            CreatedAt = DateTime.UtcNow
        };

        db.Users.AddRange(admin, studentUser);
        await db.SaveChangesAsync();

        var profile = new StudentProfile
        {
            UserId = studentUser.Id,
            EnrollmentId = "DA-2026-001",
            CourseStream = "BCA",
            CollegeName = "Sample College, Bengaluru",
            BatchName = "DA-2026-A",
            JoinDate = DateTime.Today.AddDays(-14)
        };
        db.StudentProfiles.Add(profile);
        await db.SaveChangesAsync();

        db.DailyTasks.AddRange(
            new DailyTask
            {
                StudentProfileId = profile.Id,
                TaskDate = DateTime.Today,
                Attendance = "Present",
                TopicCovered = "ASP.NET Core MVC Controllers",
                TaskTitle = "Build Login Page",
                Status = "Completed",
                HoursSpent = 2.5m,
                MentorFeedback = "Clean structure. Great progress.",
                StudentNotes = "Completed auth form and validation."
            },
            new DailyTask
            {
                StudentProfileId = profile.Id,
                TaskDate = DateTime.Today,
                Attendance = "Present",
                TopicCovered = "SQL Server CRUD",
                TaskTitle = "Student Daily Tracker CRUD",
                Status = "In Progress",
                HoursSpent = 1.5m,
                StudentNotes = "Working on create/edit screens."
            }
        );

        db.ContactEnquiries.Add(new ContactEnquiry
        {
            FullName = "Interested Student",
            Email = "hello@example.com",
            Phone = "9999999999",
            CourseInterest = "3-Month Internship",
            Message = "Please share batch start dates.",
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}
