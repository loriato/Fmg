using Europa.Data;
using Europa.Extensions;
using NHibernate.Linq;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ConsolidadoPrePropostaRepository : NHibernateRepository<ConsolidadoPreProposta>
    {
        public ConsolidadoPreProposta FindByPrePropostaId(long id)
        {
            return Queryable()
                .Fetch(reg => reg.Envio)
                .Fetch(reg => reg.AnaliseInicial)
                .FirstOrDefault(x => x.PreProposta.Id == id);
        }

        /// <summary>
        /// Retorna a data de ultima modificação do consolidado
        /// Retorna DateTime.MinValue se não houver nenhuma
        /// </summary>
        /// <returns></returns>
        public DateTime BuscarUltimaDataAtualizacao()
        {
            return Queryable()
                .OrderByDescending(x => x.UltimaModificacao)
                .Select(reg => reg.UltimaModificacao)
                .FirstOrDefault();
        }
    }
}
