using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewResponsavelAceiteRegraComissaoMap : BaseClassMap<ViewResponsavelAceiteRegraComissao>
    {
        public ViewResponsavelAceiteRegraComissaoMap()
        {
            Table("VW_REPONSAVEL_ACEITE_REGRA_COMISSAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("id_reponsavel_aceite_regra_comissao");
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.Inicio).Column("DT_INICIO");
            Map(reg => reg.Termino).Column("DT_TERMINO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();

        }
    }
}
