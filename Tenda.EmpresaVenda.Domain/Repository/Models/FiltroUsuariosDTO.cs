using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroUsuariosDTO
    {
        public virtual string nameOrEmail { get; set; }
        public virtual string[] tipos { get; set; }
        public virtual string name { get; set; }
        public virtual string login { get; set; }
        public virtual string email { get; set; }
        public virtual long? perfil { get; set; }
        public virtual long idSistema { get; set; }
    }
}
