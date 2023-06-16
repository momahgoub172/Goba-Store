using Goba_Store.Models;

namespace Goba_Store.View_Models;

public class CartItemViewModel
{
    public CartItemViewModel()
    {
        Product = new Product();
    }
    public Product Product { get; set; }
    public bool InCart { get; set; }
}