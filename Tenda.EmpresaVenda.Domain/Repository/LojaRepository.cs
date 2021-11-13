using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class LojaRepository : NHibernateRepository<Loja>
    {
        public LojaRepository(ISession session) : base(session)
        {
        }

        public Loja FindByIdSap(string idSap)
        {
            return Queryable().SingleOrDefault(x => x.SapId.ToUpper().Equals(idSap.ToUpper()));
        }

        public IQueryable<Loja> Listar(LojaDto filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                var nome = filtro.Nome.ToLower();
                query = query.Where(x =>
                    x.Nome.ToLower().Contains(nome) ||
                    x.NomeFantasia.ToLower().Contains(nome));
            }

            if (filtro.SapId.HasValue())
            {
                query = query.Where(x => x.SapId.Contains(filtro.SapId.ToUpper()));
            }

            if (filtro.Regional.HasValue() && !filtro.Regional.Contains(0))
            {
                query = query.Where(x => filtro.Regional.Contains(x.Regional.Id));
            }

            if (filtro.IdsSituacoes.HasValue())
            {
                query = query.Where(x => filtro.IdsSituacoes.Contains((int)x.Situacao));
            }

            if (filtro.DataIntegracaoDe.HasValue())
            {
                query = query.Where(x => x.DataIntegracao.Date >= filtro.DataIntegracaoDe);
            }

            if (filtro.DataIntegracaoAte.HasValue())
            {
                query = query.Where(x => x.DataIntegracao.Date <= filtro.DataIntegracaoAte);
            }

            return query;
        }

        public IQueryable<Loja> FindByIdSap(List<string> idSap)
        {
            return Queryable().Where(x => idSap.Contains(x.SapId.ToUpper()));
        }
    }
}