using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroEmpreendimentoDTO
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string[] Estados { get; set; }
        public List<long> IdRegionais { get; set; }
        public List<string> Regionais { get; set; }
        public int DisponibilizaCatalogo { get; set; }
        public int DisponivelVenda { get; set; }

        public TipoModalidadeComissao ModalidadeComissao { get; set; }
        public TipoModalidadeProgramaFidelidade ModalidadeProgramaFidelidade { get; set; }

    }
}
