using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class CategoryImpactRepository : ICategoryImpact
    {
      

        private readonly ApplicationDbContext _context;
        public CategoryImpactRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(CategoryImpact model)
        {
            _context.CategoryImpact_Tbl.Add(model);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
