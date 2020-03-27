using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecurityNews.Models;
using SecurityNews.Models.Services;
using SecurityNews.Models.UnitOfWork;
using SecurityNews.Models.ViewModels;


namespace SecurityNews.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class PollController : Controller
    {

        private readonly IUnitOfWork _context;

        private readonly IPollRepository _ipr;
        public PollController(IUnitOfWork context,IPollRepository ipr)
        {
            _context = context;
            _ipr = ipr;
        }
        public IActionResult Index(int page=1)
        {
            int paresh = (page - 1) * 15;
            //تعداد کل ردیف ها
            int totalCount = _context.PollOptionRepositoryUW.Get().Count();
            ViewBag.PageID = page;
            double remain = totalCount % 15;

            if (remain == 0)
            {
                ViewBag.PageCount = totalCount / 15;
            }
            else
            {
                ViewBag.PageCount = (totalCount / 15) + 1;
            }

            return View(_context.PollRepositoryUW.Get().Skip(paresh).Take(15));
        }


        [HttpGet]
        [Authorize(Roles = "PollCreate")]
        public IActionResult Create()
        {
            return View();



        }


        [HttpPost]
        public IActionResult Create(AddPollViewModel model)
        {
            if (ModelState.IsValid)
            {

                var activePoll = _context.PollRepositoryUW.Get(r => r.Active == true);

                if (activePoll.Count() >0)
                {
                    ViewBag.message = "از قبل یک نظرسنجی فعال وجود دارد.";
                    return View();
                }





                using (var transaction = _context.BeginTransaction())
                {
                    try
                    {
                        //ثبت نظرسنجی
                        Poll poll = new Poll();
                        poll.Question = model.Question;
                        poll.Active = true;
                        poll.PollStartDate = PublicClass.DateAndTimeShamsi.DateShamsi();
                        _context.PollRepositoryUW.Create(poll);
                        _context.Save();
                        //ثبت پاسخهای نظرسنجی
                        var answerList = model.Answer.Split(new string[] { Environment.NewLine },
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (var answer in answerList)
                        {
                            PollOption pollopt = new PollOption();
                            pollopt.Answer = answer;
                            pollopt.VouteCount = 0;
                            pollopt.PollID = poll.PollId;

                            _context.PollOptionRepositoryUW.Create(pollopt);
                            _context.Save();
                        }

                transaction.Commit();
                return RedirectToAction(nameof(Index));
            }
                    catch
            {
                        transaction.RollBack();
                    }
        }
            }
            return View(model);
        }



        [HttpGet]
        [Authorize(Roles = "PollDelete")]
        public IActionResult Delete(int? id)
        {

            if (id==null)
            {
                return NotFound();
            }

            Poll poll = _context.PollRepositoryUW.GetById(id);
            if (poll==null)
            {
                return NotFound();
            }

            return PartialView("_delete", poll);
        }


        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePoll(int id)
        {

            using (var transacation=_context.BeginTransaction())
            {



                try
                {

                    var polloption = _context.PollOptionRepositoryUW.Get(ou => ou.PollID == id).ToList();

                    for (int i = 0; i < polloption.Count() - 1; i++)
                    {
                        _context.PollOptionRepositoryUW.DeleteById(polloption[i].PolloptionID);
                        _context.Save();
                    }


                    _context.PollRepositoryUW.DeleteById(id);
                    _context.Save();

                    transacation.Commit();

                    return RedirectToAction("Index");
                }
                catch 
                {

                    transacation.RollBack();
                }

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        [Authorize(Roles = "PollClose")]
        public IActionResult ClosePoll(int? id)
        {

            if (id==null)
            {
                return NotFound();
            }

            Poll pl = _context.PollRepositoryUW.GetById(id);
            if (id==null)
            {
                return NotFound();
            }

            return PartialView("_closepoll", pl);
        }
        [HttpPost]
        public IActionResult ClosePoll(int id)
        {
            _ipr.ClosePoll(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "PollResult")]
        public ActionResult PollResult(int? id)
        {

            if (id ==null)
            {
                return NotFound();
            }

            var pollresult = _context.PollRepositoryUW.Get(t => t.PollId == id).Single();
            List<VoteResultViewModel> voteresult = new List<VoteResultViewModel>();

            foreach (PollOption  vr in _context.PollOptionRepositoryUW.Get(t=>t.PollID==pollresult.PollId))
            {
                voteresult.Add(new VoteResultViewModel {

                    label=vr.Answer,
                    data=vr.VouteCount

                });
            }

            ViewBag.getListofAnswer = JsonConvert.SerializeObject(voteresult);

            return PartialView("_pollresult",pollresult);
        }
    }
}