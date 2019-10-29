using System.Collections.Generic;
using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        void Create(TEntity entity);

        void Update(TEntity entityToUpdate);

        void Delete(TEntity entityToDelete);
    }
}
