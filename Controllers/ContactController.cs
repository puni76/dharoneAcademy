using DharoneAcademy.Data;
using DharoneAcademy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DharoneAcademy.Controllers;

public class ContactController : Controller
{
    private readonly AppDbContext _db;

    public ContactController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Contact";
        return View(new ContactEnquiry());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactEnquiry model)
    {
        ViewData["Title"] = "Contact";
        if (!ModelState.IsValid)
            return View(model);

        model.CreatedAt = DateTime.UtcNow;
        _db.ContactEnquiries.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Thank you! Our team will contact you shortly.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Enquiries()
    {
        ViewData["Title"] = "Enquiries";
        var list = await _db.ContactEnquiries.OrderByDescending(e => e.CreatedAt).ToListAsync();
        return View(list);
    }

    [Authorize(Roles = "Admin"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Resolve(int id)
    {
        var item = await _db.ContactEnquiries.FindAsync(id);
        if (item is null) return NotFound();
        item.IsResolved = true;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Enquiry marked as resolved.";
        return RedirectToAction(nameof(Enquiries));
    }
}
