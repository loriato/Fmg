namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class ContabilizacaoRequisicaoDTO
    {
        public string Numero { get; set; }

        public int NumeroItem { get; set; }

        public double Quantidade { get; set; }

        public string Divisao { get; set; }

        public string CentroCusto { get; set; }

        public string NumeroOrdem { get; set; }

        public string CentroDeLucro { get; set; }

    }
}
