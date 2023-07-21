using Goba_Store.DataAccess.Repository.IRepository;
using Goba_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Goba_Store.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            //base is Repository class
            var CategoryFromDb = base.FirstOrDefault(c=>c.Id == category.Id);
            if (CategoryFromDb != null)
            {
                CategoryFromDb.Name = category.Name;
                CategoryFromDb.Id = category.Id;
                CategoryFromDb.DisplayOrder = category.DisplayOrder;
            }
            _db.SaveChanges();
        }
    }
}
