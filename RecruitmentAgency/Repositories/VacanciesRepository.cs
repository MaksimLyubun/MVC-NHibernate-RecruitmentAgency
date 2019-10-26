using System.Collections.Generic;
using RecruitmentAgency.Models;
using RecruitmentAgency.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System;

namespace RecruitmentAgency.Repositories
{
    public class VacanciesRepository : IRepository<Vacancies>
    {
        const string insertStoreProcedure = "EXECUTE [dbo].[AddVacancy] " +
            "@Name=:Name, " +
            "@Description=:Description, " +
            "@Term=:Term, @Company=:Company, " +
            "@MinExperience=:MinExperience, " +
            "@Salary=:Salary, " +
            "@UserId=:UserId, " +
            "@Archived=:Archived";

        private readonly ISession _session;

        public VacanciesRepository(ISession session)
        {
            _session = session;
        }

        public IEnumerable<Vacancies> GetAll()
        {
            return _session.Query<Vacancies>()
                .Fetch(e => e.User)
                .ToList();
        }

        public Vacancies GetById(int id)
        {
            return _session.Query<Vacancies>()
                .Where(u => u.Id == id)
                .FirstOrDefault();
        }

        public void Create(Vacancies entity)
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

        public void Update(Vacancies entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }

        public void Delete(Vacancies entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }
    }
}
