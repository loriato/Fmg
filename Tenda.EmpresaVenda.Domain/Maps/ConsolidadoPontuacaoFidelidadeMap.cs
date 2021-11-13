using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ConsolidadoPontuacaoFidelidadeMap : BaseClassMap<ConsolidadoPontuacaoFidelidade>
    {
        public ConsolidadoPontuacaoFidelidadeMap()
        {
            Table("TBL_CONSOLIDADO_PONTUACAO_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_CONSOLIDADO_PONTUACAO_FIDELIDADE").GeneratedBy.Sequence("SEQ_CONSOLIDADO_PONTUACAO_FIDELIDADE");

            Map(reg => reg.IdProposta).Column("ID_PROPOSTA").Not.Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Not.Nullable();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Not.Nullable();
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA").Nullable();
            Map(reg => reg.IdItemPontuacaoFidelidade).Column("ID_ITEM_PONTUACAO_FIDELIDADE").Not.Nullable();
            Map(reg => reg.Pontuacao).Column("VL_PONTUACAO").Not.Nullable();
            Map(reg => reg.DataPontuacao).Column("DT_PONTUACAO").Not.Nullable();
            Map(reg => reg.SituacaoPontuacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoPontuacao>>().Not.Nullable();
            Map(reg => reg.Faturado).Column("FL_FATURADO").Not.Nullable();
            Map(reg => reg.IdPontuacaoFidelidade).Column("ID_PONTUACAO_FIDELIDADE").Nullable();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>();
            Map(reg => reg.IdValorNominal).Column("ID_VL_NOMINAL").Nullable(); 
        }
    }
}
