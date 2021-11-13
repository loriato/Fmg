using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class CoordenadorViabilizadorRepository:NHibernateRepository<CoordenadorViabilizador>
    {
        public bool CoordenadorViabilizadorUnico(long idViabilizador)
        {
            return Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .Where(x => x.Coordenador != null)
                .Any();
        }

        public List<long> IdsViabilizadorByCoordenador(long idCoordenador)
        {
            return Queryable()
                .Where(x => x.Coordenador.Id == idCoordenador)
                .Select(x => x.Viabilizador.Id)
                .ToList();
        }
    }
}
