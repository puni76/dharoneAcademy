using DharoneAcademy.Data;
using DharoneAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Controllers;

[Authorize(Roles = "Admin")]
public class StudentsController : Controller
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["Title"] = "Students";
        ViewData["Search"] = search;

        var query = _db.StudentProfiles.Include(s => s.User).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.EnrollmentId.Contains(search) ||
                s.CourseStream.Contains(search) ||
                (s.User != null && (s.User.FullName.Contains(search) || s.User.Email.Contains(search))));
        }

        return View(await query.OrderByDescending(s => s.Id).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Student Details";
        var student = await _db.StudentProfiles
            .Include(s => s.User)
            .Include(s => s.DailyTasks)
            .FirstOrDefaultAsync(s => s.Id == id);

        return student is null ? NotFound() : View(student);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Edit Student";
        var student = await _db.StudentProfiles.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentProfile model)
    {
        ViewData["Title"] = "Edit Student";
        if (id != model.Id) return NotFound();

        var student = await _db.StudentProfiles.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        if (student is null) return NotFound();

        student.CourseStream = model.CourseStream;
        student.CollegeName = model.CollegeName;
        student.BatchName = model.BatchName;
        student.IsActive = model.IsActive;
        if (student.User != null)
        {
            student.User.FullName = model.User?.FullName ?? student.User.FullName;
            student.User.Phone = model.User?.Phone ?? student.User.Phone;
            student.User.IsActive = model.IsActive;
        }

        await _db.SaveChangesAsync();
        TempData["Success"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Delete Student";
        var student = await _db.StudentProfiles.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _db.StudentProfiles.FirstOrDefaultAsync(s => s.Id == id);
        if (student is null) return NotFound();

        var user = await _db.Users.FindAsync(student.UserId);
        if (user != null) _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Student removed.";
        return RedirectToAction(nameof(Index));
    }
}
