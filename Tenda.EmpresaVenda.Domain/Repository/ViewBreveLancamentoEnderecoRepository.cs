using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;


namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewBreveLancamentoEnderecoRepository : NHibernateRepository<ViewBreveLancamentoEndereco>
    {
        public IQueryable<ViewBreveLancamentoEndereco> Listar(BreveLancamentoListarDTO filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                filtro.Nome = filtro.Nome.ToLower();
                query = query.Where(reg =>
                    reg.Nome.ToLower().Contains(filtro.Nome));
            }

            if (filtro.Cidade.HasValue())
            {
                filtro.Cidade = filtro.Cidade.ToLower();
                query = query.Where(reg =>
                    reg.Cidade.ToLower().Contains(filtro.Cidade));
            }



            if (filtro.IdEmpreendimento.HasValue())
            {
                query = query.Where(reg => reg.IdEmpreendimento == filtro.IdEmpreendimento.Value);
            }

            if (filtro.IdRegional.HasValue() && !filtro.IdRegional.Contains(0) && !filtro.IdRegional.Contains(null))
            {

                query = query.Where(reg => filtro.IdRegional.Contains(reg.IdRegional));
            }

            if (filtro.Estados.HasValue() && !filtro.Estados.Contains(""))
            {
                filtro.Estados = filtro.Estados.Select(reg => reg.ToUpper()).ToArray();
                query = query.Where(reg => filtro.Estados.Contains(reg.Estado.ToUpper()));
            }

            return query;
        }
    }
}
