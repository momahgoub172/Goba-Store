using System.Diagnostics;
using Goba_Store.Data;
using Microsoft.AspNetCore.Mvc;
using Goba_Store.Models;
using Goba_Store.View_Models;
using Microsoft.EntityFrameworkCore;
using Goba_Store.Services;

namespace Goba_Store.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        HomeViewModel model = new HomeViewModel()
        {
            Products = _db.Products.Include(p => p.Category),
            Categories = _db.Categories
        };
        return View(model);
    }
   
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}