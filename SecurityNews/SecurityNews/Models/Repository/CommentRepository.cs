using SecurityNews.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Models.Repository
{
    public class CommentRepository: ICommentRepository
    {


        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)

        {
            _context = context;
        }


        public void IncreaseLike(int id)
        {
            var result = (from u in _context.Comments_Tbl where u.Id == id select u);
            var currentComment = result.FirstOrDefault();


            if (result.Count() !=0)
            {
                currentComment.LikeCount++;
                _context.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

        }


        //افزایش تعداد دیسلایک
        public void IncreasedisLike(int id)
        {
            var result = (from c in _context.Comments_Tbl where c.Id == id select c);
            var currentComment = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentComment.DisLikeCount++;
                _context.Comments_Tbl.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }


        public void AcceptOrReject(int id)
        {



            var result = (from u in _context.Comments_Tbl where u.Id == id select u);
            var currentComment = result.FirstOrDefault();


            if (result.Count() !=0)
            {

                if (currentComment.status==true)
                {
                    currentComment.status = false;
                }
                else
                {
                    currentComment.status = true;
                }

                _context.Comments_Tbl.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }
        }
    }
}
