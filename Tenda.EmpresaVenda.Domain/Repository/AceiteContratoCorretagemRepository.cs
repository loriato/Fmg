using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AceiteContratoCorretagemRepository : NHibernateRepository<AceiteContratoCorretagem>
    {
        public bool PossuiContratoAssinado(long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Any();
        }

        public AceiteContratoCorretagem AceiteDaEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable()
              .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
              .OrderByDescending(reg => reg.DataAceite)
              .FirstOrDefault();
        }
        public bool PossuiUltimoContratoAssinado(long idEmpresaVenda, long idContrato)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Where(reg => reg.ContratoCorretagem.Id == idContrato)
                .Any();
        }
    }
}
