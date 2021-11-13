using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG01")]
    public class RelatorioComissaoController : BaseController
    {
        private ViewRelatorioComissaoRepository _relatorioComissaoRepository { get; set; }
        private RelatorioVendaUnificadoService _relatorioVendaUnificadoService { get; set; }

        [BaseAuthorize("PAG01", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);
            var result = _relatorioComissaoRepository.Listar(filtro);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG01", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.RelatorioDeVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("PAG01", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);
            byte[] file = Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.RelatorioDeVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var results = _relatorioComissaoRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(20)
                    .CreateCellValue(model.Divisao).Width(20)
                    .CreateCellValue(model.NomeEmpreendimento).Width(40)
                    .CreateCellValue(model.EmpresaDeVenda).Width(20)
                    .CreateCellValue(model.CentralVenda).Width(20)
                    .CreateCellValue(model.NomePontoVenda).Width(40)
                    .CreateCellValue(model.NomeCliente.ToString()).Width(40)
                    .CreateCellValue(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.DataSicaqPreproposta.HasValue ? model.DataSicaqPreproposta.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.StatusContrato).Width(20)
                    .CreateCellValue(model.EmReversao == true ? "Sim" : "Não").Width(20)
                    .CreateCellValue(model.StatusRepasse).Width(20)
                    .CreateCellValue(model.DataRepasse.HasValue ? model.DataRepasse.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.StatusConformidade).Width(20)
                    .CreateCellValue(model.DataConformidade.HasValue ? model.DataConformidade.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.PassoAtual).Width(20)
                    .CreateCellValue(model.DataKitCompleto.HasValue ? model.DataKitCompleto.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.CodigoRegra).Width(20)
                    //.CreateCellValue(model.FaixaUmMeio.ToString() + '%').Width(10)
                    //.CreateCellValue(model.FaixaDois.ToString() + '%').Width(10)
                    .CreateCellValue(RenderComissao(model)).Width(20)
                    .CreateCellValue(RenderFaixa(model)).Width(20)
                    .CreateCellValue(RegraPagamento(model.RegraPagamento, model.TipoPagamento)).Width(20)
                    //.CreateCellValue(model.ComissaoPagarUmMeio.ToString() + '%').Width(10)
                    //.CreateCellValue(model.ComissaoPagarDois.ToString() + '%').Width(10)
                    .CreateCellValue(RenderComissaoAPagar(model)).Width(20)
                    .CreateCellValue(model.CodigoFornecedor).Width(20)
                    .CreateCellValue(model.NomeFornecedor).Width(20)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(20)
                    .CreateCellValue(model.DescricaoTorre).Width(20)
                    .CreateCellValue(model.DescricaoUnidade).Width(20)
                    .CreateCellValue(model.DataVenda.HasValue ? model.DataVenda.Value.ToDate() : empty).Width(20)
                    .CreateMoneyCell(Convert.ToDecimal( (model.ValorVGV.HasValue?model.ValorVGV.Value.ToString("F"):"0"))).Width(30)
                    .CreateCellValue(model.CodigoPreProposta).Width(20)
                    .CreateCellValue(model.CodigoProposta).Width(20)
                    .CreateCellValue(model.DescricaoTipologia).Width(20)
                    .CreateCellValue(model.DataRegistro.HasValue ? model.DataRegistro.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.Pago == true ? "Sim" : "Não").Width(20)
                    .CreateCellValue(model.DataPagamento.HasValue ? model.DataPagamento.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.Faturado?GlobalMessages.Sim:GlobalMessages.Nao)
                    .CreateCellValue(model.Observacao).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.Divisao,
                GlobalMessages.Empreendimento,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Central,
                GlobalMessages.PontoVenda,
                GlobalMessages.Cliente,
                GlobalMessages.DataSicaqValidade,
                GlobalMessages.DataSicaq,
                GlobalMessages.StatusContrato,
                GlobalMessages.Reversao,
                GlobalMessages.StatusRepasse,
                GlobalMessages.DataRepasse,
                GlobalMessages.StatusConformidade,
                GlobalMessages.DataConformidade,
                GlobalMessages.StatusPRO,
                GlobalMessages.DataKitCompleto,
                GlobalMessages.CodigoRegra,
                //GlobalMessages.ComissaoAcordadoUmMeio,
                //GlobalMessages.ComissaoAcordadoDois,
                GlobalMessages.Comissao,
                GlobalMessages.Faixa,
                GlobalMessages.RegraPagamento,
                //GlobalMessages.ComissaoPagarUmMeio,
                //GlobalMessages.ComissaoPagarDois,
                GlobalMessages.ComissaoPagar,
                GlobalMessages.CodigoFornecedor,
                GlobalMessages.NomeFornecedor,
                GlobalMessages.Empresa,
                GlobalMessages.Bloco,
                GlobalMessages.Unidade,
                GlobalMessages.DataVenda,
                GlobalMessages.ValorSemPremiada,
                GlobalMessages.PreProposta,
                GlobalMessages.Proposta,
                GlobalMessages.Tipologia,
                GlobalMessages.DataRegistro,
                GlobalMessages.Pago,
                GlobalMessages.DataPedido,
                GlobalMessages.Faturado,
                GlobalMessages.Observacao
            };
            return header.ToArray();
        }

        public string RegraPagamento(string regraPagamento, TipoPagamento tipo)
        {
            switch (tipo)
            {
                case TipoPagamento.KitCompleto:
                    return regraPagamento + "% Kit Completo";
                    break;
                case TipoPagamento.Repasse:
                    return regraPagamento + "% Repasse";
                    break;
                case TipoPagamento.Conformidade:
                    return regraPagamento + "% Conformidade";
                    break;
                default:
                    break;
            }
            return "";
        }

        public string RenderComissao(ViewRelatorioComissao model)
        {
            if (model.Modalidade == TipoModalidadeComissao.Fixa)
            {
                if (model.FlagFaixaUmMeio)
                {
                    return model.FaixaUmMeio + "%";
                }
                return model.FaixaDois + "%";
            }
            return model.Faixa + "%";
        }

        public string RenderFaixa(ViewRelatorioComissao model)
        {
            if (model.Tipologia.HasValue())
            {
                return model.Tipologia.AsString();
            }

            if (model.FlagFaixaUmMeio)
            {
                return Tipologia.FaixaUmMeio.AsString();
            }

            return Tipologia.FaixaDois.AsString();
        }

        public string RenderComissaoAPagar(ViewRelatorioComissao model)
        {
            if (model.Tipologia.HasValue())
            {
                switch (model.Tipologia)
                {
                    case Tipologia.PNE:
                        return model.ComissaoPagarPNE + "%";
                        break;
                    case Tipologia.FaixaUmMeio:
                        return model.ComissaoPagarUmMeio + "%";
                        break;
                    case Tipologia.FaixaDois:
                        return model.ComissaoPagarDois + "%";
                        break;
                }
            }

            if (model.FlagFaixaUmMeio)
            {
                return model.ComissaoPagarUmMeio + "%";
            }
            return model.ComissaoPagarDois + "%";
        }

        [BaseAuthorize("PAG01", "ExportarTodos")]
        public FileContentResult ExportarRelatorioVendaUnificadoTodos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _relatorioVendaUnificadoService.ExportarRelatorioVendaUnificado(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.RelatorioDeVendaUnificado;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public RelatorioComissaoDTO FiltroAuxiliar(RelatorioComissaoDTO filtro)
        {
            if (filtro.Estados.HasValue() && filtro.Estados.Contains("") )
            {
                filtro.Estados.Clear();
            }

            return filtro;
        }
    }
}