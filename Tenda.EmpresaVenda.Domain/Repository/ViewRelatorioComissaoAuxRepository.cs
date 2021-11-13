using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRelatorioComissaoAuxRepository : NHibernateRepository<ViewRelatorioComissaoAux>
    {
        public IQueryable<ViewRelatorioComissaoAux> Listar(RelatorioComissaoDTO filtro)
        {
            var query = Queryable();
            if (filtro.IdEmpreendimento.HasValue())
            {
                query = query.Where(x => x.IdEmpreendimento == filtro.IdEmpreendimento);
            }

            if (!filtro.CodigoPreProposta.IsEmpty())
            {
                query = query.Where(x => x.CodigoPreProposta.ToUpper().Equals(filtro.CodigoPreProposta.ToUpper()));
            }

            if (!filtro.CodigoProposta.IsEmpty())
            {
                query = query.Where(x => x.CodigoProposta.ToUpper().Equals(filtro.CodigoProposta.ToUpper()));
            }

            if (filtro.Estados.HasValue())
            {
                query = query.Where(x => filtro.Estados.Contains(x.Regional.ToUpper()));
            }

            if (!filtro.NomeFornecedor.IsEmpty())
            {
                query = query.Where(x => x.NomeFornecedor.ToUpper().Equals(filtro.NomeFornecedor.ToUpper()));
            }

            if (!filtro.CodigoFornecedor.IsEmpty())
            {
                query = query.Where(x => x.CodigoFornecedor.ToUpper().Equals(filtro.CodigoFornecedor.ToUpper()));
            }

            if (filtro.DataVendaDe.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value >= filtro.DataVendaDe.Value.Date);
            }

            if (filtro.DataVendaAte.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value <= filtro.DataVendaAte.Value.Date);
            }

            if (!filtro.NomeCliente.IsEmpty())
            {
                query = query.Where(x => x.NomeCliente.ToLower().Contains(filtro.NomeCliente.ToLower()));
            }

            if (!filtro.StatusContrato.IsEmpty())
            {
                query = query.Where(x => x.StatusContrato.ToLower().Equals(filtro.StatusContrato.ToLower()));
            }

            return query;
        }
    }
}