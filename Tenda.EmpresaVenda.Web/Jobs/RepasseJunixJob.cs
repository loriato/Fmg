using Europa.Commons;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class RepasseJunixJob : BaseJob
    {
        public PropostaSuatImportService _propostaSuatImportService { get; set; }

        public ImportacaoJunixRepository _importacaoJunixRepository { get; set; }


        protected override void Init()
        {
            _propostaSuatImportService = new PropostaSuatImportService();
            _propostaSuatImportService._session = _session;

            _importacaoJunixRepository = new ImportacaoJunixRepository();
            _importacaoJunixRepository._session = _session;

            _propostaSuatImportService._corretorRepository = new CorretorRepository();
            _propostaSuatImportService._corretorRepository._session = _session;

            _propostaSuatImportService._notificacaoRepository = new NotificacaoRepository();
            _propostaSuatImportService._notificacaoRepository._session = _session;

            _propostaSuatImportService._planoPagamentoRepository = new PlanoPagamentoRepository();
            _propostaSuatImportService._planoPagamentoRepository._session = _session;
        }

        public override void Process()
        {
            //retorna lista dos arquivos existentes na pasta
            var importacaoJunix = _importacaoJunixRepository.BuscarUmaImportacaoAguardando();

            if (importacaoJunix == null)
            {
                WriteLog(TipoLog.Informacao, "Não existem arquivos aguardando processamento. A rotina será finalizada.");

                return;
            }

            try
            {
                importacaoJunix.Execucao = _currentExecution;
                importacaoJunix.DataInicio = DateTime.Now;
                importacaoJunix.Situacao = SituacaoArquivo.EmProcessamento;

                ITransaction transaction = _session.BeginTransaction();
                _importacaoJunixRepository.Save(importacaoJunix);
                transaction.Commit();

                //processa o arquivo
                ProcessFile(importacaoJunix);

                //Criado para utilização do consolidado de relatório de comissão
                if (ProjectProperties.AtivarRefreshViewRelatorioComissao)
                {
                    WriteLog(TipoLog.Informacao, "Atualizando Relatório de Comissão");
                    _session.CreateSQLQuery("refresh materialized view concurrently vw_rel_comissao")
                        .SetTimeout(240)
                        .ExecuteUpdate();
                    WriteLog(TipoLog.Informacao, "Relatório de Comissão atualizado com sucesso!!");
                }
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);

                ITransaction transaction = _session.BeginTransaction();
                importacaoJunix.Situacao = SituacaoArquivo.Erro;
                _importacaoJunixRepository.Save(importacaoJunix);
                transaction.Commit();
            }

        }

        public void ProcessFile(ImportacaoJunix importacaoJunix)
        {
            ImportTaskDTO importTask = new ImportTaskDTO();

            importTask.FileName = importacaoJunix.Arquivo.Nome;

            // Internamente as transações são iniciadas e paradas pelo service.
            _propostaSuatImportService.InternalProcess(importacaoJunix.Arquivo, ref importTask);


            if (importTask.ErrorCount == 0)
            {
                importacaoJunix.Situacao = SituacaoArquivo.Processado;
            }
            else
            {
                WriteLog(TipoLog.Informacao, string.Format("Arquivo [{0}] com ERRO", importacaoJunix.Arquivo.Nome));

                //preenche situação do arquivo
                if (importTask.SuccessCount == 0)
                {
                    importacaoJunix.Situacao = SituacaoArquivo.Erro;
                }
                else
                {
                    importacaoJunix.Situacao = SituacaoArquivo.ProcessadoRessalva;
                }
            }

            var logs = importTask.FullLog.Split('\n').Reverse();

            foreach (var log in logs)
            {
                if (!log.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, log);
                }
            }

            //preenche a data de finalização
            importacaoJunix.DataFim = DateTime.Now;

            ITransaction transaction = _session.BeginTransaction();
            _importacaoJunixRepository.Save(importacaoJunix);
            transaction.Commit();
        }

    }
}