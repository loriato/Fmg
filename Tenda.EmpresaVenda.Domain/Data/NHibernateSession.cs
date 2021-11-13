using Autofac;
using Autofac.Integration.Mvc;
using Europa.Domain.Data;
using NHibernate;
using System.Collections.Generic;
using System.Reflection;
using Tenda.Domain.Core.Data;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Data
{
    public static class NHibernateSession
    {
        public static IStatelessSession StatelessSession()
        {
            return BaseSessionFactory.OpenStatelessSession(ProjectProperties.CsEmpresaVenda, CurrentAssemblies());
        }

        public static ISession Session()
        {
            return BaseSessionFactory.OpenSession(ProjectProperties.CsEmpresaVenda, null, CurrentAssemblies());
        }

        public static ISession Session(IInterceptor localSessionInterceptor)
        {
            return BaseSessionFactory.OpenSession(ProjectProperties.CsEmpresaVenda, localSessionInterceptor, CurrentAssemblies());
        }

        public static ISession NestedScopeSession()
        {
            var nestedScope = AutofacDependencyResolver.Current.RequestLifetimeScope.BeginLifetimeScope(builder =>
                builder.Register(x => NHibernateSession.Session(x.Resolve<DefaultInterceptor>()))
                    .As<ISession>());
            return nestedScope.Resolve<ISession>();
        }

        public static Assembly[] CurrentAssemblies()
        {
            var assemblies = new List<Assembly> {
                Assembly.GetExecutingAssembly(),
                Tenda.Domain.Security.Data.NHibernateSession.GetAssemby()
            };
            return assemblies.ToArray();
        }

        public static void CloseIfOpen(ISession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }
    }
}