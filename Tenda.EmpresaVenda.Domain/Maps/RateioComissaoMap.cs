using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RateioComissaoMap : BaseClassMap<RateioComissao>
    {
        public RateioComissaoMap()
        {
            Table("TBL_RATEIOS_COMISSAO");

            Id(reg => reg.Id).Column("ID_RATEIO_COMISSAO").GeneratedBy.Sequence("SEQ_RATEIOS_COMISSAO");

            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRateioComissao>>();
            References(reg => reg.Contratante).Column("ID_EMPRESA_VENDA_CONTRATANTE").ForeignKey("FK_RATEIO_X_EMPRESA_VENDA_01");
            References(reg => reg.Contratada).Column("ID_EMPRESA_VENDA_CONTRATADA").ForeignKey("FK_RATEIO_X_EMPRESA_VENDA_02");
            References(reg => reg.Empreendimento).Column("ID_EMPREENDIMENTO").ForeignKey("FK_RATEIO_X_EMPREENDIMENTO_01").Nullable();
        }
    }
}
