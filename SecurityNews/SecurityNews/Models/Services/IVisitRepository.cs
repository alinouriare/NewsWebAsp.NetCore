using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Services
{
  public  interface IVisitRepository
    {
        void IncreasVisit(string Ip, string date);
    }
}
