using AutoMapper;
using Goba_Store.DataAccess;
using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;
using Goba_Store.Utility;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Goba_Store.Controllers;

[Authorize(Roles = Constants.AdminRole)]
public class ProductController : Controller
{
    private readonly IProductRepository _proRepo;
    private readonly IWebHostEnvironment _environment;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository proRepo, IWebHostEnvironment environment, IMapper mapper)
    {
        _proRepo = proRepo;
        _environment = environment;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var Products = _proRepo.GetAll(includeProperities: "Category");
        return View(Products);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Details(int? id)
    {
        List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
        }

        ProductDetailsViewModel model = new ProductDetailsViewModel()
        {
            Product = _proRepo.FirstOrDefault(p => p.Id == id, includeProperities: "Category")
        };

        foreach (var item in ShoppingCartList)
        {
            if (item.Product.Id == id)
                model.InCart = true;
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ProductViewModel viewModel = new ProductViewModel();
        viewModel.CategoryDropDown = _proRepo.GetCategoryDropDownList();

        return View(viewModel);
    }

    //[HttpPost]
    //public IActionResult Create(ProductViewModel viewModel)
    //{
    //    var product = _mapper.Map<Product>(viewModel);
    //    if (!ModelState.IsValid)
    //    {
    //        viewModel.CategoryDropDown = _db.Categories.Select(i => new SelectListItem
    //        {
    //            Text = i.Name,
    //            Value = i.Id.ToString()
    //        });
    //        return View(viewModel);
    //    }

    //    Upload image
    //    product.Image = UploadImage();
    //    _db.Products.Add(product);
    //    _db.SaveChanges();
    //    return RedirectToAction(nameof(Index));
    //}

    [HttpPost]
    public IActionResult Create(ProductViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var product = _mapper.Map<Product>(viewModel);

            // Upload image
            product.Image = UploadImage();

            _proRepo.Add(product);
            _proRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        viewModel.CategoryDropDown = _proRepo.GetCategoryDropDownList();

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _proRepo.Find(id);
        if (product == null)
            return NotFound();

        var viewModel = _mapper.Map<ProductViewModel>(product);
        viewModel.CategoryDropDown = _proRepo.GetCategoryDropDownList();
        return View(viewModel);
    }
    [HttpPost]
    public IActionResult Edit(ProductViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var product = _mapper.Map<Product>(viewModel);

            if (Request.Form.Files.Count > 0)
            {
                // Delete old image
                DeleteImage(product.Id);
                // Then upload the new one
                product.Image = UploadImage();
            }
            else
            {
                // keep the old image
                var existingProduct = _proRepo.FirstOrDefault(p => p.Id == product.Id);
                product.Image = existingProduct.Image;
            }

            _proRepo.Update(product);
            _proRepo.Save();
            return RedirectToAction("Index");
        }

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _proRepo.FirstOrDefault(p => p.Id == id, includeProperities: "Category");
        if (product == null)
            return NotFound();
        return View(product);
    }
    
    public IActionResult DeletePost(int id)
    {
        var product = _proRepo.FirstOrDefault(i=>i.Id ==id);
        if (product == null)
            return NotFound();
        //Delete old image
        DeleteImage(id);
        _proRepo.Remove(product);
        _proRepo.Save();
        return RedirectToAction(nameof(Index));
    }


    private string UploadImage()
    {
        var files = Request.Form.Files;
        var rootPath = _environment.WebRootPath;
        var uploadedFile = rootPath + Constants.ImagePath;
        var fileName = Guid.NewGuid().ToString();
        var extention = Path.GetExtension(files[0].FileName);
        //create new one and save it
        using (FileStream stream = new FileStream(Path.Combine(uploadedFile, fileName + extention), FileMode.Create))
        {
            files[0].CopyTo(stream);
        }

        return fileName + extention;
    }

    private void DeleteImage(int ProductId)
    {
        var rootPath = _environment.WebRootPath;
        var uploadedFile = rootPath + Constants.ImagePath;
        var extention = Path.GetExtension(_proRepo.FirstOrDefault(p => p.Id == ProductId).Image);
        //delete old image
        var oldImage = Path.Combine(uploadedFile, _proRepo.FirstOrDefault(p => p.Id == ProductId).Image+extention);
        if (System.IO.File.Exists(oldImage))
        {
            System.IO.File.Delete(oldImage);
        }
    }
}

