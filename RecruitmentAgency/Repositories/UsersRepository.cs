using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class UsersRepository : IRepository<Users>
    {
        private readonly ISession _session;

        public UsersRepository(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<Users> GetAll()
        {
            return _session.Query<Users>()
                .Fetch(p => p.UserRole)
                .ToList();
        }

        public Users GetById(int id)
        {
            return _session.Query<Users>()
                .Where(u => u.Id == id)
                .Fetch(p => p.UserRole)
                .FirstOrDefault();
        }
        
        public void Create(Users entity)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(entity);
                tran.Commit();
            }
        }

        public void Update(Users entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(Users entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
