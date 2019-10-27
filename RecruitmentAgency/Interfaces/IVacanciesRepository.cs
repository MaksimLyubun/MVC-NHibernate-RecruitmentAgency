using System.Collections.Generic;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Interfaces
{
    public interface IVacanciesRepository : IRepository<Vacancy>
    {
        IEnumerable<Vacancy> GetByUserId(int userId);

        IEnumerable<Vacancy> GetNotArchived();

        IEnumerable<Vacancy> GetNotArchivedBySummary(Summary summary);
    }
}
