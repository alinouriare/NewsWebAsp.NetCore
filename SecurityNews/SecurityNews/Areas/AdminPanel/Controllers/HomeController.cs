using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models.UnitOfWork;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {


        private readonly IUnitOfWork _context;

        public HomeController(IUnitOfWork db)
        {
            _context = db;
        }


        public IActionResult Index(int page = 1)
        {
            int paresh = (page - 1) * 10;
            //تعداد کل ردیف ها
            int totalCount = _context.VisitRepositoryUW.Get().Count();
            ViewBag.PageID = page;
            double remain = totalCount % 10;

            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 10;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 10) + 1;
            }

            //ViewBag.ViewTitle = "لیست اخبار";
            //ViewBag.searchString = newssearch;
            var model = _context.VisitRepositoryUW.Get
                ().Skip(paresh).Take(10);
            return View(model);
        }
    }
}