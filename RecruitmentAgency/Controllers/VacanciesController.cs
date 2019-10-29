using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Models;
using RecruitmentAgency.Repositories;

namespace RecruitmentAgency.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly IVacanciesRepository _vacanciesRepository;
        private readonly IUsersRepository _usersRepository;

        public VacanciesController()
        {
            _vacanciesRepository = new VacanciesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }
        
        public ActionResult Index()
        {
            User user = _usersRepository.GetByName(User.Identity.Name);

            ViewBag.user = user;
            ViewBag.userId = user.Id;

            return View();
        }

        [Authorize]
        public ActionResult VacanciesPartial(Summary summary)
        {
            User user = _usersRepository.GetByName(User.Identity.Name);

            ViewBag.user = user;

            if (user.IsEmployee())
            {
                List<Vacancy> vacancies = _vacanciesRepository.GetByUserId(user.Id).ToList();

                return PartialView("_vacanciesPartial", vacancies);
            }

            if (user.IsAdmin())
            {
                List<Vacancy> vacancies = _vacanciesRepository.GetAll().ToList();

                return PartialView("_vacanciesPartial", vacancies);
            }

            if ( summary != null)
            {
                List<Vacancy> vacancies = summary.Id != 0 ?
                    _vacanciesRepository.GetNotArchivedBySummary(summary).ToList() :
                    _vacanciesRepository.GetNotArchived().ToList();

                return PartialView("_vacanciesPartial", vacancies);
            }
            else
            {
                return RedirectToAction("Index", "Vacancies");
            }
        }

        [Authorize]
        public ActionResult Details(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return RedirectToAction("Index");
            }

            Vacancy vacancy = _vacanciesRepository.GetById(vacancyId);
            User user = _usersRepository.GetByName(User.Identity.Name);

            ViewBag.userRoleName = user.UserRole.Name;
            ViewBag.user = user;

            return View(vacancy);
        }

        [Authorize]
        public ActionResult Delete(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return RedirectToAction("Index");
            }

            Vacancy vacancy = _vacanciesRepository.GetById(vacancyId);
            _vacanciesRepository.Delete(vacancy);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View("Edit", new Vacancy());
        }

        [Authorize]
        public ActionResult Edit(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return RedirectToAction("Index");
            }

            return View(_vacanciesRepository.GetById(vacancyId));
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                vacancy.UserId = _usersRepository.GetByName(User.Identity.Name).Id;

                if (vacancy.Id == 0)
                {
                    _vacanciesRepository.Create(vacancy);
                    return RedirectToAction("Index");
                }

                _vacanciesRepository.Update(vacancy);

                return RedirectToAction("Details", new { vacancyId = vacancy.Id });
            }
            
            return View(vacancy);
        }
        
    }
}