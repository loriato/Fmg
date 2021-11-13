using Europa.Extensions;
using Flurl.Http;
using System;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        public DataSourceResponse<ViewOcorrenciasCockpitMidas> ListarOcorrencias(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("cockpitmidas", "listarocorrencias");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewOcorrenciasCockpitMidas>>(response);
        }

        public DataSourceResponse<ViewNotasCockpitMidas> ListarNotas(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("cockpitmidas", "listarnotas");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<ViewNotasCockpitMidas>>(response);
        }

        public FileDto ExportarTodosOcorrencias(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("cockpitmidas")
                .AppendPathSegment("exportarTodosOcorrencias");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }
        public FileDto ExportarPaginaOcorrencias(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("cockpitmidas")
                .AppendPathSegment("exportarPaginaOcorrencias");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }

        public FileDto ExportarTodosNotas(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("cockpitmidas")
                .AppendPathSegment("exportarTodosNotas");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }

        public FileDto ExportarPaginaNotas(FiltroCockpitMidas filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("cockpitmidas")
                .AppendPathSegment("exportarPaginaNotas");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }



    }

}

