using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AreaKanbanPrePropostaRepository : NHibernateRepository<AreaKanbanPreProposta>
    {
        public AreaKanbanPreProposta FindByDescricao(string descricao)
        {
            return Queryable()
                .Where(x => x.Descricao.ToLower().Equals(descricao.ToLower()))
                .SingleOrDefault();
        }

        public bool ValidarDescricaoUnica(string descricao,long idAreaKanban)
        {
            return Queryable()
                .Where(x => x.Id != idAreaKanban)
                .Where(x => x.Descricao.ToLower().Equals(descricao.ToLower()))
                .IsEmpty();
        }

    }
}
