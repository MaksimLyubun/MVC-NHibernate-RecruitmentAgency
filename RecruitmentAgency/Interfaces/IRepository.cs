using System.Collections.Generic;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        void Create(TEntity entity);

        void Update(TEntity entityToUpdate);

        void Delete(TEntity entityToDelete);
    }
}
