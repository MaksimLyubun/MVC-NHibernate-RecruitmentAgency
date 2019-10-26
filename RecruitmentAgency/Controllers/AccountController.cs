﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Linq;

using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Repositories;
using System.Collections.Generic;

namespace RecruitmentAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<UserRoles> _userRolesRepository;
        private readonly IRepository<Users> _usersRepository;

        public AccountController()
        {
            _userRolesRepository = new UserRolesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }
        
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult UsersPartial()
        {
            Users user = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First();

            if (user.UserRole.Name == "Администратор")
            {
                return PartialView("_usersPartial", _usersRepository.GetAll());
            }

            return RedirectToAction("Index", "Account");
        }

        [Authorize]
        public ActionResult Create()
        {
            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name");
            ViewBag.Roles = roles;

            return View("Create", new RegisterViewModel());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users() { UserName = model.UserName, UserRole = _userRolesRepository.GetAll().Where(r => r.Id == model.UserRole).First() };
                var result = UsersManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name");
            ViewBag.Roles = roles;

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int userId)
        {
            Users user = _usersRepository.GetById(userId);
            ViewBag.userId = user.Id;

            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name", user.UserRole.Id);
            ViewBag.Roles = roles;
            
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                UserName = user.UserName,
                UserRole = user.UserRole.Id,
            };
            
            return View(registerViewModel);
        }

        [Authorize]
        public ActionResult ChangePassword(int userId)
        {
            ViewBag.userId = userId;
            
            RegisterViewModel registerViewModel = new RegisterViewModel();

            return View(registerViewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(RegisterViewModel registerViewModel, int userId)
        {
            Users user = _usersRepository.GetById(userId);

            ModelState.Remove("UserName");
            ModelState.Remove("UserRole");
            if (ModelState.IsValid)
            {
                user.PasswordHash = UsersManager.PasswordHasher.HashPassword(registerViewModel.Password);

                _usersRepository.Update(user);
                
                return RedirectToAction("Index", "Account");
            }
            
            return View(registerViewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(RegisterViewModel registerViewModel, int userId)
        {
            Users user = _usersRepository.GetById(userId);

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserRoles userRole = _userRolesRepository.GetById(registerViewModel.UserRole);

                if (userRole.Id != user.UserRole.Id)
                {
                    Users newUser = new Users()
                    {
                        UserName = registerViewModel.UserName,
                        PasswordHash = user.PasswordHash,
                        UserRole = userRole
                    };

                    _usersRepository.Delete(user);

                    _usersRepository.Create(newUser);

                    return RedirectToAction("Index", "Account");
                }

                user.UserName = registerViewModel.UserName;
                
                _usersRepository.Update(user);

                return RedirectToAction("Index", "Account");
            }
            
            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name", user.UserRole.Id);
            ViewBag.Roles = roles;

            return View(registerViewModel);
        }

        [Authorize]
        public ActionResult Delete(int userId)
        {
            Users user = _usersRepository.GetById(userId);
            _usersRepository.Delete(user);

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SignInManager.PasswordSignIn(model.UserName, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    var userRole = _usersRepository.GetAll().Where(u => u.UserName == model.UserName).First().UserRole;

                    switch (userRole.Name)
                    {
                        case "Администратор":
                            return RedirectToAction("Index", "Vacancies");
                        case "Соискатель":
                            return RedirectToAction("Details", "Summaries");
                        case "Работодатель":
                            return RedirectToAction("Index", "Vacancies");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильно введён логин или пароль.");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            SelectList roles = new SelectList(_userRolesRepository.GetAll().Where(r => r.Id != 1), "Id", "Name");
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users() { UserName = model.UserName, UserRole = _userRolesRepository.GetAll().Where(r=> r.Id == model.UserRole).First()};
                var result = UsersManager.Create(user, model.Password);
                
                if (result.Succeeded)
                {
                    SignInManager.SignIn(user, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            SelectList roles = new SelectList(_userRolesRepository.GetAll().Where(r => r.Id != 1), "Id", "Name");
            ViewBag.Roles = roles;

            return View(model);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }

        public UsersManager UsersManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }
    }
}