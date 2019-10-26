using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class UserRolesRepository : IRepository<UserRoles>
    {
        private readonly ISession _session;

        public UserRolesRepository(ISession session)
        {
            _session = session;
        }
        
        public IEnumerable<UserRoles> GetAll()
        {
            return _session.Query<UserRoles>()
                .ToList();
        }

        public UserRoles GetById(int id)
        {
            return _session.Query<UserRoles>()
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public void Create(UserRoles entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update(UserRoles entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(UserRoles entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
