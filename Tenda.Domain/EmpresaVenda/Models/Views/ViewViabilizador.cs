using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewViabilizador : BaseEntity
    {
        public virtual long IdSupervisor { get; set; }
        public virtual string NomeSupervisor { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public virtual long IdViabilizador { get; set; }
        public virtual long IdSupervisorViabilizador { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            return NomeViabilizador;
        }
    }
}
