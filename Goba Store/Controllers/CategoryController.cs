using Goba_Store.Data;
using Goba_Store.Models;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace Goba_Store.Controllers;

public class CategoryController : Controller
{
    private readonly AppDbContext _db;

    public CategoryController(AppDbContext Db)
    {
        _db = Db;
    }
    // GET
    public IActionResult Index()
    {
        var Categories = _db.Categories;
        return View(Categories);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoryViewModel viewModel )
    {
        if (ModelState.IsValid)
        {
            //Map viewModel To Category
            var Cat = new Category
            {
                Name = viewModel.Name,
                DisplayOrder = viewModel.DisplayOrder
            };
            _db.Categories.Add(Cat);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var Cat = _db.Categories.Find(id);
        if (Cat == null)
            return NotFound();
        return View(Cat);
    }
    [HttpPost]
    public IActionResult Edit(Category Cat)
    {
        if (ModelState.IsValid)
        {
            _db.Categories.Update(Cat);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(Cat);
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var Cat = _db.Categories.Find(id);
        if (Cat == null)
            return NotFound();
        return View(Cat);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _db.Categories.Remove(_db.Categories.Find(id));
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    
}