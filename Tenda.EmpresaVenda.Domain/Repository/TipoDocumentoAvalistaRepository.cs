using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class TipoDocumentoAvalistaRepository:NHibernateRepository<TipoDocumentoAvalista>
    {
        public List<TipoDocumentoAvalista> ListarTodos()
        {
            return Queryable()
                .Where(reg => reg.Situacao == Situacao.Ativo)
                .OrderBy(reg => reg.Ordem)
                .ToList();
        }
    }
}
