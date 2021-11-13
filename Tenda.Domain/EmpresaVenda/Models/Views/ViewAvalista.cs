using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewAvalista:BaseEntity
    {
        public virtual string NomeAvalista { get; set; }
        public virtual string NomeEstado { get; set; }
        public virtual Situacao AvalistaSituacao { get; set;}
        public virtual long IdPerfil { get; set; }
        public virtual long IdAvalista { get; set; }
        public virtual long IdEstadoAvalista { get; set; }
        public virtual bool Ativo { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
