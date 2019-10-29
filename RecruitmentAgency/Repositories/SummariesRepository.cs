using System.Collections.Generic;
using System.Linq;

using NHibernate;
using NHibernate.Linq;

using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Repositories
{
    public class SummariesRepository: BaseRepository<Summary>, ISummariesRepository
    {
        public SummariesRepository(ISession session) : base (session) { }

        public override IEnumerable<Summary> GetAll()
        {
            return _session.Query<Summary>()
                .Fetch(s => s.User);
        }

        public override Summary GetById(int id)
        {
            return _session.Query<Summary>()
                .Fetch(s => s.User)
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
    }
}
