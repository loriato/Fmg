using Europa.Commons;
using Europa.Commons.LDAP;
using Europa.Cryptography;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Login;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using ApplicationInfo = Tenda.EmpresaVenda.Api.Models.ApplicationInfo;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/seg")]
    public class SegurancaController : BaseApiController
    {
        private LoginService _loginService { get; set; }
        private UsuarioPortalRepository UsuarioPortalRepository { get; set; }
        private ParametroSistemaRepository ParametroSistemaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        public AcessoRepository _acessoRepository { get; set; }
        private EmpresaVendaRepository EmpresaVendaRepository { get; set; }
        private UsuarioPerfilSistemaRepository UsuarioPerfilSistemaRepository { get; set; }
        private MenuService _menuService { get; set; }
        private PermissaoService _permissaoService { get; set; }
        private AgenteVendaHouseRepository _agenteVendaHouseRepository { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }

        [HttpPost]
        [Route("login")]
        [AuthenticateUserByToken(true)]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage Login(LoginRequestDto loginRequestDto)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin("chamada no endpoint --login--");
            var transaction = _session.BeginTransaction();
            var exc = new ApiException();

            GenericFileLogUtil.DevLogWithDateOnBegin("Validar login");
            ValidarLogin(loginRequestDto, exc);
            GenericFileLogUtil.DevLogWithDateOnBegin("Login válido");

            var loginDto = MontarDto(loginRequestDto);

            try
            {
                GenericFileLogUtil.DevLogWithDateOnBegin("Iniciando autenticação");
                var acesso = _loginService.Login(loginDto);

                ValidarAcesso(acesso, exc);

                var usuarioPortal = UsuarioPortalRepository.FindById(acesso.Usuario.Id);

                // Usado para criar os usuários automaticamente no primeiro login
                if (usuarioPortal == null)
                {
                    usuarioPortal = new UsuarioPortal(acesso.Usuario);
                    UsuarioPortalRepository.Save(usuarioPortal);
                }

                transaction.Commit();

                if (loginRequestDto.CodigoSistema == ApplicationInfo.CodigoSistemaPortalHome)
                {
                    GenericFileLogUtil.DevLogWithDateOnBegin("Validar acesso no portal Home");
                    ValidarAcessoPortalHome(acesso);
                    GenericFileLogUtil.DevLogWithDateOnBegin("Acesso Válido");
                }

                var response = MontarLoginResponse(acesso);
                GenericFileLogUtil.DevLogWithDateOnBegin("login autorizado");
                return Response(response);
            }
            catch (COMException)
            {
                exc.AddError(GlobalMessages.LoginActiveDirectoryInalcancavel);
                exc.ThrowIfHasError();
            }
            catch (LdapException exception)
            {
                ExceptionLogger.LogException(exception);

                exc.GetResponse().Data = exception.ErrorCode;
                if (exception.ErrorCode == LdapError.InvalidCredentials)
                {
                    exc.AddError(GlobalMessages.UsuarioOuSenhaInvalidos);
                }
                else if (exception.ErrorCode == LdapError.AccountLocked)
                {
                    exc.AddError(GlobalMessages.UsuarioOuSenhaBloqueados);
                }
                else
                {
                    exc.AddError(exception.Message);
                }

                exc.ThrowIfHasError();
            }
            catch (BusinessRuleException bre)
            {
                ExceptionLogger.LogException(bre);
                exc.AddErrors(bre.Errors);
                exc.ThrowIfHasError();
            }

            return Response(HttpStatusCode.OK);
        }

        private void ValidarLogin(LoginRequestDto loginRequestDto, ApiException exc)
        {
            if (loginRequestDto.Username.IsEmpty())
            {
                exc.AddError(nameof(loginRequestDto.Username),
                    string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Usuario));
            }

            if (loginRequestDto.Password.IsEmpty())
            {
                exc.AddError(nameof(loginRequestDto.Password),
                    string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Senha));
            }

            exc.ThrowIfHasError();
        }

        private LoginDto MontarDto(LoginRequestDto loginRequestDto)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin("Montando DTO");
            var loginDto = new LoginDto();
            loginDto.ClienteIpAddress = HttpUtil.RequestIp(new HttpRequestWrapper(HttpContext.Current.Request));
            loginDto.Server = Environment.MachineName;
            loginDto.UserAgent = Request.Headers?.UserAgent?.ToString();
            loginDto.Server = Environment.MachineName;
            loginDto.Username = loginRequestDto.Username.ToLower();
            loginDto.CodigoSistema = loginRequestDto.CodigoSistema.HasValue() ? loginRequestDto.CodigoSistema : ApplicationInfo.CodigoSistema;
            loginDto.Password = loginRequestDto.Password;
            loginDto.LoginViaActiveDirectory = ProjectProperties.LoginViaActiveDirectory;
            return loginDto;
        }

        private void ValidarAcesso(Acesso acesso, ApiException exc)
        {
            if (acesso == null || acesso.Usuario == null)
            {
                exc.AddError(GlobalMessages.UsuarioOuSenhaIncorretos);
                exc.ThrowIfHasError();
            }

            switch (acesso.Usuario.Situacao)
            {
                case SituacaoUsuario.PendenteAprovacao:
                    exc.AddError(GlobalMessages.CadastroPendenteAprovacao);
                    break;
                case SituacaoUsuario.Cancelado:
                case SituacaoUsuario.Suspenso:
                    exc.AddError(GlobalMessages.UsuarioBloqueado);
                    break;
            }

            exc.ThrowIfHasError();
        }

        private void ValidarAcessoPortalHome(Acesso acesso)
        {
            var exc = new ApiException();

            var isCoordenadorHouse = UsuarioPerfilSistemaRepository.UsuarioPertenceAPerfil(acesso.Usuario.Id, ProjectProperties.IdPerfilCoordenadorHouse, acesso.Sistema.Id);

            var isSupervisorHouse = UsuarioPerfilSistemaRepository.UsuarioPertenceAPerfil(acesso.Usuario.Id, ProjectProperties.IdPerfilSupervisorHouse, acesso.Sistema.Id);

            var isAgenteVendaHouse = UsuarioPerfilSistemaRepository.UsuarioPertenceAPerfil(acesso.Usuario.Id, ProjectProperties.IdPerfilAgenteVenda, acesso.Sistema.Id);

            if (!isCoordenadorHouse && !isSupervisorHouse && !isAgenteVendaHouse)
            {
                exc.AddError(string.Format(GlobalMessages.UsuarioNaoEhPerfil,string.Format("{0} ou {1} ou {2}",GlobalMessages.Coordenador,GlobalMessages.Supervisor,GlobalMessages.AgenteVenda)));
                exc.ThrowIfHasError();
            }

            var idUsuario = acesso.Usuario.Id;

            var hierarquia = _hierarquiaHouseRepository.Queryable()
                .Where(x => x.Coordenador.Id == idUsuario || x.Supervisor.Id == idUsuario || x.AgenteVenda.Id == idUsuario)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Any();

            if (!hierarquia && isAgenteVendaHouse)
            {
                exc.AddError(GlobalMessages.UsuarioNaoPossuiLoja);
                exc.ThrowIfHasError();
            }

            if (!hierarquia)
            {
                exc.AddError(string.Format("Usuário não possui permissão para acessar a loja"));
                exc.ThrowIfHasError();
            }
            
        }

        private LoginResponseDto MontarLoginResponse(Acesso acesso)
        {
            var response = new LoginResponseDto();
            response.Login = acesso.Usuario.Login;
            response.Id = acesso.Usuario.Id;
            response.Nome = acesso.Usuario.Nome;
            response.Email = acesso.Usuario.Email;
            response.Autorizacao = acesso.Autorizacao;
            response.InicioAcesso = acesso.InicioSessao;
            response.IdAcesso = acesso.Id;
            
            //FIX-ME: futuramente o Hermmann definirá o que fazer
            var perfis = _loginService.GetPerfisFromAcesso(acesso.Id);
            var idPerfis = perfis.Select(reg => reg.Id).ToList();
            response.PerfilInicial = UsuarioNoPerfilInicial(idPerfis);
            response.Perfis = perfis.Select(x => new EntityDto { Id = x.Id, Nome = x.Nome }).ToList();
            response.Menu = _menuService.MontarMenuPerfil(acesso.Sistema.Codigo, idPerfis);
            response.Permissoes = _permissaoService.Permissoes(acesso.Sistema.Codigo, idPerfis).Select(x =>
                new LoginPermissaoDto
                {
                    UnidadeFuncional = x.Key,
                    Funcionalidades = x.Value
                }).ToList();

            var loja = GetLoja(acesso,idPerfis);
            response.Lojas = GetLojas(acesso, idPerfis);

            if (loja.HasValue()&&(response.Lojas.IsEmpty()||response.Lojas.Count==1))
            {
                response.IdLoja = loja.Id;
                response.NomeLoja = loja.NomeFantasia;
                response.EstadoLoja = loja.Estado;
            }
            
            return response;
        }

        private Tenda.Domain.EmpresaVenda.Models.EmpresaVenda GetLoja(Acesso acesso,List<long> idPerfis)
        {
            var apiEx = new ApiException();
            
            if (acesso.Sistema.Codigo.Equals(ApplicationInfo.CodigoSistemaPortalHome))
            {
                var idUsuario = acesso.Usuario.Id;

                var isCoordenadorHouse = idPerfis.Contains(ProjectProperties.IdPerfilCoordenadorHouse);

                var isSupervisorHouse = idPerfis.Contains(ProjectProperties.IdPerfilSupervisorHouse);

                var isAgenteVendaHouse = idPerfis.Contains(ProjectProperties.IdPerfilAgenteVenda);

                var filtroHierarquiaHouseDto = new FiltroHierarquiaHouseDto();
                filtroHierarquiaHouseDto.Situacao = SituacaoHierarquiaHouse.Ativo;

                if (isCoordenadorHouse)
                {
                    filtroHierarquiaHouseDto.IdCoordenadorHouse = idUsuario;
                }
                else if (isSupervisorHouse)
                {
                    filtroHierarquiaHouseDto.IdSupervisorHouse = idUsuario;
                }
                else if (isAgenteVendaHouse)
                {
                    filtroHierarquiaHouseDto.IdAgenteVenda = idUsuario;
                }
                else
                {
                    apiEx.AddError(string.Format("O usuário {0} não possui permissão para acessar a loja", acesso.Usuario.Nome));
                    apiEx.ThrowIfHasError();
                }

                var hierarquia = _hierarquiaHouseRepository.ListarHierarquiaHouse(filtroHierarquiaHouseDto);

                var house = hierarquia.OrderByDescending(x => x.Inicio).Select(x => x.House).OrderBy(x => x.Id).FirstOrDefault();

                if (house.IsEmpty())
                {
                    apiEx.AddError(string.Format("O usuário {0} não possui vinculo ativo com alguma loja", acesso.Usuario.Nome));
                    apiEx.ThrowIfHasError();
                }

                return house;
            }

            var corretor = _corretorRepository.FindByIdUsuario(acesso.Usuario.Id);

            var empresaVenda = corretor.EmpresaVenda;

            return empresaVenda;
        }

        private List<HouseDto> GetLojas(Acesso acesso, List<long> idPerfis)
        {
            var apiEx = new ApiException();

            var dicionario = new List<HouseDto>();

            if (!acesso.Sistema.Codigo.Equals(ApplicationInfo.CodigoSistemaPortalHome))
            {
                return dicionario;
            }

            var idUsuario = acesso.Usuario.Id;

            var isCoordenadorHouse = idPerfis.Contains(ProjectProperties.IdPerfilCoordenadorHouse);

            var isSupervisorHouse = idPerfis.Contains(ProjectProperties.IdPerfilSupervisorHouse);

            var isAgenteVendaHouse = idPerfis.Contains(ProjectProperties.IdPerfilAgenteVenda);

            var filtroHierarquiaHouseDto = new FiltroHierarquiaHouseDto();
            filtroHierarquiaHouseDto.Situacao = SituacaoHierarquiaHouse.Ativo;

            if (isCoordenadorHouse)
            {
                filtroHierarquiaHouseDto.IdCoordenadorHouse = idUsuario;
            }
            else if (isSupervisorHouse)
            {
                filtroHierarquiaHouseDto.IdSupervisorHouse = idUsuario;
            }
            else if (isAgenteVendaHouse)
            {
                filtroHierarquiaHouseDto.IdAgenteVenda = idUsuario;
            }
            else
            {
                apiEx.AddError(string.Format("O usuário {0} não possui permissão para acessar a loja", acesso.Usuario.Nome));
                apiEx.ThrowIfHasError();
            }

            var hierarquia = _hierarquiaHouseRepository.ListarHierarquiaHouse(filtroHierarquiaHouseDto);

            var houses = hierarquia.OrderByDescending(x => x.Inicio).Select(x => x.House).OrderBy(x => x.Id).ToList();

            if (houses.IsEmpty())
            {
                apiEx.AddError(string.Format("O usuário {0} não possui vinculo ativo com alguma loja", acesso.Usuario.Nome));
                apiEx.ThrowIfHasError();
            }

            foreach(var house in houses)
            {
                if (dicionario.Select(x=>x.Id).Contains(house.Id))
                {
                    continue;
                }
                dicionario.Add(new HouseDto { Id = house.Id, Nome = house.NomeFantasia, Estado = house.Estado });
            }

            return dicionario;
        }

        [HttpPost]
        [Route("logout")]
        [Transaction(TransactionAttributeType.Required)]
        [AuthenticateUserByTokenAttribute("SEG01", "Logout")]
        public HttpResponseMessage Logout()
        {
            _loginService.Logout(CurrentRequestState().Acesso);
            return Response(HttpStatusCode.OK);
        }

        private bool UsuarioNoPerfilInicial(List<long> idPerfis)
        {
            var perfilInicial = ParametroSistemaRepository.Queryable()
                .Where(reg => reg.Sistema.Codigo == ApplicationInfo.CodigoSistema)
                .Select(reg => reg.PerfilInicial)
                .SingleOrDefault();

            if (perfilInicial == null) return false;
            return idPerfis.TrueForAll(x => x == perfilInicial.Id);
        }
        /**
         * É passado o código do sistema específico pois o token era gerado incorretamente
         */
        [HttpPost]
        [Route("loginCorretor")]
        [AuthenticateUserByToken(true)]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage LoginCorretor(LoginRequestDto loginRequestDto)
        {
            var exc = new ApiException();
            var retorno = new MensagemRetornoDto();
            if (loginRequestDto.Username.IsEmpty())
            {
                exc.AddError(nameof(loginRequestDto.Username),
                    string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Usuario));
            }

            if (loginRequestDto.Password.IsEmpty())
            {
                exc.AddError(nameof(loginRequestDto.Password),
                    string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Senha));
            }

            exc.ThrowIfHasError();

            try
            {
                LoginDto loginDto = new LoginDto();
                loginDto.ClienteIpAddress = HttpUtil.GetClientIp(HttpContext.Current);
                loginDto.Server = Environment.MachineName;
                loginDto.UserAgent = Request.Headers?.UserAgent?.ToString();
                loginDto.Username = loginRequestDto.Username.ToLower();
                //loginDto.CodigoSistema = ApplicationInfo.CodigoSistema;
                loginDto.CodigoSistema = ProjectProperties.CodigoEmpresaVendaPortal;
                loginDto.Password = loginRequestDto.Password;
                // A gestão de usuários do portal é realizado e mantido pela área de negócios, não por SI
                loginDto.LoginViaActiveDirectory = false;
                loginDto.SincronizarGruposActiveDirectory = false;

                var acesso = _loginService.Login(loginDto);

                loginRequestDto.Password = null;

                if (acesso == null || acesso.Usuario == null)
                {
                    exc.AddError(GlobalMessages.UsuarioSenhaIncorreto);
                }
                else if (SituacaoUsuario.Ativo.Equals(acesso.Usuario.Situacao))
                {
                    UsuarioPortal usuarioPortal = UsuarioPortalRepository.FindById(acesso.Usuario.Id);

                    // Usado para criar os usuários automaticamente no primeiro login
                    if (usuarioPortal == null)
                    {
                        usuarioPortal = new UsuarioPortal(acesso.Usuario);
                        UsuarioPortalRepository.Save(usuarioPortal);
                    }

                    var corretor = _corretorRepository.FindByIdUsuario(acesso.Usuario.Id);
                    // O usuário possui login, mas não tem corretor
                    if (corretor == null)
                    {
                        //ModelState.AddModelError("login_failed", GlobalMessages.LoginUsuarioSemAcessoPortalCorretores);
                        exc.AddError(GlobalMessages.LoginUsuarioSemAcessoPortalCorretores);
                        exc.ThrowIfHasError();
                    }

                    // A empresa de venda do usuário está cancelada ou suspensa
                    if (corretor.EmpresaVenda.Situacao != Situacao.Ativo)
                    {
                        //ModelState.AddModelError("login_failed", GlobalMessages.LoginEmpresaVendaSuspensa);
                        exc.AddError(GlobalMessages.LoginEmpresaVendaSuspensa);
                    }

                    exc.ThrowIfHasError();

                    var perfis = _loginService.GetPerfisFromAcesso(acesso.Id);
                    var idPerfis = perfis.Select(reg => reg.Id).ToList();

                    var retornoCorretor = new RetornoLoginCorretorDto();

                    retornoCorretor.UsuarioPortal = new UsuarioPortal();
                    retornoCorretor.UsuarioPortal.Nome = usuarioPortal.Nome;
                    retornoCorretor.UsuarioPortal.Login = usuarioPortal.Login;
                    retornoCorretor.UsuarioPortal.Email = usuarioPortal.Email;
                    retornoCorretor.UsuarioPortal.Situacao = usuarioPortal.Situacao;
                    //retornoCorretor.Hash = SslAes256.EncryptString(usuarioPortal.Senha);

                    retornoCorretor.Acesso = new Acesso();
                    retornoCorretor.Acesso.Usuario = new Usuario();
                    retornoCorretor.Acesso.Sistema = new Sistema();
                    retornoCorretor.Acesso.Autorizacao = acesso.Autorizacao;

                    retornoCorretor.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
                    retornoCorretor.EmpresaVenda.Loja = new Tenda.Domain.EmpresaVenda.Models.Loja();
                    retornoCorretor.EmpresaVenda.Loja.SapId = corretor.EmpresaVenda.Loja.SapId;

                    retorno.Sucesso = true;
                    retorno.Objeto = retornoCorretor;
                }
                else if (SituacaoUsuario.PendenteAprovacao.Equals(acesso.Usuario.Situacao))
                {
                    //ModelState.AddModelError("login_failed", GlobalMessages.CadastroPendenteAprovacao);
                    exc.AddError(GlobalMessages.CadastroPendenteAprovacao);
                }
                else
                {
                    //ModelState.AddModelError("login_failed", GlobalMessages.UsuarioBloqueado);
                    exc.AddError(GlobalMessages.CadastroPendenteAprovacao);
                }

                exc.ThrowIfHasError();
            }
            catch (BusinessRuleException bre)
            {
                retorno.Sucesso = false;
                retorno.Mensagens.AddRange(bre.Errors);
                retorno.Campos.AddRange(bre.ErrorsFields);

                return Response(HttpStatusCode.BadRequest, retorno);
            }

            return Response(HttpStatusCode.OK, retorno);
        }

        [HttpPost]
        [Route("logoutCorretor")]
        [AuthenticateUserByToken(true)]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Logout(Acesso acessoAtual)
        {
            var acesso = _acessoRepository.FindByAuthToken(acessoAtual.Autorizacao);
            var retorno = new MensagemRetornoDto();

            if (acesso.IsEmpty())
            {
                retorno.Sucesso = false;
                retorno.Objeto = acesso;
                return Response(HttpStatusCode.NotFound, retorno);
            }
            acesso.FimSessao = DateTime.Now;
            acesso.FormaEncerramento = FormaEncerramento.Logout;
            _acessoRepository.Save(acesso);
            Session.Flush();

            retorno.Sucesso = true;
            retorno.Objeto = acesso;
            return Response(HttpStatusCode.OK, retorno);
        }

        [HttpPost]
        [Route("clearCache")]
        public HttpResponseMessage ClearCache()
        {
            ProjectProperties.ClearPropertiesCache();
            var response = new BaseResponse();
            response.SuccessResponse("Cache limpo com sucesso");
            return Response(response);
        }
    }
}