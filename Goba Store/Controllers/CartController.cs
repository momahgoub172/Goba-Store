using Goba_Store.Data;
using Goba_Store.Models;
using Goba_Store.Services;
using Goba_Store.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace Goba_Store.Controllers
{
    public class CartController : Controller
    {

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
                /*
            * 1-get cart session
            * 2-add item to it
            * 3-set new session
            */

            List<CartItemViewModel> ShoppingCartList = new List<CartItemViewModel>();
            //get other products if there is any products in cart
            if (HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<ShoppingCart>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<CartItemViewModel>>(Constants.CartSession).ToList();
            }
            //if this product is the first product in cart
            ShoppingCartList.Add(new CartItemViewModel { Product = new Product {Id= id },InCart=true });
            HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
            return RedirectToAction("index","Home");
        }



        public IActionResult RemoveFromCart(int id)
        {
            /*
             * 1-get cart session
             * 2-remove item from it
             * 3-set new session
             */

            List<CartItemViewModel> ShoppingCartList = new List<CartItemViewModel>();
            if (HttpContext.Session.GetObj<IEnumerable<CartItemViewModel>>(Constants.CartSession) != null
               && HttpContext.Session.GetObj<IEnumerable<CartItemViewModel>>(Constants.CartSession).ToList().Count > 0)
            {
                ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<CartItemViewModel>>(Constants.CartSession).ToList();
            }

            var removedItem = ShoppingCartList.SingleOrDefault(i => i.Product.Id == id);

            if (removedItem != null)
                ShoppingCartList.Remove(removedItem);

            HttpContext.Session.SetObj(Constants.CartSession, ShoppingCartList);
            return RedirectToAction("index","home");
        }
    }
}
