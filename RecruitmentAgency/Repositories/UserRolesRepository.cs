using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class UserRolesRepository : IRepository<UserRole>
    {
        private readonly ISession _session;

        public UserRolesRepository(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<UserRole> GetAll()
        {
            return _session.Query<UserRole>();
        }

        public UserRole GetById(int id)
        {
            return _session.Query<UserRole>()
                .Where(ur => ur.Id == id)
                .FirstOrDefault();
        }

        public void Create(UserRole entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update(UserRole entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(UserRole entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
