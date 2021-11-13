using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS11")]
    public class ConsultaPrePropostaController : BaseController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }


        // GET: ConsultaProposta
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS11", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, ConsultaPrePropostaDto dto)
        {
            dto = FiltroAuxiliar(dto);
            
            var idCorretor = SessionAttributes.Current().Corretor.Id;

            var corretor = SessionAttributes.Current().Corretor;
            var podeVisualizar = corretor.Funcao == TipoFuncao.Corretor && corretor.EmpresaVenda.CorretorVisualizarClientes;

            dto.IdCorretorVisualizador = corretor.Id;
            dto.PodeVisualizar = podeVisualizar;

            var results = _viewPrePropostaRepository.Listar(request, dto, idCorretor);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS11", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, ConsultaPrePropostaDto dto)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, dto);
            string nomeArquivo = GlobalMessages.PreProposta;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("EVS11", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, ConsultaPrePropostaDto dto)
        {
            byte[] file = Exportar(request, dto);
            string nomeArquivo = GlobalMessages.PreProposta;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, ConsultaPrePropostaDto dto)
        {
            dto = FiltroAuxiliar(dto);

            var results = _viewPrePropostaRepository.Listar(request, dto, null);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Codigo).Width(25)
                    .CreateCellValue(model.NomeCliente).Width(20)
                    .CreateCellValue(model.SituacaoPrePropostaSuatEvs == GlobalMessages.SituacaoProposta_AguardandoAuditoria ? GlobalMessages.EmAnalise : model.SituacaoPrePropostaSuatEvs).Width(40)
                    .CreateCellValue(model.NumeroDocumentosPendentes).Width(40)
                    .CreateCellValue(model.MotivoParecer).Width(100)
                    .CreateCellValue(model.MotivoPendencia).Width(100)
                    .CreateCellValue(model.NomePontoVenda).Width(40)
                    .CreateCellValue(model.NomeCorretor).Width(40)
                    .CreateCellValue(model.NomeViabilizador).Width(40)
                    .CreateCellValue(model.NomeElaborador).Width(40)
                    .CreateCellValue(model.NomeBreveLancamento).Width(40)
                    .CreateCellValue(model.Elaboracao.ToDate()).Width(20)
                    .CreateCellValue(model.DataEnvio.HasValue ? model.DataEnvio.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.NomeAssistenteAnalise).Width(40)
                    .CreateCellValue(model.TipoRenda).Width(40)
                    .CreateMoneyCell(model.RendaApurada).Width(40)
                    .CreateMoneyCell(model.FgtsApurado).Width(100)
                    .CreateMoneyCell(model.Entrada).Width(15)
                    .CreateMoneyCell(model.PreChaves)
                    .CreateMoneyCell(model.PreChavesIntermediaria).Width(15)
                    .CreateMoneyCell(model.Fgts).Width(40)
                    .CreateMoneyCell(model.Subsidio).Width(40)
                    .CreateMoneyCell(model.Financiamento).Width(40)
                    .CreateMoneyCell(model.PosChaves).Width(40)
                    .CreateCellValue(model.StatusSicaq.AsString()).Width(20)
                    .CreateCellValue(model.NomeAnalistaSicaq).Width(40)
                    .CreateCellValue(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDate() : empty).Width(30)
                    .CreateMoneyCell(model.ParcelaAprovada).Width(45)
                    .CreateCellValue(model.OrigemCliente.AsString()).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Codigo,
                GlobalMessages.Cliente,
                GlobalMessages.StatusAnalise,
                GlobalMessages.NumeroDocumentosPendentes,
                GlobalMessages.ParecerTenda,
                GlobalMessages.MotivoPendencia,
                GlobalMessages.PontoVenda,
                GlobalMessages.Corretor,
                GlobalMessages.Viabilizador,
                GlobalMessages.Elaborador,
                GlobalMessages.Produto,
                GlobalMessages.DataElaboracao,
                GlobalMessages.DataUltimoEnvio,
                GlobalMessages.AssistenteAnalise,
                GlobalMessages.TipoRenda,
                GlobalMessages.RendaFamiliar,
                GlobalMessages.FGTSApurado,
                GlobalMessages.Entrada,
                GlobalMessages.PreChaves,
                GlobalMessages.PreChavesIntermediaria,
                GlobalMessages.FGTS,
                GlobalMessages.Subsidio,
                GlobalMessages.Financiamento,
                GlobalMessages.PosChaves,
                GlobalMessages.StatusSicaq,
                GlobalMessages.AnalistaSicaq,
                GlobalMessages.DataHoraSicaq,
                GlobalMessages.ParcelaAprovadaDoSICAQ,
                GlobalMessages.OrigemCliente
            };
            return header.ToArray();
        }

        private ConsultaPrePropostaDto FiltroAuxiliar(ConsultaPrePropostaDto filtro)
        {
            if (filtro.Situacoes.HasValue())
            {
                if (filtro.Situacoes.Contains(SituacaoProposta.EmAnaliseSimplificada))
                {
                    filtro.Situacoes.Add(SituacaoProposta.AguardandoAuditoria);
                }
            }

            if (filtro.IdEmpresaVenda.IsEmpty())
            {
                filtro.IdEmpresaVenda = new List<long> { 
                    SessionAttributes.Current().EmpresaVendaId 
                };
            }

            return filtro;
        }
    }
}