using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class SupervisorAgenteVendaHouseRepository : NHibernateRepository<SupervisorAgenteVendaHouse>
    {
        public SupervisorAgenteVendaHouse BuscarVinculoSupervisorAgenteVendaAtivo(long idAgenteVenda)
        {
            return Queryable().Where(x => x.AgenteVenda.Id == idAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();
        }

        public SupervisorAgenteVendaHouse BuscarVinculoSupervisorAgenteVendaAtivo(long idSupervisor,long idAgenteVenda)
        {
            return Queryable().Where(x=>x.Supervisor.Id==idSupervisor)
                .Where(x => x.AgenteVenda.Id == idAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();
        }
    }
}
