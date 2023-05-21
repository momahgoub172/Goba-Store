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

    public IActionResult Details(int? id)
    {
        List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList();
        }

        ProductDetailsViewModel model = new ProductDetailsViewModel()
        {
            Product = _db.Products.Include(p => p.Category)
                .Where(p => p.Id == id).FirstOrDefault(),
            InCart = false

        };

        foreach(var item in ShoppingCartList)
        {
            if(item.ProductId == id)
                model.InCart = true;
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Details(int id)
    {
        /*
         * 1-get cart session
         * 2-add item to it
         * 3-set new session
         */

        List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
        //get other products if there is any products in cart
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList();
        }
        //if this product is the first product in cart
        ShoppingCartList.Add(new ShoppingCart { ProductId = id });
        HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult RemoveFromCart(int id)
    {
        /*
         * 1-get cart session
         * 2-remove item from it
         * 3-set new session
         */

        List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList();
        }

        var removedItem = ShoppingCartList.SingleOrDefault(i => i.ProductId == id);

        if(removedItem != null)
            ShoppingCartList.Remove(removedItem);

        HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
        return RedirectToAction(nameof(Index));
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