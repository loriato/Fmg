using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class TipoDocumentoRepository : NHibernateRepository<TipoDocumento>
    {
        public List<TipoDocumento> ListarTodos()
        {
            return Queryable()
                .Where(reg => reg.Situacao == Situacao.Ativo)
                .OrderBy(reg => reg.Ordem)
                .ToList();
        }

        public List<TipoDocumento> ListarTodosPortalEV()
        {
            var docs = ListarTodos();
            docs = docs.Where(reg => reg.VisivelPortal == true).ToList();
            return docs;
        }

        public List<TipoDocumento> ListarTodosHouse()
        {
            var docs = ListarTodos();
            docs = docs.Where(reg => reg.VisivelLoja == true).ToList();
            return docs;
        }

        public DataSourceResponse<TipoDocumento> Listar(FiltroTiposDocumentoDTO filtro)
        {
            var query = Queryable()
                       .Where(reg => reg.Situacao == Situacao.Ativo);

            if (filtro.Nome.HasValue())
            {
                query = query.Where(reg => reg.Nome.Contains(filtro.Nome));
            }

            return query.ToDataRequest(filtro.Request);
        }
    }
}