using System.Diagnostics;
using DharoneAcademy.Models;
using Microsoft.AspNetCore.Mvc;

namespace DharoneAcademy.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _config;

    public HomeController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        return View();
    }

    public IActionResult Program()
    {
        ViewData["Title"] = "Internship Program";
        return View();
    }

    public IActionResult Curriculum()
    {
        ViewData["Title"] = "Curriculum";
        return View();
    }

    public IActionResult Career()
    {
        ViewData["Title"] = "Career Building";
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About Us";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
