using Goba_Store.Application.Services.Interfaces;
using Goba_Store.Application.View_Models;
using Goba_Store.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Goba_Store.Controllers;

[Authorize(Roles = Constants.AdminRole)]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _environment;

    public ProductController(IProductService productService, IWebHostEnvironment environment)
    {
        _productService = productService;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View(_productService.GetAllProducts());
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        List<ProductDetailsViewModel> ShoppingCartList = new List<ProductDetailsViewModel>();
        if (HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession) != null
           && HttpContext.Session.GetObj<IEnumerable<ShoppingCartViewModel>>(Constants.CartSession).ToList().Count > 0)
        {
            ShoppingCartList = HttpContext.Session.GetObj<IEnumerable<ProductDetailsViewModel>>(Constants.CartSession).ToList();
        }
       return View(_productService.GetDetailsOfProductInCart(id, ShoppingCartList));
    }

    [HttpGet]
    public IActionResult Create()
    {
        ProductViewModel viewModel = new ProductViewModel();
        viewModel.CategoryDropDown = _productService.GetDropDownList();

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(ProductViewModel viewModel,[FromForm] IFormFileCollection files)
    {
        if (ModelState.IsValid)
        {
            _productService.CreateProduct(viewModel , files);
            return RedirectToAction(nameof(Index));
        }

        viewModel.CategoryDropDown = _productService.GetDropDownList();

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound();
        product.CategoryDropDown = _productService.GetDropDownList();
        return View(product);
    }
    [HttpPost]
    public IActionResult Edit(ProductViewModel viewModel , [FromForm] IFormFileCollection files)
    {
        if (ModelState.IsValid)
        {
            _productService.UpdateProduct(viewModel , files);
            return RedirectToAction("Index");
        }
        viewModel.CategoryDropDown = _productService.GetDropDownList();
        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (id == null || id == 0)
            return NotFound();
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound();
        return View(product);
    }

    public IActionResult DeletePost(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound();;
        _productService.DeleteProduct(id);
        return RedirectToAction(nameof(Index));
    }

}

