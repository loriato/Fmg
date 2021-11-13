using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroEmailDTO
    {
        public List<SituacaoEnvioFila> Situacoes { get; set; }
        public DateTime? PeriodoDe { get; set; }
        public DateTime? PeriodoAte { get; set; }
        public string Destinatario { get; set; }
    }
}
