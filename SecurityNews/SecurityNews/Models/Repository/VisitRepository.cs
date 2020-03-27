using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class VisitRepository: IVisitRepository
    {



        private readonly ApplicationDbContext _context;

        public VisitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //افزایش آمار بازدید  
        public void IncreasVisit(string Ip, string date)
        {
            var result = (from v in _context.Visit_Tbl
                          where v.IpAddress.Equals(Ip) && v.DateTime.Equals(date)
                          select v);
            var current = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                current.VisitHit++;


                _context.Visit_Tbl.Attach(current);
                _context.Entry(current).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
