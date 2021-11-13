using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RegraComissaoPadraoRepository : NHibernateRepository<RegraComissaoPadrao>
    {
        public RegraComissaoPadrao BuscarRegraVigente(string regional)
        {
            return Queryable()
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo)
                .Where(reg => reg.Regional == regional)
                .SingleOrDefault();
        }
        public override void Save(RegraComissaoPadrao entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);
            }

            var sequence = entity.Id.ToString();
            entity.Codigo = "R" + sequence.PadLeft(6, '0');

            _session.SaveOrUpdate(entity);

        }

    }
}
