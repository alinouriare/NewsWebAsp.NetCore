using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.Repository;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryImpactController : Controller
    {


        private readonly IUnitOfWork _iuw;

        public CategoryImpactController(IUnitOfWork iuw)
        {
            _iuw = iuw;
        }
        [Authorize(Roles = "Management")]
        public IActionResult Index()
        {

          
            ViewBag.ViewTitle = "لیست دسته بندی";
            return View(_iuw.CategoryImpactRepositoryUW.Get());
        }

        [HttpGet]
        [Authorize(Roles = "Management")]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "فرم ایجاد دسته بندی";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryImpact model)
        {
            if (ModelState.IsValid)
            {
                _iuw.CategoryImpactRepositoryUW.Create(model);
                _iuw.Save();

                ViewBag.ViewTitle = "فرم ایجاد دسته بندی";
                ViewBag.SuccessMessage = "اطلاعات با موفقیت ثبت شد";
            }
            
            return View(model);

        }
        [HttpGet]
        [Authorize(Roles = "Management")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CategoryImpact ct = _iuw.CategoryImpactRepositoryUW.GetById(id);
            if (ct==null)
            {
                return NotFound();
            }
            ViewBag.ViewTitle = "فرم ویرایش دسته بندی";
            return View(ct);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int CategoryImpactId, CategoryImpact model)
        {
            if (CategoryImpactId != model.CategoryImpactId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _iuw.CategoryImpactRepositoryUW.Update(model);
                    _iuw.CategoryImpactRepositoryUW.Save();
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

            if (id ==null)
            {
                return NotFound();
            }
            var ct = _iuw.CategoryImpactRepositoryUW.GetById(id);
            if (ct==null)
            {
                return NotFound();
            }

            return PartialView(ct);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryImpact(int id)
        {
            _iuw.CategoryImpactRepositoryUW.DeleteById(id);
            _iuw.CategoryImpactRepositoryUW.Save();


            return RedirectToAction(nameof(Index));

        }

    }
}