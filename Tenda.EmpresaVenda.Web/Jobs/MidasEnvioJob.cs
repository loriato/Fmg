using Europa.Commons;
using Europa.Extensions;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Financeiro;
using Tenda.EmpresaVenda.ApiService.Models.Midas;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class MidasEnvioJob : BaseJob
    {
        private ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }
        private NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        private NotaFiscalPagamentoService _noteFiscalPagamentoService { get; set; }
        private OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }
        private NotaFiscalPagamentoOcorrenciaService _notaFiscalPagamentoOcorrenciaService { get; set; }
        private NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository { get; set; }
        public NumeroPedidoSapRepository _numeroPedidoSapRepository { get; set; }
        private MidasApiService _midasApiService { get; set; }
        protected override void Init()
        {
            _viewRelatorioVendaUnificadoRepository = new ViewRelatorioVendaUnificadoRepository();
            _viewRelatorioVendaUnificadoRepository._session = _session;

            _ocorrenciasMidasRepository = new OcorrenciasMidasRepository();
            _ocorrenciasMidasRepository._session = _session;

            _notaFiscalPagamentoRepository = new NotaFiscalPagamentoRepository();
            _notaFiscalPagamentoRepository._session = _session;

            _notaFiscalPagamentoOcorrenciaService = new NotaFiscalPagamentoOcorrenciaService();
            _notaFiscalPagamentoOcorrenciaService._session = _session;

            _notaFiscalPagamentoOcorrenciaRepository = new NotaFiscalPagamentoOcorrenciaRepository();
            _notaFiscalPagamentoOcorrenciaRepository._session = _session;

            _numeroPedidoSapRepository = new NumeroPedidoSapRepository();
            _numeroPedidoSapRepository._session = _session;

            _noteFiscalPagamentoService = new NotaFiscalPagamentoService();
            _noteFiscalPagamentoService._notaFiscalPagamentoRepository = _notaFiscalPagamentoRepository;
            _noteFiscalPagamentoService._notaFiscalPagamentoOcorrenciaRepository = _notaFiscalPagamentoOcorrenciaRepository;
            _noteFiscalPagamentoService._session = _session;

            var perBaseUrl = new PerBaseUrlFlurlClientFactory();
            _midasApiService = new MidasApiService(perBaseUrl);
        }

        public override void Process()
        {
            //Mandar Notas Aprovadas
            var integracaoAprovada = new List<AprovarIntegracaoRequestDto>();
            var notasAguardando = _viewRelatorioVendaUnificadoRepository.Queryable()
                .Where(x => x.IdNotaFiscalPagamento != null)
                .Where(x => x.SituacaoNotaFiscal == SituacaoNotaFiscal.AguardandoEnvioMidas)
                .GroupBy(x => new { x.IdNotaFiscalPagamento, x.PedidoSap, x.Regional, x.NotaFiscal, x.SituacaoNotaFiscal })
                .Select(x => new ViewRelatorioVendaUnificado
                {
                    IdNotaFiscalPagamento = x.Key.IdNotaFiscalPagamento,
                    PedidoSap = x.Key.PedidoSap,
                    Regional = x.Key.Regional,
                    NotaFiscal = x.Key.NotaFiscal,
                    SituacaoNotaFiscal = x.Key.SituacaoNotaFiscal
                })
                .ToList();
            List<string> Regionais = ProjectProperties.MidasRegionaisList.Split(';').ToList();

            var transaction = _session.BeginTransaction();

            if (ProjectProperties.MidasPassthrough)
            {
                WriteLog(TipoLog.Informacao, "Midas desativado");

                var notasAprovadasPassthrough = _notaFiscalPagamentoRepository.FindNotaFiscalPagamentoPreAprovado();
                var total = notasAprovadasPassthrough.Count();

                WriteLog(TipoLog.Informacao, string.Format("Total de notas Pre-aprovadas: {0}", total));

                foreach (var nota in notasAprovadasPassthrough)
                {
                    WriteLog(TipoLog.Informacao, string.Format("Avançando Nota Fiscal: {0}", nota.NotaFiscal));
                    _noteFiscalPagamentoService.MudarSituacao(nota, SituacaoNotaFiscal.Aprovado);
                }
            }
            else
            {
                if (notasAguardando.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, "Não há notas ficais para serem avaliadas");
                    return;
                }

                var total = notasAguardando.Count();
                WriteLog(TipoLog.Informacao, string.Format("Total de notas pre-aprovadas: {0}", total));

                foreach (var nota in notasAguardando)
                {
                    //Regional
                    if (!Regionais.Contains(nota.Regional))
                    {
                        transaction = _session.BeginTransaction();
                        WriteLog(TipoLog.Informacao, string.Format("Regional: {0} da Nota Fiscal: {1} não inclusa no parametro.",nota.Regional, nota.NotaFiscal));
                        WriteLog(TipoLog.Informacao, string.Format("Avançando Nota Fiscal: {0}", nota.NotaFiscal));
                        var notaFiscal = _notaFiscalPagamentoRepository.FindById(nota.IdNotaFiscalPagamento);
                        if (notaFiscal.IsNull())
                        {
                            WriteLog(TipoLog.Informacao, string.Format("Erro Nota Fiscal: {0} não encotrada no banco.", nota.NotaFiscal));
                            continue;
                        }
                        _noteFiscalPagamentoService.MudarSituacao(notaFiscal, SituacaoNotaFiscal.Aprovado);
                        transaction.Commit();
                        continue;
                    }

                    var crz = _notaFiscalPagamentoOcorrenciaRepository.FindByNotaFiscalId(nota.IdNotaFiscalPagamento);

                    if (!crz.HasValue())
                    {
                        WriteLog(TipoLog.Erro, string.Format("Erro na Nota Fiscal: {0} | PedidoSap: {1}, Cruzamento não encontrado!", nota.NotaFiscal, nota.PedidoSap));
                        continue;
                    }

                    var dataAprovamento = DateTime.Now.ToString();
                    List<string> listaNumeroItemDocumentoCompra = _numeroPedidoSapRepository.ListarNumeroItemDocumentoCompraPorPedidoSap(nota.PedidoSap);

                    if (listaNumeroItemDocumentoCompra.IsEmpty())
                    {
                        WriteLog(TipoLog.Erro, string.Format("Erro na Nota Fiscal: {0} | PedidoSap: {1}, Numero do Item do documento de compra não encontrado!", nota.NotaFiscal, nota.PedidoSap));
                        continue;
                    }

                    var numeroItemComposto = "";
                    var i = 0;

                    foreach(var numeroItem in listaNumeroItemDocumentoCompra)
                    {
                        if (i != 0)
                            numeroItemComposto += "|";
                        numeroItemComposto += string.Format("{0}/{1}/1,000/1", nota.PedidoSap, numeroItem);
                        i++;
                    }

                    AprovarIntegracaoRequestDto aprovarIntegracao = new AprovarIntegracaoRequestDto
                    {
                        DocumentArea = "COM",
                        OccurrenceId = crz.Ocorrencia.OccurenceId.ToString(),
                        Description = string.Format("Data/Hora: {0} - Aprovado por: Portal EV", dataAprovamento),
                        Status = "910",
                        Accept = "true",
                        FormOfPayment = "T",
                        Anticipation = "",
                        HasInterestOnPayment = "N",
                        OrderCause = "",
                        PurchaseOrder = numeroItemComposto,
                        ApprovalDate = dataAprovamento
                    };

                    integracaoAprovada.Add(aprovarIntegracao);
                }
                try
                {
                    if (integracaoAprovada.HasValue())
                    {
                        var responses = _midasApiService.AprovarIntegracaoOcorrencia(integracaoAprovada);

                        transaction = _session.BeginTransaction();

                        foreach (var response in responses.SuccessList)
                        {
                            IntegracaoAprovada(response);
                            WriteLog(TipoLog.Informacao, string.Format("Ocorrencia: {0} - {1} Integrada com sucesso.", response.OccurrenceId, response.PurchaseOrder));
                        }
                        foreach (var response in responses.ErrorList)
                        {
                            WriteLog(TipoLog.Erro, string.Format("Ocorrencia: {0} - {1} Erro de Integração. Mensagem: {2}", response.OccurrenceId, response.PurchaseOrder, response.Message));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(TipoLog.Erro, string.Format(ex.Message));
                    ExceptionLogger.LogException(ex);
                }
            }
            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }
        
        public void IntegracaoAprovada(AprovarIntegracaoResponseDto aprovarIntegracaoResponseDto)
        {
            if (aprovarIntegracaoResponseDto.OccurrenceId.HasValue())
            {
                WriteLog(TipoLog.Informacao, "OccorenceId retornado.");
                _noteFiscalPagamentoService.MudarSituacaoMidasIntegracaoAprovada(aprovarIntegracaoResponseDto.OccurrenceId);
            }
            else
            {
                WriteLog(TipoLog.Erro, "OccoreceId retornado nullo");
            }
        }
    }
}