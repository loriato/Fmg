using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ItemRegraComissaoPadraoMap : BaseClassMap<ItemRegraComissaoPadrao>
    {
        public ItemRegraComissaoPadraoMap()
        {
            Table("TBL_ITENS_REGRA_COMISSAO_PADRAO");

            Id(reg => reg.Id).Column("ID_ITEM_REGRA_COMISSAO_PADRAO").GeneratedBy.Sequence("SEQ_ITENS_REGRA_COMISSAO_PADRAO");
            Map(reg => reg.FaixaUmMeio).Column("VL_FAIXA_UM_MEIO");
            Map(reg => reg.FaixaDois).Column("VL_FAIXA_DOIS");
            Map(reg => reg.ValorConformidade).Column("VL_CONFORMIDADE");
            Map(reg => reg.ValorKitCompleto).Column("VL_KIT_COMPLETO");
            Map(reg => reg.ValorRepasse).Column("VL_REPASSE");

            Map(reg => reg.MenorValorNominalUmMeio).Column("VL_MENOR_NOMINAL_UM_MEIO");
            Map(reg => reg.IgualValorNominalUmMeio).Column("VL_IGUAL_NOMINAL_UM_MEIO");
            Map(reg => reg.MaiorValorNominalUmMeio).Column("VL_MAIOR_NOMINAL_UM_MEIO");

            Map(reg => reg.MenorValorNominalDois).Column("VL_MENOR_NOMINAL_DOIS");
            Map(reg => reg.IgualValorNominalDois).Column("VL_IGUAL_NOMINAL_DOIS");
            Map(reg => reg.MaiorValorNominalDois).Column("VL_MAIOR_NOMINAL_DOIS");

            Map(reg => reg.MenorValorNominalPNE).Column("VL_MENOR_NOMINAL_PNE");
            Map(reg => reg.IgualValorNominalPNE).Column("VL_IGUAL_NOMINAL_PNE");
            Map(reg => reg.MaiorValorNominalPNE).Column("VL_MAIOR_NOMINAL_PNE");

            Map(reg => reg.Modalidade).Column("TP_MODALIDADE_COMISSAO").CustomType<EnumType<TipoModalidadeComissao>>();

            References(x => x.RegraComissaoPadrao).Column("ID_REGRA_COMISSAO_PADRAO").ForeignKey("FK_ITEM_REGRA_P_X_REGRA_COMISSAO_P_01");
            References(x => x.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_ITEM_REGRA_PADRAO_X_EMPREENDIMENTO_01");
        }
    }
}
