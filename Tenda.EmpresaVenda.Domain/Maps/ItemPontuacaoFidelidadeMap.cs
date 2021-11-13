using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ItemPontuacaoFidelidadeMap : BaseClassMap<ItemPontuacaoFidelidade>
    {
        public ItemPontuacaoFidelidadeMap()
        {
            Table("TBL_ITENS_PONTUACAO_FIDELIDADE");

            Id(reg => reg.Id).Column("ID_ITEM_PONTUACAO_FIDELIDADE").GeneratedBy.Sequence("SEQ_ITENS_PONTUACAO_FIDELIDADE");

            Map(reg => reg.PontuacaoFaixaUmMeio).Column("VL_PONTUACAO_UM_MEIO").Not.Nullable();
            Map(reg => reg.PontuacaoFaixaDois).Column("VL_PONTUACAO_DOIS").Not.Nullable();
            Map(reg => reg.PontuacaoPNE).Column("VL_PONTUACAO_PNE").Not.Nullable();

            Map(reg => reg.PontuacaoFaixaUmMeioSeca).Column("VL_PONTUACAO_UM_MEIO_SECA").Not.Nullable();
            Map(reg => reg.PontuacaoFaixaUmMeioNormal).Column("VL_PONTUACAO_UM_MEIO_NORMAL").Not.Nullable();
            Map(reg => reg.PontuacaoFaixaUmMeioTurbinada).Column("VL_PONTUACAO_UM_MEIO_TURBINADA").Not.Nullable();

            Map(reg => reg.PontuacaoFaixaDoisSeca).Column("VL_PONTUACAO_DOIS_SECA").Not.Nullable();
            Map(reg => reg.PontuacaoFaixaDoisNormal).Column("VL_PONTUACAO_DOIS_NORMAL").Not.Nullable();
            Map(reg => reg.PontuacaoFaixaDoisTurbinada).Column("VL_PONTUACAO_DOIS_TURBINADA").Not.Nullable();

            Map(reg => reg.PontuacaoPNESeca).Column("VL_PONTUACAO_PNE_SECA").Not.Nullable();
            Map(reg => reg.PontuacaoPNENormal).Column("VL_PONTUACAO_PNE_NORMAL").Not.Nullable();
            Map(reg => reg.PontuacaoPNETurbinada).Column("VL_PONTUACAO_PNE_TURBINADA").Not.Nullable();

            Map(reg => reg.FatorUmMeio).Column("VL_FATOR_UM_MEIO").Not.Nullable();
            Map(reg => reg.PontuacaoPadraoUmMeio).Column("VL_PONTUACAO_PADRAO_UM_MEIO").Not.Nullable();

            Map(reg => reg.FatorDois).Column("VL_FATOR_DOIS").Not.Nullable();
            Map(reg => reg.PontuacaoPadraoDois).Column("VL_PONTUACAO_PADRAO_DOIS").Not.Nullable();

            Map(reg => reg.FatorUmMeioSeca).Column("VL_FATOR_UM_MEIO_SECA").Not.Nullable();
            Map(reg => reg.FatorUmMeioNormal).Column("VL_FATOR_UM_MEIO_NORMAL").Not.Nullable();            
            Map(reg => reg.FatorUmMeioTurbinada).Column("VL_FATOR_UM_MEIO_TURBINADA").Not.Nullable();
            
            Map(reg => reg.FatorDoisSeca).Column("VL_FATOR_DOIS_SECA").Not.Nullable();
            Map(reg => reg.FatorDoisNormal).Column("VL_FATOR_DOIS_NORMAL").Not.Nullable();
            Map(reg => reg.FatorDoisTurbinada).Column("VL_FATOR_DOIS_TURBINADA").Not.Nullable();
            
            Map(reg => reg.FatorPNESeca).Column("VL_FATOR_PNE_SECA").Not.Nullable();
            Map(reg => reg.FatorPNENormal).Column("VL_FATOR_PNE_NORMAL").Not.Nullable();
            Map(reg => reg.FatorPNETurbinada).Column("VL_FATOR_PNE_TURBINADA").Not.Nullable();
            
            Map(reg => reg.QuantidadeMinima).Column("NR_QTD_MINIMA").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoPontuacaoFidelidade>>().Not.Nullable();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.Codigo).Column("DS_CODIGO").Nullable();

            Map(reg => reg.Modalidade).Column("TP_MODALIDADE_PROGRAMA_FIDELIDADE").CustomType<EnumType<TipoModalidadeProgramaFidelidade>>().Nullable();

            References(reg => reg.PontuacaoFidelidade).Column("ID_PONTUACAO_FIDELIDADE").ForeignKey("FK_ITEM_PONTUACAO_FIDELIDADE_X_PONTUACAO_FIDELIDADE_01");
            References(reg => reg.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_ITEM_PONTUACAO_FIDELIDADE_X_EMPREENDIMENTO_01");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_ITEM_PONTUACAO_FIDELIDADE_X_EMPRESA_VENDA_01");

        }
    }
}
