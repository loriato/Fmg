using Europa.Data.Model;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace Europa.Data
{
    public class NHibernateRepository<TEntity> where TEntity : BaseEntity
    {
        public ISession _session { get; set; }

        public NHibernateRepository()
        {

        }

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        protected ISession Session { get { return _session; } }

        public IQueryable<TEntity> Queryable()
        {
            return _session.Query<TEntity>();
        }

        public TEntity FindById(long id)
        {
            return _session.Get<TEntity>(id);
        }

        public virtual void Save(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }

        /// <summary>
        /// Dá um hit no database para garantir que o registro existe e apenas inicializa o proxy
        /// </summary>
        /// <param name="id"></param>
        public TEntity Load(long id)
        {
            return _session.Load<TEntity>(id);
        }

        public void Flush()
        {
            Session.Flush();
        }

        public IQueryable<TEntity> UpdatedFrom(DateTime referenceDate)
        {
            return Queryable().Where(reg => reg.AtualizadoEm > referenceDate);
        }
    }
}
