using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class CrudGenericMethod<Tentity> where Tentity :class
     {
        private readonly ApplicationDbContext _context;
        private DbSet<Tentity> _table;

        public CrudGenericMethod(ApplicationDbContext context)
        {
            _context = context;
            _table = _context.Set<Tentity>();
        }


        public virtual void Create(Tentity entity)
        {

            _table.Add(entity);
        }

        public virtual void Update(Tentity entity)
        {
            
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(Tentity entity)
        {
            if (_context.Entry(entity).State==EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
        }

        public virtual void DeleteById(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }
        public virtual IEnumerable<Tentity> Get(Expression<Func<Tentity,bool>> whereVariable = null, Func<IQueryable<Tentity>,
            IOrderedQueryable<Tentity>> orderbyVariable = null,string joinString = "")
        {

            IQueryable<Tentity> query = _table;
            if (whereVariable !=null)
            {
                query = query.Where(whereVariable);
            }
            if (orderbyVariable !=null)
            {
                query = orderbyVariable(query);
            }
            if (joinString !="")
            {
                foreach (string joins in joinString.Split(','))
                {
                    query = query.Include(joins);
                }
            }

            return query.ToList();
        }

        //

       
        public virtual Tentity GetById(object id)
        {
            return _table.Find(id);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }

    }
}
