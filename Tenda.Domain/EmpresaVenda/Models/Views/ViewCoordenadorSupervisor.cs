using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCoordenadorSupervisor : BaseEntity
    {
        public virtual long IdSupervisor { get; set; }
        public virtual string NomeSupervisor { get; set; }
        public virtual string NomeCoordenador { get; set; }
        public virtual long IdCoordenador { get; set; }
        public virtual long IdCoordenadorSupervisor { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            return NomeSupervisor;
        }
    }
}
