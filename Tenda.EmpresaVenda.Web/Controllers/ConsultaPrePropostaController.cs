using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS11")]
    public class ConsultaPrePropostaController : BaseController
    {
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private ViewUsuarioEmpresaVendaRepository _viewUsuarioEmpresaVendaRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        private CoordenadorSupervisorRepository _coordenadorSupervisorRepository { get; set; }
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        // GET: ConsultaProposta
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS11", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, ConsultaPrePropostaDto dto)
        {
            dto = AplicarFiltroSituacoes(dto);
            dto = AplicarFiltroAuxiliar(dto);

            var results = _viewPrePropostaRepository.Listar(request, dto, null);

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
            dto = AplicarFiltroAuxiliar(dto);
            var results = _viewPrePropostaRepository.Listar(request, dto, null);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel.CreateCellValue(model.Regional).Width(25);
                excel.CreateCellValue(model.UF).Width(25);
                excel.CreateCellValue(model.Codigo).Width(25);
                excel.CreateCellValue(model.NomeCliente).Width(20);
                excel.CreateCellValue(model.CpfCnpj).Width(25);
                excel.CreateCellValue(model.SituacaoPrePropostaSuatEvs).Width(20);
                excel.CreateCellValue(model.NumeroDocumentosPendentes).Width(25);
                excel.CreateCellValue(model.MotivoParecer).Width(100);
                excel.CreateCellValue(model.MotivoPendencia).Width(20);
                excel.CreateCellValue(model.NomeEmpresaVenda).Width(40);
                excel.CreateCellValue(model.NomePontoVenda).Width(40);
                excel.CreateCellValue(model.NomeCorretor).Width(40);
                excel.CreateCellValue(model.NomeViabilizador).Width(40);
                excel.CreateCellValue(model.NomeCCA).Width(40);
                excel.CreateCellValue(model.NomeElaborador).Width(40);
                excel.CreateCellValue(model.NomeBreveLancamento).Width(40);
                excel.CreateDateTimeCell(model.Elaboracao.HasValue() ? model.Elaboracao.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(20);
                excel.CreateDateTimeCell(model.DataEnvio.HasValue ? model.DataEnvio.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(20);
                excel.CreateCellValue(model.NomeAssistenteAnalise).Width(20);
                excel.CreateCellValue(model.TipoRenda.IsEmpty() ? empty : model.TipoRenda.AsString()).Width(20);
                excel.CreateMoneyCell(model.RendaApurada).Width(20);
                excel.CreateMoneyCell(model.FgtsApurado).Width(20);
                excel.CreateMoneyCell(model.Entrada).Width(15);
                excel.CreateMoneyCell(model.PreChaves);
                excel.CreateMoneyCell(model.PreChavesIntermediaria).Width(20);
                excel.CreateMoneyCell(model.Fgts).Width(40);
                excel.CreateMoneyCell(model.Subsidio).Width(40);
                excel.CreateMoneyCell(model.Financiamento).Width(40);
                excel.CreateMoneyCell(model.PosChaves).Width(40);
                excel.CreateCellValue(model.StatusSicaq.HasValue ? model.StatusSicaq.AsString() : empty).Width(20);
                excel.CreateCellValue(model.StatusSicaqPrevio.HasValue ? model.StatusSicaqPrevio.AsString() : empty).Width(20);
                excel.CreateCellValue(model.NomeAnalistaSicaq.HasValue() ? model.NomeAnalistaSicaq : empty).Width(20);
                excel.CreateDateTimeCell(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(20);
                excel.CreateDateTimeCell(model.DataSicaqPrevio.HasValue ? model.DataSicaqPrevio.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(20);
                excel.CreateMoneyCell(model.ParcelaAprovada).Width(45);
                excel.CreateMoneyCell(model.ParcelaAprovadaPrevio).Width(45);
                excel.CreateCellValue(model.ContadorSicaq);
                excel.CreateCellValue(model.OrigemCliente.AsString()).Width(40);
                excel.CreateCellValue(model.Faturado?GlobalMessages.Sim:GlobalMessages.Nao).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.UF,
                GlobalMessages.Codigo,
                GlobalMessages.Cliente,
                GlobalMessages.CpfCnpj,
                GlobalMessages.SituacaoPreProposta,
                GlobalMessages.NumeroDocumentosPendentes,
                GlobalMessages.ParecerTenda,
                GlobalMessages.MotivoPendencia,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.PontoVenda,
                GlobalMessages.Corretor,
                GlobalMessages.Viabilizador,
                GlobalMessages.UltimoCCAResponsavel,
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
                GlobalMessages.StatusSicaqPrevio,
                GlobalMessages.AnalistaSicaq,
                GlobalMessages.DataHoraSicaq,
                GlobalMessages.DataHoraSicaqPrevio,
                GlobalMessages.ParcelaAprovadaDoSICAQ,
                GlobalMessages.ParcelaAprovadaPrevioDoSICAQ,
                GlobalMessages.ContadorSicaq,
                GlobalMessages.OrigemCliente,
                GlobalMessages.Faturado

            };
            return header.ToArray();
        }

        private ConsultaPrePropostaDto AplicarFiltroSituacoes(ConsultaPrePropostaDto filtro)
        {
            // Filtro apenas 
            if (filtro != null && filtro.Situacoes.IsEmpty())
            {
                var situacoesParaVisualizacao = new List<SituacaoProposta>() {
                    SituacaoProposta.EmElaboracao,
                    SituacaoProposta.AguardandoAnaliseSimplificada,
                    SituacaoProposta.EmAnaliseSimplificada,
                    SituacaoProposta.DocsInsuficientesSimplificado,
                    SituacaoProposta.DocsInsuficientesCompleta,
                    SituacaoProposta.AguardandoAuditoria,
                    SituacaoProposta.Condicionada,
                    SituacaoProposta.AnaliseSimplificadaAprovada,
                    SituacaoProposta.SICAQComErro,
                    SituacaoProposta.AguardandoFluxo,
                    SituacaoProposta.FluxoEnviado,
                    SituacaoProposta.AguardandoIntegracao,
                    SituacaoProposta.Cancelada,
                    SituacaoProposta.Integrada,
                    SituacaoProposta.Reprovada,
                    SituacaoProposta.EmAnaliseCompleta,
                    SituacaoProposta.AguardandoAnaliseCompleta,
                    SituacaoProposta.AnaliseCompletaAprovada
                };
                filtro.Situacoes = situacoesParaVisualizacao;
            }

            return filtro;
        }

        private ConsultaPrePropostaDto AplicarFiltroAuxiliar(ConsultaPrePropostaDto filtro)
        {
            filtro.IdsViabilizadores = new List<long> { };

            var idPerfisUsuario = SessionAttributes.Current().Perfis.Select(x => x.Id).ToList();
            var isAdm = idPerfisUsuario.Contains(ProjectProperties.IdPerfilAdministrador);

            if (isAdm)
            {
                return filtro;
            }

            if (idPerfisUsuario.Contains(ProjectProperties.IdPerfilViabilizadorHouse))
            {
                filtro.IdsPontoVenda = _pontoVendaRepository.BuscarPontosVendaPorViabilizador(SessionAttributes.Current().UsuarioPortal.Id).Select(x => x.Id).ToList();
                filtro.IdsPontoVenda.Add(-1);
            }

            if (filtro.IdsPontoVenda.IsEmpty())
            {
                filtro.IdsPontoVenda = new List<long> {};
            }

            if (idPerfisUsuario.Contains(ProjectProperties.IdPerfilSupervisorCicloFinanceiro))
            {
                filtro.IsSupervisor = true;

                var idsViabilizadores = _supervisorViabilizadorRepository.ViabilizadorByIdSupervisor(SessionAttributes.Current().UsuarioPortal.Id);
                filtro.IdsViabilizadores.AddRange(idsViabilizadores);
                filtro.IdsViabilizadores.Add(SessionAttributes.Current().UsuarioPortal.Id);

                var pontosVenda = _pontoVendaRepository.BuscarPontosVendaPorViabilizador(idsViabilizadores).Select(x => x.Id).ToList();

                filtro.IdsPontoVenda.AddRange(pontosVenda);

            }

            if (idPerfisUsuario.Contains(ProjectProperties.IdPerfilCoordenadorCicloFinanceiro))
            {
                filtro.IsCoordenador = true;

                var idsSupervisores = _coordenadorSupervisorRepository.IdsSupervisorByCoordenador(SessionAttributes.Current().UsuarioPortal.Id);
                var idsViabilizadorSupervisor = _supervisorViabilizadorRepository.ViabilizadorByIdSupervisor(idsSupervisores);

                var idsViabilizadoresCoordenador = _coordenadorViabilizadorRepository.IdsViabilizadorByCoordenador(SessionAttributes.Current().UsuarioPortal.Id);

                idsViabilizadoresCoordenador.AddRange(idsViabilizadorSupervisor);

                filtro.IdsViabilizadores.AddRange(idsViabilizadoresCoordenador);

                var pontosVenda = _pontoVendaRepository.BuscarPontosVendaPorViabilizador(idsViabilizadoresCoordenador).Select(x => x.Id).ToList();

                filtro.IdsPontoVenda.AddRange(pontosVenda);
            }

            return filtro;
        }

    }
}