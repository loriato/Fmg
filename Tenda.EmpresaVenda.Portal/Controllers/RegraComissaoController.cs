using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class RegraComissaoController : BaseController
    {
        private RegraComissaoService _regraComissaoService { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        private AceiteRegraComissaoRepository _aceiteRegraComissaoRepository { get; set; }
        private TokenAceiteService _tokenAceiteService { get; set; }
        private TokenAceiteRepository _tokenAceiteRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private RegraComissaoEvsService _regraComissaoEvsService { get; set; }
        private AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ViewResponsavelAceiteRegraComissaoRepository _viewResponsavelAceiteRegraComissaoRepository { get; set; }

        public ActionResult Index()
        {
            var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(SessionAttributes.Current().Corretor.EmpresaVenda.Id, SessionAttributes.Current().Corretor.Id);
            if (!SessionAttributes.Current().IsDiretor() && !PodeAceitarRegraComissao) { return View("AcessoNaoPermitido"); }

            long empresaVenda = SessionAttributes.Current().EmpresaVenda.Id;

            var regraComissaoEvsVigente = _regraComissaoEvsService.RegraComissaoEvsVigente(empresaVenda);
            var aceiteMaisRecente = _aceiteRegraComissaoEvsRepository.AceiteMaisRecente(empresaVenda);

            if(!aceiteMaisRecente.IsEmpty() && aceiteMaisRecente.RegraComissaoEvs.Tipo == TipoRegraComissao.Campanha && !regraComissaoEvsVigente.IsEmpty() && regraComissaoEvsVigente.Tipo != TipoRegraComissao.Campanha)
            {
                aceiteMaisRecente = _aceiteRegraComissaoEvsRepository.SegundoAceite(empresaVenda);
            }

            // Direcionando ao fluxo de aprovação de regra de comissão
            if (aceiteMaisRecente == null || aceiteMaisRecente.RegraComissaoEvs.Id != regraComissaoEvsVigente.Id)
            {
                return RedirectToAction("FluxoAceiteRegraComissao");
            }

            var resourcePath = _staticResourceService.LoadResource(regraComissaoEvsVigente.Arquivo.Id);
            var urlRegraComissao = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);

            RegraComissaoViewModel viewModel = new RegraComissaoViewModel();
            viewModel.RegraComissao = urlRegraComissao;
            viewModel.IdRegraAceiteMaisRecente = aceiteMaisRecente.RegraComissaoEvs.Id;

            string formatoVisualizadoRegraComissaoEvs = "[{1} até {2}] {0} ";

            var todosAceites = _aceiteRegraComissaoEvsRepository.ListarTodosAceites(empresaVenda)
                .Fetch(reg => reg.RegraComissaoEvs)
                .ToList();

            viewModel.RegrasAceitas = todosAceites
                .Select(reg => new SelectListItem()
                {
                    Value = reg.RegraComissaoEvs.Id.ToString(),
                    Text =
                        string.Format(formatoVisualizadoRegraComissaoEvs,
                            reg.RegraComissaoEvs.Descricao,
                            reg.RegraComissaoEvs.InicioVigencia.Value.ToDate(),
                            reg.RegraComissaoEvs.TerminoVigencia.HasValue() ?
                            reg.RegraComissaoEvs.TerminoVigencia.Value.ToDate() : " o momento"
                            )
                })
                .ToList();

            return View(viewModel);
        }

        public ActionResult Download(long idRegraComissaoEvs)
        {
            var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(SessionAttributes.Current().Corretor.EmpresaVenda.Id, SessionAttributes.Current().Corretor.Id);
            if (!SessionAttributes.Current().IsDiretor() && !PodeAceitarRegraComissao) { return View("AcessoNaoPermitido"); }

            var regraComissaoEvs = _regraComissaoEvsRepository.FindById(idRegraComissaoEvs);
            return File(regraComissaoEvs.Arquivo.Content,
                regraComissaoEvs.Arquivo.ContentType,
                regraComissaoEvs.Arquivo.Nome);
        }

        public ActionResult DocumentoRegraComissao(long idRegraComissaoEvs)
        {
            var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(SessionAttributes.Current().Corretor.EmpresaVenda.Id, SessionAttributes.Current().Corretor.Id);
            if (!SessionAttributes.Current().IsDiretor() && !PodeAceitarRegraComissao) { return View("AcessoNaoPermitido"); }

            //long empresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
            var regraComissaoEvs = _regraComissaoEvsRepository.FindById(idRegraComissaoEvs);
            var resourcePath = _staticResourceService.LoadResource(regraComissaoEvs.Arquivo.Id);
            var urlRegraComissao = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);
            RegraComissaoViewModel viewModel = new RegraComissaoViewModel();
            viewModel.RegraComissao = urlRegraComissao;
            return Json(viewModel);
        }

        public ActionResult FluxoAceiteRegraComissao()
        {
            var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(SessionAttributes.Current().Corretor.EmpresaVenda.Id, SessionAttributes.Current().Corretor.Id);
            if (!SessionAttributes.Current().IsDiretor() && !PodeAceitarRegraComissao) { return View("AcessoNaoPermitido"); }

            var regraComissaoEvsVigente = _regraComissaoEvsService.RegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
            // Exibindo mensagem deerro no caso de falha de configuração
            if (regraComissaoEvsVigente == null)
            {
                return View("RegraComissaoNaoConfigurada");
            }
            var resourcePath = _staticResourceService.LoadResource(regraComissaoEvsVigente.Arquivo.Id);
            var urlRegraComissao = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);

            RegraComissaoViewModel viewModel = new RegraComissaoViewModel();
            viewModel.RegraComissao = urlRegraComissao;
            viewModel.RegraComissaoEvs = regraComissaoEvsVigente;
            viewModel.RegraComissaoEvsSuspensa = _regraComissaoEvsRepository.BuscarRegrasEvsSuspensas(SessionAttributes.Current().EmpresaVendaId);

            return View("FluxoAceiteRegraComissao", viewModel);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AceitarRegraComissao()
        {
            var regraComissaoEvsVigente = _regraComissaoEvsService.RegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var token = _tokenAceiteService.CriarTokenPara(usuario, regraComissaoEvsVigente);
            EnviarEmailTokenAtivacao(SessionAttributes.Current().Corretor.Email, SessionAttributes.Current().Corretor.Nome, token.Token, token.CriadoEm.AddDays(1));

            // Fazendo o redirect para que o refresh na tela não reenvie o token
            return RedirectToAction("InformarTokenAutorizacao");
        }

        public ActionResult InformarTokenAutorizacao()
        {
            return View("InformarTokenAutorizacao");
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ReenviarToken()
        {
            var regraComissaoEvsVigente = _regraComissaoEvsService.RegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var token = _tokenAceiteService.CriarTokenPara(usuario, regraComissaoEvsVigente);
            EnviarEmailTokenAtivacao(SessionAttributes.Current().Corretor.Email, SessionAttributes.Current().Corretor.Nome, token.Token, token.CriadoEm.AddDays(1));

            return Json(new { });
        }

        private void EnviarEmailTokenAtivacao(string destinatario, string nome, string token, DateTime dataExpiracao)
        {
            string siteUrl = ProjectProperties.EvsBaseUrl;
            var imgFooter = siteUrl + "/static/images/template-email/footer.png";
            var imgHeader = siteUrl + "/static/images/template-email/header.png";
            var imgLeft = siteUrl + "/static/images/template-email/left.png";
            var imgRight = siteUrl + "/static/images/template-email/right.png";
            Dictionary<string, string> toReplace = new Dictionary<string, string>();
            toReplace.Add("imgFooter", imgFooter);
            toReplace.Add("imgHeader", imgHeader);
            toReplace.Add("imgLeft", imgLeft);
            toReplace.Add("imgRight", imgRight);
            toReplace.Add("token", token);
            toReplace.Add("nome", nome);
            toReplace.Add("dataExpiracao", dataExpiracao.ToDateTime());
            var templateName = "token-regra-comissao.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);
            var email = EmailService.CriarEmail(destinatario, "[Tenda] Aceite de Regra de Comissão - Token", corpoEmail);
            EmailService.EnviarEmail(email);
        }

        public ActionResult VerificarToken(string token)
        {
            var json = new JsonResponse();
            var regraComissaoEvs = _regraComissaoEvsService.RegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var tokenSelecionado = _tokenAceiteRepository.TokenAtivoDe(usuario, regraComissaoEvs, token);
            json.Sucesso = tokenSelecionado == null ? false : true;
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [Transaction(TransactionAttributeType.Required)]
        public ActionResult InformarToken(string token)
        {
            var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(SessionAttributes.Current().Corretor.EmpresaVenda.Id, SessionAttributes.Current().Corretor.Id);
            if (!SessionAttributes.Current().IsDiretor() && !PodeAceitarRegraComissao) { return View("AcessoNaoPermitido"); }

            var regraComissaoEvs = _regraComissaoEvsService.RegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
            var usuario = SessionAttributes.Current().UsuarioPortal;

            var tokenSelecionado = _tokenAceiteRepository.TokenAtivoDe(usuario, regraComissaoEvs, token);

            if (tokenSelecionado == null)
            {
                var json = new JsonResponse();
                json.Sucesso = false;
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            tokenSelecionado.Ativo = false;

            AceiteRegraComissaoEvs aceiteRegraComissaoEvs = new AceiteRegraComissaoEvs
            {
                RegraComissaoEvs = regraComissaoEvs,
                EmpresaVenda = SessionAttributes.Current().EmpresaVenda,
                Aprovador = usuario,
                DataAceite = DateTime.Now,
                TokenAceite = tokenSelecionado,
                Acesso = SessionAttributes.Current().Acesso
            };

            _aceiteRegraComissaoEvsRepository.Save(aceiteRegraComissaoEvs);

            AceiteRegraComissao aceiteRegraComissao = new AceiteRegraComissao();
            aceiteRegraComissao.RegraComissao = regraComissaoEvs.RegraComissao;
            aceiteRegraComissao.EmpresaVenda = SessionAttributes.Current().EmpresaVenda;
            aceiteRegraComissao.Aprovador = usuario;
            aceiteRegraComissao.DataAceite = DateTime.Now;
            aceiteRegraComissao.TokenAceite = tokenSelecionado;
            aceiteRegraComissao.Acesso = SessionAttributes.Current().Acesso;

            _aceiteRegraComissaoRepository.Save(aceiteRegraComissao);
            _tokenAceiteRepository.Save(tokenSelecionado);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RegraComissaoEvSuspensa()
        {
            var json = new JsonResponse();
            var regraComissaoEvs = _regraComissaoEvsRepository.BuscarRegrasEvsSuspensas(SessionAttributes.Current().EmpresaVendaId);
            json.Objeto = regraComissaoEvs.IsEmpty() ? null : regraComissaoEvs.Descricao;
            json.Sucesso = regraComissaoEvs == null ? false : true;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}