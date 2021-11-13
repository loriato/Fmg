using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Controllers;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS14")]
    public class MatrizPagadoriaController : BaseController
    {
        public RegraComissaoService _regraComissaoService { get; set; }
        public RegraComissaoRepository _regraComissaoRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }

        [BaseAuthorize("EVS14", "Visualizar")]
        public ActionResult Index(String regional = null, long[] evendas = null, long regra = 0, bool ultimo = false,long idEmpresaVenda = 0)
        {
            RegraComissao item;

            if (regra > 0)
            {
                item = _regraComissaoRepository.FindById(regra);
            }
            else
            {
                item = new RegraComissao();
                if (regional.HasValue())
                {
                    item.Regional = regional;
                }
            }
            
            ViewBag.EmpresasVendasSelecionadas = "";

            if (evendas.HasValue())
            {
                item.Situacao = SituacaoRegraComissao.Rascunho;
                item.InicioVigencia = null;
                item.TerminoVigencia = null;

                foreach (var evenda in evendas)
                {
                    ViewBag.EmpresasVendasSelecionadas += evenda + ",";
                }
            }
            else if (regra > 0)
            {
                var idEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regra)
                    .Select(x => x.EmpresaVenda.Id)
                    .ToList();

                foreach(var idEv in idEvs)
                {
                    ViewBag.EmpresasVendasSelecionadas += idEv + ",";
                }

                ViewBag.NomeEmpresaVenda = _regraComissaoEvsRepository.BuscarPorRegraComissao(regra)
                    .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                    .Select(x => x.EmpresaVenda.NomeFantasia)
                    .FirstOrDefault();
            }

            if (item.Arquivo.HasValue())
            {
                item.Arquivo = new Arquivo
                {
                    Id = item.Arquivo.Id
                };
            }

            ViewBag.EvsEdicao = evendas;
            ViewBag.Novo = evendas.HasValue();
            ViewBag.PorUltimaAtualizacao = ultimo;
            ViewBag.IdEmpresaVenda = idEmpresaVenda;

            return View(item);
        }

        [HttpPost]
        public ActionResult BuscarMatriz(long? idRegraComissao, long[] listaEvs, string regional, bool editable,
            bool novo, bool ultimaAtt, TipoModalidadeComissao? modalidade)
        {
            var option = _regraComissaoService.BuscarMatriz(idRegraComissao == null ? 0 : idRegraComissao.Value,
                listaEvs, regional, editable, novo, ultimaAtt, modalidade.Value);
            var str = JsonConvert.SerializeObject(option, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "Incluir")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(List<ItemRegraComissao> itens, RegraComissao regraComissao)
        {
            var response = new JsonResponse();
            try
            {
                var result = _regraComissaoService.SalvarMatriz(itens, regraComissao, SessionAttributes.Current().UsuarioPortal);

                var mensagem = (result.Situacao == SituacaoRegraComissao.Rascunho
                                         ? GlobalMessages.Rascunho + " "
                                         : "") + GlobalMessages.RegraComissao + " - " +
                                     result.Regional;
                response.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, result.Descricao));

                response.Sucesso = true;
                response.Objeto = new
                {
                    Id = result.Id,
                    Descricao = result.Descricao
                };

            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);

                CurrentTransaction().Rollback();
            }

            return Json(response, JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public ActionResult DownloadPdf(long regraComissao, long? empresaVenda)
        {
            var regra = _regraComissaoRepository.FindById(regraComissao);
            return File(regra.Arquivo.Content, regra.Arquivo.ContentType, regra.Arquivo.Nome);
        }

        [BaseAuthorize("EVS14", "Incluir")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult GerarPdf(long regraComissao)
        {
            try
            {
                var fileName = "LogoTendaPdf.png";
                var TargetPath = @"/Static/images/";
                var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
                byte[] logo = System.IO.File.ReadAllBytes(path);

                var regra = _regraComissaoRepository.FindById(regraComissao);
                if (regra.Situacao != SituacaoRegraComissao.Rascunho)
                {
                    var bre = new BusinessRuleException();
                    bre.AddError(GlobalMessages.MsgRegraComissaoNaoGerarPdf).Complete();
                    bre.ThrowIfHasError();
                }

                var rc = _regraComissaoService.GerarPdf(regraComissao, logo);
                return Json(new JsonResponse
                {
                    Sucesso = true,
                    Mensagens = new List<string> { GlobalMessages.PdfGeradoComSucesso }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessRuleException bre)
            {
                return Json(new JsonResponse
                {
                    Sucesso = false,
                    Mensagens = bre.Errors
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GerarExcel(long? idRegraComissao, long[] listaEvs, string regional, bool ultimaAtt)
        {
            var excel = _regraComissaoService.GerarExcel(idRegraComissao == null ? 0 : idRegraComissao.Value,
                listaEvs, regional, ultimaAtt);
            string nomeArquivo = GlobalMessages.MatrizPagadoriaTenda + "-" + regional;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(excel, MimeMappingWrapper.Xlsx, $"{nomeArquivo.Trim()}_{date}.xlsx");
        }
    }
}