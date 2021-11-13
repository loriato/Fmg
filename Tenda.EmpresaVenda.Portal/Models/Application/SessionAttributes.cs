using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Europa.Web.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Portal.Models.Application
{
    //Melhorias: Ao invés de deuxar o Menu, Menu HTML e as Permissões de Primeiro Nível na sessão do usuário,
    //jogar esses atributos para uma classe pela aplicação, segregando por perfil.
    public class SessionAttributes : ISessionAttributes
    {
        private static readonly string SessionName = "CurrentHttpSession";

        public UsuarioPortal UsuarioPortal { get; set; }
        public Corretor Corretor { get; set; }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public Acesso Acesso { get; set; }
        // FIXME: Jogar para Static de Aplicação
        public IDictionary<string, List<string>> Permissoes { get; set; }
        public string LastUser { get; set; }
        public MenuItem Menu { get; set; }
        public string MenuHtml { get; set; }
        public string InfoHtml { get; set; }
        public string FotoFachada { get; set; }
        public List<Perfil> Perfis { get; set; }
        public List<Regionais> Regionais { get; set; }
        public bool ExibirModalNotificacao { get; set; }
        public int NotificacoesNaoLidas { get; set; }
        public bool ContratoAssinado { get; set; }
        public bool ExibirModalBannerShow { get; set; }
        public bool ExibirModalBannerPortalEV { get; set; }
        public string TokenAcessoSimulador { get; set; }
        public bool AcessoEVSuspensa { get; set; }

        public SessionAttributes()
        {
            UsuarioPortal = null;
            Acesso = null;
            Permissoes = null;
            Perfis = null;
            Regionais = null;
        }

        public bool IsValidSession(string identity)
        {
            if (UsuarioPortal == null)
            {
                return false;
            }
            return identity != null && identity.Equals(LastUser);
        }

        public bool IsValidSession()
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return false;
            }
            return IsValidSession(user.Identity.Name);
        }

        public void LoginWithUser(UsuarioPortal usuario, List<Perfil> perfis, Acesso acesso, Corretor corretor, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,List<Regionais> regionais)
        {
            UsuarioPortal = usuario;
            Perfis = perfis;
            Corretor = corretor;
            Acesso = acesso;
            LastUser = usuario.Login;
            EmpresaVenda = empresaVenda;
            Regionais = regionais;
            InfoHtml = GerarInformacoes();
            FotoFachada = empresaVenda.FotoFachadaUrl;
        }

        public void Invalidate()
        {
            UsuarioPortal = null;
            Perfis = null;
            Acesso = null;
            Permissoes = null;
            EmpresaVenda = null;
            Regionais = null;
            InfoHtml = null;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        private string GerarInformacoes()
        {
            var info = new StringBuilder();

            if (!UsuarioPortal.Email.IsNull())
            {
                info.Append("</br><b>")
                .Append(GlobalMessages.Email).Append(": ").Append(UsuarioPortal.Email).Append("</b><br/><br/>");
            }
            info.Append(GlobalMessages.Empresa).Append(": ").Append(EmpresaVenda.NomeFantasia).Append("<br/><br/>");
            info.Append(GlobalMessages.Perfis).Append(": ").Append(string.Join(", ", Perfis.Select(x => x.Nome))).Append("<br/><br/>");
            info.Append(GlobalMessages.Regionais).Append(": ").Append(string.Join(", ", Regionais.Select(x => x.Nome))).Append("<br/><br/>");
            info.Append(GlobalMessages.AcessoAtual).Append(": #").Append(Acesso.Id.ToString().PadLeft(7, '0')).Append("<br><br/>")
                .Append(GlobalMessages.InicioAcesso).Append(": ").Append(Acesso.InicioSessao.ToString("dd/MM/yyyy HH:mm <br/><br/>"))
                .Append("<button class='btn-popover btn btn-steel' onclick='AlterarSenha()'>").Append(GlobalMessages.AlterarSenha).Append("</button> <br/><br/>")
                .Append("<button class='btn-popover btn btn-steel' onclick='Sair()'>").Append(GlobalMessages.Sair).Append("</button></br></br>");
            return info.ToString();
        }

        public static SessionAttributes Current()
        {
            SessionAttributes currentSession = (SessionAttributes)HttpContext.Current.Session[SessionName];
            if (currentSession == null)
            {
                currentSession = new SessionAttributes();
                HttpContext.Current.Session[SessionName] = currentSession;
            }
            return currentSession;
        }

        public bool HasPermission(string codigoUnidadeFuncional, string comandoFuncionalidade)
        {
            if (codigoUnidadeFuncional.IsEmpty() || Permissoes.IsEmpty())
            {
                return false;
            }
            List<string> comandos;
            if (Permissoes.TryGetValue(codigoUnidadeFuncional, out comandos))
            {
                if (comandoFuncionalidade.IsNull())
                {
                    return true;
                }
                return comandos.Contains(comandoFuncionalidade);
            }
            return false;
        }

        public long GetUser()
        {
            if (Current() != null && Current().UsuarioPortal != null)
            {
                return Current().UsuarioPortal.Id;
            }
            return 0;
        }

        public long GetAccess()
        {
            if (Current() != null && Current().UsuarioPortal != null)
            {
                return Current().Acesso.Id;
            }
            return 0;
        }
        public List<long> GetRoles()
        {
            if (Current() != null && Current().UsuarioPortal != null)
            {
                return Current().Perfis.Select(reg => reg.Id).ToList();
            }
            return new List<long> { 0 };
        }
        public long EmpresaVendaId
        {
            get { return EmpresaVenda.Id; }
        }

        public bool IsDiretor()
        {
            return TipoFuncao.Diretor == Corretor.Funcao;
        }

    }
}