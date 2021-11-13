using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class NotaFiscalPagamentoMap : BaseClassMap<NotaFiscalPagamento>
    {
        public NotaFiscalPagamentoMap()
        {
            Table("TBL_NOTA_FISCAL_PAGAMENTOS");

            Id(x => x.Id).Column("ID_NOTA_FISCAL_PAGAMENTO").GeneratedBy.Sequence("SEQ_NOTA_FISCAL_PAGAMENTOS");

            Map(x => x.NotaFiscal).Column("DS_NOTA_FISCAL");
            Map(x => x.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoNotaFiscal>>();
            Map(x => x.Motivo).Column("DS_MOTIVO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(x => x.DataPedido).Column("DT_PEDIDO").Nullable();
            Map(x => x.DataEnviado).Column("DT_ENVIADO").Nullable();
            Map(x => x.DataRecebido).Column("DT_RECEBIDO").Nullable();
            Map(x => x.DataAprovado).Column("DT_APROVADO").Nullable();
            Map(x => x.DataReprovado).Column("DT_REPROVADO").Nullable();
            Map(x => x.RevisaoNF).Column("NR_ENVIO_NOTA_FISCAL").Nullable();

            Map(reg => reg.Chave).Column("DS_CHAVE").Nullable();

            References(x => x.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_NOTA_FISCAL_X_ARQUIVO_01");
        }
    }
}
