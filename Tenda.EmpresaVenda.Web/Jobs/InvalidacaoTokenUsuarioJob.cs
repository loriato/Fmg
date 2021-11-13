using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class InvalidacaoTokenUsuarioJob : BaseJob
    {
        private UsuarioRepository _usuarioRepository;
        private UsuarioService _usuarioService;
        private TokenAceiteRepository _tokenAceiteRepository;
        private TokenAceiteService _tokenAceiteService;
        protected override void Init()
        {
            _usuarioRepository = new UsuarioRepository(_session);
            _usuarioService = new UsuarioService();
            _usuarioService._session = _session;
            _usuarioService._usuarioRepository = _usuarioRepository;
            _tokenAceiteRepository = new TokenAceiteRepository();
            _tokenAceiteRepository._session = _session;
            _tokenAceiteService = new TokenAceiteService();
            _tokenAceiteService._session = _session;
            _tokenAceiteService._tokenAceiteRepository = _tokenAceiteRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Process()
        {
            const int NumeroDeHorasCorte = 24;
            WriteLog(TipoLog.Informacao, "Invalidador de Tokens de Usuários");
            WriteLog(TipoLog.Informacao, "Iniciando processamento");

            var quantidadeHorasCorte = ProjectProperties.QuantidadeHorasInvalidacaoTokenUsuario.IsEmpty() ?
                NumeroDeHorasCorte :
                ProjectProperties.QuantidadeHorasInvalidacaoTokenUsuario;
            ITransaction transaction = _session.BeginTransaction();

            try
            {
                var idUsuario = ProjectProperties.IdUsuarioSistema;
                var quantidadeUsuariosAtualizados = _usuarioService.InvalidarTokenUsuarios(quantidadeHorasCorte, idUsuario);
                WriteLog(TipoLog.Informacao, String.Format("Quantidade de usuários atualizados: {0:0000}", quantidadeUsuariosAtualizados));
                transaction.Commit();

                transaction = _session.BeginTransaction();
                var tokensInvalidados = _tokenAceiteService.InvalidarTokens(quantidadeHorasCorte);
                WriteLog(TipoLog.Informacao, String.Format("Quantidade de tokens invalidados: {0:0000}", tokensInvalidados));
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                WriteLog(TipoLog.Erro, String.Format("Erro ao atualizar usuários. Mensagem: {0}", e.Message));
                ExceptionLogger.LogException(e);
            }

            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }
    }
}