using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Services;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    
  
    public class AdvertisingController : Controller
    {
        //private readonly UserManager<ApplicationUsers> _userManager;

        //private readonly SignInManager<ApplicationUsers> _signInManager;


        /// <summary>
        /// 
        /// </summary>

        private readonly IUnitOfWork _context;

        private readonly IUploadfile _upload;

        private readonly IAdvertiseRepository _iar;

        private readonly IHostingEnvironment _webroot;

        public AdvertisingController(IUnitOfWork context,IUploadfile upload,IAdvertiseRepository iar,
            IHostingEnvironment webroot
          
           )
        {
            _context = context;
            _upload = upload;
            _iar = iar;
            _webroot = webroot;

          
           
        }
        //[Authorize(Roles = "ADVERTISE,AdvertiseCreate,DELETEADVERTISE,CHANGEADVERTISE")]
        //[Authorize(Roles = "ADVERTISE")]
        //[AllowAnonymous]

        public IActionResult Index(int page=1)
        {
            int paresh = (page - 1) * 10;
            //تعداد کل ردیف ها
            int totalCount = _context.AdvertisRepositoryUW.Get().Count();
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


            var model = _context.AdvertisRepositoryUW.Get().Skip(paresh).Take(10);
            ViewBag.ViewTitle = "تبلیغات";
            return View(model);
        }



        [HttpGet]
        [Authorize(Roles = "AdvertiseCreate")]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "ایجاد تبلیغ جدید";
            return View();
        }

        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {
            string filename = _upload.UploadFiles(files, "upload\\advImage\\", "");
            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });
        }


        
        [HttpPost]

        public IActionResult Create(Advertise model, string gifPath)
        {
            if (gifPath==null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                model.FromDate = PublicClass.ConvertfaToEndigit.toEnNumber(model.FromDate);
                model.ToDate = PublicClass.ConvertfaToEndigit.toEnNumber(model.ToDate);
                _context.AdvertisRepositoryUW.Create(model);
                _context.Save();
            }


            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "ChangeAdvertise")]
        public IActionResult ChangeStatus()
        {

          
                return PartialView("_changeStatus");
           
        

        }
        [HttpPost]
        public IActionResult ChangeStatus(int id)
        {
            _iar.changeStatus(id);
            return RedirectToAction("Index");

        }


        [HttpGet]
        [Authorize(Roles = "DeleteAdvertise")]
        public IActionResult Delete()
        {
            
                return PartialView("_delete");
           
         
           
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            //حذف فایل از روت سایت
            var query = _context.AdvertisRepositoryUW.GetById(id);
            if (query != null)
            {
                string gifname = query.gifPath;
                var dirpath = Path.Combine(_webroot.WebRootPath + "\\upload\\advImage\\" + gifname);
                System.IO.File.Delete(dirpath);
            }


            _context.AdvertisRepositoryUW.DeleteById(id);
            _context.AdvertisRepositoryUW.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}