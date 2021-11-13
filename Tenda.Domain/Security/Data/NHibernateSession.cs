using Europa.Domain.Data;
using NHibernate;
using System.Reflection;
using Tenda.Domain.Shared;

namespace Tenda.Domain.Security.Data
{
    public static class NHibernateSession
    {
        public static ISession Session(IInterceptor localSessionInterceptor)
        {
            return BaseSessionFactory.OpenSession(ProjectProperties.CsSeguranca, localSessionInterceptor, GetAssemby());
        }

        public static ISession Session()
        {
            return BaseSessionFactory.OpenSession(ProjectProperties.CsSeguranca, null, GetAssemby());
        }

        public static ISession Session(string csKey)
        {
            return BaseSessionFactory.OpenSession(csKey, null, GetAssemby());
        }

        public static IStatelessSession StatelessSession()
        {
            return BaseSessionFactory.OpenStatelessSession(ProjectProperties.CsSeguranca, GetAssemby());
        }

        public static IStatelessSession StatelessSession(string csKey)
        {
            return BaseSessionFactory.OpenStatelessSession(csKey, GetAssemby());
        }

        public static Assembly GetAssemby()
        {
            return Assembly.GetExecutingAssembly();
        }

        public static void CloseIfOpen(IStatelessSession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }
    }
}
