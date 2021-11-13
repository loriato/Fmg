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
    public class MidasCruzamentoJob : BaseJob
    {
        private ViewNotaFiscalPagamentoRepository _viewNotaFiscalPagamentoRepository { get; set; }
        private NotaFiscalPagamentoService _noteFiscalPagamentoService { get; set; }
        private OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }
        private NotaFiscalPagamentoOcorrenciaService _notaFiscalPagamentoOcorrenciaService { get; set; }
        private NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository { get; set; }
        private NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EnderecoFornecedorRepository _enderecoFornecedorRepository { get; set; }
        private MidasApiService _midasApiService { get; set; }
        private ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }

        protected override void Init()
        {
            _viewNotaFiscalPagamentoRepository = new ViewNotaFiscalPagamentoRepository();
            _viewNotaFiscalPagamentoRepository._session = _session;

            _ocorrenciasMidasRepository = new OcorrenciasMidasRepository();
            _ocorrenciasMidasRepository._session = _session;

            _notaFiscalPagamentoOcorrenciaRepository = new NotaFiscalPagamentoOcorrenciaRepository();
            _notaFiscalPagamentoOcorrenciaRepository._session = _session;

            _notaFiscalPagamentoOcorrenciaService = new NotaFiscalPagamentoOcorrenciaService();
            _notaFiscalPagamentoOcorrenciaService._session = _session;
            _notaFiscalPagamentoOcorrenciaService._notaFiscalPagamentoOcorrenciaRepository = _notaFiscalPagamentoOcorrenciaRepository;

            _notaFiscalPagamentoRepository = new NotaFiscalPagamentoRepository();
            _notaFiscalPagamentoRepository._session = _session;

            _empreendimentoRepository = new EmpreendimentoRepository();
            _empreendimentoRepository._session = _session;

            _enderecoFornecedorRepository = new EnderecoFornecedorRepository();
            _enderecoFornecedorRepository._session = _session;

            _noteFiscalPagamentoService = new NotaFiscalPagamentoService();
            _noteFiscalPagamentoService._session = _session;
            _noteFiscalPagamentoService._notaFiscalPagamentoRepository = _notaFiscalPagamentoRepository;

            _viewRelatorioVendaUnificadoRepository = new ViewRelatorioVendaUnificadoRepository();
            _viewRelatorioVendaUnificadoRepository._session = _session;

            var perBaseUrl = new PerBaseUrlFlurlClientFactory();
            _midasApiService = new MidasApiService(perBaseUrl);
        }

        public override void Process()
        {
            var notasAguardando = _viewRelatorioVendaUnificadoRepository.Queryable()
                .Where(x => x.IdNotaFiscalPagamento != null)
                .Where(x => x.SituacaoNotaFiscal == SituacaoNotaFiscal.AguardandoProcessamento)
                .ToList();

            List<string> Regionais = ProjectProperties.MidasRegionaisList.Split(';').ToList();

            var transaction = _session.BeginTransaction();
            WriteLog(TipoLog.Informacao, string.Format("Processando {0} nota(s) fiscais.", notasAguardando.Count));
            if (ProjectProperties.MidasPassthrough)
            {
                WriteLog(TipoLog.Informacao, string.Format("Midas desativado."));
            }
            foreach (var nota in notasAguardando)
            {
                if (ProjectProperties.MidasPassthrough || !Regionais.Contains(nota.Regional))
                {
                    transaction = _session.BeginTransaction();
                    WriteLog(TipoLog.Informacao, string.Format("Avançando Nota Fiscal: {0}", nota.NotaFiscal));

                    var notaFiscal = _notaFiscalPagamentoRepository.FindById(nota.IdNotaFiscalPagamento);
                    notaFiscal.Situacao = SituacaoNotaFiscal.AguardandoAvaliacao;
                    _notaFiscalPagamentoRepository.Save(notaFiscal);

                    if (transaction.IsActive)
                    {
                        transaction.Commit();
                    }

                    continue;
                }
                try
                {
                    transaction = _session.BeginTransaction();

                    var fornecedor = _enderecoFornecedorRepository.Queryable()
                        .Where(x => x.CodigoFornecedor.Equals(nota.CodigoEmpresa))
                        .Where(x => x.Estado.Equals(nota.Estado))
                        .FirstOrDefault();

                    OcorrenciasMidas ocorrencia = null;
                    if (fornecedor.HasValue())
                    {
                        ocorrencia = _ocorrenciasMidasRepository.Queryable()
                            .Where(x => x.TaxIdProvider.Equals(nota.CnpjEmpresaVenda) && x.TaxIdTaker.Equals(fornecedor.Cnpj) && x.Document.Number.Equals(nota.NotaFiscal))
                            .FirstOrDefault();
                    }

                    if (ocorrencia.IsEmpty())
                    {
                        var empreendimento = _empreendimentoRepository.FindById(nota.IdEmpreendimento);
                        ocorrencia = _ocorrenciasMidasRepository.Queryable()
                            .Where(x => x.TaxIdProvider.Equals(nota.CnpjEmpresaVenda) && x.TaxIdTaker.Equals(empreendimento.CNPJ) && x.Document.Number.Equals(nota.NotaFiscal))
                            .FirstOrDefault();
                    }

                    if (ocorrencia.IsEmpty())
                    {
                        //aqui explica o que não deu certo no cruzamento
                        WriteLog(TipoLog.Informacao, string.Format("Nota Fiscal: {0} CNPJ EV: {1} não foi encontrado match.", nota.NotaFiscal, nota.CnpjEmpresaVenda));
                        continue;
                    }

                    var notaFiscal = _notaFiscalPagamentoRepository.FindById(nota.IdNotaFiscalPagamento);

                    var cruzamentosExistentes = _notaFiscalPagamentoOcorrenciaRepository.Queryable()
                            .Where(x => x.NotaFiscalPagamento.Id == nota.IdNotaFiscalPagamento || x.Ocorrencia.Id == ocorrencia.Id);

                    if (cruzamentosExistentes.HasValue())
                    {
                        //aqui avisa que o cruzamento já existe e exclui os exsitentes para criar um novo afim de atualizar os dados do cruzamento
                        WriteLog(TipoLog.Informacao, string.Format("Cruzamento de Nota Fiscal: {0} ou Ocorrencia: {1} já existe no cruzamento.", nota.NotaFiscal, ocorrencia.OccurenceId));

                        foreach(var cruzamentoExistente in cruzamentosExistentes)
                        {
                            WriteLog(TipoLog.Informacao, string.Format("Excluindo cruzamento de Nota Fiscal: {0} e Ocorrencia: {1}.", cruzamentoExistente.NotaFiscalPagamento.NotaFiscal, cruzamentoExistente.Ocorrencia.OccurenceId));
                            _notaFiscalPagamentoOcorrenciaRepository.Delete(cruzamentoExistente);
                        }
                    }

                    //se chegou aqui, temos a ocorrência e a nota fiscal para fazer o cruzamento
                    WriteLog(TipoLog.Informacao, string.Format("Criando cruzamento de Nota Fiscal: {0} e Ocorrencia: {1}.", nota.NotaFiscal, ocorrencia.OccurenceId));
                    var cruzamento = new NotaFiscalPagamentoOcorrencia();

                    cruzamento.NotaFiscalPagamento = new NotaFiscalPagamento { Id = nota.IdNotaFiscalPagamento };
                    cruzamento.Ocorrencia = new OcorrenciasMidas { Id = ocorrencia.Id };


                    notaFiscal.Situacao = SituacaoNotaFiscal.AguardandoAvaliacao;
                    _notaFiscalPagamentoRepository.Save(notaFiscal);

                    _notaFiscalPagamentoOcorrenciaRepository.Save(cruzamento);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    WriteLog(TipoLog.Erro, ex.Message);
                    transaction.Rollback();
                }
                if (transaction.IsActive)
                {
                    transaction.Commit();
                }
            }
        }
    }
}