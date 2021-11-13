using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    class ViewFuncionalidadeMap : BaseClassMap<ViewFuncionalidade>
    {
        public ViewFuncionalidadeMap()
        {
            Table("VW_FUNCIONALIDADES");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_FUNCIONALIDADE");
            Map(reg => reg.NomeFuncionalidade).Column("NM_FUNCIONALIDADE");
            Map(reg => reg.TipoCrud).Column("TP_CRUD").CustomType<EnumType<TipoCrud>>();
            Map(reg => reg.Comando).Column("DS_COMANDO");
            Map(reg => reg.IdUnidadeFuncional).Column("ID_UNIDADE_FUNCIONAL");
            Map(reg => reg.NomeUnidadeFuncional).Column("NM_UNIDADE_FUNCIONAL");
            Map(reg => reg.CodigoUnidadeFuncional).Column("CD_UNIDADE_FUNCIONAL");
            Map(reg => reg.IdSistema).Column("ID_SISTEMA");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
            Map(reg => reg.Logar).Column("FL_LOGAR");
        }
    }
}
