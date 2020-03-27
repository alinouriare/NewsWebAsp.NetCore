using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class NewsRepository: INewsRepository
    {

        private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context)

        {
            _context = context;
        }

        public void RefreshVisitorCount(int Id)
        {
            var result = (from n in _context.News_Tbl where n.NewsId == Id select n);
            var currentNews = result.FirstOrDefault();

            if (result.Count() !=0)
            {

                currentNews.VisitCount++;
                _context.News_Tbl.Attach(currentNews);
                _context.Entry(currentNews).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
