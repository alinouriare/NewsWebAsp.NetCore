using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Services
{
  public  interface IPollRepository
    {

         void ClosePoll(int id);
        void setVote(int id);
    }
}
