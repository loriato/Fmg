using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class ItemRegraComissaoDTO
    {
        public long IdRegraComissao { get; set; }
        public long IdEmpreendimento { get; set; }
        public long IdEmpresaVenda { get; set; }
        public double FaixaUmMeio { get; set; }
        public double FaixaDois { get; set; }
        public double ValorKitCompleto { get; set; }
        public double ValorConformidade { get; set; }
        public double ValorRepasse { get; set; }

        public ItemRegraComissaoDTO() { }
        public ItemRegraComissaoDTO(RegraComissao regraComissao, ItemRegraComissao itemRegraComissao)
        {
            IdRegraComissao = regraComissao.Id;
            IdEmpreendimento = itemRegraComissao.Empreendimento.Id;
            IdEmpresaVenda = itemRegraComissao.EmpresaVenda.Id;
            FaixaUmMeio = itemRegraComissao.FaixaUmMeio;
            FaixaDois = itemRegraComissao.FaixaDois;
            ValorKitCompleto = itemRegraComissao.ValorKitCompleto;
            ValorConformidade = itemRegraComissao.ValorConformidade;
            ValorRepasse = itemRegraComissao.ValorRepasse;
        }
    }
}
