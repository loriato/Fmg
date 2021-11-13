using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class BreveLancamentoRepository : NHibernateRepository<BreveLancamento>
    {
        public List<BreveLancamento> DisponiveisParaCatalogo()
        {
            return Queryable()
                .Where(reg => reg.DisponivelCatalogo)
                .ToList();
        }
        public BreveLancamento BuscarBreveLancamentoAssociado(long idEmpreendimento)
        {
            return Queryable().Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .SingleOrDefault();

        }
        public IQueryable<BreveLancamento> Listar(string nome)
        {
            var query = Queryable();

            if(nome.HasValue())
            {
                nome = nome.ToLower();
                query = query.Where(reg =>
                    reg.Nome.ToLower().Contains(nome));
            }

            return query.OrderBy(reg => reg.Sequencia ?? int.MaxValue); ;
        }

        public BreveLancamento FindByDivisao(string divisao)
        {
            var query = Queryable();
            var breveLancamento = query.FirstOrDefault(x => x.Empreendimento.Divisao.ToLower().Equals(divisao.ToLower()));

            return breveLancamento;
        }
    }
}