using System.Collections.Generic;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByName(string name);
    }
}
