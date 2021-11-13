using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace Europa.Data
{
    public class NHibernateStatelessRepository<TEntity>
    {
        public IStatelessSession _statelessSession { get; set; }

        public IQueryable<TEntity> Queryable()
        {
            return _statelessSession.Query<TEntity>();
        }

        public TEntity FindById(long id)
        {
            return _statelessSession.Get<TEntity>(id);
        }
    }
}
