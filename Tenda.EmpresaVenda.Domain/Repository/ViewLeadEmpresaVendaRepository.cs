using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewLeadEmpresaVendaRepository : NHibernateRepository<ViewLeadEmpresaVenda>
    {
        public DataSourceResponse<ViewLeadEmpresaVenda> ListarDatatable(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            var query = Queryable();

            if (!filtro.IdEmpresaVenda.IsEmpty())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (!filtro.IdCorretor.IsEmpty())
            {
                query = query.Where(x => x.IdCorretor == filtro.IdCorretor);
            }

            if (!filtro.Pacote.IsEmpty())
            {
                query = query.Where(x => x.Pacote.Equals(filtro.Pacote));
            }

            if (!filtro.SituacaoLead.IsEmpty())
            {
                query = query.Where(x => x.SituacaoLead == filtro.SituacaoLead);
            }
            if (filtro.Liberar.HasValue())
            {
                query = query.Where(x => x.Liberar == filtro.Liberar);
            }
            if (filtro.CodigoPreProposta.HasValue())
            {
                query = query.Where(x => x.CodigoPreProposta.ToUpper().Contains(filtro.CodigoPreProposta.ToUpper()));
            }
            if (filtro.NomeLead.HasValue())
            {
                query = query.Where(x => x.NomeLead.ToLower().Contains(filtro.NomeLead.ToLower()));
            }
            if (filtro.Telefone.HasValue())
            {
                query = query.Where(x => x.Telefone1.Contains(filtro.Telefone.OnlyNumber()) || x.Telefone2.Contains(filtro.Telefone.OnlyNumber()));
            }
            if(filtro.DataIndicacaoAte.HasValue() && filtro.DataIndicacaoAte.HasValue())
            {
                query = query.Where(x => x.CriadoEm.Date <= filtro.DataIndicacaoAte && x.CriadoEm.Date >= filtro.DataIndicacaoDe);
            }

            return query.ToDataRequest(request);
        }

        public IQueryable<ViewLeadEmpresaVenda> ListarAutoCompletePacoteDiretor(DataSourceRequest request, long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.IdEmpresaVenda == idEmpresaVenda)
                .Where(x => x.Liberar == true)
                .GroupBy(x => x.Pacote)
                .Select(x => x.FirstOrDefault())
                .AsQueryable();
        }

        public IQueryable<ViewLeadEmpresaVenda> ListarAutoCompletePacoteCorretor(DataSourceRequest request, long idCorretor)
        {
            return Queryable()
                .Where(x => x.IdCorretor == idCorretor)
                .Where(x => x.Liberar == true)
                .GroupBy(x => x.Pacote)
                .Select(x => x.FirstOrDefault())
                .AsQueryable();
        }
    }
}
