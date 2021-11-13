using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewEmpresaVendaRepository : NHibernateRepository<ViewEmpresaVenda>
    {
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public IQueryable<ViewEmpresaVenda> Listar(string empresaVenda, string cnpj, string creci, List<Situacao> situacoes, List<long> regionais, List<string> estados)
        {
            var query = Queryable();
            query = query.Where(x => x.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda);
            if (!empresaVenda.IsEmpty())
            {
                empresaVenda = empresaVenda.ToLower();
                query = query.Where(reg => reg.NomeFantasia.ToLower().Contains(empresaVenda) || reg.RazaoSocial.ToLower().Contains(empresaVenda) || reg.NomeLoja.ToLower().Contains(empresaVenda));
            }
            if (!cnpj.IsEmpty())
            {
                cnpj = cnpj.OnlyNumber();
                query = query.Where(reg => reg.CNPJ.StartsWith(cnpj.OnlyNumber()));
            }
            if (!creci.IsEmpty())
            {
                query = query.Where(reg => reg.CreciJuridico.Contains(creci));
            }
            if (!situacoes.IsEmpty())
            {
                query = query.Where(reg => situacoes.Contains(reg.Situacao));
            }
            if (!regionais.IsEmpty() && (!estados.IsEmpty() && !estados.Contains("Todos")))
            {
                var list = _regionalEmpresaRepository.Queryable()
                    .Where(w => (w.EmpresaVenda.ConsiderarUF == TipoSimNao.Sim &&
                    regionais.Contains(w.Regional.Id) &&
                    estados.Contains(w.EmpresaVenda.Estado)) ||
                    (w.EmpresaVenda.ConsiderarUF == TipoSimNao.Nao &&
                    regionais.Contains(w.Regional.Id)))
                    .Select(s => s.EmpresaVenda.Id);

                query = query.Where(w => list.Contains(w.Id));
            }
            if ((!estados.IsEmpty() && !estados.Contains("Todos")) && regionais.IsEmpty())
            {
                query = query.Where(w => estados.Contains(w.Estado));
            }
            if (!regionais.IsEmpty() && (estados.IsEmpty() || estados.Contains("Todos")))
            {
                var list = _regionalEmpresaRepository.Queryable().Where(w => regionais.Contains(w.Regional.Id)).Select(s => s.EmpresaVenda.Id);
                query = query.Where(w => list.Contains(w.Id));
            }
            

            return query;
        }
    }
}
