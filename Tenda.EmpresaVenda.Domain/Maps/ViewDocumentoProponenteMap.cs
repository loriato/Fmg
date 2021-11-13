using Europa.Data;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewDocumentoProponenteMap : BaseClassMap<ViewDocumentoProponente>
    {
        public ViewDocumentoProponenteMap()
        {
            Table("VW_DOCUMENTOS_PROPONENTE");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_DOCUMENTO_PROPONENTE");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.IdTipoDocumento).Column("ID_TIPO_DOCUMENTO");
            Map(reg => reg.NomeTipoDocumento).Column("NM_TIPO_DOCUMENTO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<SituacaoAprovacaoDocumento>().Nullable();
            Map(reg => reg.IdProponente).Column("ID_PROPONENTE");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.Motivo).Column("DS_MOTIVO");
            Map(reg => reg.DataExpiracao).Column("DT_EXPIRACAO");
            Map(reg => reg.IdParecerDocumentoProponente).Column("ID_PARECER_DOCUMENTO_PROPONENTE").Nullable();
            Map(reg => reg.DataCriacaoParecer).Column("DT_CRIACAO_PARECER").Nullable();
            Map(reg => reg.Parecer).Column("DS_PARECER").Nullable();
            Map(reg => reg.DataValidadeParecer).Column("DT_VALIDADE").Nullable();
        }
    }
}
