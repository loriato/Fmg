using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class LeadRepository : NHibernateRepository<Lead>
    {
        public bool CheckIfOtherPacoteInLead(string descricao)
        {
            return Queryable().Where(x => x.DescricaoPacote.ToUpper() == descricao.ToUpper()).Any();
        }

        public IQueryable<Lead> ListarAutoCompletePacote(DataSourceRequest request)
        {
            var results = Queryable();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("descricaopacote"))
                    {
                        results = results.Where(x => x.DescricaoPacote.ToLower().Contains(filtro.value.ToLower()));
                    }
                    if (filtro.column.ToLower().Equals("liberado"))
                    {
                        results = results.Where(x => x.Liberar.ToString().Equals(filtro.value));
                    }

                }
            }
            return results
                  .GroupBy(x => x.DescricaoPacote)
                  .Select(x => x.FirstOrDefault())
                  .AsQueryable();
        }

        public List<Lead> BuscarLeadPorPacote(string pacote)
        {
            return Queryable()
                .Where(x => x.DescricaoPacote.ToUpper().Equals(pacote.ToUpper())).ToList();
        }
        public List<Lead> BuscarLeadPorPacote(List<string> pacote)
        {
            return Queryable()
                .Where(x => pacote.Contains(x.DescricaoPacote)).ToList();
        }
        public IQueryable<Lead> BuscarPacoteNaoLiberados()
        {
            var results = Queryable();
            results = results.Where(x => !x.Liberar);

            return results
                  .GroupBy(x => x.DescricaoPacote)
                  .Select(x => x.FirstOrDefault())
                  .AsQueryable();
        }
        public Lead BuscarLeadPorCodigoLead(string codigoLead)
        {
            return Queryable().Where(x => x.CodigoLead == codigoLead).SingleOrDefault();
        }
    }
}
