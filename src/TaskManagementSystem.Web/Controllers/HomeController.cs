using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Tasks");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
