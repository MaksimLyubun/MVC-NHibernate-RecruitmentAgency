using System.Collections.Generic;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Interfaces
{
    public interface ISummariesRepository: IRepository<Summary>
    {
        IEnumerable<Summary> GetByVacancy(Vacancy vacancy);

        Summary GetByUserId(int userId);
    }
}
