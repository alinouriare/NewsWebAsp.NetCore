using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Services;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class NewsController : Controller
    {

        private readonly IUploadfile _upload;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _iuw;

        public NewsController(IUnitOfWork iuw,IUploadfile upload, UserManager<ApplicationUsers> userManager)
        {
            _iuw = iuw;
            _upload = upload;
            _userManager = userManager;
        }
     

        public IActionResult Index(int page = 1, string newssearch = null)
        {


            if (newssearch != null)
            {
                int paresh = (page - 1) * 8;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.Title.Contains(newssearch)).Count();
                ViewBag.PageID = page;
                double remain = totalCount % 8;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 8;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 8) + 1;
                }

                ViewBag.ViewTitle = " لیست اخبار جستجو شده";
                ViewBag.searchString = newssearch;
                var model = _iuw.NewsRepositoryUW.Get
                    (u => u.UserID == _userManager.GetUserId(User) && u.Title.Contains(newssearch), i => i.OrderByDescending(z => z.NewsId), "tblCategoryImpact").Skip(paresh).Take(8);
                return View(model);
            }
            else
            {
                int paresh = (page - 1) * 8;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 8;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 8;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 8) + 1;
                }



                ViewBag.ViewTitle = "لیست اخبار";
                var model = _iuw.NewsRepositoryUW.Get
                    (u => u.UserID == _userManager.GetUserId(User), i => i.OrderByDescending(z => z.NewsId), "tblCategoryImpact").Skip(paresh).Take(8);
                return View(model);
            }
         
           
            





















        }

        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {

            string filename = _upload.UploadFiles(files, "upload\\indexImage\\", "");
            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });

        }

        [HttpGet]


        [Authorize(Roles = "NewsCreate")]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "فرم افزودن خبر";
            ViewBag.CategoryListImpact = _iuw.CategoryImpactRepositoryUW.Get();
            ViewBag.CategoryListPlatform = _iuw.CategoryPlatformRepositoryUW.Get();
            ViewBag.UserID = _userManager.GetUserId(User);

            return View();
        }

        [HttpPost]

        public IActionResult Create(News model, string IndexImage,byte r1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (IndexImage==null)
                    {
                        model.IndexImage= "defaultpic.jpg";
                    }
                    else
                    {
                        model.IndexImage = IndexImage;
                    }

                    News news = new News
                    {

                        Title = model.Title,
                        Abstract = model.Abstract,
                        Content = model.Content,
                        NewsDate = model.NewsDate,
                        NewsTime = model.NewsTime,
                        CategoryImpactID = model.CategoryImpactID,
                        CategoryPlatformID=model.CategoryPlatformID,
                        CVE=model.CVE,
                        NewsDateEN=model.NewsDateEN,
                        Path=model.Path,
                        UserID = model.UserID,
                        PlaceNewsID=model.PlaceNewsID,
                        MetaTag = model.MetaTag,
                        MetaDescription = model.MetaDescription,
                        NewsType =r1,
                        IndexImage = model.IndexImage

                    };
                    _iuw.NewsRepositoryUW.Create(news);
                    _iuw.Save();
                    return RedirectToAction("Index");
                }
                catch 
                {

                    throw;
                }
            }

            ViewBag.CategoryListImpact = _iuw.CategoryImpactRepositoryUW.Get();
            ViewBag.CategoryListPlatform = _iuw.CategoryPlatformRepositoryUW.Get();
            ViewBag.UserID = _userManager.GetUserId(User);
            ViewBag.ViewTitle = "فرم افزودن خبر";

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "NewsEdit")]
        public IActionResult Edit(int id)
        {

            var model = _iuw.NewsRepositoryUW.GetById(id);
            if (model !=null)
            {
                ViewBag.ViewTitle = "فرم ویرایش خبر";
                ViewBag.UserID = _userManager.GetUserId(User);
                ViewBag.CategoryListImpact = _iuw.CategoryImpactRepositoryUW.Get();
                ViewBag.CategoryListPlatform = _iuw.CategoryPlatformRepositoryUW.Get();
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(News model, string newIndexImage, string currentImageName, byte r1)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (newIndexImage!=null)
                    {
                        model.IndexImage = newIndexImage;
                    }
                    else
                    {
                        model.IndexImage = currentImageName;
                    }
                    model.NewsType = r1;
                    _iuw.NewsRepositoryUW.Update(model);
                    _iuw.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch 
                {

                    throw;
                }
            }
            else
            {
                ViewBag.ViewTitle = "فرم ویرایش خبر";
                return View(model);
            }
        }
        [HttpGet]
        [Authorize(Roles = "NewsDelete")]
        public IActionResult Delete(int? id)
        {
            if (id ==null)
            {
                return NotFound();
            }
            var news = _iuw.NewsRepositoryUW.GetById(id);
            if (news==null)
            {
                return NotFound();
            }
            return PartialView("_Delete", news);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteNews(int id)
        {

            _iuw.NewsRepositoryUW.DeleteById(id);
            _iuw.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}