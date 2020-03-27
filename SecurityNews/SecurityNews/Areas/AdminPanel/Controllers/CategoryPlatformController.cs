using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.UnitOfWork;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class CategoryPlatformController : Controller
    {

        private readonly IUnitOfWork _iuw;
        public CategoryPlatformController(IUnitOfWork iuw)
        {
            _iuw = iuw;
        }
        [Authorize(Roles = "Management")]
        public IActionResult Index(int page=1)
        {

            int paresh = (page - 1) * 5;
            //تعداد کل ردیف ها
            int totalCount = _iuw.CategoryPlatformRepositoryUW.Get().Count();
            ViewBag.PageID = page;
            double remain = totalCount % 5;

            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 5;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 5) + 1;
            }

            ViewBag.ViewTitle = "لیست دسته بندی پلتفرم";
            return View(_iuw.CategoryPlatformRepositoryUW.Get().Skip(paresh).Take(5));
        }

        [HttpGet]
        [Authorize(Roles = "Management")]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "فرم ایجاد دسته بندی پلتفرم";
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryPlatform model)
        {

            if (ModelState.IsValid)
            {
                _iuw.CategoryPlatformRepositoryUW.Create(model);
                _iuw.CategoryPlatformRepositoryUW.Save();

                ViewBag.ViewTitle = "فرم ایجاد دسته بندی پلتفرم";
                ViewBag.SuccessMessage = "اطلاعات با موفقیت ثبت شد";
            }

            return View(model);

        }
        [HttpGet]
        [Authorize(Roles = "Management")]
        public IActionResult Edit(int? id)
        {

            if (id==null)
            {
                return NotFound();
            }
            CategoryPlatform ct = _iuw.CategoryPlatformRepositoryUW.GetById(id);
            if (ct==null)
            {
                return NotFound();
            }

            ViewBag.ViewTitle = "فرم ویرایش دسته بندی پلتفرم";
            return View(ct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryPlatform model,int CategoryPlatformtId)
        {

            if (CategoryPlatformtId !=model.CategoryPlatformtId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _iuw.CategoryPlatformRepositoryUW.Update(model);
                    _iuw.CategoryPlatformRepositoryUW.Save();
                }
                catch 
                {

                    throw;
                }
            }
            ViewBag.SuccessMessage = "اطلاعات با موفقیت ویرایش شد";
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Management")]
        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            CategoryPlatform ct = _iuw.CategoryPlatformRepositoryUW.GetById(id);
            if (ct==null)
            {
                return NotFound();
            }

            return PartialView(ct);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryPlatform(int id)
        {
            _iuw.CategoryPlatformRepositoryUW.DeleteById(id);
            _iuw.CategoryPlatformRepositoryUW.Save();


            return RedirectToAction(nameof(Index));

        }
    }
}