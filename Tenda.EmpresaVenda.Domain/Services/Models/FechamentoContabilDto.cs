using System;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FechamentoContabilDto
    {
        public virtual long IdFechamento { get; set; }
        public virtual DateTime InicioFechamento { get; set; }
        public virtual DateTime TerminoFechamento { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int QuantidadeDiasLembrete { get; set; }
        public bool EmFechamentoContabil { get; set; }
    }
}
