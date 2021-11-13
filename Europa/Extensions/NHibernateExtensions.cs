using NHibernate;

namespace Europa.Extensions
{
    public static class NHibernateExtensions
    {
        private const string NLS_COMP_BINARY = "ALTER SESSION SET NLS_COMP = BINARY";
        private const string NLS_SORT_BINARY = "ALTER SESSION SET NLS_SORT = BINARY";

        private const string NLS_COMP_LINGUISTIC = "ALTER SESSION SET NLS_COMP = LINGUISTIC";
        private const string NLS_SORT_WEST_EUROPEAN_AI = "ALTER SESSION SET NLS_SORT = WEST_EUROPEAN_AI";

        /// <summary>
        /// Execute SQL to alter session defining:
        ///  - NLS_COMP = BINARY
        ///  - NLS_SORT = BINARY
        /// </summary>
        /// <param name="session"></param>
        public static void AlterSessionToBinary(this ISession session)
        {
            session.CreateSQLQuery(NLS_COMP_BINARY).UniqueResult();
            session.CreateSQLQuery(NLS_SORT_BINARY).UniqueResult();
        }

        /// <summary>
        /// Execute SQL to alter stateless session defining:
        ///  - NLS_COMP = BINARY
        ///  - NLS_SORT = BINARY
        /// </summary>
        /// <param name="session"></param>
        public static void AlterStatelessSessionToBinary(this IStatelessSession session)
        {
            session.CreateSQLQuery(NLS_COMP_BINARY).UniqueResult();
            session.CreateSQLQuery(NLS_SORT_BINARY).UniqueResult();
        }

        /// <summary>
        /// Execute SQL to alter session defining:
        ///  - NLS_COMP = LINGUISTIC
        ///  - NLS_SORT = WEST_EUROPEAN_AI
        /// </summary>
        /// <param name="session"></param>
        public static void AlterSessionToDefault(this ISession session)
        {
            session.CreateSQLQuery(NLS_COMP_LINGUISTIC).UniqueResult();
            session.CreateSQLQuery(NLS_SORT_WEST_EUROPEAN_AI).UniqueResult();
        }

        public static void CloseIfOpen(this ISession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        public static void CloseIfOpen(this IStatelessSession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        public static void RollbackIfActive(this ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
            }
        }

        public static void CommitIfActive(this ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
        }
    }
}
