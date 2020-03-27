using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Services
{
  public  interface IEntityDataBaseTransaction:IDisposable
    {

        void Commit();
        void RollBack();
    }
}
