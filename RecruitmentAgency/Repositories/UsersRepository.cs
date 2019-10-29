using System.Collections.Generic;
using System.Linq;

using NHibernate;
using NHibernate.Linq;

using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(ISession session) : base(session) { }
        
        public override IEnumerable<User> GetAll()
        {
            return _session.Query<User>()
                .Fetch(u => u.UserRole);
        }

        public override User GetById(int id)
        {
            return _session.Query<User>()
                .Where(u => u.Id == id)
                .Fetch(u => u.UserRole)
                .First();
        }

        public User GetByName(string name)
        {
            return _session.Query<User>()
                .Where(u => u.UserName == name)
                .Fetch(u => u.UserRole)
                .First();
        }
    }
}
