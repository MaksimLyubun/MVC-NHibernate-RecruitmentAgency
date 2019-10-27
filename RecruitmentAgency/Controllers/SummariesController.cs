using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Repositories;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace RecruitmentAgency.Controllers
{
    public class SummariesController : Controller
    {
        private readonly ISummariesRepository _summariesRepository;
        private readonly IUsersRepository _usersRepository;

        public SummariesController()
        {
            _summariesRepository = new SummariesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.user = _usersRepository.GetByName(User.Identity.Name);

            return View();
        }

        [Authorize]
        public ActionResult SummariesPartial(Vacancy vacancy)
        {
            List<Summary> summaries = vacancy != null ?
                _summariesRepository.GetByVacancy(vacancy).ToList() :
                _summariesRepository.GetAll().ToList();

            return PartialView("_summariesPartial", summaries);
        }


        [Authorize]
        public ActionResult Details(int? summaryId)
        {
            Summary summary = new Summary();
            User user = _usersRepository.GetByName(User.Identity.Name);

            ViewBag.user = user;

            if ( summaryId != null)
            {
                summary = _summariesRepository.GetById((int)summaryId);
            }
            else
            {
                summary = _summariesRepository.GetByUserId(user.Id);
            }

            return View(summary);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            User user = _usersRepository.GetByName(User.Identity.Name);
            Summary summary = _summariesRepository.GetByUserId(user.Id);
            return View(summary != null? summary: new Summary());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Summary summary)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        summary.Photo = memoryStream.ToArray();
                    }
                }
            }

            if (ModelState.IsValid)
            {
                summary.UserId = _usersRepository.GetByName(User.Identity.Name).Id;

                if (summary.Id != 0)
                    _summariesRepository.Update(summary);
                else
                    _summariesRepository.Create(summary);

                return RedirectToAction("Details");
            }
            
            return View(summary);
        }
    }
}