using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroValorNominalEmpreendimentoDTO
    {
        public long IdEmpreendimento { get; set; }
        public DateTime? VigenteEm { get; set; }
        public List<string> Estados { get; set; }
        public SituacaoValorNominal Situacao { get; set; }

        public string Nome { get; set; }
        public string Estado { get; set; }
    }
}