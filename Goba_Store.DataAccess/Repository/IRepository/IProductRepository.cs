using Goba_Store.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goba_Store.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        public void Update(Product product);
        IEnumerable<SelectListItem> GetCategoryDropDownList();
    }
}
