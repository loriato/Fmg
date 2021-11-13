using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;


namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class RegraAtivaSemAceiteJob : BaseJob
    {
        public RegraAtivaSemAceiteService _regraAtivaSemAceiteService { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        public PontoVendaRepository _pontoVendaRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }

        protected override void Init()
        {
            
            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _session;
            _corretorRepository = new CorretorRepository();
            _corretorRepository._session = _session;
            _usuarioPortalRepository = new UsuarioPortalRepository(_session);
            _usuarioPortalRepository._session = _session;
            _pontoVendaRepository = new PontoVendaRepository();
            _pontoVendaRepository._session = _session;
            _regraComissaoEvsRepository = new RegraComissaoEvsRepository(_session);
            _regraComissaoEvsRepository._session = _session;
            _aceiteRegraComissaoEvsRepository = new AceiteRegraComissaoEvsRepository();
            _aceiteRegraComissaoEvsRepository._session = _session;

            _regraAtivaSemAceiteService = new RegraAtivaSemAceiteService();
            _regraAtivaSemAceiteService._regraComissaoEvsRepository = _regraComissaoEvsRepository;
            _regraAtivaSemAceiteService._aceiteRegraComissaoEvsRepository = _aceiteRegraComissaoEvsRepository;
            _regraAtivaSemAceiteService._session = _session;

        }
        public override void Process()
        {
            //Buscar empresa de venda com regra de comissão sem aceite
            var idempresaVenda = _regraAtivaSemAceiteService.EmpresasSemAceite();
            // Buscar id de usuarios  dos corretores das evs
            var idUsuarioCorretores = _corretorRepository.ListarEmailsDiretoresEmpresaDeVendas(idempresaVenda);
            // Buscar id de usuarios dos viabilizadores do ponto de venda;
            var idUsuarioViabilizador = _pontoVendaRepository.BuscarViabilizadoresPontoVenda(idempresaVenda);

            //Juntando a Lista de Usuarios
            var idUsuarios = idUsuarioViabilizador;
            idUsuarios.AddRange(idUsuarioCorretores);
            var indiceAtual = 0;

            // Buscar lista de Usuarios 
            var result = _usuarioPortalRepository.BuscarUsuarios(idUsuarios);
            var Total = result.Count();

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}",Total));
            
            WriteLog(TipoLog.Informacao, "Iniciando Envio de E-mails ");

            foreach(var usuario in result)
            {
                try
                {
                    //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Regex regex = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                    var match = regex.Match(usuario.Email);

                    if (!match.Success)
                    {
                        WriteLog(TipoLog.Erro, string.Format("E-mail inválido: {0} | Usuário: {1}", usuario.Email,usuario.Nome));
                    }else if (!usuario.Email.IsEmpty())
                    {
                        _regraAtivaSemAceiteService.EnviarEmail(usuario.Nome, usuario.Email);
                        indiceAtual++;
                    }
                    else
                    {
                        WriteLog(TipoLog.Informacao, String.Format("O usuário {0:0000} não possui email", usuario.Nome));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                    WriteLog(TipoLog.Erro, string.Format("{0} | {1} | {2}", usuario.Nome,usuario.Email, e.Message));
                }
            }
            WriteLog(TipoLog.Informacao, String.Format("Emails enviados {0:0000} de {1:0000}", indiceAtual, Total));
            
        }
    }
}
