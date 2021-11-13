using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EstadoCidadeRepository : NHibernateRepository<EstadoCidade>
    {
        public List<ViewEstado> Estados()
        {
            var estados = new List<ViewEstado>();
            var id = 1;

            foreach(var uf in EstadosSelectList.Values)
            {
                estados.Add(new ViewEstado { Id=id,Estado=uf.Value});
                id++;
            }

            return estados;
        }
        public DataSourceResponse<ViewEstado> ListarEstados(DataSourceRequest request,EstadoCidadeDTO filtro)
        {
            var query = Estados().AsQueryable();

            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => x.Estado.Contains(filtro.Estado));
            }

            return query.ToDataRequest(request);
        }
        public DataSourceResponse<EstadoCidade> ListarCidades(DataSourceRequest request,EstadoCidadeDTO filtro)
        {
            var query = Queryable();

            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => x.Estado.Equals(filtro.Estado));
            }

            if (filtro.Cidade.HasValue())
            {
                query = query.Where(x => x.Cidade.ToLower().Contains(filtro.Cidade.ToLower()));
            }

            return query.ToDataRequest(request);

        }

        public List<EstadoCidade> FindByEstado(string estado)
        {
            var query = Queryable();

            if (estado.HasValue())
            {
                query = query.Where(x => x.Estado.Equals(estado));
            }

            return query.OrderBy(x=>x.Id).ToList();
        }
    }
}
