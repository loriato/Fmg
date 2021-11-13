using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EstadoAvalista : BaseEntity
    {
        public virtual string NomeEstado { get; set; }
        public virtual UsuarioPortal Avalista { get; set; }
        public override string ChaveCandidata()
        {
            return Avalista.Nome;
        }
    }
}
