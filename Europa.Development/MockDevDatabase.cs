using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Tenda.EmpresaVenda.Domain.Data;

namespace Europa.Development
{
    public static class MockDevDatabase
    {
        private static IDictionary<string, ISessionFactory> sessionFactorys = new Dictionary<string, ISessionFactory>();
        private static ISession _session = null;
        private static IStatelessSession _statelessSession = null;
        private static String CsMaster = "CS_TND_EMPRESA_VENDA";
        public static IStatelessSession OpenStatelessSession()
        {
            return CurrentSessionFactory(CsMaster, NHibernateSession.CurrentAssemblies())
                .OpenStatelessSession();
        }

        public static ISession OpenSession()
        {
            return CurrentSessionFactory(CsMaster, NHibernateSession.CurrentAssemblies()).OpenSession();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            ISessionFactory factory;
            if (!sessionFactorys.TryGetValue(connectionString, out factory))
            {
                factory = CreateSessionFactory(connectionString, assemblies);
            }

            return factory;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CreateSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            FluentConfiguration fluentConfiguration = Fluently.Configure();
            fluentConfiguration.Database(PostgreSQLConfiguration.Standard
                .Dialect<PostgreSQLDialect>()
                .ConnectionString(c => c.FromConnectionStringWithKey(connectionString))
                .Driver<NpgsqlDriver>());

            foreach (Assembly assembly in assemblies)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
            }

            bool generateDdl = false;

            SchemaUpdate schemaUpdate = null;
            if (generateDdl)
            {
                //fluentConfiguration.ExposeConfiguration(cfg => ScriptsToExecute = cfg.GenerateSchemaCreationScript(new Oracle12cDialect()));
                fluentConfiguration.ExposeConfiguration(cfg => schemaUpdate = new SchemaUpdate(cfg));
                fluentConfiguration.ExposeConfiguration(cfg => schemaUpdate.Execute(CreateDDL(), true));
            }

            //Configuration config = null;
            //fluentConfiguration.ExposeConfiguration(cfg => config = cfg);

            //Use this when mapping the database
            //NHibernateSchemaValidator.Validate(fluentConfiguration.BuildConfiguration());

            ISessionFactory factory = fluentConfiguration.BuildSessionFactory();

            if (schemaUpdate != null && schemaUpdate.Exceptions.Count > 0)
            {
                throw (schemaUpdate.Exceptions.First());
            }

            sessionFactorys.Add(connectionString, factory);

            return factory;
        }

        private static Action<string> CreateDDL()
        {
            string path = @"\tmp\update-evs.sql";
            Action<string> updateExport = x =>
            {
                using (var file = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(file))
                {
                    sw.Write(x);
                }
            };
            return updateExport;
        }

        public static void CloseCurrentSession()
        {
            CloseSession(_session);
        }

        public static void CloseSession(ISession session)
        {
            if (session != null && session.IsOpen)
                session.Close();
        }

        public static void TearDown()
        {
            CloseCurrentSession();
            CloseAllSessionFactory();
        }

        public static void CloseAllSessionFactory()
        {
            foreach (ISessionFactory factory in sessionFactorys.Values)
            {
                if (factory.IsClosed == false)
                {
                    factory.Close();
                }
            }
        }
    }
}
