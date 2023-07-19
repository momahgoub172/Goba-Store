using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Goba_Store.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Goba_Store.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbset;


        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbset = db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Find(int id)
        {
            return dbset.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperities = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
                query = query.Where(filter);
            if (includeProperities != null)
            {
                foreach (var item in includeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.AsNoTracking().FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperities = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
                query = query.Where(filter);
            if(includeProperities != null)
            {
                foreach (var item in includeProperities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (orderby != null)
                query = orderby(query);
            return query.AsNoTracking().ToList();

        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
