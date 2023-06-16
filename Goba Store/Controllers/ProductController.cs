﻿using Goba_Store.Data;
using Goba_Store.Models;
using Goba_Store.Services;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Goba_Store.Controllers;

[Authorize(Policy = "AdminOnly")]
public class ProductController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _environment;

    public ProductController(AppDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    public IActionResult Index()
    {
        var Products = _db.Products.Include(p => p.Category);

        return View(Products);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Details(int? id)
    {
        List<HomeProductViewModel> ShoppingCartList = new List<HomeProductViewModel>();
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession).ToList();
        }

        HomeProductViewModel model = new HomeProductViewModel()
        {
            Product = _db.Products.Include(p => p.Category)
                .Where(p => p.Id == id).FirstOrDefault()
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
        IEnumerable<SelectListItem> CategoryDropDown = _db.Categories.Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });

        ViewBag.CategoryDropDown = CategoryDropDown;

        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<SelectListItem> CategoryDropDown = _db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;
            return View(product);
        }
        
        //Upload image
        var files = Request.Form.Files;
        var rootPath = _environment.WebRootPath;
        var uploadedFile = rootPath + Constants.ImagePath;
        var fileName = Guid.NewGuid().ToString();
        var extention = Path.GetExtension(files[0].FileName);
        using (FileStream stream = new FileStream(Path.Combine(uploadedFile,fileName+extention),FileMode.Create))
        {
            files[0].CopyTo(stream);
        }


        product.Image = fileName + extention;
        _db.Products.Add(product);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _db.Products.Find(id);
        if (product == null)
            return NotFound();
        
        IEnumerable<SelectListItem> CategoryDropDown = _db.Categories.Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });

        ViewBag.CategoryDropDown = CategoryDropDown;
        return View(product);
    }
    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {   var files = Request.Form.Files;
            var rootPath = _environment.WebRootPath;
            if (files.Count > 0)
            {
                var uploadedFile = rootPath + Constants.ImagePath;
                var fileName = Guid.NewGuid().ToString();
                var extention = Path.GetExtension(files[0].FileName);
                //delete old image
                var oldImage = Path.Combine(uploadedFile, _db.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id).Image);
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
                //create new one and save it
                using (FileStream stream = new FileStream(Path.Combine(uploadedFile,fileName+extention),FileMode.Create))
                {
                    files[0].CopyTo(stream);
                }

                product.Image = fileName + extention;
            }
            else
            {
                product.Image = _db.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id).Image;
            }

            _db.Products.Update(product);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }
    
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _db.Products.Include(c=>c.Category).FirstOrDefault(p=>p.Id == id);
        if (product == null)
            return NotFound();
        return View(product);
    }
    
    public IActionResult DeletePost(int? id)
    {
        var product = _db.Products.FirstOrDefault(i=>i.Id ==id);
        if (product == null)
            return NotFound();
        var uploadedFile = _environment.WebRootPath + Constants.ImagePath;
        //delete image
        var oldImage = Path.Combine(uploadedFile, _db.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id).Image);
        if (System.IO.File.Exists(oldImage))
        {
            System.IO.File.Delete(oldImage);
        }
        _db.Products.Remove(product);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}

