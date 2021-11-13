using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class LeadEmpresaVenda : BaseEntity
    {
        public virtual Corretor Corretor { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual Lead Lead { get; set; }
        public virtual SituacaoLead Situacao { get; set; }
        public virtual TipoDesistencia Desistencia { get; set; }
        public virtual string DescricaoDesistencia { get; set; }
        public virtual string Anotacoes { get; set; }
        public virtual PreProposta PreProposta { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
