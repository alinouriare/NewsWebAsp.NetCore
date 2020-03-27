using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class PollRepository: IPollRepository
    {

        private readonly ApplicationDbContext _context;


        public PollRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public void ClosePoll(int id)
        {
            var result = (from u in _context.Poll_Tbl where u.PollId == id select u);
            var currentPoll = result.FirstOrDefault();

            if (result.Count() !=0)
            {
                currentPoll.Active = false;
                currentPoll.PollEndDate = PublicClass.DateAndTimeShamsi.DateShamsi();


                _context.Poll_Tbl.Attach(currentPoll);
                _context.Entry(currentPoll).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

        }



        public void setVote(int id)
        {

            var result = (from p in _context.Polloption_Tbl where p.PolloptionID == id select p);
            var currentPolloption = result.FirstOrDefault();

            if (result.Count() !=0)
            {
                currentPolloption.VouteCount++;

                _context.Polloption_Tbl.Attach(currentPolloption);
                _context.Entry(currentPolloption).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
