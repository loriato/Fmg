using Europa.Commons;
using NHibernate;

namespace Europa.Data.Interceptors
{
    public class SqlStatementInterceptor : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin(sql.ToString());
            return sql;
        }
    }
}
