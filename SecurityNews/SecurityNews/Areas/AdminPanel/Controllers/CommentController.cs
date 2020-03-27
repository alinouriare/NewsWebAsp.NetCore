using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class CommentController : Controller
    {

        private readonly IUnitOfWork _context;

        private readonly ICommentRepository _icr;
        public CommentController(IUnitOfWork context,ICommentRepository icr)
        {
            _context = context;
            _icr = icr;
        }


        public IActionResult Index(int page = 1, string fromdate = null, string todate = null)
        {

            if (fromdate != null || todate != null)
            {
                //جستجو انجام شده است

                if (fromdate != null)
                {
                    fromdate = PublicClass.ConvertfaToEndigit.toEnNumber(fromdate);
                }
                if (todate != null)
                {
                    todate = PublicClass.ConvertfaToEndigit.toEnNumber(todate);
                }

                int paresh = (page - 1) * 2;
                //تعداد کل ردیف ها

                int totalCount = 0;
                if (fromdate != null)
                {
                    if (todate != null)
                    {
                        //FromDate And ToDate
                        totalCount = _context.commentRepositoryUW.Get(
                            c => c.commentDate.CompareTo(fromdate) >= 0 && c.commentDate.CompareTo(todate) <= 0
                            ).Count();
                    }
                    else
                    {
                        //FromDate
                        totalCount = _context.commentRepositoryUW.Get(
                        c => c.commentDate.CompareTo(fromdate) >= 0).Count();
                    }
                }
                if (todate != null && fromdate == null)
                {
                    //Todate
                    totalCount = _context.commentRepositoryUW.Get(
                        c => c.commentDate.CompareTo(todate) <= 0).Count();
                }
                ViewBag.PageID = page;
                double remain = totalCount % 15;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 2;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 2) + 1;
                }


                ViewBag.ViewTitle = "لیست نظرات";




                if (fromdate != null)
                {
                    if (todate != null)
                    {
                        //FromDate And ToDate

                        ViewBag.from = fromdate;
                        ViewBag.to = todate;
                        return View(_context.commentRepositoryUW.Get
                            (c => c.commentDate.CompareTo(fromdate) >= 0 && c.commentDate.CompareTo(todate) <= 0
                        , null, "TblNews").Skip(paresh).Take(2));
                    }
                    else
                    {
                        //FromDate
                        ViewBag.from = fromdate;
                        return View(_context.commentRepositoryUW.Get
                        (c => c.commentDate.CompareTo(fromdate) >= 0
                    , null, "TblNews").Skip(paresh).Take(2));
                    }
                }
                if (todate != null && fromdate == null)
                {
                    //Todate
                    ViewBag.to = todate;
                    return View(_context.commentRepositoryUW.Get
                      (c => c.commentDate.CompareTo(todate) <= 0
                  , null, "TblNews").Skip(paresh).Take(2));
                }
                return View();


            }
            else
            {
                //جستجو  انجام نشده است
                int paresh = (page - 1) * 15;
                //تعداد کل ردیف ها
                int totalCount = _context.commentRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 15;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 15;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 15) + 1;
                }


                ViewBag.ViewTitle = "لیست نظرات";
                return View(_context.commentRepositoryUW.Get(null, null, "TblNews").Skip(paresh).Take(15));
            }
        }





        [HttpGet]
        [Authorize(Roles = "CommentAccept")]
        public IActionResult AcceptOrReject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Comment cm = _context.commentRepositoryUW.GetById(id);
            if (cm == null)
            {
                return NotFound();
            }

            if (cm.status == true)
            {
                ViewBag.Title = " عدم نمایش نظر";
            }
            else
            {
                ViewBag.Title = "تایید و نمایش نظر";

            }

            return PartialView("_acceptorreject", cm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AcceptOrReject(int id)
        {
            _icr.AcceptOrReject(id);
            return RedirectToAction(nameof(Index));
        }
        

        [HttpGet]
        [Authorize(Roles = "CommentDelete")]
        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Comment cm = _context.commentRepositoryUW.GetById(id);
            if (cm==null)
            {
                return NotFound();
            }
            return PartialView("_delete", cm);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteComment(int id)
        {
            _context.commentRepositoryUW.DeleteById(id);
            _context.commentRepositoryUW.Save();

            return RedirectToAction(nameof(Index));
        }

    }
}