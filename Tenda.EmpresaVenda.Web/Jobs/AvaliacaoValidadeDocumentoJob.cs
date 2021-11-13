using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    /// <summary>
    /// Deve verificar as pré-propostas que estejam nas situações PreAnaliseAprovada e Retorno.
    /// Para cada uma das Pré-Propostas, verificar se existe algum documento que a DataExpiracao seja anterior a data atual. 
    /// Para cada um dos documentos vencidos, mudar a situação para Pendente, e definir o motivo como 'Pendência Automática - Validade de Documento Expirada'
    /// Caso a proposta possua um documento vencido, a mesma deve ser direcionada para o status de Pendente.
    /// Usar PrePropostaService.MudarSituacaoProposta para gerar o histórico.
    /// O usuário da execução deve ser um usuário de sistema
    /// </summary>
    public class AvaliacaoValidadeDocumentoJob : BaseJob
    {
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        public DocumentoProponenteService _documentoProponenteService { get; set; }
        public PrePropostaService _prePropostaService { get; set; }
        public UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        public ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }
        public ProponenteRepository _proponenteRepository { get; set; }
        public PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        public ClienteRepository _clienteRepository { get; set; }
        public HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        public HistoricoPrePropostaService _historicoPrePropostaService { get; set; }
        public DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }
        public UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }

        protected override void Init()
        {
            _prePropostaRepository = new PrePropostaRepository();
            _prePropostaRepository._session = _session;
            _documentoProponenteRepository = new DocumentoProponenteRepository();
            _documentoProponenteRepository._session = _session;
            _usuarioPortalRepository = new UsuarioPortalRepository(_session);
            _proponenteRepository = new ProponenteRepository();
            _proponenteRepository._session = _session;
            _planoPagamentoRepository = new PlanoPagamentoRepository();
            _planoPagamentoRepository._session = _session;
            _clienteRepository = new ClienteRepository();
            _clienteRepository._session = _session;
            _historicoPrePropostaRepository = new HistoricoPrePropostaRepository();
            _historicoPrePropostaRepository._session = _session;
            _parecerDocumentoProponenteRepository = new ParecerDocumentoProponenteRepository();
            _parecerDocumentoProponenteRepository._session = _session;
            _documentoRuleMachinePrePropostaRepository = new DocumentoRuleMachinePrePropostaRepository();
            _documentoRuleMachinePrePropostaRepository._session = _session;
            _notificacaoRepository = new NotificacaoRepository();
            _notificacaoRepository._session = _session;
            _usuarioGrupoCCARepository = new UsuarioGrupoCCARepository();
            _usuarioGrupoCCARepository._session = _session;

            _documentoProponenteService = new DocumentoProponenteService();
            _documentoProponenteService._documentoProponenteRepository = _documentoProponenteRepository;
            _documentoProponenteService._parecerDocumentoProponenteRepository = _parecerDocumentoProponenteRepository;
            _documentoProponenteService._session = _session;

            _historicoPrePropostaService = new HistoricoPrePropostaService();
            _historicoPrePropostaService._historicoPrePropostaRepository = _historicoPrePropostaRepository;
            _historicoPrePropostaService._usuarioGrupoCCARepository = _usuarioGrupoCCARepository;
            _historicoPrePropostaService._session = _session;

            _prePropostaService = new PrePropostaService();
            _prePropostaService._clienteRepository = _clienteRepository;
            _prePropostaService._documentoProponenteRepository = _documentoProponenteRepository;
            _prePropostaService._historicoPrePropostaService = _historicoPrePropostaService;
            _prePropostaService._prePropostaRepository = _prePropostaRepository;
            _prePropostaService._proponenteRepository = _proponenteRepository;
            _prePropostaService._documentoRuleMachinePrePropostaRepository = _documentoRuleMachinePrePropostaRepository;
            _prePropostaService._session = _session;
            _prePropostaService._notificacaoRepository = _notificacaoRepository;
        }

        public override void Process()
        {
            WriteLog(TipoLog.Informacao, "Avaliação de Validade de Documentos");

            var entidadesFinalizadas = _documentoProponenteRepository.BuscarDocumentosPrePropostasFinalizadas();
            var usuarioCriador = !ProjectProperties.UsuarioSistema.IsEmpty() ? _usuarioPortalRepository.UsuarioPorLogin(ProjectProperties.UsuarioSistema)
                : _usuarioPortalRepository.UsuarioPorLogin("admin.europa");

            WriteLog(TipoLog.Informacao, "Iniciando processamento");

            var totalRegistros = entidadesFinalizadas.Count;
            var tamanhoPagina = !ProjectProperties.TamanhoPaginaRoboAvaliacaoValidadeDocumento.IsEmpty() ? ProjectProperties.TamanhoPaginaRoboAvaliacaoValidadeDocumento : 50;
            var indiceAtual = 0;

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}", totalRegistros));

            ITransaction transaction = null;
            
            foreach (var entidade in entidadesFinalizadas)
            {

                transaction = _session.BeginTransaction();

                indiceAtual++;
                try
                {
                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalRegistros));
                    }

                    foreach (var documento in entidade.Value)
                    {
                        documento.Situacao = SituacaoAprovacaoDocumento.Pendente;
                        documento.Motivo = GlobalMessages.PendenciaAutomaticaValidadeExpirada;
                        documento.AtualizadoPor = usuarioCriador.Id;
                        documento.AtualizadoEm = DateTime.Now;
                        _documentoProponenteRepository.Save(documento);
                    }

                    //FIX-ME: necessário pois há uma chamada no service para listar os documentos 
                    //e validar as pendencias
                    transaction.Commit();
                    transaction = _session.BeginTransaction();

                    var situacao = SituacaoProposta.DocsInsuficientesSimplificado;

                    switch (entidade.Key.SituacaoProposta)
                    {
                        case SituacaoProposta.AnaliseSimplificadaAprovada:
                            situacao = SituacaoProposta.DocsInsuficientesSimplificado;
                            break;

                        case SituacaoProposta.EmAnaliseCompleta:
                            situacao = SituacaoProposta.DocsInsuficientesCompleta;
                            break;
                    }

                    _prePropostaService.MudarSituacaoProposta(entidade.Key, situacao, usuarioCriador);

                    transaction.Commit();
                    WriteLog(TipoLog.Informacao, String.Format("Processada com sucesso proposta em {0} | {1}", entidade.Key.Id, entidade.Key.Codigo));
                }catch(BusinessRuleException bre)
                {
                    transaction.Rollback();
                    foreach(var erro in bre.Errors)
                    {
                        WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", entidade.Key.Id, entidade.Key.Codigo, erro));
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", entidade.Key.Id, entidade.Key.Codigo, e.Message));
                    ExceptionLogger.LogException(e);
                }
            }

            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
        }
    }
}