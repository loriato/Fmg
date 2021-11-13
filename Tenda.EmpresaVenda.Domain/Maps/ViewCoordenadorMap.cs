using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCoordenadorMap:BaseClassMap<ViewCoordenador>
    {
        public ViewCoordenadorMap()
        {
            Table("VW_COORDENADOR");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_COORDENADOR");

            Map(reg => reg.NomeCoordenador).Column("NM_COORDENADOR");
            Map(reg => reg.SituacaoCoordenador).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.TipoHierarquiaCicloFinanceiro).Column("TP_HIERARQUIA_CICLO_FINANCEIRO").CustomType<EnumType<TipoHierarquiaCicloFinanceiro>>();
        }
    }
}
