using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class PosVendaController : BaseController
    {
        private ViewConsultaPosVendaRepository _viewConsultaPosVendaRepository { get; set; }
        private StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }
        private ViewRelatorioComissaoRepository _viewRelatorioComissaoRepository { get; set; }
        private StatusConformidadeRepository _statusConformidadeRepository { get; set; }

        [BaseAuthorize("EVS16", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarDadosPosVendas(DataSourceRequest request, PosVendaDTO filtro)
        {
            filtro = PreFiltro(filtro);
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;

            var dados = new DadosGraficoPosVendaDTO();
            dados.PrePropostasFinalizadas = new List<int>();
            var query = _viewConsultaPosVendaRepository.Listar(filtro);

            var quantidade = _viewConsultaPosVendaRepository.ListarSemDocsAvalista(filtro);
            dados.PrePropostasFinalizadas.Add(quantidade);
            quantidade = _viewConsultaPosVendaRepository.ListarDocsAvalistaEnviados(filtro);
            dados.PrePropostasFinalizadas.Add(quantidade);
            quantidade = _viewConsultaPosVendaRepository.ListarAvalistaPreAprovado(filtro);
            dados.PrePropostasFinalizadas.Add(quantidade);

            dados.PosVendasConformidade = new List<int>();
            dados.DiasPosVendasConformidade = new List<string>();

            dados.TotalKitCompleto = _viewConsultaPosVendaRepository.TotalKitCompleto(filtro);
            dados.TotalRepasse = _viewConsultaPosVendaRepository.TotalRepasse(filtro);
            dados.TotalConformidade = _viewConsultaPosVendaRepository.TotalConformidade(filtro);

            var date = DateTime.Now.AddDays(-7);

            for (var i = 0; i < 0; i++)
            {
                if (i < 8)
                {
                    var count = _viewConsultaPosVendaRepository
                        .Queryable()
                        .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                        .Where(x => x.DataConformidade != null)
                        .Where(x => x.DataConformidade.Value.Date == date.Date)
                        .Count();
                    dados.PosVendasConformidade.Add(count);
                }

                var dayName = date.ToString("ddd");
                dayName = dayName[0].ToString().ToUpper() + dayName.Substring(1);
                dados.DiasPosVendasConformidade.Add(dayName);
                date = date.AddDays(1);
            }

            var queryRepasse = _viewConsultaPosVendaRepository.Queryable()
                .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.DataRepasse != null);

            var dataPreAnterior = filtro.Inicio.AddDays(-7);

            if (dataPreAnterior.DayOfWeek == DayOfWeek.Saturday)
            {
                dataPreAnterior = dataPreAnterior.AddDays(-1);
            }

            if (dataPreAnterior.DayOfWeek == DayOfWeek.Sunday)
            {
                dataPreAnterior = dataPreAnterior.AddDays(-2);
            }

            var quantidadePreAnterior = queryRepasse
                .Count(x => x.DataRepasse == dataPreAnterior);
            var quantidadeInicio = queryRepasse
                .Count(x => x.DataRepasse == filtro.Inicio);
            var quantidadeTermino = queryRepasse
                .Count(x => x.DataRepasse == filtro.Termino);

            if (quantidadePreAnterior == 0 && quantidadeInicio == 0)
            {
                dados.PorcentagemAnterior = 0;
            }
            else
            {
                if (quantidadePreAnterior == 0)
                {
                    quantidadePreAnterior = 1;
                }

                dados.PorcentagemAnterior = (((decimal) quantidadeInicio) * 100 / quantidadePreAnterior) - 100;
            }

            if (quantidadeInicio == 0 && quantidadeTermino == 0)
            {
                dados.PorcentagemAtual = 0;
            }
            else
            {
                if (quantidadeInicio == 0)
                {
                    quantidadeInicio = 1;
                }

                dados.PorcentagemAtual = (((decimal) quantidadeTermino) * 100 / quantidadeInicio) - 100;
            }

            var json = new JsonResponse();
            json.Sucesso = true;
            json.Objeto = dados;

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarConsultaPosVenda(DataSourceRequest request, PosVendaDTO filtro)
        {
            filtro = PreFiltro(filtro);
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;

            var result = _viewConsultaPosVendaRepository.Listar(request, filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderSituacaoPreProposta()
        {
            var vipList = new List<string>();
            vipList.Add("KIT COMPLETO");
            vipList.Add("KIT ENVIADO");
            vipList.Add("KIT PENDENTE");
            vipList.Add("KIT REENVIADO");
            vipList.Add("VENDA GERADA");
            vipList.Add("AGUARDANDO ASSINATURA DIGITAL");
            vipList.Add("MARCADA PARA ASSINATURA DIGITAL");

            var results = _statusSuatStatusEvsRepository.Listar()
                .Where(x => vipList.Contains(x.DescricaoSuat.ToUpper()));

            var list = results.Select(x => new SelectListItem {Text = x.DescricaoEvs, Value = x.Id.ToString()});

            return PartialView("_RenderSituacaoPrePropostaDropdownList", list);
        }

        public FileContentResult ExportarTodos(DataSourceRequest request, PosVendaDTO dto)
        {
            dto.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;

            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, dto);
            string nomeArquivo = GlobalMessages.ConsultaPosVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public FileContentResult ExportarPagina(DataSourceRequest request, PosVendaDTO dto)
        {
            dto.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;

            byte[] file = Exportar(request, dto);
            string nomeArquivo = GlobalMessages.ConsultaPosVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, PosVendaDTO dto)
        {
            var results = _viewConsultaPosVendaRepository.Listar(request, dto);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.CodigoPreProposta).Width(25)
                    .CreateCellValue(model.CodigoProposta).Width(25)
                    .CreateCellValue(model.NomeClientePreProposta).Width(20)
                    .CreateCellValue(model.SituacaoPreProposta)
                    .CreateCellValue(model.NomeCorretor)
                    .CreateCellValue(RenderSituacaoContrato(model));
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.CodigoPreProposta,
                GlobalMessages.CodigoProposta,
                GlobalMessages.Cliente,
                GlobalMessages.Situacao,
                GlobalMessages.Corretor,
                GlobalMessages.SituacaoContrato
            };
            return header.ToArray();
        }

        public ActionResult RenderStatusConformidade()
        {
            var vipList = _statusConformidadeRepository.Listar();

            var results = vipList.Select(x => new SelectListItem { Text = x.DescricaoEvs, Value = x.DescricaoEvs.ToLower() });

            return PartialView("_RenderStatusConformidadeDropdownList", results);
        }

        public string RenderSituacaoContrato(ViewConsultaPosVenda view)
        {
            if(view.DataConformidade!=null && view.DataRepasse != null)
            {
                return SituacaoContrato.Repassado.AsString() + "/" + SituacaoContrato.Conforme.AsString();
            }

            if (view.DataRepasse != null)
            {
                return SituacaoContrato.Repassado.AsString();
            }

            if (view.DataConformidade != null)
            {
                return SituacaoContrato.Conforme.AsString();
            }

            return String.Empty;
        }
        public PosVendaDTO PreFiltro(PosVendaDTO filtro)
        {
            var vipList = new List<string>();
            vipList.Add("KIT COMPLETO");
            vipList.Add("KIT ENVIADO");
            vipList.Add("KIT PENDENTE");
            vipList.Add("KIT REENVIADO");
            vipList.Add("VENDA GERADA");
            vipList.Add("AGUARDANDO ASSINATURA DIGITAL");
            vipList.Add("MARCADA PARA ASSINATURA DIGITAL");

            var aux = _statusSuatStatusEvsRepository.Listar()
                .Where(x => vipList.Contains(x.DescricaoSuat.ToUpper()));

            filtro.SituacaoKitCompleto = aux.Where(x => x.DescricaoSuat.ToUpper().Equals("KIT COMPLETO")).Select(x => x.Id).FirstOrDefault();

            return filtro;
        }
    }
}