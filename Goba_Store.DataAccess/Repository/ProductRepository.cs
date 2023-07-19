using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goba_Store.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetCategoryDropDownList()
        {
            return _db.Categories.Select(c => new SelectListItem {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void Update(Product product)
        {
           _db.Products.Update(product);
        }
    }
}
