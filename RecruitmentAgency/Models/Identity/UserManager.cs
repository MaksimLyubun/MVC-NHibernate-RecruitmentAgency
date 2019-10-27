using Microsoft.AspNet.Identity;

namespace RecruitmentAgency.Models.Identity
{
    public class UsersManager : UserManager<User, int>
    {
        public UsersManager(IUserStore<User, int> store) 
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}