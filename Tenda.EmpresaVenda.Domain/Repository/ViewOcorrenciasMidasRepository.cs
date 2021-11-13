using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewOcorrenciasMidasRepository : NHibernateRepository<ViewOcorrenciasCockpitMidas>
    {
        private ViewNotasCockpitMidasRepository _viewNotasCockpitMidasRepository { get; set; }
        public DataSourceResponse<ViewOcorrenciasCockpitMidas> ListarOcorrencias(FiltroCockpitMidas filtro)
        {
            var queryInicial = Queryable();


            var query = RemoverOcorrenciasDuplicadas(queryInicial.ToList()).AsQueryable();


            query = query.Where(reg => reg.Comissionavel == true);

            if (filtro.Ocorrencia.HasValue())
            {
                query = query.Where(reg => reg.IdOcorrencia == filtro.Ocorrencia);
            }

            if (filtro.CNPJPrestador.HasValue())
            {
                query = query.Where(reg => reg.CNPJPrestador.StartsWith(filtro.CNPJPrestador.OnlyNumber()));
            }

            if (filtro.CNPJTomador.HasValue())
            {
                query = query.Where(reg => reg.CNPJTomador.StartsWith(filtro.CNPJTomador.OnlyNumber()));
            }

            if (filtro.IdsEmpresaVenda.HasValue() && filtro.IdsEmpresaVenda[0] != 0)
            {
                query = query.Where(reg => filtro.IdsEmpresaVenda.Contains(reg.IdEmpresaVenda));
            }


            if (filtro.Match.HasValue())
            {
                query = query.Where(reg => Convert.ToInt32(reg.Match) == Convert.ToInt32((bool)filtro.Match));
            }

            if (filtro.NumeroNotaFiscal.HasValue())
            {
                query = query.Where(x => x.NfeNumber.ToUpper().Contains(filtro.NumeroNotaFiscal.ToUpper()) || x.NfeNumber.ToUpper().Contains(filtro.NumeroNotaFiscal.ToUpper()));
            }

            if (filtro.Estado.HasValue() && !filtro.Estado.Contains(""))
            {
                query = query.Where(reg => filtro.Estado.Contains(reg.EstadoPrestador.ToUpper()));
            }

            return query.ToDataRequest(filtro.Request);
        }

        private List<ViewOcorrenciasCockpitMidas> RemoverOcorrenciasDuplicadas(List<ViewOcorrenciasCockpitMidas> query)
        {
            List<ViewOcorrenciasCockpitMidas> queryTratada = new List<ViewOcorrenciasCockpitMidas>();
            queryTratada.AddRange(query);
            foreach(var vw in query)
            {
                if(queryTratada.Count(reg => reg.IdOcorrencia == vw.IdOcorrencia) > 1)
                {
                    queryTratada.Remove(vw);
                }
            }

            return queryTratada;
        }
    }
}
