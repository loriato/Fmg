using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class HistoricoRegraComissaoRepository: NHibernateRepository<HistoricoRegraComissao>
    {
        public HistoricoRegraComissao AtualDaRegra(long idRegraEvs)
        {
            return Queryable()
                .Where(reg=>reg.RegraComissaoEvs.Id == idRegraEvs)
                .Where(reg => reg.Status == Situacao.Ativo)
                .OrderBy(reg => reg.Inicio)
                .SingleOrDefault();
        }

        public List<HistoricoRegraComissao> FindByIdRegraComissaoEvs(long idRegra)
        {
            return Queryable()
                .Where(x => x.RegraComissaoEvs.Id == idRegra)
                .ToList();
        }

    }
}
