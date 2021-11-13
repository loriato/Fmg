using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;
using RegraComissaoDTO = Tenda.EmpresaVenda.Web.Models.RegraComissaoDTO;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS15")]
    public class RegraComissaoPadraoController : BaseController
    {
        public RegraComissaoPadraoService _regraComissaoPadraoService { get; set; }
        public RegraComissaoPadraoRepository _regraComissaoPadraoRepository { get; set; }
        public ViewRegraComissaoPadraoRepository _viewRegraComissaoPadraoRepository { get; set; }

        [BaseAuthorize("EVS15", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS15", "Visualizar")]
        public ActionResult Matriz(bool? create, String regional = null, long regra = 0, bool ultimo = false)
        {
            RegraComissaoPadrao item;

            if (regra > 0)
            {
                item = _regraComissaoPadraoRepository.FindById(regra);
            }
            else
            {
                item = new RegraComissaoPadrao();
                if (regional.HasValue())
                {
                    item.Regional = regional;
                }
            }

            if (item.Arquivo.HasValue())
            {
                item.Arquivo = new Arquivo
                {
                    Id = item.Arquivo.Id
                };
            }
            
            ViewBag.Novo = create == true;
            ViewBag.PorUltimaAtualizacao = ultimo;

            return View(item);
        }

        [BaseAuthorize("EVS15", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            var result = _viewRegraComissaoPadraoRepository.Listar(filtro);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS15", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(RegraComissaoDTO model)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                RegraComissaoPadrao regra = new RegraComissaoPadrao();
                regra.Descricao = model.Descricao;
                regra.Regional = model.Regional;
                regra.Situacao = SituacaoRegraComissao.Rascunho;

                _regraComissaoPadraoService.Salvar(regra, model.Arquivo, bre);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, regra.ChaveCandidata(),
                    GlobalMessages.Incluido.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS15", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Atualizar(RegraComissaoDTO model)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            RegraComissaoPadrao regra = new RegraComissaoPadrao();
            try
            {
                regra = _regraComissaoPadraoRepository.FindById(model.Id.Value);
                if (regra.IsEmpty())
                {
                    bre.AddError(GlobalMessages.RegraComissaoNaoEncontrada).Complete();
                }

                if (model.Descricao.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                        .AddField("Descricao").Complete();
                }

                bre.ThrowIfHasError();

                regra.Descricao = model.Descricao;
                _regraComissaoPadraoRepository.Save(regra);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, regra.ChaveCandidata(),
                    GlobalMessages.Atualizado.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS15", "Liberar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Liberar(long idRegra)
        {
            var json = new JsonResponse();
            try
            {
                var regra = _regraComissaoPadraoRepository.FindById(idRegra);

                _regraComissaoPadraoService.Liberar(regra);

                var fileName = "LogoTendaPdf.png";
                var TargetPath = @"/Static/images/";
                var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
                byte[] logo = System.IO.File.ReadAllBytes(path);

                _regraComissaoPadraoService.GerarPdf(idRegra, logo);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroLiberadoComSucesso, regra.ChaveCandidata()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #region Matriz

        [HttpPost]
        public ActionResult BuscarMatriz(long? idRegraComissao, string regional, bool editable,
            bool novo, bool ultimaAtt, TipoModalidadeComissao? modalidade)
        {
            var option = _regraComissaoPadraoService.BuscarMatriz(idRegraComissao == null ? 0 : idRegraComissao.Value,
                regional, editable, novo, ultimaAtt,modalidade.Value);
            var str = JsonConvert.SerializeObject(option, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS15", "Incluir")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarMatriz(List<ItemRegraComissaoPadraoDto> itens, RegraComissaoPadrao regraComissao)
        {
            var itensRegras = new List<ItemRegraComissaoPadrao>();
            foreach (var item in itens)
            {
                var itemRegraComissao = new ItemRegraComissaoPadrao
                {
                    Id = item.IdItemRegraComissaoPadrao,
                    Empreendimento = new Empreendimento
                    {
                        Id = item.IdEmpreendimento
                    },
                    RegraComissaoPadrao = regraComissao,
                    FaixaUmMeio = item.FaixaUmMeio,
                    FaixaDois = item.FaixaDois,
                    ValorConformidade = item.ValorConformidade,
                    ValorRepasse = item.ValorRepasse,
                    ValorKitCompleto = item.ValorKitCompleto,
                    MenorValorNominalUmMeio = item.MenorValorNominalUmMeio,
                    IgualValorNominalUmMeio = item.IgualValorNominalUmMeio,
                    MaiorValorNominalUmMeio = item.MaiorValorNominalUmMeio,
                    MenorValorNominalDois = item.MenorValorNominalDois,
                    IgualValorNominalDois = item.IgualValorNominalDois,
                    MaiorValorNominalDois = item.MaiorValorNominalDois,
                    MenorValorNominalPNE = item.MenorValorNominalPNE,
                    IgualValorNominalPNE = item.IgualValorNominalPNE,
                    MaiorValorNominalPNE = item.MaiorValorNominalPNE,
                    Modalidade = item.Modalidade
                };
                itensRegras.Add(itemRegraComissao);
            }

            var json = new JsonResponse();
            try
            {
                var regra = _regraComissaoPadraoService.SalvarMatriz(itensRegras, regraComissao);

                json.Sucesso = true;
                var tituloRegistro = (regra.Situacao == SituacaoRegraComissao.Rascunho
                                         ? GlobalMessages.Rascunho + " "
                                         : "") + GlobalMessages.RegraComissao + " - " +
                                     regra.Regional;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, tituloRegistro));
                json.Objeto = new RegraComissao
                {
                    Id = regra.Id,
                    Descricao = regra.Descricao
                };
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
                json.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS15", "Incluir")]
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

                var regra = _regraComissaoPadraoRepository.FindById(regraComissao);
                if (regra.Situacao != SituacaoRegraComissao.Rascunho)
                {
                    var bre = new BusinessRuleException();
                    bre.AddError(GlobalMessages.MsgRegraComissaoNaoGerarPdf).Complete();
                    bre.ThrowIfHasError();
                }

                var rc = _regraComissaoPadraoService.GerarPdf(regraComissao, logo);
                return Json(new JsonResponse
                {
                    Sucesso = true,
                    Mensagens = new List<string> {GlobalMessages.PdfGeradoComSucesso}
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
        public ActionResult GerarExcel(long? idRegraComissao, string regional, bool ultimaAtt)
        {
            var excel = _regraComissaoPadraoService.GerarExcel(idRegraComissao == null ? 0 : idRegraComissao.Value,
                regional, ultimaAtt);
            string nomeArquivo = GlobalMessages.MatrizPagadoriaTenda + "-" + regional;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(excel, MimeMappingWrapper.Xlsx, $"{nomeArquivo.Trim()}_{date}.xlsx");
        }

        [BaseAuthorize("EVS15", "DownloadRegraComissao")]
        public FileContentResult DownloadRegraComissao(long idRegra)
        {
            var regra = _regraComissaoPadraoRepository.FindById(idRegra);
            return File(regra.Arquivo.Content, regra.Arquivo.ContentType, regra.Arquivo.Nome);
        }

        #endregion
    }
}