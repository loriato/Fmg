using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class FechamentoContabilNotificaJob : BaseJob
    {
        //repos
        public FechamentoContabilService _fechamentoContabilService { get; set; }
        public FechamentoContabilRepository _fechamentoContabilRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public FilaEmailService _filaEmailService { get; set; }

        public FilaEmailRepository _filaEmailRepository { get; set; }
        public FilaEmailValidator _filaEmailValidator { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }

        protected override void Init()
        {
            //repos init

            _fechamentoContabilRepository = new FechamentoContabilRepository();
            _fechamentoContabilRepository._session = _session;

            _corretorRepository = new CorretorRepository();
            _corretorRepository._session = _session;

            _notificacaoRepository = new NotificacaoRepository();
            _notificacaoRepository._session = _session;

            _filaEmailRepository = new FilaEmailRepository();
            _filaEmailRepository._session = _session;

            _arquivoRepository = new ArquivoRepository();
            _arquivoRepository._session = _session;

            _filaEmailService = new FilaEmailService();
            _filaEmailService._session = _session;
            _filaEmailService._filaEmailValidator = new FilaEmailValidator();
            _filaEmailService._filaEmailRepository = _filaEmailRepository;
            _filaEmailService._arquivoRepository = _arquivoRepository;

            _fechamentoContabilService = new FechamentoContabilService();
            _fechamentoContabilService._session = _session;
            _fechamentoContabilService._fechamentoContabilRepository = _fechamentoContabilRepository;
            _fechamentoContabilService._corretorRepository = _corretorRepository;
            _fechamentoContabilService._notificacaoRepository = _notificacaoRepository;
            _fechamentoContabilService._filaEmailService = _filaEmailService;
        }
        public override void Process()
        {
            //process
            List<FechamentoContabil> fechamentos =  _fechamentoContabilRepository.ListFechamentosANotificar();


            if (fechamentos.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há fechamentos a serem notificados");
                return;
            }

            var transaction = _session.BeginTransaction();

            foreach (FechamentoContabil fechamento in fechamentos)
            {
                try
                {
                        _fechamentoContabilService.ComunicarFechamentoContabil(fechamento);
                }
                catch (BusinessRuleException bre)
                {
                    foreach (var erro in bre.Errors)
                    {
                        WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", fechamento.Id, fechamento.Descricao, erro));
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", fechamento.Id, fechamento.Descricao, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
                finally
                {
                    if (transaction.IsActive)
                    {
                        transaction.Commit();

                    }
                }
            }

            if (transaction.IsActive)
            {
                transaction.Commit();

            }
        }
    }
}