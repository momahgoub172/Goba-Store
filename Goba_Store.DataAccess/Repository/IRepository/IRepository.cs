using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Goba_Store.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        T Find(int id);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null , 
            Func<IQueryable<T> , IOrderedQueryable<T>> orderby = null,
            string includeProperities =null);

        T FirstOrDefault(Expression<Func<T, bool>> filter = null,
             string includeProperities = null);
        void Add(T entity);
        void Remove(T entity);
        void Save();
    }
}
