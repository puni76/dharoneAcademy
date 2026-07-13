using System.Security.Claims;
using DharoneAcademy.Data;
using DharoneAcademy.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Dashboard";
        var today = DateTime.Today;
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "Student";
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var tasksQuery = _db.DailyTasks
            .Include(t => t.StudentProfile)!
            .ThenInclude(s => s!.User)
            .AsQueryable();

        if (role != "Admin")
        {
            var profile = await _db.StudentProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (profile != null)
                tasksQuery = tasksQuery.Where(t => t.StudentProfileId == profile.Id);
            else
                tasksQuery = tasksQuery.Where(_ => false);
        }

        var todayTasks = await tasksQuery.Where(t => t.TaskDate == today).ToListAsync();

        var model = new DashboardViewModel
        {
            UserName = User.Identity?.Name ?? "Learner",
            Role = role,
            TotalStudents = await _db.StudentProfiles.CountAsync(s => s.IsActive),
            PresentToday = todayTasks.Count(t => t.Attendance == "Present"),
            TasksPending = await tasksQuery.CountAsync(t => t.Status == "Pending" || t.Status == "In Progress"),
            TasksCompleted = await tasksQuery.CountAsync(t => t.Status == "Completed"),
            OpenEnquiries = role == "Admin" ? await _db.ContactEnquiries.CountAsync(e => !e.IsResolved) : 0,
            RecentTasks = await tasksQuery
                .OrderByDescending(t => t.TaskDate)
                .ThenByDescending(t => t.Id)
                .Take(8)
                .ToListAsync()
        };

        return View(model);
    }
}
