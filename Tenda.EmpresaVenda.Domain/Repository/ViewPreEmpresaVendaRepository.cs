using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPreEmpresaVendaRepository : NHibernateRepository<ViewPreEmpresaVenda>
    {


        public IQueryable<ViewPreEmpresaVenda> Listar(string empresaVenda, string cnpj, string creci)
        {
            var query = Queryable();
            if (!empresaVenda.IsEmpty())
            {
                empresaVenda = empresaVenda.ToLower();
                query = query.Where(reg => reg.NomeFantasia.ToLower().Contains(empresaVenda) || reg.RazaoSocial.ToLower().Contains(empresaVenda) || reg.NomeLoja.ToLower().Contains(empresaVenda));
            }
            if (!cnpj.IsEmpty())
            {
                query = query.Where(reg => reg.CNPJ.StartsWith(cnpj.OnlyNumber()));
            }
            if (!creci.IsEmpty())
            {
                query = query.Where(reg => reg.CreciJuridico.Contains(creci));
            }
            return query;
        }
    }
}

