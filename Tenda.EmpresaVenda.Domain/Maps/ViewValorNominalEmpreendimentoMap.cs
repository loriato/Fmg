using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewValorNominalEmpreendimentoMap : BaseClassMap<ViewValorNominalEmpreendimento>
    {
        public ViewValorNominalEmpreendimentoMap()
        {
            Table("VW_VALORES_NOMINAIS_EMPREENDIMENTOS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VALOR_NOMINAL");

            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.Divisao).Column("CD_DIVISAO");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA");
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA");
            Map(reg => reg.FaixaUmMeioDe).Column("VL_FAIXA_UM_MEIO_DE");
            Map(reg => reg.FaixaUmMeioAte).Column("VL_FAIXA_UM_MEIO_ATE");
            Map(reg => reg.FaixaDoisDe).Column("VL_FAIXA_DOIS_DE");
            Map(reg => reg.FaixaDoisAte).Column("VL_FAIXA_DOIS_ATE");
            Map(reg => reg.PNEDe).Column("VL_PNE_DE");
            Map(reg => reg.PNEAte).Column("VL_PNE_ATE");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoValorNominal>>();
        }
    }
}
