using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRequisicaoCompraSapMap : BaseClassMap<ViewRequisicaoCompraSap>
    {
        public ViewRequisicaoCompraSapMap()
        {
            Table("VW_REQUISICAO_COMPRAS_SAP");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_REQUISICAO_COMPRA_SAP");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA");
            Map(reg => reg.Numero).Column("NR_REQUISICAO_COMPRA");
            Map(reg => reg.Texto).Column("DS_TEXTO");
            Map(reg => reg.Status).Column("DS_STATUS");
            Map(reg => reg.Proposta).Column("CD_PROPOSTA");
            Map(reg => reg.EmpresaVenda).Column("NM_FANTASIA");
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>();
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");

        }
    }
}
