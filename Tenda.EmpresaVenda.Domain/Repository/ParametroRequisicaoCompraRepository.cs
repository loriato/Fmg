using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;


namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ParametroRequisicaoCompraRepository : NHibernateRepository<ParametroRequisicaoCompra>
    {
        public IQueryable<ParametroRequisicaoCompra> Listar(string chave)
        {
            var query = Queryable();
            if (!chave.IsEmpty())
            {
                query = query = query.Where(x => x.Codigo.ToLower().Contains(chave.ToLower()) ||
                                         x.Valor.ToLower().Contains(chave.ToLower()) ||
                                         x.Descricao.ToLower().Contains(chave.ToLower()));
            }

            return query;
        }

        public string BucarValorParametro(string Codigo)
        {
            return Queryable().Where(x => x.Codigo.ToUpper().Equals(Codigo.ToUpper()))
                .Select(x => x.Valor)
                .SingleOrDefault();
        }
    }
}
