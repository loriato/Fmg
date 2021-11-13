using NHibernate;

namespace Tenda.Domain.Core.Services
{
    public abstract class BaseService
    {

        public ISession _session { get; set; }

        protected ISession Session { get { return _session; } }

        public BaseService()
        {

        }

        public BaseService(ISession Session)
        {
            _session = Session;
        }

    }

}
