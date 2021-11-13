using Europa.Commons;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class ConsolidadoRelatorioComissaoJob : BaseJob
    {
        public ConsolidadoRelatorioComissaoRepository _consolidadoRelatorioComissaoRepository { get; set; }
        public PropostaSuatRepository _propostaSuatRepository { get; set; }
        public ConsolidadoRelatorioComissaoService _consolidadoRelatorioComissaoService { get; set; }
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        public AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }
        public SinteseStatusContratoJunixRepository _sinteseStatusContratoJunixRepository { get; set; }
        public StatusConformidadeRepository _statusConformidadeRepository { get; set; }
        public PagamentoRepository _pagamentoRepository { get; set; }
        public RateioComissaoRepository _rateioComissaoRepository { get; set; }
        public ValorNominalRepository _valorNominalRepository { get; set; }
        protected override void Init()
        {
            _consolidadoRelatorioComissaoRepository = new ConsolidadoRelatorioComissaoRepository();
            _consolidadoRelatorioComissaoRepository._session = _session;

            _prePropostaRepository = new PrePropostaRepository();
            _prePropostaRepository._session = _session;

            _empreendimentoRepository = new EmpreendimentoRepository();
            _empreendimentoRepository._session = _session;

            _lojaRepository = new LojaRepository(_session);

            _regraComissaoEvsRepository = new RegraComissaoEvsRepository(_session);

            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _session;
            _empresaVendaRepository._regraComissaoEvsRepository = _regraComissaoEvsRepository;

            _itemRegraComissaoRepository = new ItemRegraComissaoRepository();
            _itemRegraComissaoRepository._session = _session;

            _aceiteRegraComissaoEvsRepository = new AceiteRegraComissaoEvsRepository();
            _aceiteRegraComissaoEvsRepository._session = _session;

            _sinteseStatusContratoJunixRepository = new SinteseStatusContratoJunixRepository();
            _sinteseStatusContratoJunixRepository._session = _session;

            _statusConformidadeRepository = new StatusConformidadeRepository();
            _statusConformidadeRepository._session = _session;

            _pagamentoRepository = new PagamentoRepository();
            _pagamentoRepository._session = _session;

            _rateioComissaoRepository = new RateioComissaoRepository();
            _rateioComissaoRepository._session = _session;

            _valorNominalRepository = new ValorNominalRepository();
            _valorNominalRepository._session = _session;

            _consolidadoRelatorioComissaoService = new ConsolidadoRelatorioComissaoService();
            _consolidadoRelatorioComissaoService._session = _session;
            _consolidadoRelatorioComissaoService._consolidadoRelatorioComissaoRepository = _consolidadoRelatorioComissaoRepository;
            _consolidadoRelatorioComissaoService._prePropostaRepository = _prePropostaRepository;
            _consolidadoRelatorioComissaoService._empreendimentoRepository = _empreendimentoRepository;
            _consolidadoRelatorioComissaoService._lojaRepository = _lojaRepository;
            _consolidadoRelatorioComissaoService._empresaVendaRepository = _empresaVendaRepository;
            _consolidadoRelatorioComissaoService._regraComissaoEvsRepository = new RegraComissaoEvsRepository(_session);
            _consolidadoRelatorioComissaoService._itemRegraComissaoRepository = _itemRegraComissaoRepository;
            _consolidadoRelatorioComissaoService._aceiteRegraComissaoEvsRepository = _aceiteRegraComissaoEvsRepository;
            _consolidadoRelatorioComissaoService._sinteseStatusContratoJunixRepository = _sinteseStatusContratoJunixRepository;
            _consolidadoRelatorioComissaoService._statusConformidadeRepository = _statusConformidadeRepository;
            _consolidadoRelatorioComissaoService._pagamentoRepository = _pagamentoRepository;
            _consolidadoRelatorioComissaoService._rateioComissaoRepository = _rateioComissaoRepository;
            _consolidadoRelatorioComissaoService._valorNominalRepository = _valorNominalRepository;

            _propostaSuatRepository = new PropostaSuatRepository();
            _propostaSuatRepository._session = _session;
            _propostaSuatRepository._lojaRepository = _lojaRepository;
            _propostaSuatRepository._empresaVendaRepository = _empresaVendaRepository;
            _propostaSuatRepository._pagamentoRepository = _pagamentoRepository;
        }

        public override void Process()
        {
            // Pegar a última data de registro consolidado (usar uma data de controle de ultima atualização, vinda do registro)
            var dataCorte = _consolidadoRelatorioComissaoRepository.BuscarUltimaDataAtualizacao();

            if (dataCorte.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Data de corte Default");
            }
            else
            {
                WriteLog(TipoLog.Informacao, "Data de corte: " + dataCorte.ToDateTimeSeconds());
            }

            var tamanhoPagina = !ProjectProperties.TamanhoPaginaRoboConsolidadoPreProposta.IsEmpty() ? ProjectProperties.TamanhoPaginaRoboConsolidadoPreProposta : 50;

            // Juntar as propostas que foram mexidas (olhar propostas, planos pagamentos, proponentes, arquivos, etc)
            var propostasSemConsolidacao = _propostaSuatRepository.BuscarPropostasParaConsolidar(dataCorte);
            
            var totalPropostas = propostasSemConsolidacao.Count;
            var indiceAtual = 0;
            var processadosComSucesso = totalPropostas;

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}", totalPropostas));

            ITransaction transaction = _session.BeginTransaction();

            foreach (var proposta in propostasSemConsolidacao)
            {
                try
                {
                    indiceAtual++;

                    //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Iniciando consolidado da proposta {0}", proposta.CodigoProposta));
                    //GenericFileLogUtil.DevLogWithDateOnBegin(String.Format("ID: {0} | Proposta:{1} | " +
                    //    "Adiantamento Conformidade:{2} | Data Conformidade:{3} | " +
                    //    "Adiantamento Repasse:{4} | Data Repasse:{5} | " +
                    //    "Faturado:{3} | Data Faturamento:{4}",
                    //    proposta.Id, proposta.CodigoProposta,
                    //    proposta.AdiantamentoConformidade.HasValue() ? proposta.AdiantamentoConformidade.AsString() : "",
                    //    proposta.DataConformidade.ToString(),
                    //    proposta.AdiantamentoRepasse.HasValue() ? proposta.AdiantamentoRepasse.AsString() : "",
                    //    proposta.DataRepasse.ToString(),
                    //    proposta.Faturado, proposta.DataFaturado));

                    _consolidadoRelatorioComissaoService.ProcessarProposta(proposta);

                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        transaction.Commit();
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalPropostas));
                        transaction = _session.BeginTransaction();
                    }

                    proposta.DetalhesConsolicacao = string.Format("Consolidada em {0} ",DateTime.Now);

                    proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;

                    _propostaSuatRepository.Save(proposta);
                    WriteLog(TipoLog.Informacao, String.Format("Sucesso em {0} | {1}. Mensagem: Consolidada com sucesso", proposta.Id, proposta.CodigoProposta));
                    
                    //GenericFileLogUtil.DevLogWithDateOnBegin(String.Format("Sucesso em {0} | {1}. Mensagem: Consolidada com sucesso", proposta.Id, proposta.CodigoProposta));
                    //GenericFileLogUtil.DevLogWithDateOnBegin(String.Format("ID: {0} | Proposta:{1} | " +
                    //    "Adiantamento Conformidade:{2} | Data Conformidade:{3} | " +
                    //    "Adiantamento Repasse:{4} | Data Repasse:{5} | " +
                    //    "Faturado:{3} | Data Faturamento:{4}",
                    //    proposta.Id, proposta.CodigoProposta,
                    //    proposta.AdiantamentoConformidade.HasValue() ? proposta.AdiantamentoConformidade.AsString() : "",
                    //    proposta.DataConformidade.ToString() ,
                    //    proposta.AdiantamentoRepasse.HasValue() ? proposta.AdiantamentoRepasse.AsString() : "",
                    //    proposta.DataRepasse.ToString() ,
                    //    proposta.Faturado, proposta.DataFaturado));

                }
                catch (BusinessRuleException bre)
                {
                    processadosComSucesso--;
                    var erro = bre.Errors[0].ToString();

                    proposta.DetalhesConsolicacao = String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, erro);

                    proposta.AtualizadoEm = DateTime.Now;
                    proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;

                    _propostaSuatRepository.Save(proposta);
                    // Depois vemos se tem alguma tela em que isso pode ser exibido 
                    WriteLog(TipoLog.Aviso, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, erro));

                    //GenericFileLogUtil.DevLogWithDateOnBegin(String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, erro));
                    //GenericFileLogUtil.DevLogWithDateOnBegin(String.Format("ID: {0} | Proposta:{1} | " +
                    //    "Adiantamento Conformidade:{2} | Data Conformidade:{3} | " +
                    //    "Adiantamento Repasse:{4} | Data Repasse:{5} | " +
                    //    "Faturado:{3} | Data Faturamento:{4}",
                    //    proposta.Id, proposta.CodigoProposta,
                    //    proposta.AdiantamentoConformidade.HasValue() ? proposta.AdiantamentoConformidade.AsString() : "",
                    //    proposta.DataConformidade.ToString(),
                    //    proposta.AdiantamentoRepasse.HasValue() ? proposta.AdiantamentoRepasse.AsString() : "",
                    //    proposta.DataRepasse.ToString(),
                    //    proposta.Faturado, proposta.DataFaturado));

                }
                catch (Exception e)
                {
                    processadosComSucesso--;
                    transaction.RollbackIfActive();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
            }
            transaction.CommitIfActive();
            WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000} com sucesso", processadosComSucesso, totalPropostas));
        }
    }
}