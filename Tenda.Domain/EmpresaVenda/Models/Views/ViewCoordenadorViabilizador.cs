using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCoordenadorViabilizador : BaseEntity
    {
        public virtual long IdViabilizador { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public virtual string NomeCoordenador { get; set; }
        public virtual long IdCoordenador { get; set; }
        public virtual long IdCoordenadorViabilizador { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            return NomeViabilizador;
        }
    }
}
