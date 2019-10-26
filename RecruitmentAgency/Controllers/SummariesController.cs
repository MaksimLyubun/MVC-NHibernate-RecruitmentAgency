using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
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
        private readonly IRepository<Summaries> _summariesRepository;
        private readonly IRepository<Users> _usersRepository;

        public SummariesController()
        {
            _summariesRepository = new SummariesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult SummariesPartial(Vacancies vacancy)
        {
            List<Summaries> summaries = vacancy != null ?
                _summariesRepository.GetAll().Where(v => v.Experience >= vacancy.MinExperience).ToList() :
                _summariesRepository.GetAll().ToList();

            return PartialView("_summariesPartial", summaries);
        }


        [Authorize]
        public ActionResult Details(int? summaryId)
        {
            Summaries summary = new Summaries();

            if ( summaryId != null)
            {
                summary = _summariesRepository.GetById((int)summaryId);
            }
            else
            {
                Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();
                summary = _summariesRepository.GetAll().Where(s => s.UserId == user.Id).FirstOrDefault();
            }

            return View(summary);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit()
        {
            Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();
            Summaries summary = _summariesRepository.GetAll().Where(s => s.UserId == user.Id).FirstOrDefault();
            return View(summary != null? summary: new Summaries());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Summaries summary)
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
                summary.UserId = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First().Id;

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