using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class ConsolidadoPrePropostaJob : BaseJob
    {
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        public ProponenteRepository _proponenteRepository { get; set; }
        public ConsolidadoPrePropostaRepository _consolidadoPrePropostaRepository { get; set; }
        public ConsolidadoPrePropostaService _consolidadoPrePropostaService { get; set; }
        public HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        public ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }
        public StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }
        protected override void Init()
        {
            _prePropostaRepository = new PrePropostaRepository();
            _prePropostaRepository._session = _session;
            _planoPagamentoRepository = new PlanoPagamentoRepository();
            _planoPagamentoRepository._session = _session;
            _proponenteRepository = new ProponenteRepository();
            _proponenteRepository._session = _session;
            _consolidadoPrePropostaRepository = new ConsolidadoPrePropostaRepository();
            _consolidadoPrePropostaRepository._session = _session;
            _historicoPrePropostaRepository = new HistoricoPrePropostaRepository();
            _historicoPrePropostaRepository._session = _session;
            _documentoProponenteRepository = new DocumentoProponenteRepository();
            _documentoProponenteRepository._session = _session;
            _parecerDocumentoProponenteRepository = new ParecerDocumentoProponenteRepository();
            _parecerDocumentoProponenteRepository._session = _session;
            _statusSuatStatusEvsRepository = new StatusSuatStatusEvsRepository();
            _statusSuatStatusEvsRepository._session = _session;


            _consolidadoPrePropostaService = new ConsolidadoPrePropostaService();
            _consolidadoPrePropostaService._consolidadoPrePropostaRepository = _consolidadoPrePropostaRepository;
            _consolidadoPrePropostaService._proponenteRepository = _proponenteRepository;
            _consolidadoPrePropostaService._historicoPrePropostaRepository = _historicoPrePropostaRepository;
            _consolidadoPrePropostaService._prePropostaRepository = _prePropostaRepository;
            _consolidadoPrePropostaService._documentoProponenteRepository = _documentoProponenteRepository;
            _consolidadoPrePropostaService._parecerDocumentoProponenteRepository = _parecerDocumentoProponenteRepository;
            _consolidadoPrePropostaService._planoPagamentoRepository = _planoPagamentoRepository;
            _consolidadoPrePropostaService._statusSuatStatusEvsRepository = _statusSuatStatusEvsRepository;
            _consolidadoPrePropostaService._session = _session;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Process()
        {
            // Pegar a última data de registro consolidado (usar uma data de controle de ultima atualização, vinda do registro)
            var dataCorte = _consolidadoPrePropostaRepository.BuscarUltimaDataAtualizacao();
            WriteLog(TipoLog.Informacao, "Consolidado de Pré Proposta");
            WriteLog(TipoLog.Informacao, "Iniciando processamento");
            // Juntar as propostas que foram mexidas (olhar propostas, planos pagamentos, proponentes, arquivos, etc)
            var prepropostasSemConsolidacao = _prePropostaRepository.BuscarPrePropostasParaConsolidar(dataCorte);

            var totalPrePropostas = prepropostasSemConsolidacao.Count;
            var tamanhoPagina = !ProjectProperties.TamanhoPaginaRoboConsolidadoPreProposta.IsEmpty() ? ProjectProperties.TamanhoPaginaRoboConsolidadoPreProposta : 50;
            var indiceAtual = 0;

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}", totalPrePropostas));

            ITransaction transaction = _session.BeginTransaction();
            foreach (var preProposta in prepropostasSemConsolidacao)
            {
                indiceAtual++;
                try
                {
                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        transaction.Commit();
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalPrePropostas));
                        transaction = _session.BeginTransaction();
                    }

                    _consolidadoPrePropostaService.ProcessarPreProposta(preProposta);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", preProposta.Id, preProposta.Codigo, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
            }
            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }
    }
}