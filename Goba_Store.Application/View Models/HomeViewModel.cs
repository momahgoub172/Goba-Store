using Goba_Store.Models;

namespace Goba_Store.Application.View_Models
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}