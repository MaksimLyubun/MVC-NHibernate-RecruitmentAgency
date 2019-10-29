using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Repositories;
using System.Web.Mvc;

namespace RecruitmentAgency.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<UserRole> _userRolesRepository;
        private readonly IUsersRepository _usersRepository;

        public HomeController()
        {
            _userRolesRepository = new BaseRepository<UserRole>(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }

        public ActionResult Index()
        {
            ViewBag.user = _usersRepository.GetByName(User.Identity.Name);

            return View(CurrentUser);
        }

        private User currentUser = null;
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    string userName = User.Identity.Name;
                    if (userName != null)
                    {
                        currentUser = new DatabaseContext().Users.FindByNameAsync(userName).Result;
                    }
                }
                return currentUser;
            }
        }

        public ActionResult UserName()
        {
            string userNameText = "";
            if (User.Identity.IsAuthenticated)
            {
                userNameText = User.Identity.Name;
            }

            ViewBag.userNameText = userNameText;
            return PartialView("_userNamePartial");
        }

        public ActionResult UserRole()
        {
            string userRoleText = "";
            if (User.Identity.IsAuthenticated)
            {
                UserRole userRole = _usersRepository.GetByName(User.Identity.Name).UserRole;
                userRoleText = userRole != null? userRole.Name: null;
            }

            ViewBag.userRoleText = userRoleText;
            return PartialView("_userRolePartial");
        }
    }
}