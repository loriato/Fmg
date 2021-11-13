using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class BannerPortalEvRepository : NHibernateRepository<BannerPortalEv>
    {
        public IQueryable<BannerPortalEv> Listar(BannerPortalEvDTO filtro)
        {
            var query = Queryable();

            if (filtro.Descricao.HasValue())
            {
                var nome = filtro.Descricao.ToLower();
                query = query.Where(x =>
                    x.Descricao.ToLower().Contains(nome));
            }

            if (filtro.Tipo.HasValue())
            {
                query = query.Where(x => x.Tipo == filtro.Tipo.Value);
            }

            if (filtro.Tipo.HasValue())
            {
                query = query.Where(x => x.Tipo == filtro.Tipo.Value);
            }

            if (filtro.IdRegional.HasValue())
            {
                query = query.Where(x => x.Regional.Id == filtro.IdRegional || x.Regional.IsEmpty());
            }

            if (filtro.Estado.HasValue() && !filtro.Estado.Contains("Todas"))
            {
                var nome = filtro.Estado.Select(x => x.ToLower());
                query = query.Where(x =>
                    nome.Contains(x.Estado.ToLower()) || x.Estado == null || x.Estado == "");
            }

            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao.Value);
            }

            if (filtro.Diretor.HasValue())
            {
                query = query.Where(x => x.Diretor == filtro.Diretor);
            }

            if (filtro.Exibicao.HasValue())
            {
                query = query.Where(x => x.Exibicao == filtro.Exibicao);
            }

            if (filtro.InicioVigencia.HasValue())
            {
                query = query.Where(x => x.InicioVigencia.Date >= filtro.InicioVigencia);
            }

            if (filtro.FimVigencia.HasValue())
            {
                query = query.Where(x => x.FimVigencia.Value.Date <= filtro.FimVigencia);
            }

            return query;
        }

        public List<BannerPortalEv> BannersVencidos()
        {
            return Queryable()
                .Where(x=>x.Situacao==SituacaoBanner.Ativo)
                .Where(x => x.FimVigencia.Value.Date < DateTime.Now.Date)
                .ToList();
        }
                
    }
}
