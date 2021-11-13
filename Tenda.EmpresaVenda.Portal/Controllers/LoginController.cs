using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Commons;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class LoginController : Controller
    {
        private MenuService _menuService { get; set; }
        private LoginService _loginService { get; set; }
        private PermissaoService _permissaoService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ParametroSistemaRepository _parametroSistemaRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private AceiteContratoCorretagemRepository _aceiteContratoCorretagemRepository { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private CorretorService _corretorService { get; set; }
        private RegraComissaoEvsService _regraComissaoEvsService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ContratoCorretagemRepository _contratoCorretagemRepository { get; set; }
        private RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        private ViewResponsavelAceiteRegraComissaoRepository _viewResponsavelAceiteRegraComissaoRepository { get; set; }
        public ActionResult Index()
        {
            if (SessionAttributes.Current().IsValidSession())
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpGet]
        public ActionResult Logar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logar(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            try
            {
                LoginDto loginDto = new LoginDto();
                loginDto.ClienteIpAddress = HttpUtil.RequestIp(Request);
                loginDto.Server = Environment.MachineName;
                loginDto.UserAgent = Request.UserAgent;
                loginDto.Username = viewModel.Username.ToLower();
                loginDto.CodigoSistema = ApplicationInfo.CodigoSistema;
                loginDto.Password = viewModel.Password;
                // A gestão de usuários do portal é realizado e mantido pela área de negócios, não por SI
                loginDto.LoginViaActiveDirectory = false;
                loginDto.SincronizarGruposActiveDirectory = false;

                var acesso = _loginService.Login(loginDto);

                viewModel.Password = null;

                if (acesso == null || acesso.Usuario == null)
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.UsuarioSenhaIncorreto);
                }
                else if (SituacaoUsuario.Ativo.Equals(acesso.Usuario.Situacao))
                {
                    SessionAttributes current = SessionAttributes.Current();

                    UsuarioPortal usuarioPortal = _usuarioPortalRepository.FindById(acesso.Usuario.Id);

                    // Usado para criar os usuários automaticamente no primeiro login
                    if (usuarioPortal == null)
                    {
                        usuarioPortal = new UsuarioPortal(acesso.Usuario);
                        _usuarioPortalRepository.Save(usuarioPortal);
                    }

                    var corretor = _corretorRepository.FindByIdUsuario(acesso.Usuario.Id);

                    //verifica se o corretor esta cadastrado para dar o aceite na regra de comissao
                    var PodeAceitarRegraComissao = _viewResponsavelAceiteRegraComissaoRepository.PodeAceitarRegraComissao(corretor.EmpresaVenda.Id, corretor.Id);

                    var perfis = _loginService.GetPerfisFromAcesso(acesso.Id);
                    var idPerfis = perfis.Select(reg => reg.Id).ToList();

                    bool isEVSuspensaWithAcess = HasAcessoEVSuspensa(corretor, idPerfis);

                    // O usuário possui login, mas não tem corretor
                    if (corretor == null)
                    {
                        ModelState.AddModelError("login_failed", GlobalMessages.LoginUsuarioSemAcessoPortalCorretores);
                        return View("Index", viewModel);
                    }

                    // A empresa de venda do usuário está cancelada ou suspensa
                    if (corretor.EmpresaVenda.Situacao != Situacao.Ativo && !isEVSuspensaWithAcess)
                    {
                        ModelState.AddModelError("login_failed", GlobalMessages.LoginEmpresaVendaSuspensa);
                        return View("Index", viewModel);
                    }

                    if (corretor.TipoCorretor == TipoCorretor.AgenteVenda)
                    {
                        ModelState.AddModelError("login_failed", "Você não possui acesso a este portal");
                        return View("Index", viewModel);
                    }

                    //Buscar ultimo Contrato Corretagem
                    var idContratoCorretagem = _contratoCorretagemRepository.BuscarContratoMaisRecenteComArquivo().Id;
                    // A empresa de venda já aceitou o ultimo contrato de corretagem
                    bool contratoCorretagemAssinado = false;
                    if (idContratoCorretagem.HasValue())
                    {
                        contratoCorretagemAssinado = _aceiteContratoCorretagemRepository.PossuiUltimoContratoAssinado(corretor.EmpresaVenda.Id, idContratoCorretagem);
                    }
                    // Se o contrato não estiver assinado, e o usuário não for diretor, preciso barrar o login
                    if ((!contratoCorretagemAssinado && TipoFuncao.Diretor != corretor.Funcao) || (!contratoCorretagemAssinado && PodeAceitarRegraComissao))
                    {
                        ModelState.AddModelError("login_failed", GlobalMessages.LoginEmpresaVendaSemContratoAceite);
                        return View("Index", viewModel);
                    }

                    if (corretor.EmpresaVenda.FotoFachada.HasValue())
                    {
                        var resourcePath = _staticResourceService.LoadResource(corretor.EmpresaVenda.FotoFachada.Id);
                        string webRoot = GetWebAppRoot();
                        corretor.EmpresaVenda.FotoFachadaUrl = _staticResourceService.CreateUrl(webRoot, resourcePath);
                    }
                    var Regionais = new List<Regionais>();
                    if (!corretor.EmpresaVenda.IsEmpty())
                    {
                        Regionais = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(corretor.EmpresaVenda.Id).Select(s=>s.Regional).ToList();
                    }
                    current.LoginWithUser(usuarioPortal, perfis, acesso, corretor, corretor.EmpresaVenda,Regionais);
                    current.Menu = _menuService.MontarMenuPerfil(ApplicationInfo.CodigoSistema, idPerfis);
                    current.Permissoes = _permissaoService.Permissoes(ApplicationInfo.CodigoSistema, idPerfis);

                    MenuItemToBootStrap menuConverter = new MenuItemToBootStrap();
                    string urlAcesso = GetWebAppRoot();
                    current.AcessoEVSuspensa = isEVSuspensaWithAcess;
                    current.MenuHtml = menuConverter.ProcessMenu(urlAcesso, current.Menu);
                    current.NotificacoesNaoLidas = _notificacaoRepository.NaoLidasDoUsuario(usuarioPortal.Id, usuarioPortal.UltimaLeituraNotificacao, DateTime.Now).Count();
                    current.ExibirModalNotificacao = true;
                    current.ExibirModalBannerShow = false;
                    current.ExibirModalBannerPortalEV = true;
                    current.ContratoAssinado = contratoCorretagemAssinado;
                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);

                    if (isEVSuspensaWithAcess)
                    {
                        current.Permissoes = current.Permissoes.Where(x => x.Key.Equals("EVS19")).ToDictionary(i => i.Key, i => i.Value);

                        return RedirectToAction("Index", "Financeiro");
                    }

                    // O usuário é diretor e o contrato não foi assinado aceite
                    if (!contratoCorretagemAssinado)
                    {
                        return RedirectToAction("FluxoAceiteContrato", "contratocorretagem");
                    }

                    _regraComissaoService.AtivarCampanhaNoLogin(SessionAttributes.Current().EmpresaVendaId);

                    var regraEv = _regraComissaoEvsRepository.BuscarCampanhaAguardandoInativacao(SessionAttributes.Current().EmpresaVendaId);
                    if (!regraEv.IsEmpty() && regraEv.Tipo == TipoRegraComissao.Campanha)
                    {
                        _regraComissaoService.InativarCampanha(regraEv.RegraComissao);
                    }

                    if (viewModel.ReturnUrl.IsNull())
                    {
                        // Se for diretor, tem que exibir mensagem referente ao aceite
                        if (TipoFuncao.Diretor == corretor.Funcao || PodeAceitarRegraComissao)
                        {
                            bool possuiAceiteRegraEvsComissao = _regraComissaoEvsService.PossuiAceiteParaRegraComissaoEvsVigente(corretor.EmpresaVenda.Id);
                            if (!possuiAceiteRegraEvsComissao)
                            {
                                return RedirectToAction("Index", "consultapreproposta", new { code = "0001" });
                            }
                        }

                        return RedirectToAction("Index", "consultapreproposta");
                    }

                    return Redirect(viewModel.ReturnUrl);
                }
                else if (SituacaoUsuario.PendenteAprovacao.Equals(acesso.Usuario.Situacao))
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.CadastroPendenteAprovacao);
                }
                else
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.UsuarioBloqueado);
                }
            }
            catch (BusinessRuleException bre)
            {
                ModelState.AddModelError("login_failed", bre.Errors.First());
            }
            return View("Index", viewModel);

        }

        private bool HasAcessoEVSuspensa(Corretor corretor, List<long> idsPerfis)
        {
            bool isEvSuspensa = corretor.EmpresaVenda.Situacao == Situacao.Suspenso;
            var idsPerfisEVSuspensa = ProjectProperties.IdsPerfisLoginEVSuspensa;
            bool isUsuarioWithAcess = idsPerfisEVSuspensa.Intersect(idsPerfis).Any();

            if(isEvSuspensa && isUsuarioWithAcess)
            {
                return true;
            }

            return false;
        }

        private bool UsuarioNoPerfilInicial(List<long> idPerfis)
        {
            var perfilInicial = _parametroSistemaRepository.Queryable()
                .Where(reg => reg.Sistema.Codigo == ApplicationInfo.CodigoSistema)
                .Select(reg => reg.PerfilInicial)
                .SingleOrDefault();

            if (perfilInicial == null)
            {
                return false;
            }
            return idPerfis.TrueForAll(x => x == perfilInicial.Id);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            SessionAttributes session = SessionAttributes.Current();

            var regraEv = _regraComissaoEvsRepository.BuscarCampanhaAguardandoInativacao(session.EmpresaVendaId);
            if (!regraEv.IsEmpty() && regraEv.Tipo == TipoRegraComissao.Campanha)
            {
                _regraComissaoService.InativarCampanha(regraEv.RegraComissao);
            }

            _loginService.Logout(session.Acesso);
            session.Invalidate();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ReenviarTokenAtivacao(string email)
        {
            var jsonResponse = new JsonResponse();
            var bre = new BusinessRuleException();

            using (var session = _usuarioPortalRepository._session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        if (email.IsEmpty()) bre.AddError(String.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Email)).Complete();
                        bre.ThrowIfHasError();

                        if (!email.IsValidEmail()) bre.AddError(GlobalMessages.EmailInvalido).Complete();
                        bre.ThrowIfHasError();

                        var corretor = _corretorRepository.FindByEmail(email);
                        if (corretor.HasValue()) _corretorService.CriarTokenAtivacaoEEnviarEmail(corretor);

                        jsonResponse.Sucesso = true;
                        jsonResponse.Mensagens.Add(String.Format(GlobalMessages.TokenAtivacaoReenviado, email));
                        transaction.Commit();
                    }
                    catch (BusinessRuleException ex)
                    {
                        jsonResponse.Mensagens.AddRange(ex.Errors);
                        transaction.Rollback();
                    }
                }
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        private string GetWebAppRoot()
        {
            var host = (Request.Url.IsDefaultPort) ?
                Request.Url.Host :
                Request.Url.Authority;
            host = string.Format("{0}://{1}", Request.Url.Scheme, host);
            if (Request.ApplicationPath == "/")
                return host;
            else
                return host + Request.ApplicationPath;
        }
    }
}