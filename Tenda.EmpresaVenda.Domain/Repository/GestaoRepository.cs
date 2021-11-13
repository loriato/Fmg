using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class GestaoRepository : NHibernateRepository<Gestao>
    {
        public IQueryable<Gestao> Listar(GestaoDTO filtro)
        {
            var query = Queryable();
            if (filtro.ReferenciaDe.HasValue)
            {
                query = query.Where(reg => reg.DataReferencia.Date >= filtro.ReferenciaDe.Value.Date);
            }

            if (filtro.ReferenciaAte.HasValue)
            {
                query = query.Where(reg => reg.DataReferencia.Date <= filtro.ReferenciaAte.Value.Date);
            }

            if(filtro.idTipoCusto.HasValue)
            {
                query = query.Where(reg => reg.TipoCusto.Id == filtro.idTipoCusto);
            }

            if (filtro.idClassificacao.HasValue)
            {
                query = query.Where(reg => reg.Classificacao.Id == filtro.idClassificacao);
            }

            if (filtro.idFornecedor.HasValue)
            {
                query = query.Where(reg => reg.Fornecedor.Id == filtro.idFornecedor);
            }
            if (filtro.idEmpresaVenda.HasValue)
            {
                query = query.Where(reg => reg.EmpresaVenda.Id == filtro.idEmpresaVenda);
            }
            if (filtro.idPontoVenda.HasValue)
            {
                query = query.Where(reg => reg.PontoVenda.Id == filtro.idPontoVenda);
            }
            if (filtro.idCentroCusto.HasValue)
            {
                query = query.Where(reg => reg.CentroCusto.Id == filtro.idCentroCusto);
            }
            return query;
        }
    }
}
