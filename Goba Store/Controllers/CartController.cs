﻿using Goba_Store.Data;
using Goba_Store.Models;
using Goba_Store.Services;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Goba_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        public CartController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            List<HomeProductViewModel> ShoppingCartList = new List<HomeProductViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
                && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession).ToList();
            }

            List<int> ProductsInCart = ShoppingCartList.Select(i => i.Product.Id).ToList();
            IEnumerable<CartProductViewModel> ProductsList = _db.Products.Where(p => ProductsInCart.Contains(p.Id))
                .Select(p => new CartProductViewModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.Image,
                    ShortDescription = p.ShortDescription,
                    Id = p.Id
                });

            return View(ProductsList);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
                /*
            * 1-get cart session
            * 2-add item to it
            * 3-set new session
            */

            List<HomeProductViewModel> ShoppingCartList = new List<HomeProductViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession).ToList();
            }
            //if this product is the first product in cart
            ShoppingCartList.Add(new HomeProductViewModel { Product = new Product {Id= id },InCart=true });
            HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
            return RedirectToAction("index","Home");
        }
        
        public IActionResult RemoveFromCart(int id,string? controllername)
        {
            /*
             * 1-get cart session
             * 2-remove item from it
             * 3-set new session
             */

            List<HomeProductViewModel> ShoppingCartList = new List<HomeProductViewModel>();
            if (HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<HomeProductViewModel>>(Constants.CartSession).ToList();
            }

            var removedItem = ShoppingCartList.SingleOrDefault(i => i.Product.Id == id);

            if (removedItem != null)
                ShoppingCartList.Remove(removedItem);

            HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
            return RedirectToAction("index",controllername);
        }
        
        //TODO:Inquiry confirmation
    }
}
