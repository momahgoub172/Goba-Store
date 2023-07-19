using AutoMapper;
using Goba_Store.DataAccess;
using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goba_Store.Controllers;

[Authorize(Roles = Constants.AdminRole)]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _catRepo;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository catRepo, IMapper mapper)
    {
        _catRepo = catRepo;
        _mapper = mapper;
    }
    // GET
    public IActionResult Index()
    {

        return View(_catRepo.GetAll());
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
            //var Cat = new Category
            //{
            //    Name = viewModel.Name,
            //    DisplayOrder = viewModel.DisplayOrder
            //};
            var Cat = _mapper.Map<Category>(viewModel);

            _catRepo.Add(Cat);
            _catRepo.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var Cat = _catRepo.Find(id);
        if (Cat == null)
            return NotFound();
        return View(Cat);
    }
    [HttpPost]
    public IActionResult Edit(CategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var Cat = _mapper.Map<Category>(viewModel);
            _catRepo.Update(Cat);
            _catRepo.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var Cat = _catRepo.Find(id);
        if (Cat == null)
            return NotFound();
        return View(Cat);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _catRepo.Remove(_catRepo.Find(id));
        _catRepo.Save();
        return RedirectToAction(nameof(Index));
    }
    
}