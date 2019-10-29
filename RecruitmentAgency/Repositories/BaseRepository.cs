using System.Collections.Generic;
using System.Linq;
using NHibernate;

using RecruitmentAgency.Interfaces;

namespace RecruitmentAgency.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        protected readonly ISession _session;

        public BaseRepository(ISession session)
        {
            _session = session;
        }

        public virtual void Create(TEntity entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entityToDelete);
                transaction.Commit();
            }
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _session.Query<TEntity>();
        }

        public virtual TEntity GetById(int id)
        {
            return _session.Query<TEntity>()
                .Where(s => s.Id == id)
                .FirstOrDefault();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(entityToUpdate);
                transaction.Commit();
            }
        }
    }
}