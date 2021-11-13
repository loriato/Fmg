using Europa.Data;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewDocumentoEmpresaVendaMap : BaseClassMap<ViewDocumentoEmpresaVenda>
    {
        public ViewDocumentoEmpresaVendaMap()
        {
            Table("VW_DOCUMENTOS_EMPRESA_VENDA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_DOCUMENTO_EMPRESA_VENDA");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.NomeArquivo).Column("NM_ARQUIVO");
            Map(reg => reg.IdTipoDocumento).Column("ID_TIPO_DOCUMENTO_EMPRESA_VENDA");
            Map(reg => reg.NomeTipoDocumento).Column("NM_TIPO_DOCUMENTO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO_DOCUMENTO").CustomType<SituacaoAprovacaoDocumento>().Nullable();
            Map(reg => reg.IdResponsavel).Column("ID_USUARIO");
            Map(reg => reg.NomeResponsavel).Column("NM_USUARIO");
            Map(reg => reg.Motivo).Column("DS_MOTIVO");
            Map(reg => reg.IdParecerDocumentoEmpresaVenda).Column("ID_PARECER_DOCUMENTO_EMPRESA_VENDA").Nullable();
            Map(reg => reg.DataCriacaoParecer).Column("DT_PARECER").Nullable();
            Map(reg => reg.Parecer).Column("DS_PARECER").Nullable();
            Map(reg => reg.DataValidadeParecer).Column("DT_VALIDADE").Nullable();

        }
    }
}
