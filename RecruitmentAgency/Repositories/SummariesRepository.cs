using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class SummariesRepository: IRepository<Summaries>
    {
        private readonly ISession _session;

        public SummariesRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Summaries> GetAll()
        {
            return _session.Query<Summaries>()
                .Fetch(e => e.User)
                .ToList();
        }

        public Summaries GetById(int id)
        {
            return _session.Query<Summaries>()
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }
        
        public void Create(Summaries entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update(Summaries entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(Summaries entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
