using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Data;
using System.Data.Common;

namespace Europa.Data.Conventions
{
    /// <summary>
    /// Maps a <see cref="System.TimeSpan" /> Property to an <see cref="DbType.TimeSpan" /> column 
    /// </summary>
    [Serializable]
    public partial class TimeSpanType : TimeAsTimeSpanType
    {
        public override void Set(DbCommand st, object value, int index, ISessionImplementor session)
        {
            var obj = (TimeSpan)value;
            ((IDbDataParameter)st.Parameters[index]).Value = new TimeSpan(0, obj.Hours, obj.Minutes, obj.Seconds);
        }
    }
}