using Goba_Store.Models;

namespace Goba_Store.Application.View_Models
{

    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool InCart { get; set; }
    }
}