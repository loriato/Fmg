using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCoordenadorSupervisorHouse:BaseEntity
    {
        public virtual long IdSupervisorHouse { get; set; }
        public virtual string NomeSupervisorHouse { get; set; }
        public virtual string LoginSupervisorHouse { get; set; }
        public virtual long IdCoordenadorSupervisorHouse { get; set; }
        public virtual long IdCoordenadorHouse { get; set; }
        public virtual string NomeCoordenadorHouse { get; set; }
        public virtual string LoginCoordenadorHouse { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
