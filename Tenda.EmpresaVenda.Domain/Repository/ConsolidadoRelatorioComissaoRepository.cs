using Europa.Data;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ConsolidadoRelatorioComissaoRepository : NHibernateRepository<ConsolidadoRelatorioComissao>
    {
        public DateTime BuscarUltimaDataAtualizacao()
        {
            return Queryable()
                .OrderByDescending(x => x.UltimaModificacao)
                .Select(reg => reg.UltimaModificacao)
                .FirstOrDefault();
        }

        public ConsolidadoRelatorioComissao FindByIdProposta(long idProposta)
        {
            return Queryable().Where(x => x.IdProposta == idProposta).SingleOrDefault();
        }
    }
}
