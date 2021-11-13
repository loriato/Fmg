using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class CoordenadorSupervisorRepository:NHibernateRepository<CoordenadorSupervisor>
    {
        public bool CoordenadorSupervisorUnico(long idSupervisor)
        {
            return Queryable()
                .Where(x => x.Supervisor.Id == idSupervisor)
                .Where(x => x.Coordenador != null)
                .Any();
        }

        public List<long> IdsSupervisorByCoordenador(long idCoordenador)
        {
            return Queryable()
                .Where(x => x.Coordenador.Id == idCoordenador)
                .Select(x => x.Supervisor.Id)
                .ToList();
        }
        public List<CoordenadorSupervisor> CruzamentoMultiploPorCoordenador(long idCoordenador)
        {
            return Queryable()
                .Where(x => x.Coordenador.Id == idCoordenador)
                .Where(x => x.Supervisor != null).ToList();
        }
    }
}
