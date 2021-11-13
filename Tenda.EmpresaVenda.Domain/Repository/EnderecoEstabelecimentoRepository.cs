using Europa.Data;
using NHibernate;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoEstabelecimentoRepository : NHibernateRepository<EnderecoEstabelecimento>
    {
        public EnderecoEstabelecimentoRepository(ISession session) : base(session)
        {
        }

        public EnderecoEstabelecimento FindByEmpreendimento(long idEmpreendimento)
        {
            return Queryable().Where(x => x.Empreendimento.Id == idEmpreendimento).FirstOrDefault();
        }

    }
}
