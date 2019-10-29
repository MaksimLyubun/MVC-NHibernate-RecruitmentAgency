using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System;

namespace RecruitmentAgency.Repositories
{
    public class VacanciesRepository : BaseRepository<Vacancy>, IVacanciesRepository
    {
        const string insertStoreProcedure = "EXECUTE [dbo].[AddVacancy] " +
            "@Name=:Name, " +
            "@Description=:Description, " +
            "@Term=:Term, @Company=:Company, " +
            "@MinExperience=:MinExperience, " +
            "@Salary=:Salary, " +
            "@UserId=:UserId, " +
            "@Archived=:Archived";
        
        public VacanciesRepository(ISession session) : base(session) { }

        public override IEnumerable<Vacancy> GetAll()
        {
            return _session.Query<Vacancy>()
                .Fetch(v => v.User);
        }

        public override Vacancy GetById(int id)
        {
            return _session.Query<Vacancy>()
                .Fetch(v => v.User)
                .Where(v => v.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Vacancy> GetByUserId(int userId)
        {
            return _session.Query<Vacancy>()
                .Where(v => v.UserId == userId);
        }

        public IEnumerable<Vacancy> GetNotArchived()
        {
            return _session.Query<Vacancy>()
                .Fetch(v => v.User)
                .Where(v => !v.Archived);
        }

        public IEnumerable<Vacancy> GetNotArchivedBySummary(Summary summary)
        {
            return _session.Query<Vacancy>()
                .Fetch(v => v.User)
                .Where(v => !v.Archived && v.MinExperience <= summary.Experience);
        }

        public override void Create(Vacancy entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var query = _session.CreateSQLQuery(insertStoreProcedure);

                query.SetParameter("Name", entity.Name);
                query.SetParameter("Description", entity.Description);
                query.SetParameter("Term", entity.Term);
                query.SetParameter("Company", entity.Company);
                query.SetParameter("MinExperience", entity.MinExperience);
                query.SetParameter("Salary", entity.Salary);
                query.SetParameter("UserId", entity.UserId);
                query.SetParameter("Archived", entity.Archived);
                query.UniqueResult();

                transaction.Commit();
            }
        }
    }
}
