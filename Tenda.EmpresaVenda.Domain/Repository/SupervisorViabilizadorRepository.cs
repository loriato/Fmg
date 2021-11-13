using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class SupervisorViabilizadorRepository : NHibernateRepository<SupervisorViabilizador>
    {
        public List<long> ViabilizadorByIdSupervisor(long idSupervisor)
        {
            return Queryable()
                .Where(x => x.Supervisor.Id == idSupervisor)
                .Select(x => x.Viabilizador.Id)
                .ToList();
        }

        public List<long> ViabilizadorByIdSupervisor(List<long> idsSupervisor)
        {
            return Queryable()
                .Where(x =>idsSupervisor.Contains(x.Supervisor.Id))
                .Select(x => x.Viabilizador.Id)
                .ToList();
        }

        public List<long> IdViabilizadoresInvalidos(long idSupervisor)
        {
            return Queryable().Where(x => x.Supervisor.Id != idSupervisor).Select(x => x.Viabilizador.Id).ToList();
        }

        public SupervisorViabilizador CruzamentoUnicoPorViabilizador(long idViabilizador)
        {
            return Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .Where(x => x.Supervisor != null)
                .SingleOrDefault();
        }
        public List<SupervisorViabilizador> CruzamentoMultiploPorSupervisor(long idSupervisor)
        {
            return Queryable()
                .Where(x => x.Supervisor.Id == idSupervisor)
                .Where(x => x.Supervisor != null).ToList();
        }
    }
}
