using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class CockpitMidasService : BaseService
    {
        private ViewOcorrenciasMidasRepository _viewOcorrenciasMidasRepository { get; set; }
        private ViewNotasCockpitMidasRepository _viewNotasCockpitMidasRepository { get; set; }
        public DataSourceResponse<ViewOcorrenciasCockpitMidas> ListarOcorrencias(FiltroCockpitMidas filtro)
        {
            var lista = _viewOcorrenciasMidasRepository.ListarOcorrencias(filtro);
            return lista;
        }

        public DataSourceResponse<ViewNotasCockpitMidas> ListarNotas(FiltroCockpitMidas filtro)
        {
            var lista = _viewNotasCockpitMidasRepository.ListarNotas(filtro);
            return lista;
        }

        public FileDto MontarExportarDtoOcorrencias(FiltroCockpitMidas filtro)
        {
            var results = _viewOcorrenciasMidasRepository.ListarOcorrencias(filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(5)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderOcorrencias());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.IdOcorrencia).Width(15)
                    .CreateCellValue(model.CNPJTomador).Width(20)
                    .CreateCellValue(model.CNPJPrestador).Width(20)
                    .CreateCellValue(model.NfeNumber).Width(50)
                    .CreateCellValue(model.Match).Width(10);
            }
            excel.Close();

            var result = new FileDto
            {
                Bytes = excel.DownloadFile(),
                FileName = $"{GlobalMessages.Ocorrencias}_{DateTime.Now:yyyyMMddHHmmss}",
                Extension = "xlsx",
                ContentType = MimeMappingWrapper.MimeType.Xlsx
            };
            return result;
        }

        public FileDto MontarExportarDtoNotas(FiltroCockpitMidas filtro)
        {
            var results = _viewNotasCockpitMidasRepository.ListarNotas(filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(12)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderNotas());

            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.IdOcorrencia).Width(15)
                    .CreateCellValue(model.NFeMidas).Width(20)
                    .CreateCellValue(model.NotaFiscal).Width(20)
                    .CreateCellValue(model.Match).Width(10)
                    .CreateCellValue(model.NomeFantasia).Width(50)
                    .CreateCellValue(model.CodigoPreProposta).Width(50)
                    .CreateCellValue(model.Estado).Width(10)
                    .CreateCellValue(model.PedidoSap).Width(20)
                    .CreateCellValue(model.SituacaoNotaFiscal).Width(20)
                    .CreateCellValue(model.Motivo).Width(50)
                    .CreateCellValue(model.DataPrevisaoPagamento.HasValue ? model.DataPrevisaoPagamento.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.Pago).Width(10);
            }
            excel.Close();

            var result = new FileDto
            {
                Bytes = excel.DownloadFile(),
                FileName = $"{GlobalMessages.NotaFiscal}_{DateTime.Now:yyyyMMddHHmmss}",
                Extension = "xlsx",
                ContentType = MimeMappingWrapper.MimeType.Xlsx
            };
            return result;
        }

        private string[] GetHeaderOcorrencias()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Ocorrencia,
                GlobalMessages.CNPJTomador,
                GlobalMessages.CNPJPrestador,
                GlobalMessages.NotaFiscal,
                GlobalMessages.Match
            };
            return header.ToArray();
        }

        private string[] GetHeaderNotas()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Ocorrencia,
                GlobalMessages.NumeroNotaFiscalP + " Midas",
                GlobalMessages.NumeroNotaFiscalP + " Portal",
                GlobalMessages.Match,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Proposta,
                GlobalMessages.UF,
                GlobalMessages.NumeroPedido,
                GlobalMessages.Situacao,
                GlobalMessages.MotivoRecusa,
                GlobalMessages.PrevisaoPagamento,
                GlobalMessages.Pago
            };
            return header.ToArray();
        }

    }
}

