using Europa.Data;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PropostaFilaSUATRepository : NHibernateRepository<PropostaFilaSUAT>
    {
        public override void Save(PropostaFilaSUAT entity)
        {
            entity.UltimaModificacao = DateTime.Now;
            _session.SaveOrUpdate(entity);
        }

        public DateTime BuscarUltimaDataAtualizacao()
        {
            return Queryable()
                .OrderByDescending(x => x.UltimaModificacao)
                .Select(reg => reg.UltimaModificacao)
                .FirstOrDefault();
        }

        public PropostaFilaSUAT FindByIdPropostaIdNode(long idProposta, long idNode)
        {
            return Queryable()
                .Where(x => x.IdProposta == idProposta)
                .Where(x => x.IdNode == idNode)
                .FirstOrDefault();
        }
    }
}
