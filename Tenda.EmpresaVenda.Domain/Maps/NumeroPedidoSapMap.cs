using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class NumeroPedidoSapMap : BaseClassMap<NumeroPedidoSap>
    {
        public NumeroPedidoSapMap()
        {
            Table("TBL_NUMEROS_PEDIDOS_SAP");

            Id(reg => reg.Id).Column("ID_NUMERO_PEDIDO_SAP").GeneratedBy.Sequence("SEQ_TBL_NUMEROS_PEDIDOS_SAP");
            Map(reg => reg.Mandante).Column("DS_MANDANTE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.NumeroRequisicaoCompra).Column("NR_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.NumeroItemRequisicaoCompra).Column("NR_ITEM_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.LinhaTexto).Column("DS_LINHA_TEXTO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.NumeroDocumentoCompra).Column("NR_DOCUMENTO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.NumeroItemDocumentoCompra).Column("NR_ITEM_DOCUMENTO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.Data).Column("DT_NUMERO_PEDIDO").Nullable();
            Map(reg => reg.CodigoLiberacaoDocumentoCompra).Column("CD_LIBERACAO_DOCUMENTO_COMPRA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.Status).Column("DS_STATUS").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();

        }
    }
}
