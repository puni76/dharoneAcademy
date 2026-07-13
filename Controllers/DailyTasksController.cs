using System.Security.Claims;
using DharoneAcademy.Data;
using DharoneAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Controllers;

[Authorize]
public class DailyTasksController : Controller
{
    private readonly AppDbContext _db;

    public DailyTasksController(AppDbContext db) => _db = db;

    private bool IsAdmin => User.IsInRole("Admin");

    public async Task<IActionResult> Index(DateTime? date)
    {
        ViewData["Title"] = "Daily Tasks";
        ViewData["FilterDate"] = date?.ToString("yyyy-MM-dd");

        var query = _db.DailyTasks
            .Include(t => t.StudentProfile)!
            .ThenInclude(s => s!.User)
            .AsQueryable();

        if (!IsAdmin)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _db.StudentProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (profile == null) return View(new List<DailyTask>());
            query = query.Where(t => t.StudentProfileId == profile.Id);
        }

        if (date.HasValue)
            query = query.Where(t => t.TaskDate == date.Value.Date);

        return View(await query.OrderByDescending(t => t.TaskDate).ThenByDescending(t => t.Id).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Add Daily Task";
        await LoadStudentsAsync();
        return View(new DailyTask { TaskDate = DateTime.Today });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DailyTask model)
    {
        ViewData["Title"] = "Add Daily Task";

        if (!IsAdmin)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _db.StudentProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (profile == null) return Forbid();
            model.StudentProfileId = profile.Id;
            ModelState.Remove(nameof(model.StudentProfileId));
        }

        if (!ModelState.IsValid)
        {
            await LoadStudentsAsync(model.StudentProfileId);
            return View(model);
        }

        model.CreatedAt = DateTime.UtcNow;
        _db.DailyTasks.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Daily task saved.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Daily Task";
        var task = await _db.DailyTasks.FindAsync(id);
        if (task is null) return NotFound();
        if (!await CanAccessAsync(task)) return Forbid();

        await LoadStudentsAsync(task.StudentProfileId);
        return View(task);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DailyTask model)
    {
        ViewData["Title"] = "Edit Daily Task";
        if (id != model.Id) return NotFound();

        var existing = await _db.DailyTasks.FindAsync(id);
        if (existing is null) return NotFound();
        if (!await CanAccessAsync(existing)) return Forbid();

        if (!IsAdmin)
            model.StudentProfileId = existing.StudentProfileId;

        if (!ModelState.IsValid)
        {
            await LoadStudentsAsync(model.StudentProfileId);
            return View(model);
        }

        existing.StudentProfileId = model.StudentProfileId;
        existing.TaskDate = model.TaskDate;
        existing.Attendance = model.Attendance;
        existing.TopicCovered = model.TopicCovered;
        existing.TaskTitle = model.TaskTitle;
        existing.Status = model.Status;
        existing.HoursSpent = model.HoursSpent;
        existing.MentorFeedback = IsAdmin ? model.MentorFeedback : existing.MentorFeedback;
        existing.StudentNotes = model.StudentNotes;

        await _db.SaveChangesAsync();
        TempData["Success"] = "Daily task updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Delete Daily Task";
        var task = await _db.DailyTasks
            .Include(t => t.StudentProfile)!
            .ThenInclude(s => s!.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task is null) return NotFound();
        if (!await CanAccessAsync(task)) return Forbid();
        return View(task);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _db.DailyTasks.FindAsync(id);
        if (task is null) return NotFound();
        if (!await CanAccessAsync(task)) return Forbid();

        _db.DailyTasks.Remove(task);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Daily task deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CanAccessAsync(DailyTask task)
    {
        if (IsAdmin) return true;
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var profile = await _db.StudentProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
        return profile != null && task.StudentProfileId == profile.Id;
    }

    private async Task LoadStudentsAsync(int? selected = null)
    {
        var students = await _db.StudentProfiles
            .Include(s => s.User)
            .Where(s => s.IsActive)
            .OrderBy(s => s.User!.FullName)
            .Select(s => new { s.Id, Name = s.User!.FullName + " (" + s.EnrollmentId + ")" })
            .ToListAsync();

        ViewBag.StudentProfileId = new SelectList(students, "Id", "Name", selected);
    }
}
