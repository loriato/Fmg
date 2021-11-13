using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class LayoutProgramaFidelidadeRepository : NHibernateRepository<LayoutProgramaFidelidade>
    {
        public override void Save(LayoutProgramaFidelidade entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);
            }

            var sequence = entity.Id.ToString();
            entity.Codigo = "LPF" + sequence.PadLeft(6, '0');

            _session.SaveOrUpdate(entity);

        }
        public LayoutProgramaFidelidade LayoutAtivo()
        {
            return Queryable()
                .Where(x => x.Situacao == Situacao.Ativo)
                .SingleOrDefault();
        }
    }
}
