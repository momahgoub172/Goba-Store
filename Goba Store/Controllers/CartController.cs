using Goba_Store.Application.View_Models;
using Goba_Store.DataAccess;
using Goba_Store.Models;
using Goba_Store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession) != null
                && HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
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

        [HttpGet]
        public IActionResult Summary()
        {
            List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
            }
            List<int> ProductsInCart = ShoppingCartList.Select(i => i.Product.Id).ToList();
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var viewModel = new SummeryViewModel {
            Products = _db.Products.Where(p => ProductsInCart.Contains(p.Id)).ToList(),
            User = _db.AppUser.FirstOrDefault(u=>u.Id == Claims.Value)

            };


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
                /*
            * 1-get cart session
            * 2-add item to it
            * 3-set new session
            */

            List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
            }
            //if this product is the first product in cart
            ShoppingCartList.Add(new ProductDetailsViewModel { Product = new Product {Id= id },InCart=true });
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

            List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
            if (HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
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
