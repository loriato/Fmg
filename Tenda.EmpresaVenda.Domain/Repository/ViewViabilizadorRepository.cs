using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewViabilizadorRepository : NHibernateRepository<ViewViabilizador>
    {
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        public DataSourceResponse<ViewViabilizador> Listar(DataSourceRequest request ,SupervisorViabilizadorDTO filtro)
        {
            //lista dos viabilizadores que já possuem um supervisor
            //var viabilizadoresInvalidos = IdViabilizadoresInvalidos(filtro.IdSupervisor);

            var query = Queryable().Where(x => x.IdSupervisor == filtro.IdSupervisor);
            
            if (filtro.NomeViabilizador.HasValue())
            {
                query = query.Where(x => x.NomeViabilizador.ToUpper().Contains(filtro.NomeViabilizador.ToUpper()));
            }

            return query.ToDataRequest(request);
        }
        public DataSourceResponse<ViewViabilizador> ListarTodosViabilizadores(DataSourceRequest request)
        {
            return Queryable().ToDataRequest(request);
        }

        public List<long> IdViabilizadoresInvalidos(long idSupervisor)
        {
            return _supervisorViabilizadorRepository.IdViabilizadoresInvalidos(idSupervisor);
        }

    }
}
