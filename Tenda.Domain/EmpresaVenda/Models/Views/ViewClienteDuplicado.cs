using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewClienteDuplicado : BaseEntity
    {
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string CPF { get; set; }

        public virtual long IdViabilizador { get; set; }
        public virtual string NomeViabilizador { get; set; }

        public virtual long IdSupervisor { get; set; }
        public virtual string NomeSupervisor { get; set; }

        public virtual long IdCoordenadorSupervisor { get; set; }
        public virtual string NomeCoordenadorSupervisor { get; set; }

        public virtual long IdCoordenadorViabilizador { get; set; }
        public virtual string NomeCoordenadorViabilizador { get; set; }

        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }

        public override string ChaveCandidata()
        {
            return NomeCliente;
        }
    }
}
