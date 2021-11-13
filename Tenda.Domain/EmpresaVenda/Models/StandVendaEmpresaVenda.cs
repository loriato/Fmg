using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class StandVendaEmpresaVenda : BaseEntity
    {
        public virtual PontoVenda PontoVenda { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual StandVenda StandVenda { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
