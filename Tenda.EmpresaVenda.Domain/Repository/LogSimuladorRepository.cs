using Europa.Data;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class LogSimuladorRepository:NHibernateRepository<LogSimulador>
    {
        public void IncluirSeNaoExistir(UsuarioPortal usuario,string codigoSistema)
        {
            var exists = Queryable().Any(x => x.Usuario.Id == usuario.Id);

            if (!exists)
            {
                var log = new LogSimulador();
                log.Usuario = new UsuarioPortal { Id = usuario.Id };
                log.CodigoSistema = codigoSistema;
                Save(log);
            }
        }
    }
}
