using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class PerfilSistemaGrupoActiveDiretoryRepository : NHibernateRepository<PerfilSistemaGrupoActiveDirectory>
    {
        public List<PerfilSistemaGrupoActiveDirectory> ConfiguracaoDoSistema(long idSistema)
        {
            return Queryable()
                .Where(reg => reg.Sistema.Id == idSistema)
                .Where(reg => reg.Perfil.Situacao == Enums.Situacao.Ativo)
                .ToList();
        }

        public bool VerificarUnicidade(long id, string grupoActiveDirectory)
        {
            return Queryable()
                    .Where(x => x.Id != id)
                    .Where(x => x.GrupoActiveDirectory == grupoActiveDirectory)
                    .Any();
        }
    }
}
