using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using System.Linq;

using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Models.Views;
using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Repositories;

namespace RecruitmentAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<UserRole> _userRolesRepository;
        private readonly IUsersRepository _usersRepository;

        public AccountController()
        {
            _userRolesRepository = new BaseRepository<UserRole>(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }
        
        public ActionResult Index()
        {
            ViewBag.user = _usersRepository.GetByName(User.Identity.Name);

            return View();
        }
        
        [Authorize]
        public ActionResult UsersPartial()
        {
            User user = _usersRepository.GetByName(User.Identity.Name);

            if (user.IsAdmin())
            {
                ViewBag.userId = user.Id;
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
                User user = new User() { UserName = model.UserName, UserRole = _userRolesRepository.GetById(model.UserRole) };
                IdentityResult result = UsersManager.Create(user, model.Password);

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
            if(userId <= 0)
            {
                return RedirectToAction("Index");
            }

            User user = _usersRepository.GetById(userId);
            ViewBag.userId = user.Id;

            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name", user.UserRole.Id);
            ViewBag.Roles = roles;
            
            UserViewModel model = new UserViewModel()
            {
                UserName = user.UserName,
                UserRole = user.UserRole.Id,
            };
            
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword(int userId)
        {
            if (userId <= 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.userId = userId;

            PasswordViewModel model = new PasswordViewModel();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model, int userId)
        {
            if (userId <= 0)
            {
                return RedirectToAction("Index");
            }

            User user = _usersRepository.GetById(userId);

            ModelState.Remove("UserName");
            ModelState.Remove("UserRole");
            if (ModelState.IsValid)
            {
                user.PasswordHash = UsersManager.PasswordHasher.HashPassword(model.Password);

                _usersRepository.Update(user);
                
                return RedirectToAction("Index", "Account");
            }
            
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(UserViewModel model, int userId)
        {
            if (userId <= 0)
            {
                return RedirectToAction("Index");
            }

            User user = _usersRepository.GetById(userId);

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserRole userRole = _userRolesRepository.GetById(model.UserRole);

                if (userRole.Id != user.UserRole.Id)
                {
                    User newUser = new User()
                    {
                        UserName = model.UserName,
                        PasswordHash = user.PasswordHash,
                        UserRole = userRole
                    };

                    _usersRepository.Delete(user);

                    _usersRepository.Create(newUser);

                    return RedirectToAction("Index", "Account");
                }

                user.UserName = model.UserName;
                
                _usersRepository.Update(user);

                return RedirectToAction("Index", "Account");
            }
            
            SelectList roles = new SelectList(_userRolesRepository.GetAll(), "Id", "Name", user.UserRole.Id);
            ViewBag.Roles = roles;

            return View(model);
        }

        [Authorize]
        public ActionResult Delete(int userId)
        {
            if (userId <= 0)
            {
                return RedirectToAction("Index");
            }

            User user = _usersRepository.GetById(userId);
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
                    User user = _usersRepository.GetByName(model.UserName);

                    if(user.IsEmployee())
                    {
                        return RedirectToAction("Index", "Vacancies");
                    }

                    if (user.IsAdmin())
                    {
                        return RedirectToAction("Index", "Vacancies");
                    }

                    if (user.IsJobseeker())
                    {
                        return RedirectToAction("Details", "Summaries");
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
                var user = new User() { UserName = model.UserName, UserRole = _userRolesRepository.GetById(model.UserRole)};
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