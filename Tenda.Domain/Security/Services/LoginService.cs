using Europa.Commons;
using Europa.Commons.LDAP;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services.Models;

namespace Tenda.Domain.Security.Services
{
    public class LoginService : BaseService
    {
        public SistemaService _sistemaService { get; set; }
        public UsuarioRepository _usuarioRepository { get; set; }
        public UsuarioPerfilSistemaRepository _upsRepository { get; set; }
        public AcessoRepository _acessoRepository { get; set; }
        public AcessoPerfilRepository _acessoPerfilRepository { get; set; }
        public UsuarioPortalService _usuarioPortalService { get; set; }
        public PerfilSistemaGrupoActiveDiretoryRepository _psadRepository { get; set; }

        private bool ParametroSistemaValido(ParametroSistema parametro)
        {
            return !parametro.IsNull() &&
                !string.IsNullOrWhiteSpace(parametro.ServidorAD) &&
                !string.IsNullOrWhiteSpace(parametro.DominioAD);
        }


        public Acesso Login(LoginDto loginDto)
        {
            //Garantindo tratamentos de CS/CI
            loginDto.Username = loginDto.Username.ToLower();

            Sistema sistema = _sistemaService.FindByCodigo(loginDto.CodigoSistema);

            Session.Evict(sistema);

            Usuario usuario = _usuarioRepository.FindByLogin(loginDto.Username);


            string hashedPass = HashUtil.SHA512(loginDto.Password);

            if (usuario == null || usuario.Senha.IsEmpty() || !usuario.Senha.Equals(hashedPass))
            {
                BusinessRuleException exc = new BusinessRuleException();
                exc.AddError(GlobalMessages.UsuarioSenhaIncorreto).Complete();
                throw exc;
            }


            return LoginAsMultipleProfile(sistema, usuario, loginDto);
        }

        /// <summary>
        /// Melhoria Solicitada pela Área de SI
        ///  - Em model.GruposActiveDirectory irão vir todos os grupos de determinado usuário.
        ///  - Baseado nestes grupos e na associação entre Perfis do Sistema e Grupos do Active Diretory devemos deixar associados ao usuário apenas aos grupos definidos pelo AD.
        ///  - Se ao fim do processo não houver grupos, não deixar o usuário logar.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="model"></param>
        private void SincronizarGruposActiveDiretory(Sistema sistema, Usuario usuario, LdapModel model)
        {
            // Os perfis cadastrados para o usuário no momento
            var perfisDoUsuario = _upsRepository.Listar(usuario.Id, sistema.Id);

            // As configurações de associação entre os perfis e o AD
            var configuracoesDoSistema = _psadRepository.ConfiguracaoDoSistema(sistema.Id);
            if (!configuracoesDoSistema.Any())
            {
                throw new BusinessRuleException("As configurações de associação entre os perfis do sistema e o Active Directory estão inválidas. Entre em contato com a TI Tenda");
            }

            // Deixando apenas as configurações de sistema que o usuário possui configurado no Active Directory
            configuracoesDoSistema = configuracoesDoSistema.Where(reg => model.GruposActiveDiretory.Contains(reg.GrupoActiveDirectory.ToUpper())).ToList();

            // Os perfis do usuário que foram corretamente ajustados e não precisam ser removidos
            var perfisUsuarioValidados = new List<long>();
            foreach (var configuracao in configuracoesDoSistema)
            {
                var perfilDoUsuario = perfisDoUsuario.Where(perf => perf.Perfil.Id == configuracao.Perfil.Id).SingleOrDefault();
                // O usuário não possui o perfil, preciso criar o perfil do usuário baseado na configuração do AD
                if (perfilDoUsuario == null)
                {
                    var ups = new UsuarioPerfilSistema
                    {
                        Sistema = sistema,
                        Usuario = usuario,
                        Perfil = configuracao.Perfil
                    };
                    _upsRepository.Save(ups);
                }
                perfisUsuarioValidados.Add(configuracao.Perfil.Id);
            }

            // Verificando se algum dos perfis não está mais na configuração do cadastro do usuário
            var perfisRemover = perfisDoUsuario.Where(reg => !perfisUsuarioValidados.Contains(reg.Perfil.Id)).ToList();
            if (perfisRemover.Any())
            {
                foreach (var perfilRemover in perfisRemover)
                {
                    _upsRepository.Delete(perfilRemover);
                }

            }
        }

        public List<Perfil> GetPerfisFromAcesso(long idAcesso)
        {
            return _acessoPerfilRepository.Queryable().Where(x => x.Acesso.Id == idAcesso).Select(x => x.Perfil).ToList();
        }

        private Acesso LoginAsMultipleProfile(Sistema sistema, Usuario usuario, LoginDto loginDto)
        {


            // Para tratativas de fluxo antigo, que deve continar


            Acesso acesso = new Acesso();
            acesso.Usuario = usuario;
            acesso.Sistema = sistema;
            acesso.InicioSessao = DateTime.Now;
            acesso.IpOrigem = loginDto.ClienteIpAddress;
            acesso.Servidor = loginDto.Server;
            acesso.Navegador = loginDto.UserAgent;
            acesso.Autorizacao = HashUtil.SHA1(DateTime.Now.ToString() + acesso.Id + Guid.NewGuid()) + HashUtil.MD5(acesso.Usuario.Login + acesso.IpOrigem);
            _acessoRepository.Save(acesso);


            Session.Flush();
            //Necessário para resolver a regra de criação de usuário de portal do Pós Venda
            Session.Evict(acesso.Usuario);
            return acesso;
        }


        #region Logout
        public void Logout(Acesso acessoAtual)
        {
            Logout(acessoAtual, FormaEncerramento.Logout);
        }

        public void LogoutForced(Acesso acessoAtual)
        {
            Logout(acessoAtual, FormaEncerramento.Forced);
        }

        public void LogoutTimeout(Acesso acessoAtual)
        {
            Logout(acessoAtual, FormaEncerramento.TimeOut);
        }

        private void Logout(Acesso acessoAtual, FormaEncerramento formaEncerramento)
        {
            if (acessoAtual == null)
            {
                return;
            }
            acessoAtual.FimSessao = DateTime.Now;
            acessoAtual.FormaEncerramento = formaEncerramento;
            _acessoRepository.Save(acessoAtual);
            Session.Flush();
        }
        #endregion
    }
}
