using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewDocumentoProponenteRepository : NHibernateRepository<ViewDocumentoProponente>
    {
        public List<ViewDocumentoProponente> ListarPreProposta(long idPreProposta, long? idCliente, SituacaoAprovacaoDocumento? situacao)
        {
            var query = Queryable()
                .Where(reg => reg.IdPreProposta == idPreProposta);

            if (idCliente.HasValue())
            {
                query = query.Where(reg => reg.IdCliente == idCliente);
            }
            if (situacao.HasValue())
            {
                query = query.Where(reg => reg.Situacao == situacao);
            }
            return query.ToList();
        }
    }
}
