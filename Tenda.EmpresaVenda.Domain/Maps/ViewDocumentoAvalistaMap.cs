using Europa.Data;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewDocumentoAvalistaMap : BaseClassMap<ViewDocumentoAvalista>
    {
        public ViewDocumentoAvalistaMap()
        {
            Table("VW_DOCUMENTOS_AVALISTA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_DOCUMENTO_AVALISTA");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.IdTipoDocumento).Column("ID_TIPO_DOCUMENTO_AVALISTA");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<SituacaoAprovacaoDocumento>().Nullable();
            Map(reg => reg.NomeAvalista).Column("NM_AVALISTA");
            Map(reg => reg.IdAvalista).Column("ID_AVALISTA");
            Map(reg => reg.NomeTipoDocumento).Column("NM_TIPO_DOCUMENTO");
            Map(reg => reg.Motivo).Column("DS_MOTIVO");
            Map(reg => reg.IdParecerDocumentoProponente).Column("ID_PARECER_DOCUMENTO_AVALISTA").Nullable();
            Map(reg => reg.DataCriacaoParecer).Column("DT_CRIACAO_PARECER").Nullable();
            Map(reg => reg.Parecer).Column("DS_PARECER").Nullable();
            Map(reg => reg.DataValidadeParecer).Column("DT_VALIDADE").Nullable();
            
        }
    }
}
