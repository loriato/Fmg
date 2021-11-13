using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCoordenadorHouse : BaseEntity
    {
        public virtual long IdCoordenadorHouse { get; set; }
        public virtual string NomeCoordenadorHouse { get; set; }
        public virtual string LoginCoordenadorHouse { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
