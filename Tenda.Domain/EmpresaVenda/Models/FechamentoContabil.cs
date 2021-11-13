using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class FechamentoContabil:BaseEntity
    {
        public virtual DateTime InicioFechamento { get; set; }
        public virtual DateTime TerminoFechamento { get; set; }
        public virtual int QuantidadeDiasLembrete { get; set; }
        public virtual string Descricao { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
