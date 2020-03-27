using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecurityNews.Models;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Models.ViewModels;

namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class RoleController : Controller
    {

        private readonly RoleManager<ApplicationRoles> _roleManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IAspNetUserRoleRepository _iau;
        private readonly IUnitOfWork _context;
        private readonly ApplicationDbContext _contextU;

        public RoleController(RoleManager<ApplicationRoles> roleManager,UserManager<ApplicationUsers> userManager,IAspNetUserRoleRepository iau,
            IUnitOfWork context,
            ApplicationDbContext contextU)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _iau = iau;
            _context = context;
            _contextU = contextU;
        }
        [Authorize(Roles = "Management")]
        public IActionResult Index()
        {
         

            //List<ApplicationRoleViewModel> model = new List<ApplicationRoleViewModel>();
            //var alluser = _contextU.UserRoles.ToList();
            //model = _roleManager.Roles.Select(r => new ApplicationRoleViewModel()
            //{

            //    Id = r.Id,
            //    Name = r.Name,
            //    Description = r.Description,
            //    NumberOfUsers = alluser.Count(ur => ur.RoleId == r.Id)

            //}).ToList();
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            nodes.Add(new TreeViewNode {

                id = "asd",
                parent = "#",
                text = "اجزای سیستم"

            });
            foreach (ApplicationRoles subnode in _roleManager.Roles.Where(r =>r.RoleLevel != "0"))
            {
                nodes.Add(new TreeViewNode {

                    id=subnode.Id.ToString(),
                    parent=subnode.RoleLevel.ToString(),
                    text=subnode.Description
                });
            }



            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            ViewBag.ViewTitle = "نمایش درختی اجزای سیستم";
            return View();


        }
        [HttpGet]

        [Authorize(Roles = "Management")]
        public IActionResult Create()
        {

            ViewBag.SystemPart = _roleManager.Roles.ToList();
            ViewBag.ViewTitle = "ایجاد اجزای جدید";
            return View();
        }

        [HttpPost]
     

        public async Task<IActionResult> Create(ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
              
                ApplicationRoles role = new ApplicationRoles();

                role.Name = model.Name;
                role.Description = model.Description;
                role.RoleLevel = model.RoleLevel;

                IdentityResult result =await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    ViewBag.ViewTitle = "تعریف اجزای سیستم";
                    return RedirectToAction("Index");

                }
            }

            return View(model);
        }





        



        [HttpGet]
        [Authorize(Roles = "RoleUser")]
        public async Task<IActionResult> AccessRight(string Id)
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            nodes.Add(new TreeViewNode {

                id = "asd",
                parent = "#",
                text = "اجزای سیستم"

            });
            foreach (ApplicationRoles subnode in _roleManager.Roles.Where(r =>r.RoleLevel != "0"))
            {
                nodes.Add(new TreeViewNode() {

                    id = subnode.Id.ToString(),
                    parent = subnode.RoleLevel.ToString(),
                    text = subnode.Description
                });

              
            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            ApplicationUsers user = await _userManager.FindByIdAsync(Id);
            if (user!=null)
            {
                ViewBag.userId = Id;
                string getRoleid = _iau.GetRoleId(Id);
                if (getRoleid.Length>0)
                {
                    ViewBag.roleList = getRoleid.Substring(0, getRoleid.Length - 1);
                }
                ViewBag.ViewTitle = "ثبت دسترسی برای " + user.Name + " " + user.Family;

                return View();
            }

            return NotFound();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccessRight(string selectedItems, string userId)
        {

            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            ApplicationUsers user = await _userManager.FindByIdAsync(userId);
            if (user !=null)
            {
                //Delete All Roles for User
                var roles = await _userManager.GetRolesAsync(user);
               
                IdentityResult delRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);


                if (delRoleResult.Succeeded)
                {
                    for (int i = 0; i < items.Count ; i++)
                    {
                        ApplicationRoles approle = await _roleManager.FindByIdAsync(items[i].id);
                        if (approle != null)
                        {
                            IdentityResult result = await _userManager.AddToRoleAsync(user, approle.Name);
                            if (result.Succeeded)
                            {

                            }
                        }
                    }
                }
            }


            return RedirectToAction("Index", "User");
        }



        //[HttpGet]
        //public async Task<IActionResult> DeleteRole(string id)
        //{
        //    string name = string.Empty;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        ApplicationRoles ar = await _roleManager.FindByNameAsync(id);
        //        if (ar != null)
        //        {
        //            name = ar.Description;
        //        }

        //    }
        //    return PartialView("_deleteRole", name);
        //}
   
        //[HttpPost, ActionName("DeleteRole")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteRoles(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        ApplicationRoles ar = await _roleManager.FindByIdAsync(id);
        //        if (ar != null)
        //        {
        //            IdentityResult roleResult = _roleManager.DeleteAsync(ar).Result;
        //            if (roleResult.Succeeded)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //        }
        //    }
        //    return View();
        //}
    }

}