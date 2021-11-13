using Europa.Extensions;
using Europa.Web;
using Newtonsoft.Json;
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
    public class ContratoCorretagemController : BaseController
    {
        private TokenAceiteService _tokenAceiteService { get; set; }
        private TokenAceiteRepository _tokenAceiteRepository { get; set; }
        private ContratoCorretagemService _contratoCorretagemService { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private AceiteContratoCorretagemRepository _aceiteContratoCorretagemRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private RegraComissaoEvsService _regraComissaoEvsService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ContratoCorretagemRepository _contratoCorretagemRepository { get; set; }

        public ActionResult Index()
        {
            if (!SessionAttributes.Current().IsDiretor()) { return View("AcessoNaoPermitido"); }

            var aceiteContrato = _aceiteContratoCorretagemRepository.AceiteDaEmpresaVenda(SessionAttributes.Current().EmpresaVenda.Id);
            if (aceiteContrato == null)
            {
                return RedirectToAction("FluxoAceiteContrato");
            }

            var resourcePath = _staticResourceService.LoadResource(aceiteContrato.ContratoCorretagem.Arquivo.Id);
            var urlContrato = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);
            ContratoCorretagemViewModel viewModel = new ContratoCorretagemViewModel();
            viewModel.Contrato = urlContrato;
            return View(viewModel);
        }

        public ActionResult Download()
        {
            if (!SessionAttributes.Current().IsDiretor()) { return View("AcessoNaoPermitido"); }

            var aceite = _aceiteContratoCorretagemRepository.AceiteDaEmpresaVenda(SessionAttributes.Current().EmpresaVenda.Id);
            return File(aceite.ContratoCorretagem.Arquivo.Content,
                aceite.ContratoCorretagem.Arquivo.ContentType,
                aceite.ContratoCorretagem.Arquivo.Nome);
        }

        #region Fluxo de Aceite de Contrato
        public ActionResult FluxoAceiteContrato()
        {
            if (!SessionAttributes.Current().IsDiretor()) { return View("AcessoNaoPermitido"); }

            // Redirecionamento para a tela correta caso já exista contrato assinado
            var idContrato = _contratoCorretagemRepository.BuscarContratoMaisRecenteComArquivo().Id;
            var possuiContratoAssinado = false;

            if (idContrato.HasValue())
            {
                possuiContratoAssinado = _aceiteContratoCorretagemRepository.PossuiUltimoContratoAssinado(SessionAttributes.Current().EmpresaVenda.Id, idContrato);

            }

            if (possuiContratoAssinado)
            {                
                return RedirectToAction("Index", "consultapreproposta");
            }

            var contratoCorretagem = _contratoCorretagemService.ContratoVigente();

            //Exibindo mensagem deerro no caso de falha de configuração
            if (contratoCorretagem == null)
            {
                return View("ContratoNaoConfigurado");
            }

            var resourcePath = _staticResourceService.LoadResource(contratoCorretagem.Arquivo.Id);
            string webRoot = GetWebAppRoot();
            var urlContrato = _staticResourceService.CreateUrl(webRoot, resourcePath);

            ContratoCorretagemViewModel viewModel = new ContratoCorretagemViewModel();
            viewModel.Contrato = urlContrato;

            return View("FluxoAceiteContrato", viewModel);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AceitarContrato()
        {
            var contratoCorretagem = _contratoCorretagemService.ContratoVigente();
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var token = _tokenAceiteService.CriarTokenPara(usuario, contratoCorretagem);
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
            var contratoCorretagem = _contratoCorretagemService.ContratoVigente();
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var token = _tokenAceiteService.CriarTokenPara(usuario, contratoCorretagem);
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
            var templateName = "token-contrato.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);
            var email = EmailService.CriarEmail(destinatario, "[Tenda] Aceite de Contrato - Token", corpoEmail);
            EmailService.EnviarEmail(email);
        }

        public ActionResult VerificarToken(string token)
        {
            var json = new JsonResponse();
            var contratoCorretagem = _contratoCorretagemService.ContratoVigente();
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var tokenSelecionado = _tokenAceiteRepository.TokenAtivoDe(usuario, contratoCorretagem, token);
            json.Sucesso = tokenSelecionado == null ? false : true;
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult InformarToken(string token)
        {
            if (!SessionAttributes.Current().IsDiretor()) { return View("AcessoNaoPermitido"); }

            var contratoCorretagem = _contratoCorretagemService.ContratoVigente();
            var usuario = SessionAttributes.Current().UsuarioPortal;

            var tokenSelecionado = _tokenAceiteRepository.TokenAtivoDe(usuario, contratoCorretagem, token);

            if (tokenSelecionado == null)
            {
                var json = new JsonResponse();
                json.Sucesso = tokenSelecionado == null ? false : true;
                return Json(json, JsonRequestBehavior.AllowGet);
            }

            tokenSelecionado.Ativo = false;
            AceiteContratoCorretagem aceiteContrato = new AceiteContratoCorretagem();
            aceiteContrato.ContratoCorretagem = contratoCorretagem;
            aceiteContrato.EmpresaVenda = SessionAttributes.Current().EmpresaVenda;
            aceiteContrato.Aprovador = usuario;
            aceiteContrato.DataAceite = DateTime.Now;
            aceiteContrato.TokenAceite = tokenSelecionado;
            aceiteContrato.Acesso = SessionAttributes.Current().Acesso;

            _aceiteContratoCorretagemRepository.Save(aceiteContrato);
            _tokenAceiteRepository.Save(tokenSelecionado);

            SessionAttributes.Current().ContratoAssinado = true;

            // Se for diretor, tem que exibir mensagem referente ao aceite
            if (TipoFuncao.Diretor == SessionAttributes.Current().Corretor.Funcao)
            {
                bool possuiAceiteRegraEvsComissao = _regraComissaoEvsService.PossuiAceiteParaRegraComissaoEvsVigente(SessionAttributes.Current().EmpresaVenda.Id);
                if (!possuiAceiteRegraEvsComissao)
                {
                    return RedirectToAction("Index", "consultapreproposta", new { code = "0001" });
                }
            }
            return RedirectToAction("Index", "consultapreproposta");
        }

        #endregion
    }
}