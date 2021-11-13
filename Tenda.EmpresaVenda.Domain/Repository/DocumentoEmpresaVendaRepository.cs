using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class DocumentoEmpresaVendaRepository : NHibernateRepository<DocumentoEmpresaVenda>
    {
        public IQueryable<DocumentoEmpresaVenda> FindByIdEmpresaVenda(long idEv)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEv);
        }
        public bool EvTemProcuracao(long IdEmpresaVenda)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == IdEmpresaVenda && x.TipoDocumento.Nome == "Procuração").Any();
        }
        public bool EvTemMaisDeUmaProcuracao(long IdEmpresaVenda)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == IdEmpresaVenda && x.TipoDocumento.Nome == "Procuração").Count() > 1;
        }
    }
}
