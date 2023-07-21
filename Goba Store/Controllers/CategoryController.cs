using AutoMapper;
using Goba_Store.Application.Services.Interfaces;
using Goba_Store.Application.View_Models;
using Goba_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goba_Store.Controllers;

[Authorize(Roles = Constants.AdminRole)]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    // GET
    public IActionResult Index()
    {
        return View(_categoryService.GetAllCategories());
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
            _categoryService.CreateCategory(viewModel);
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }


    [HttpPost]
    public IActionResult Edit(CategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            _categoryService.UpdateCategory(viewModel);
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Delete(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _categoryService.DeleteCategory(id);
        return RedirectToAction(nameof(Index));
    }
    
}