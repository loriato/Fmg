using System.Collections.Generic;
using System.Linq;
using Europa.Data;
using NHibernate;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoEmpreendimentoRepository : NHibernateRepository<EnderecoEmpreendimento>
    {
        public EnderecoEmpreendimentoRepository(ISession session) : base(session)
        {
        }

        public EnderecoEmpreendimentoRepository()
        {
        }

        public EnderecoEmpreendimento FindByEmpreendimento(long idEmpreendimento)
        {
            return Queryable().Where(x => x.Empreendimento.Id == idEmpreendimento).FirstOrDefault();
        }

        public Dictionary<long, EnderecoEmpreendimento> EnderecosDeEmpreendimentos(List<long> idEmpreendimentos)
        {
            return Queryable()
                .Where(reg => idEmpreendimentos.Contains(reg.Empreendimento.Id))
                .ToDictionary(reg => reg.Empreendimento.Id, reg => reg);
        }

        public IQueryable<EnderecoEmpreendimento> EnderecosDaRegional(string regional)
        {
            return Queryable()
                .Where(reg => reg.Estado.ToUpper() == regional.ToUpper());
        }

        public List<string> RegionaisDosEmpreendimentos()
        {
            return Queryable().GroupBy(x => x.Estado).OrderBy(x => x.Key).Select(x => x.Key).ToList();
        }
    }
}