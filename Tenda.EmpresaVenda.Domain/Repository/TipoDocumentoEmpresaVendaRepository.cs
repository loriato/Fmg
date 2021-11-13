using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class TipoDocumentoEmpresaVendaRepository : NHibernateRepository<TipoDocumentoEmpresaVenda>
    {
        public List<TipoDocumentoEmpresaVenda> ListarTiposDocumentoEmpresaVenda()
        {
            return Queryable()
                .Where(x => x.Situacao == Situacao.Ativo)
                .ToList();
        }
    }
}
