using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class RequisicaoCompraDTO
    {
        public ContabilizacaoRequisicaoDTO ContabilizacaoRequisicao { get; set; }
        public ItemRequisicaoDTO ItemRequisicao { get; set; }
        public ViewPagamento Pagamento { get; set; }
    }
}
