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
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Models.ViewModels;
using SecurityNews.Services;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
  

    public class UserController : Controller
    {

        private readonly IUnitOfWork _context;
        private readonly IUploadfile _upload;

        private readonly UserManager<ApplicationUsers> _userManager;

        public UserController(IUnitOfWork context, IUploadfile upload, UserManager<ApplicationUsers> userManager)
        {
            _context = context;
            _upload = upload;
            _userManager = userManager;
        }
        public IActionResult Index()
        {


            ViewBag.ViewTitle = "لیست کاربران";
            var model = _context.UserManagerUW.Get();
            return View(model);
        }

        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> files)
        {
            //"upload\\userimage\\normalimage\\"
            //"\\upload\\userimage\\thumbnailimage\\"
            string filename = _upload.UploadFiles(files, "upload\\userimage\\normalimage\\", "\\upload\\userimage\\thumbnailimage\\");
            return Json(new { status = "success", message = "تصویر با موفقیت آپلود شد.", imagename = filename });

        }

        [HttpGet]
        [Authorize(Roles = "CreateUser")]
        public IActionResult Create()
        {
            ViewBag.ViewTitle = "فرم افزودن کاربر";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(UserViewModel model, string imagename)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (imagename == null)
                    {
                        model.UserImage = "defaultuserImage.png"; ;
                    }

                    else
                    {
                        model.UserImage = imagename;
                    }

                    ApplicationUsers user = new ApplicationUsers
                    {
                        Name = model.FirstName,
                        Family = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName,
                        Email = model.Email,
                        gender = model.gender,
                        BirthDayDate = model.BirthDayDate,
                        UserImagePath = model.UserImage


                    };


                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {

                    throw;
                }

            }
            ViewBag.Title = "فرم ایجاد کاربر";
            return View(model);
        }



        [HttpGet]
        [Authorize(Roles = "EditUser")]
        public async Task<IActionResult> Edit(string id)
        {

            EditUserViewModel model = new EditUserViewModel();
            ApplicationUsers user = await _userManager.FindByIdAsync(id);


            if (user != null)
            {
                model.FirstName = user.Name;
                model.LastName = user.Family;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.BirthDayDate = user.BirthDayDate;
                model.gender = user.gender;
                model.UserImage = user.UserImagePath;

            }

            ViewBag.ViewTitle = "فرم ویرایش کاربر";
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model, string id,
            string imagename, string chkinput)
        {


            if (ModelState.IsValid)
            {
                ApplicationUsers user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
                    user.Name = model.FirstName;
                    user.Family = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.gender = model.gender;
                    user.BirthDayDate = model.BirthDayDate;
                    //      if (imagename == null) user.UserImagePath = "defaultuserImage.png";
                    if (imagename != null) user.UserImagePath = imagename;

                    if (chkinput == "on")
                    {
                        user.PasswordHash = "AQAAAAEAACcQAAAAEEqrrbmtrftXxQcFNe5r9EYpS2S0Grn5uLVYLDXzxRW/Ng3I30nrFmExxZXmjDe3zQ==";
                    }

                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                }
            }


            ViewBag.ViewTitle = "فرم ویرایش کاربر";
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            string name = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUsers au = await _userManager.FindByIdAsync(id);
                if (au != null)
                {
                    name = au.Name + "   " + au.Family;
                    ViewBag.Name = name;
                }
            }
            return PartialView("_delete");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUsers au = await _userManager.FindByIdAsync(id);
                if (au != null)
                {
                    IdentityResult userResult = await _userManager.DeleteAsync(au);
                    if (userResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            return View();
        }


    }
}