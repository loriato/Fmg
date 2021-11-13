using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS20")]
    public class ProgramaFidelidadeController : BaseController
    {
        public PontuacaoFidelidadeEmpresaVendaRepository _pontuacaoFidelidadeEmpresaVendaRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaService _pontuacaoFidelidadeEmpresaVendaService { get; set; }
        public ViewResgatePontuacaoFidelidadeRepository _viewResgatePontuacaoFidelidadeRepository { get; set; }
        public LayoutProgramaFidelidadeRepository _layoutProgramaFidelidadeRepository { get; set; }
        public StaticResourceService _staticResourceService { get; set; }
        private AceiteTermoAceiteProgramaFidelidadeRepository _aceiteTermoAceiteProgramaFidelidadeRepository { get; set; }
        private TermoAceiteProgramaFidelidadeRepository _termoAceiteProgramaFidelidadeRepository { get; set; }

        [BaseAuthorize("EVS20", "Visualizar")]
        public ActionResult Index()
        {
            var TermoAceite = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            if (TermoAceite.HasValue())
            {
                var aceite = _aceiteTermoAceiteProgramaFidelidadeRepository.VerificarEVAceite(SessionAttributes.Current().EmpresaVendaId, TermoAceite.Id);
                if (!aceite)
                {
                    return RedirectToAction("FluxoTermoAceite");
                }
            }
            var pontos = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(SessionAttributes.Current().EmpresaVendaId);

            var pontuacao = new ProgramaFidelidadeDTO();

            if (!pontos.IsEmpty())
            {
                pontuacao.PontuacaoTotal = pontos.PontuacaoTotal;
                pontuacao.PontuacaoIndisponivel = pontos.PontuacaoIndisponivel;
                pontuacao.PontuacaoDisponivel = pontos.PontuacaoDisponivel;
                pontuacao.PontuacaoResgatada = pontos.PontuacaoResgatada;
                pontuacao.PontuacaoSolicitada = pontos.PontuacaoSolicitada;
                pontuacao.PrimeiroAcesso = true;
                //pontuacao.LinkBanner = "https://media.istockphoto.com/vectors/the-abstract-halftone-background-consists-of-different-dots-vector-id1064033650";
            }

            var layout = _layoutProgramaFidelidadeRepository.LayoutAtivo();

            if (layout.HasValue())
            {
                var resourcePath = _staticResourceService.LoadResource(layout.BannerParceiroExclusivo.Id);
                string webRoot = GetWebAppRoot();
                pontuacao.LinkBanner = _staticResourceService.CreateUrl(webRoot, resourcePath);
            }

            return View(pontuacao);
        }
        [BaseAuthorize("EVS20", "Visualizar")]
        public ActionResult FluxoTermoAceite()
        {
            if (!SessionAttributes.Current().IsDiretor()) { return View("AcessoNaoPermitido"); }

            var termoAceiteProgramaFidelidade = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            var resourcePath = _staticResourceService.LoadResource(termoAceiteProgramaFidelidade.Arquivo.Id);
            var urlTermoAceite = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);

            var dto = new TermoAceiteDTO();
            dto.TermoAceite = urlTermoAceite;

            return View(dto);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Avancar()
        {
            var aceiteTermo = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            var usuario = SessionAttributes.Current().UsuarioPortal;

            AceiteTermoAceiteProgramaFidelidade aceiteTermoProgramaFidelidade = new AceiteTermoAceiteProgramaFidelidade
            {
                TermoAceiteProgramaFidelidade = aceiteTermo,
                EmpresaVenda = SessionAttributes.Current().EmpresaVenda,
                Aprovador = usuario,
                DataAceite = DateTime.Now,
                Acesso = SessionAttributes.Current().Acesso
            };

            _aceiteTermoAceiteProgramaFidelidadeRepository.Save(aceiteTermoProgramaFidelidade);
            return RedirectToAction("Index");
        }
        [BaseAuthorize("EVS20", "SolicitarResgate")]
        public ActionResult ResgatePontos()
        {
            var pontos = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(SessionAttributes.Current().EmpresaVendaId);

            var pontuacao = new ProgramaFidelidadeDTO();

            if (!pontos.IsEmpty())
            {
                pontuacao.PontuacaoDisponivel = pontos.PontuacaoDisponivel;
                //pontuacao.LinkBanner = "https://media.istockphoto.com/vectors/the-abstract-halftone-background-consists-of-different-dots-vector-id1064033650";

            }

            var layout = _layoutProgramaFidelidadeRepository.LayoutAtivo();

            if (layout.HasValue())
            {
                var resourcePath = _staticResourceService.LoadResource(layout.BannerParceiroExclusivo.Id);
                string webRoot = GetWebAppRoot();
                pontuacao.LinkBanner = _staticResourceService.CreateUrl(webRoot, resourcePath);
            }

            return View(pontuacao);
        }

        [HttpPost]
        [BaseAuthorize("EVS20", "SolicitarResgate")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SolicitarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var response = new JsonResponse();

            try
            {
                pontuacao.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
                pontuacao.IdSolicitadoPor = SessionAttributes.Current().UsuarioPortal.Id;
                _pontuacaoFidelidadeEmpresaVendaService.SolicitarResgate(pontuacao);
                response.Sucesso = true;
                response.Mensagens.Add(GlobalMessages.MsgConfirmacaoSolicitacaoResgate);

            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS20", "Visualizar")]
        public ActionResult MeusResgates()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("EVS20", "Visualizar")]
        public JsonResult ListarResgate(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
            var result = _viewResgatePontuacaoFidelidadeRepository.ListarResgate(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS20", "Visualizar")]
        public ActionResult ComoFunciona()
        {
            var pontuacao = new ProgramaFidelidadeDTO();

            var layout = _layoutProgramaFidelidadeRepository.LayoutAtivo();

            if (layout.HasValue())
            {
                pontuacao.NomeParceiroExclusivo = layout.NomeParceiroExclusivo;
                pontuacao.LinkParceiroExclusivo = layout.LinkParceiroExclusivo;

                var resourcePath = _staticResourceService.LoadResource(layout.BannerPontos.Id);
                string webRoot = GetWebAppRoot();
                pontuacao.LinkBanner = _staticResourceService.CreateUrl(webRoot, resourcePath);

            }
            pontuacao.TermoAceite = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite().HasValue();

            return View(pontuacao);
        }

        public FileContentResult DownloadPdf()
        {
            var idEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
            var fileName = "LogoTendaPdf.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);
            var arquivo = _pontuacaoFidelidadeEmpresaVendaService.GerarPdf(idEmpresaVenda, logo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        [BaseAuthorize("EVS20", "Visualizar")]
        public ActionResult TermoAceite()
        {
            var termoAceiteProgramaFidelidade = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            var resourcePath = _staticResourceService.LoadResource(termoAceiteProgramaFidelidade.Arquivo.Id);
            var urlTermoAceite = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);

            var dto = new TermoAceiteDTO();
            dto.TermoAceite = urlTermoAceite;

            return View(dto);
        }

        public ActionResult DownloadTermoAceite()
        {
            var aceite = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            return File(aceite.Arquivo.Content, aceite.Arquivo.ContentType, aceite.Arquivo.Nome);
        }
    }
}