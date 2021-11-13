using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class UsuarioGrupoCCARepository:NHibernateRepository<UsuarioGrupoCCA>
    {
        public List<long> ListarIdGruposPorUsuario(long idUsuario)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Select(x=>x.GrupoCCA.Id)
                .ToList();
        }
        public UsuarioGrupoCCA ListarGruposPorUsuario(long idUsuario)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .ToList().FirstOrDefault();
        }
    }
}
