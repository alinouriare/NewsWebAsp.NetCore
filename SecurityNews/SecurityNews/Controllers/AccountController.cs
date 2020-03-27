using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecurityNews.Models;
using SecurityNews.Models.ViewModels;

namespace SecurityNews.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUsers> _userManager;

        private readonly SignInManager<ApplicationUsers> _signInManager;


        public AccountController(UserManager<ApplicationUsers> userManager,SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password
                    , model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                   
                    //Success Login
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var userRole = _userManager.GetRolesAsync(user).Result.ToArray();
                    //return RedirectToLocal(userRole, returnUrl);

                    return Json(new { status = "success" });
                    //return RedirectToLocal(userRole, returnUrl);
                }
                {
                    //اگر اطلاعات کاربر اشتباه بود
                    //ModelState.AddModelError("MyKey","نام کاربری یا رمز عبور اشتباه است");
                    //return PartialView("_loginPartial");
                    return Json(new { status = "fail" });
                    //Failed Login
                }
            }

            //return PartialView("_loginPartial",model);
            return Json(new { status = "fail2" });
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {

            await _signInManager.SignOutAsync();
            return Redirect("/Home");
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

        private IActionResult RedirectToLocal(string[] Roles, string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (Roles.Contains("User"))
                {
                    return Redirect("/AdminPanel/User/Index");
                }
            }
            return null;
        }
    }
}