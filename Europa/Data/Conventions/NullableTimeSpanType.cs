using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Data;
using System.Data.Common;

namespace Europa.Data.Conventions
{
    /// <summary>
    /// Mapeia um tipo <see cref="System.TimeSpan?" /> para um tipo <see cref="DbType.TimeSpan" /> column 
    /// </summary>
    [Serializable]
    public partial class NullableTimeSpanType : TimeAsTimeSpanType
    {
        public override void Set(DbCommand st, object value, int index, ISessionImplementor session)
        {
            var obj = (TimeSpan?)value;
            if (obj.HasValue)
            {
                ((IDbDataParameter)st.Parameters[index]).Value = new TimeSpan(0, obj.Value.Hours, obj.Value.Minutes, obj.Value.Seconds);
            } else
            {
                ((IDbDataParameter)st.Parameters[index]).Value = DBNull.Value;
            }
        }
    }
}