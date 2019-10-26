using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Interfaces;
using RecruitmentAgency.Repositories;
using System.Web.Mvc;
using System.Linq;

namespace RecruitmentAgency.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<UserRoles> _userRolesRepository;
        private readonly IRepository<Users> _usersRepository;

        public HomeController()
        {
            _userRolesRepository = new UserRolesRepository(new DatabaseContext().MakeSession());
            _usersRepository = new UsersRepository(new DatabaseContext().MakeSession());
        }

        public ActionResult Index()
        {
            return View(CurrentUser);
        }

        private Users currentUser = null;
        public Users CurrentUser
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
                var userRole = _usersRepository.GetAll().Where(u => u.UserName == User.Identity.Name).First().UserRole;
                userRoleText = userRole != null? userRole.Name: null;
            }

            ViewBag.userRoleText = userRoleText;
            return PartialView("_userRolePartial");
        }
    }
}