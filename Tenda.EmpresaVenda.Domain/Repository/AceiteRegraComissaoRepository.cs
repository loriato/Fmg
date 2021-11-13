using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AceiteRegraComissaoRepository : NHibernateRepository<AceiteRegraComissao>
    {

        public EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public bool BuscarAceiteParaRegraAndEmpresaVenda(long idRegraComissao, long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.RegraComissao.Id == idRegraComissao)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Any();
        }

        public AceiteRegraComissao AceiteMaisRecente(long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .OrderByDescending(reg => reg.DataAceite)
                .FirstOrDefault();
        }

        public IQueryable<AceiteRegraComissao> ListarTodosAceites(long idEmpresaVenda)
        {
            return Queryable()
               .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
               .OrderByDescending(reg => reg.DataAceite);
        }
    }
}
