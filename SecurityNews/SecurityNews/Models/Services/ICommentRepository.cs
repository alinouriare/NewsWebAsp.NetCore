using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Services
{
   public interface ICommentRepository
    {

         void IncreaseLike(int id);
        void IncreasedisLike(int id);
        void AcceptOrReject(int id);
    }
}
