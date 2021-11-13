using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRegraComissaoEvsMap : BaseClassMap<ViewRegraComissaoEvs>
    {
        public ViewRegraComissaoEvsMap()
        {
            Table("VW_REGRAS_COMISSAO_EVS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_REGRA_COMISSAO_EVS");

            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_FANTASIA");
            Map(reg => reg.Cnpj).Column("DS_CNPJ");
            Map(reg => reg.Descricao).Column("DS_REGRA_COMISSAO");
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA");
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA");
            Map(reg => reg.DataAceite).Column("DT_ACEITE");
            Map(reg => reg.Aprovador).Column("DS_APROVADOR");
            Map(reg => reg.SituacaoEvs).Column("TP_SITUACAO_RCE").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.SituacaoRc).Column("TP_SITUACAO_RC").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO");
            Map(reg => reg.TipoRegraComissao).Column("TP_REGRA_COMISSAO").CustomType<EnumType<TipoRegraComissao>>();
            Map(reg => reg.Codigo).Column("CD_REGRA_COMISSAO").Nullable();
        }
    }
}
