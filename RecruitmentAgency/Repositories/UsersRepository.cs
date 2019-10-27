using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ISession _session;

        public UsersRepository(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<User> GetAll()
        {
            return _session.Query<User>()
                .Fetch(u => u.UserRole);
        }

        public User GetById(int id)
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

        public void Create(User entity)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(entity);
                tran.Commit();
            }
        }

        public void Update(User entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(User entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
