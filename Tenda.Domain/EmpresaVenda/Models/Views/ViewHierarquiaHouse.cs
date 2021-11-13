using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewHierarquiaHouse : BaseHierarquiaHouse
    {
        public virtual long IdCoordenadorHouse { get; set; }
        public virtual string NomeCoordenadorHouse { get; set; }
        
        public virtual long IdSupervisorHouse { get; set; }
        public virtual string NomeSupervisorHouse { get; set; }
        
        public virtual long IdAgenteVendaHouse { get; set; }
        public virtual string NomeAgenteVendaHouse { get; set; }
        
        public virtual long IdHouse { get; set; }
        public virtual string NomeHouse { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
