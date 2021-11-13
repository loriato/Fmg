using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewSupervisorAgenteVendaHouse : BaseEntity
    {
        public virtual long IdUsuarioAgenteVenda { get; set; }
        public virtual string NomeUsuarioAgenteVenda { get; set; }
        public virtual string EmailUsuarioAgenteVenda { get; set; }
        public virtual string LoginUsuarioAgenteVenda { get; set; }
        public virtual long IdAgenteVenda { get; set; }

        public virtual long IdSupervisorHouse { get; set; }
        public virtual string NomeSupervisorHouse { get; set; }
        public virtual string LoginSupervisorHouse { get; set; }

        public virtual long IdCoordenadorHouse { get; set; }

        public virtual long IdHouse { get; set; }
        public virtual string NomeHouse { get; set; }
        public virtual string RegionalHouse { get; set; }

        public virtual long IdSupervisorAgenteVendaHouse { get; set; }        
        public virtual long IdAgenteVendaHouse { get; set; }
        public virtual bool Ativo { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
