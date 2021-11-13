using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRateioComissaoMap : BaseClassMap<ViewRateioComissao>
    {
        public ViewRateioComissaoMap()
        {
            Table("VW_RATEIO_COMISSAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_RATEIO_COMISSAO");
            Map(reg => reg.IdContratada).Column("ID_EMPRESA_VENDA_CONTRATADA");
            Map(reg => reg.IdContratante).Column("ID_EMPRESA_VENDA_CONTRATANTE");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.NomeContratada).Column("NM_FANTASIA_CONTRATADA");
            Map(reg => reg.NomeContratante).Column("NM_FANTASIA_CONTRATANTE");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA");
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRateioComissao>>();

        }
    }
}
