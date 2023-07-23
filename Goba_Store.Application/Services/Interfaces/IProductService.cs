using Goba_Store.Application.View_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Goba_Store.Application.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<ProductViewModel> GetAllProducts();
        public IEnumerable<SelectListItem> GetDropDownList();
        ProductViewModel GetProductById(int id);
        void CreateProduct(ProductViewModel viewModel, IFormFileCollection files);
        void UpdateProduct(ProductViewModel viewModel, IFormFileCollection files);
        void DeleteProduct(int id);
        public ProductDetailsViewModel GetDetailsOfProductInCart(int id, List<ProductDetailsViewModel> ShoppingCartList);
    }
}
