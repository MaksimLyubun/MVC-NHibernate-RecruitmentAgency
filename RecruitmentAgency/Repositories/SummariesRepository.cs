using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace RecruitmentAgency.Repositories
{
    public class SummariesRepository: ISummariesRepository
    {
        private readonly ISession _session;

        public SummariesRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Summary> GetAll()
        {
            return _session.Query<Summary>()
                .Fetch(s => s.User);
        }

        public Summary GetById(int id)
        {
            return _session.Query<Summary>()
                .Where(s => s.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Summary> GetByVacancy(Vacancy vacancy)
        {
            return _session.Query<Summary>()
                .Where(s => s.Experience >= vacancy.MinExperience);
        }

        public Summary GetByUserId(int userId)
        {
            return _session.Query<Summary>()
                .Where(s => s.UserId == userId)
                .FirstOrDefault();
        }

        public void Create(Summary entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update(Summary entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(Summary entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
