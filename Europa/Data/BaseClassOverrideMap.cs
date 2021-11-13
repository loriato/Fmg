using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Europa.Data
{
    public abstract class BaseClassOverrideMap<BaseEntity> : IAutoMappingOverride<BaseEntity>
    {
        public abstract void Override(AutoMapping<BaseEntity> mapping);
    }
}
