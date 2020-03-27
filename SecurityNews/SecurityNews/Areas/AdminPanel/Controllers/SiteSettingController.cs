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
    public class SiteSettingController : Controller
    {
        private readonly IUnitOfWork _context;

        public SiteSettingController(IUnitOfWork context)
        {
            _context = context;
        }
        [Authorize(Roles = "Management")]
        public IActionResult Index()
        {
            var model = _context.SiteSettingRepositoryUW.Get().Single();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Management")]
        public IActionResult setSetting(SiteSetting model)
        {
            if (ModelState.IsValid)
            {
                _context.SiteSettingRepositoryUW.Update(model);
                _context.Save();

                ViewBag.SuccessMessage = "اطلاعات با موفقیت ویرایش شد";
            }
            return View("Index", model);
        }
    }
}