using Microsoft.AspNet.Identity;

namespace RecruitmentAgency.Models.Identity
{
    public class UsersManager : UserManager<Users, int>
    {
        public UsersManager(IUserStore<Users, int> store) 
            : base(store)
        {
            UserValidator = new UserValidator<Users, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}