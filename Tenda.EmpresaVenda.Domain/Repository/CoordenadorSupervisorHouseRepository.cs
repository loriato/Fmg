using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class CoordenadorSupervisorHouseRepository:NHibernateRepository<CoordenadorSupervisorHouse>
    {
        public CoordenadorSupervisorHouse BuscarVinculoCoordenadorSupervisorAtivo(long idCoordenadorHouse,long idSupervisorHouse) {
            var coordenadorSupervisorHouse = Queryable()
                .Where(x => x.Coordenador.Id == idCoordenadorHouse)
                .Where(x => x.Supervisor.Id == idSupervisorHouse)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            return coordenadorSupervisorHouse;
        }

        public CoordenadorSupervisorHouse BuscarVinculoSupervisorAtivo(long idSupervisorHouse)
        {
            var coordenadorSupervisorHouse = Queryable()
                .Where(x => x.Supervisor.Id == idSupervisorHouse)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            return coordenadorSupervisorHouse;
        }

        public List<CoordenadorSupervisorHouse> FindByIdCoordenador(long idCoordenador)
        {
            var query = Queryable()
                .Where(x => x.Coordenador.Id == idCoordenador);
            return query.ToList();
        }

    }
}
