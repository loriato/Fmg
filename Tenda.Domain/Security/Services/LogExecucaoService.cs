using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class LogExecucaoService : BaseService
    {
        public QuartzConfigurationRepository _quartzConfigurationRepository { get; set; }
        public ExecucaoRepository _execucaoRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }

        public LogExecucaoService(ISession session) : base(session)
        {
            _execucaoRepository = new ExecucaoRepository(session);
            _quartzConfigurationRepository = new QuartzConfigurationRepository(session);
            _logExecucaoRepository = new LogExecucaoRepository(session);
        }

        public void CriarLog(string log, long idExecucao, TipoLog tipo)
        {
            Execucao exec = new Execucao();
            exec.Id = idExecucao;
            var logExecucao = new LogExecucao
            {
                Execucao = exec,
                Log = log,
                Tipo = tipo
            };
            _logExecucaoRepository.Save(logExecucao);
        }

        public Execucao IniciarExecucao(long idQuartz)
        {
            var execucao = new Execucao
            {
                Quartz = _quartzConfigurationRepository.FindById(idQuartz),
                DataInicioExecucao = DateTime.Now
            };
            _execucaoRepository.Save(execucao);
            return execucao;
        }

        public void FinalizarExecucao(long idExecucao)
        {
            var execucao = _execucaoRepository.FindById(idExecucao);
            execucao.DataFimExecucao = DateTime.Now;
            _execucaoRepository.Save(execucao);
        }

        public void ValidarExclucao(long id, DateTime? de, DateTime? ate)
        {
            var exc = new BusinessRuleException();
            if (id.IsEmpty())
            {
                exc.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.IdQuartz).Complete();
            }
            if (de.IsEmpty())
            {
                exc.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.DataDe).Complete();
            }
            if (ate.IsEmpty())
            {
                exc.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.DataAte).Complete();
            }
            exc.ThrowIfHasError();

            if (de.Value > ate.Value)
            {
                exc.AddError(GlobalMessages.DataDeAteInvalida).Complete();
            }
            exc.ThrowIfHasError();
        }

        public void Excluir(long id, DateTime? de, DateTime? ate)
        {
            ValidarExclucao(id, de, ate);
            _logExecucaoRepository.ExcluirLogsExecucaoPorQuartzPorPeriodo(id, de.Value, ate.Value);
            _execucaoRepository.ExcluirExecucaoPorQuartzPorPeriodo(id, de.Value, ate.Value);
        }
    }
}
