using Europa.Data.Conventions;
using Europa.Data.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Europa.Domain.Data
{
    public static class BaseSessionFactory
    {
        private static IDictionary<string, ISessionFactory> _sessionFactorys = new Dictionary<string, ISessionFactory>();

        public static IStatelessSession OpenStatelessSession(string ConnectionString, params Assembly[] Assemblies)
        {
            return CurrentSessionFactory(ConnectionString, Assemblies).OpenStatelessSession();
        }

        public static ISession OpenSession(string ConnectionString, IInterceptor localSessionInterceptor, params Assembly[] Assemblies)
        {
            ISession session;
            if (localSessionInterceptor != null)
            {
                session = CurrentSessionFactory(ConnectionString, Assemblies)
                    .WithOptions()
                    .Interceptor(localSessionInterceptor)
                    .OpenSession();
            }
            else
            {
                session = CurrentSessionFactory(ConnectionString, Assemblies).OpenSession();
            }
            session.FlushMode = FlushMode.Commit;
            return session;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentSessionFactory(string ConnectionString, params Assembly[] Assemblies)
        {
            ISessionFactory factory;
            if (!_sessionFactorys.TryGetValue(ConnectionString, out factory))
            {
                factory = CreateSessionFactory(ConnectionString, Assemblies);
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
                     .Driver<NHibernate.Driver.NpgsqlDriver>());

            foreach (Assembly assembly in assemblies)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
            }

            //Mapping Conventions
            fluentConfiguration
                .Mappings(map => map.FluentMappings.Conventions.Add<ColumnNullabilityConvention>()
            );

            bool generateDdl = false ;

            SchemaUpdate schemaUpdate = null;
            if (generateDdl)
            {
                fluentConfiguration.ExposeConfiguration(cfg => schemaUpdate = new SchemaUpdate(cfg));
                fluentConfiguration.ExposeConfiguration(cfg => schemaUpdate.Execute(CreateDDL(connectionString), true));
            }

            ISessionFactory factory = fluentConfiguration.BuildSessionFactory();
            _sessionFactorys.Add(connectionString, factory);
            return factory;
        }

        /// <summary>
        /// Verificar se existe alguma exceção que deve invalidar a criação/atualização do Schema de Banco de Dados
        /// </summary>
        /// <param name="schemaUpdate"></param>
        public static void CheckSchemaUpdateExceptions(SchemaUpdate schemaUpdate)
        {
            // Fail First
            if (schemaUpdate == null || schemaUpdate.Exceptions.Count == 0) { return; }

            List<string> knownErrors = DatabaseStandardDefinitions.PostgreCommonErrors.Select(reg => reg.Key).ToList();

            foreach (var exception in schemaUpdate.Exceptions)
            {
                Exception innerException = exception;

                // Verifico se a exceção é uma das conhecidas
                bool oneOfTheKnown = knownErrors.Any(reg => innerException.Message.StartsWith(reg));

                if (!oneOfTheKnown) { throw innerException; }

                // Descendo na hierarquia de exceção
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;

                    oneOfTheKnown = knownErrors.Any(reg => innerException.Message.StartsWith(reg));
                    if (!oneOfTheKnown) { throw innerException; }
                }
            }
        }

        public static void CloseAllSessionFactory()
        {
            foreach (ISessionFactory factory in _sessionFactorys.Values)
            {
                if (!factory.IsClosed)
                {
                    factory.Close();
                }
            }
        }

        private static Action<string> CreateDDL(string connectionString)
        {
            const string scriptSeparator = ";";
            string path = string.Format(@"\tmp\ddl-{0}.sql", connectionString.Replace("_", "-").ToLower());

            Action<string> updateExport = x =>
            {
                using (var file = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(file))
                {
                    // Ignorar DDL Views
                    if (!x.TrimStart().ToLower().StartsWith("create table vw_"))
                    {
                        sw.Write(x);
                        sw.WriteLine(scriptSeparator);
                    }
                }
            };
            return updateExport;
        }
    }
}