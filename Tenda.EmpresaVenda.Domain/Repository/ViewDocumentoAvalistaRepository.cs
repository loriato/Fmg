using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewDocumentoAvalistaRepository:NHibernateRepository<ViewDocumentoAvalista>
    {
        public List<ViewDocumentoAvalista> ListarPreProposta(long idPreProposta, long? idAvalista, SituacaoAprovacaoDocumento? situacao)
        {
            var query = Queryable()
                .Where(reg => reg.IdPreProposta == idPreProposta);

            if (idAvalista.HasValue())
            {
                query = query.Where(reg => reg.IdAvalista == idAvalista);
            }
            if (situacao.HasValue())
            {
                query = query.Where(reg => reg.Situacao == situacao);
            }
            return query.ToList();
        }
    }
}
