using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class AdvertiseRepository: IAdvertiseRepository
    {


        private readonly ApplicationDbContext _context;

        public AdvertiseRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void changeStatus(int id)
        {

            var result = (from u in _context.Advertis_Tbl where u.AdId == id select u);
            var currentAdvertise = result.FirstOrDefault();

            if (result.Count() !=0)
            {
                if (currentAdvertise.flag == 0)
                {
                    currentAdvertise.flag = 1;
                }
                else
                {
                    currentAdvertise.flag = 0;
                }


                _context.Advertis_Tbl.Attach(currentAdvertise);
                _context.Entry(currentAdvertise).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }
        }
    }
}
