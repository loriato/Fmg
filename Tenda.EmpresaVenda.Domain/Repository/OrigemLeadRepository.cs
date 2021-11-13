using Europa.Data;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class OrigemLeadRepository : NHibernateRepository<OrigemLead>
    {
        public OrigemLead OrigemLeadValido(long idUsuario)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.Situacao == Situacao.Ativo)
                .SingleOrDefault();
        }
    }
}
