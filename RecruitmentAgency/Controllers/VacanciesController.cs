using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Repositories;

namespace RecruitmentAgency.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly IRepository<Vacancies> _vacanciesRepository;
        private readonly IRepository<Users> _usersRepository;

        public VacanciesController()
        {
            _vacanciesRepository = new VacanciesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }
        
        public ActionResult Index()
        {
            Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();
            ViewBag.userId = user.Id;

            return View();
        }

        [Authorize]
        public ActionResult VacanciesPartial(Summaries summary)
        {
            Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();

            if (user.UserRole.Name == "Работодатель")
            {
                List<Vacancies> vacancies = _vacanciesRepository.GetAll().Where(v => v.UserId == user.Id).ToList();

                return PartialView("_vacanciesPartial", vacancies);
            }

            if (user.UserRole.Name == "Администратор")
            {
                var vacancies = _vacanciesRepository.GetAll();

                return PartialView("_vacanciesPartial", vacancies);
            }

            if ( summary != null)
            {
                List<Vacancies> vacancies = summary.Id != 0 ?
                    _vacanciesRepository.GetAll().Where(v => v.MinExperience <= summary.Experience && !v.Archived).ToList() :
                    _vacanciesRepository.GetAll().Where(v => !v.Archived).ToList();

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
            Vacancies vacancy = _vacanciesRepository.GetById(vacancyId);
            Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();
            ViewBag.userRoleName = user.UserRole.Name;

            return View(vacancy);
        }

        [Authorize]
        public ActionResult Delete(int vacancyId)
        {
            Vacancies vacancy = _vacanciesRepository.GetById(vacancyId);
            _vacanciesRepository.Delete(vacancy);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View("Edit", new Vacancies());
        }

        [Authorize]
        public ActionResult Edit(int vacancyId)
        {
            return View(_vacanciesRepository.GetById(vacancyId));
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Vacancies vacancy)
        {
            if (ModelState.IsValid)
            {
                vacancy.UserId = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First().Id;

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