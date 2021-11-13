using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.RegraComissao;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewResponsavelAceiteRegraComissaoRepository : NHibernateRepository<ViewResponsavelAceiteRegraComissao>
    {
        public DataSourceResponse<ViewResponsavelAceiteRegraComissao> Listar(RegraComissaoDto filtro)
        {
            return Queryable().Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda).ToDataRequest(filtro.DataSourceRequest);
        }
        public bool PodeAceitarRegraComissao(long idEmpresaVenda, long idCorretor)
        {
            return Queryable().Where(x => x.IdEmpresaVenda == idEmpresaVenda && x.IdCorretor == idCorretor).Any();
        }
    }
}
