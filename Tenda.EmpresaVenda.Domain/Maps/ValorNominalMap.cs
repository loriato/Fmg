using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ValorNominalMap : BaseClassMap<ValorNominal>
    {
        public ValorNominalMap()
        {
            Table("TBL_VALORES_NOMINAIS");

            Id(reg => reg.Id).Column("ID_VALOR_NOMINAL").GeneratedBy.Sequence("SEQ_VALORES_NOMINAIS");
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.FaixaUmMeioDe).Column("VL_FAIXA_UM_MEIO_DE");
            Map(reg => reg.FaixaUmMeioAte).Column("VL_FAIXA_UM_MEIO_ATE");
            Map(reg => reg.FaixaDoisDe).Column("VL_FAIXA_DOIS_DE");
            Map(reg => reg.FaixaDoisAte).Column("VL_FAIXA_DOIS_ATE");
            Map(reg => reg.PNEDe).Column("VL_PNE_DE");
            Map(reg => reg.PNEAte).Column("VL_PNE_ATE");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoValorNominal>>();

            References(reg => reg.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_VALOR_NOMINAL_X_EMPREENDIMENTO_01");
        }
    }
}
