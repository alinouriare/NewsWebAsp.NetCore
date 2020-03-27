using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecurityNews.Models;
using SecurityNews.Models.Services;
using SecurityNews.Models.SpiderModel;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static SecurityNews.Models.ViewModels.IndexPageSection;

namespace SecurityNews.Controllers
{
    public class HomeController : Controller
    {

        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _iuw;
        private readonly INewsRepository _inews;
        private readonly ICommentRepository _icr;
        private readonly IPollRepository _ipr;


        public HomeController(IUnitOfWork iuw, SignInManager<ApplicationUsers> signInManager,
            UserManager<ApplicationUsers> userManager,
            INewsRepository inews,
            ICommentRepository icr,
            IPollRepository ipr)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _iuw = iuw;
            _inews = inews;
            _icr = icr;
            _ipr = ipr;
        }


        public IActionResult ViewDEV
                 ()
        {

            return View();

        }
        public IActionResult Index()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var query = _iuw.UserManagerUW.GetById(_userManager.GetUserId(HttpContext.User));
                ViewBag.FullName = query.Name + " " + query.Family;
            }

            var model = new IndexModel();
            model.SliderNews = _iuw.NewsRepositoryUW.Get(u => u.PlaceNewsID == 1, n => n.OrderByDescending(ns => ns.NewsId)).Take(4).ToList();
            model.SpecialNews = _iuw.NewsRepositoryUW.Get(u => u.PlaceNewsID == 2, n => n.OrderByDescending(ns => ns.NewsId)).Take(10).ToList();
            model.LastVideo = _iuw.NewsRepositoryUW.Get(u => u.PlaceNewsID == 4, n => n.OrderByDescending(ns => ns.NewsId)).Take(6).ToList();



            model.LastNews = _iuw.NewsRepositoryUW.Get(null, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.InNews = _iuw.NewsRepositoryUW.Get(u => u.NewsType == 1, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.OutNews = _iuw.NewsRepositoryUW.Get(n => n.NewsType == 2, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.PrivateNews = _iuw.NewsRepositoryUW.Get(n => n.NewsType == 3, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();

            model.Adv = _iuw.AdvertisRepositoryUW.Get(a => (a.FromDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) <= 0
           && a.ToDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) >= 0) && a.flag == 0).ToList();


            //متاتگها
            var sitesettingResult = _iuw.SiteSettingRepositoryUW.Get().Single();
            ViewData["metaKey"] = sitesettingResult.MetaTag;
            ViewData["metadescription"] = sitesettingResult.MetaDescription;
            ViewData["Title"] = sitesettingResult.SiteTitle;


            if (_iuw.PollRepositoryUW.Get(t => t.Active == true).Count() == 1)
            {
                var pollresult = _iuw.PollRepositoryUW.Get(t => t.Active == true).Single();



                if (Request.Cookies["poll" + pollresult.PollId] == null)
                {
                    //کاربر هنوز در نظرسنجی شرکت نکرده است
                    model.PollModel = pollresult;
                }
                else
                {
                    //کاربر در نظرسنجی شرکت کرده است
                    //نمایش نتایج به صورت نمودار

                    List<VoteResultViewModel> voteresult = new List<VoteResultViewModel>();
                    foreach (PollOption vr in _iuw.PollOptionRepositoryUW.Get(p => p.PollID == pollresult.PollId))
                    {
                        voteresult.Add(new VoteResultViewModel
                        {
                            data = vr.VouteCount,
                            label = vr.Answer
                        });
                    }


                    ViewBag.getListofAnswer = JsonConvert.SerializeObject(voteresult);




                    model.PollModel = pollresult;
                }
            }
            else
            {
                model.PollModel = null;
            }



            return View(model);
        }



        public IActionResult NewsDetails(int Id)
        {
            var model = new IndexModel();

            model.LastNews = _iuw.NewsRepositoryUW.Get(null, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.InNews = _iuw.NewsRepositoryUW.Get(n => n.NewsType == 1, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.OutNews = _iuw.NewsRepositoryUW.Get(n => n.NewsType == 2, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();
            model.PrivateNews = _iuw.NewsRepositoryUW.Get(n => n.NewsType == 3, n => n.OrderByDescending(ns => ns.NewsId)).Take(15).ToList();


            ////model.NewsDetails = _iuw.NewsRepositoryUW.GetById(Id);
            //ViewBag.NewsDetails = _iuw.NewsRepositoryUW.GetById(Id);


            var newsinfo = _iuw.NewsRepositoryUW.GetById(Id);
            ViewBag.NewsDetails = newsinfo;

            ViewBag.comments = _iuw.commentRepositoryUW.Get(n => n.NewsId == Id && n.status == true);




            //ارسال اطلاعات متاتگ ها
            ViewData["metaKey"] = newsinfo.MetaTag;
            ViewData["metadescription"] = newsinfo.MetaDescription;
            ViewData["Title"] = newsinfo.Title;

            //model.Adv = _iuw.AdvertisRepositoryUW.Get(a => (a.FromDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) <= 0
            //&& a.ToDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) >= 0) && a.flag == 0).ToList();


            ////دستورات مربوط به افزایش آمار بازدید


            _inews.RefreshVisitorCount(Id);


            return View(model);
        }


        [HttpPost]
        public IActionResult InsertComment(string txtEmail, string txtFullName, string txtMessage, int newsid, int cmid)
        {

            if (txtEmail == null || txtFullName == null || txtMessage == null)
            {
                return Json(new { status = "failModel" });
            }

            try
            {
                var currentDate = DateTime.Now;
                PersianCalendar pcCalender = new PersianCalendar();
                int year = pcCalender.GetYear(currentDate);
                int month = pcCalender.GetMonth(currentDate);
                int day = pcCalender.GetDayOfMonth(currentDate);




                string Shamsidate = string.Format("{0:0000}/{1:00}/{2:00}", year, month, day);
                string NowTime = string.Format("{0:hh:mm}", Convert.ToDateTime(pcCalender.GetHour(currentDate) + ":" + pcCalender.GetMinute(currentDate)));


                Comment model = new Comment();
                //ثبت نظر در دیتابیس

                model.Email = txtEmail;
                model.FullName = txtFullName;
                model.Message = txtMessage;
                model.commentDate = Shamsidate;
                model.commentTime = NowTime;
                model.status = false;
                model.NewsId = newsid;
                model.IP = HttpContext.Connection.RemoteIpAddress.ToString();

                model.ReplyID = cmid;

                _iuw.commentRepositoryUW.Create(model);
                _iuw.Save();



                return Json(new { status = "success" });

            }
            catch
            {

                return Json(new { status = "fail" });
            }


        }

        [HttpPost]
        public async Task<IActionResult> Like(int cmid)
        {
            var IsCommentExist = _iuw.commentRepositoryUW.GetById(cmid);

            if (IsCommentExist == null)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            //چک کردن اینکه آیا از قبل کوکی ایجاد شده است یا نه
            if (Request.Cookies["commentLike"] == null)
            {
                //کوکی وجود نداشته است
                //پس کوکی باید ایجاد شود
                Response.Cookies.Append("commentLike", "," + cmid + ",", new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddYears(1) });
                _icr.IncreaseLike(cmid);

                return Json(new { status = "success", result = IsCommentExist.LikeCount, backid = cmid });
            }
            else
            {

                //اگر کوکی از قبل وجود داشت


                string cookieContent = Request.Cookies["commentLike"].ToString();
                //اگر کاربر خواست یک کامنت را 2 بار لایک کند

                if (cookieContent.Contains("," + cmid + ","))
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    //اگر کاربر قبلا کامنتی را لایک کرده است و حالا می خواهد کامنت دیگری را لایک کند

                    cookieContent += "," + cmid + ",";


                    //cookie new jadid ba data new
                    Response.Cookies.Append("commentLike", cookieContent, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

                    _icr.IncreaseLike(cmid);
                    return Json(new { status = "success", result = IsCommentExist.LikeCount, backid = cmid });
                }



            }





        }



        [HttpPost]


        public async Task<IActionResult> DisLike(int cmid)
        {
            var IsCommentExist = _iuw.commentRepositoryUW.GetById(cmid);
            if (IsCommentExist == null)
            {
                //اگر آی دی کامنت اشتباه بود
                //return null;
                return Redirect(Request.Headers["Referer"].ToString());
            }

            //چک کردن اینکه آیا از قبل کوکی ایجاد شده است یا نه
            if (Request.Cookies["commentDisLike"] == null)
            {
                //کوکی وجود نداشته است
                //پس کوکی باید ایجاد شود
                Response.Cookies.Append("commentDisLike", "," + cmid + ",",
                    new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

                _icr.IncreasedisLike(cmid);

                return Json(new { status = "success", result = IsCommentExist.DisLikeCount, backid = cmid });
            }
            else
            {
                //اگر کوکی از قبل وجود داشت

                string cookieContent = Request.Cookies["commentDisLike"].ToString();
                //اگر کاربر خواست یک کامنت را 2 بار لایک کند
                if (cookieContent.Contains("," + cmid + ","))
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    //اگر کاربر قبلا کامنتی را لایک کرده است و حالا می خواهد کامنت دیگری را لایک کند
                    cookieContent += "," + cmid + ",";
                    Response.Cookies.Append("commentDisLike", cookieContent,
                         new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

                    _icr.IncreasedisLike(cmid);
                    return Json(new { status = "success", result = IsCommentExist.DisLikeCount, backid = cmid });
                }

            }
        }



        [HttpPost]
        public IActionResult setVote(int answerid, int pollid)
        {
            if (answerid != 0 && pollid != 0)

            {
                if (Request.Cookies["poll" + pollid] == null)
                {
                    Response.Cookies.Append("poll" + pollid, "kjdhsfkshfsdkjfhs",
                       new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddMonths(1) });

                    _ipr.setVote(answerid);
                    return Json(new { status = "success" });


                }
                else
                {
                    return Json(new { status = "fail" });
                }
            }
            else
            {
                return Json(new { status = "fail" });
            }


        }




        [HttpGet]
        public IActionResult SearchResult(string txtsearch, int page = 1)
        {

            var model = new IndexModel();
            //model.Adv = _iuw.AdvertisRepositoryUW.Get(a => (a.FromDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) <= 0
            //&& a.ToDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) >= 0) && a.flag == 0).ToList();

            if (txtsearch != null)
            {

                int paresh = (page - 1) * 4;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.Title.Contains(txtsearch)).Count();
                ViewBag.PageID = page;
                double remain = totalCount % 4;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 4;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 4) + 1;
                }


                model.searchmodel = _iuw.NewsRepositoryUW.Get(n => n.Title.Contains(txtsearch))
                    .Skip(paresh).Take(4).ToList();
                if (model.searchmodel.Count() > 0)
                {
                    ViewBag.searchVal = txtsearch;
                    return View("SearchResult", model);
                }
                else
                {
                    model.searchmodel = null;
                }
            }
            return View("SearchResult", model);
        }



        [HttpGet]
        public IActionResult menuResultImpact(int? id, int page = 1)
        {
            var model = new IndexModel();
            //model.Adv = _iuw.AdvertisRepositoryUW.Get(a => (a.FromDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) <= 0
            //&& a.ToDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) >= 0) && a.flag == 0).ToList();

            if (id != null)
            {

                int paresh = (page - 1) * 10;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.CategoryImpactID == id).Count();
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


                model.searchmodel = _iuw.NewsRepositoryUW.Get(n => n.CategoryImpactID == id, null, "tblCategoryImpact")
                    .Skip(paresh).Take(10).ToList();
                if (model.searchmodel.Count() > 0)
                {
                    ViewBag.getId = id;
                    ViewBag.menudesc = model.searchmodel[0].tblCategoryImpact.Title;
                    return View(model);
                }
                else
                {
                    model.searchmodel = null;
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult menuResultPlatform(int? id, int page = 1)
        {

            var model = new IndexModel();
            //model.Adv = _iuw.AdvertisRepositoryUW.Get(a => (a.FromDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) <= 0
            //&& a.ToDate.CompareTo(PublicClass.DateAndTimeShamsi.DateShamsi()) >= 0) && a.flag == 0).ToList();

            if (id != null)
            {

                int paresh = (page - 1) * 10;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.CategoryPlatformID == id).Count();
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


                model.searchmodel = _iuw.NewsRepositoryUW.Get(n => n.CategoryPlatformID == id, null, "tblCategoryPlatform")
                    .Skip(paresh).Take(10).ToList();
                if (model.searchmodel.Count() > 0)
                {
                    ViewBag.getId = id;
                    ViewBag.menudesc = model.searchmodel[0].tblCategoryPlatform.Title;
                    return View(model);
                }
                else
                {
                    model.searchmodel = null;
                }
            }
            return View(model);
        }



        public IActionResult Show(int page = 1, string newssearch = null)
        {
            if (newssearch != null)
            {
                int paresh = (page - 1) * 14;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.Title.Contains(newssearch)).Count();
                ViewBag.PageID = page;
                double remain = totalCount % 14;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 14;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 14) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار جستجو شده";
                ViewBag.searchString = newssearch;
                var model = _iuw.NewsRepositoryUW.Get
               (u=>u.Title.Contains(newssearch), u => u.OrderByDescending(t => t.NewsId), "tblCategoryImpact").Skip(paresh).Take(14);
                return View(model);
            }
            else
            {
                int paresh = (page - 1) * 14;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 14;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 14;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 14) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار";
                var model = _iuw.NewsRepositoryUW.Get
                    ( null,u=>u.OrderByDescending(t=>t.NewsId) , "tblCategoryImpact").Skip(paresh).Take(14);
                return View(model);
            }
        }



        public IActionResult ShowCVE(int page = 1, string newssearchCVE = null)
        {
            if (newssearchCVE != null)
            {
                int paresh = (page - 1) * 14;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get(n => n.CVE.Contains(newssearchCVE)).Count();
                ViewBag.PageID = page;
                double remain = totalCount % 14;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 14;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 14) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار جستجو شده";
                ViewBag.searchStringCVE = newssearchCVE;
                var model = _iuw.NewsRepositoryUW.Get
               (u => u.CVE.Contains(newssearchCVE), u => u.OrderByDescending(t => t.NewsId), "tblCategoryImpact").Skip(paresh).Take(14);
                return View(model);
            }
            else
            {
                int paresh = (page - 1) * 14;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 14;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 14;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 14) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار";
                var model = _iuw.NewsRepositoryUW.Get
                    (null, u => u.OrderByDescending(t => t.NewsId), "tblCategoryImpact").Skip(paresh).Take(14);
                return View(model);
            }
        }


        public IActionResult Spider(int page = 1)
        {
           
                int paresh = (page - 1) * 6;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsSpiderRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 6;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 6;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 6) + 1;
                }


                string url = "https://www.us-cert.gov/ncas/current-activity";
                HtmlWeb HWeb = new HtmlWeb();
                HtmlDocument HDoc = HWeb.Load(url);
                var HtmlMainPage = HDoc.DocumentNode;
                HDoc.LoadHtml(HtmlMainPage.InnerHtml);

                if (HtmlMainPage.HasChildNodes)
                {
                    var tagA = HDoc.DocumentNode.SelectNodes("//a");
                    foreach (var item in tagA)
                    {
                        string urlA = item.Attributes["href"] != null ? item.Attributes["href"].Value : null;

                        if (urlA != null)
                        {
                            string Url = EditUrl(item.Attributes["href"].Value, url);
                            if (Url != null)
                            {
                                var rr = "https://www.us-cert.gov/ncas/current-activity";

                                var qMainLink = _iuw.MainLinkRepositoryUW.Get(a => a.Url.Equals(rr)).SingleOrDefault();


                                var qq = _iuw.InternalLinkRepositoryUW.Get(q => q.IntrnalUrl.Equals(Url)).Count();
                                if (qq == 0)
                                {
                                    InternalLink modeli = new InternalLink();

                                    modeli.IntrnalUrl = Url;
                                    modeli.MainlinkID = qMainLink.MainLinkId;
                                    modeli.SpiderDate = DateTime.Now;
                                    modeli.SpiderStatus = true;

                                    _iuw.InternalLinkRepositoryUW.Create(modeli);
                                    _iuw.Save();

                                    HtmlDocument HEditDoc = new HtmlDocument();
                                    HtmlWeb HWebq = new HtmlWeb();

                                    var PageHtml = HWebq.Load(Url);

                                    if (PageHtml != null)
                                    {
                                        string EditHtml = PageHtml.DocumentNode.InnerHtml;
                                        HEditDoc.LoadHtml(EditHtml);

                                        if (HEditDoc.DocumentNode.HasChildNodes)
                                        {
                                            var qXpathContet = HEditDoc.DocumentNode.SelectSingleNode(qMainLink.XpathContent) != null ? HEditDoc.DocumentNode.SelectSingleNode(qMainLink.XpathContent).InnerHtml : null;
                                            if (qXpathContet != null && qXpathContet != "")
                                            {
                                                string content = HEditDoc.DocumentNode.SelectSingleNode(qMainLink.XpathContent).InnerHtml;
                                                string tittle = HEditDoc.DocumentNode.SelectSingleNode(qMainLink.XpathTitle).InnerText;
                                                NewsSpider modelnews = new NewsSpider();


                                                modelnews.Content = content;
                                                modelnews.IntrnalLinkID = modeli.InternalLinkId;
                                                modelnews.NewsDate = DateTime.Now;
                                                modelnews.Status = true;
                                                modelnews.Titel = tittle;
                                                _iuw.NewsSpiderRepositoryUW.Create(modelnews);
                                                _iuw.Save();

                                            }
                                        }
                                    }
                                }



                            }
                        }




                    }
                }
           
            var model = _iuw.NewsSpiderRepositoryUW.Get();
            return View(model);
        }


        public IActionResult SpiderSS(int page = 1, string newssearchS = null)
        {

            if (newssearchS != null)
            {
                int paresh = (page - 1) * 6;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsSpiderRepositoryUW.Get(n => n.Titel.Contains(newssearchS)).Count();
                ViewBag.PageID = page;
                double remain = totalCount % 6;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 6;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 6) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار جستجو شده";
                ViewBag.searchStringCVE = newssearchS;
                var modeli = _iuw.NewsSpiderRepositoryUW.Get
               (u => u.Titel.Contains(newssearchS), u => u.OrderBy(t => t.IdNews)).Skip(paresh).Take(6);
                return View(modeli);
            }
            else
            {
                int paresh = (page - 1) * 6;
                //تعداد کل ردیف ها
                int totalCount = _iuw.NewsSpiderRepositoryUW.Get().Count();
                ViewBag.PageID = page;
                double remain = totalCount % 6;

                if (remain == 0)
                {
                    ViewBag.PageCount = totalCount / 6;
                }
                else
                {
                    ViewBag.PageCount = (totalCount / 6) + 1;
                }

                ViewBag.ViewTitle = "لیست اخبار";
                var model = _iuw.NewsSpiderRepositoryUW.Get
                    (null, u => u.OrderBy(t => t.IdNews)).Skip(paresh).Take(6);
                return View(model);
            }
        }

        public IActionResult ShowSpider(int id)
        {

            if (id != null)
            {
                var model = _iuw.NewsSpiderRepositoryUW.GetById(id);
                return View(model);
            }
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            ViewData["Title"] = "مشکل";
            ViewData["Message"] = "مشکل درخواست شما!!!!!";
            return View();
        }

        public string EditUrl(string Url, string MainUrl)
        {



            if (Url.StartsWith("/ncas/current-activity/20") && !Url.Contains("#"))
            {

                string ut = "https://www.us-cert.gov" + Url;
                return ut;
            }


            return null;



        }
    }
}
