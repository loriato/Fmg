using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewBannerPortalEVRepository : NHibernateRepository<ViewBannerPortalEV>
    {
        public VisualizacaoBannerRepository _visualizacaoBannerRepository { get; set; }
       

        public DataSourceResponse<ViewBannerPortalEV>Listar(DataSourceRequest request, BannerPortalEvDTO filtro)
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

            if (filtro.IdRegional.HasValue())
            {
                query = query.Where(x => x.IdRegional == filtro.IdRegional || x.IdRegional == null);
            }

            if (filtro.Estado.HasValue() && !filtro.Estado.Contains("Todas"))
            {
                var nome = filtro.Estado.Select(x=>x.ToLower());
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
                query = query.Where(x => x.InicioVigencia.Value.Date >= filtro.InicioVigencia.Value.Date);
            }

            if (filtro.FimVigencia.HasValue())
            {
                query = query.Where(x => x.FimVigencia.Value.Date <= filtro.FimVigencia.Value.Date);
            }

            return query.ToDataRequest(request);
        }

        public List<ViewBannerPortalEV> ShowBanners(string estado, List<Regionais> regionais, TipoFuncao funcao,long idCorretor)
        {
            List<long> IdRegionais = regionais.Select(x => x.Id).ToList();

            var query = Queryable()
                .Where(x => IdRegionais.Contains(x.IdRegional ?? 0) || x.IdRegional == null)
                .Where(x => x.Estado.Equals(estado) || x.Estado == null)
                .Where(x => x.Situacao == SituacaoBanner.Ativo)
                .Where(x => x.InicioVigencia.Value.Date <= DateTime.Now.Date)
                .Where(x => x.FimVigencia.Value.Date >= DateTime.Now.Date ||
            x.FimVigencia == null);

            if (funcao != TipoFuncao.Diretor)
            {
                query = query.Where(x=>x.Diretor == false);
            }

            var vistoUmaVez = _visualizacaoBannerRepository.VistoUnicaVez(idCorretor);

            if (vistoUmaVez.HasValue())
            {
                var ids = vistoUmaVez.Select(x => x.Id);

                query = query.Where(x => !ids.Contains(x.Id));
            }

            return query.ToList();
        }
    }
}
