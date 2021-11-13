using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRegraComissaoMap : BaseClassMap<ViewRegraComissao>
    {
        public ViewRegraComissaoMap()
        {
            Table("VW_REGRAS_COMISSAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_REGRA_COMISSAO");
            Map(reg => reg.Descricao).Column("DS_REGRA_COMISSAO");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA");
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
        }
    }
}
