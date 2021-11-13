using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPropostaFaturadaMap:BaseClassMap<ViewPropostaFaturada>
    {
        public ViewPropostaFaturadaMap()
        {
            Table("VW_PROPOSTA_FATURADA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_PROPOSTA");

            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();

            Map(reg => reg.IdLoja).Column("ID_LOJA");
            Map(reg => reg.NomeLoja).Column("NM_LOJA");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");

            Map(reg => reg.Regional).Column("DS_REGIONAL");
        }
    }
}
