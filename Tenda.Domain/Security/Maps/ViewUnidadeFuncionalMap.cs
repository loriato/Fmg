using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Maps
{
    public class ViewUnidadeFuncionalMap : BaseClassMap<ViewUnidadeFuncional>
    {
        public ViewUnidadeFuncionalMap()
        {
            Table("VW_UNIDADES_FUNCIONAIS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_UNIDADE_FUNCIONAL");
            Map(reg => reg.Codigo).Column("CD_UNIDADE_FUNCIONAL");
            Map(reg => reg.Nome).Column("NM_UNIDADE_FUNCIONAL");
            Map(reg => reg.Endereco).Column("DS_ENDERECO_ACESSO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.ExibirMenu).Column("FL_EXIBIR_MENU").CustomType<EnumType<ExibirMenu>>();
            Map(reg => reg.IdHierarquiaModulo).Column("ID_HIERARQUIA_MODULO");
            Map(reg => reg.NomeHierarquia).Column("NM_HIERARQUIA");
            Map(reg => reg.CodigoSistema).Column("CD_SISTEMA");

        }
    }
}
