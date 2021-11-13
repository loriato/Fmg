using Autofac;
using Autofac.Integration.WebApi;
using Europa.Commons;
using Europa.Data;
using Europa.Data.Model;
using Europa.Extensions;
using NHibernate;
using Quartz;
using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Data;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public abstract class BaseJob : IJob, IDisposable
    {
        protected bool IsRunning = false;

        // Session to Log (Next refactoging shoud be a IStatelessSession
        private ISession _statelessSession;

        protected ISession _session;
        protected LogExecucaoService _execucaoService;
        protected QuartzConfiguration _currentConfiguration;
        protected Execucao _currentExecution;

        protected BaseJob()
        {          

            _session = NHibernateSession.Session();
            _statelessSession = NHibernateSession.Session();
            _execucaoService = new LogExecucaoService(_statelessSession);
        }

        protected abstract void Init();

        public abstract void Process();

        public virtual async Task Execute(IJobExecutionContext context)
        {
            try
            {
                BeginExecution(context);

                WriteLog(TipoLog.Informacao, string.Format("Iniciando rotina {0} no servidor {1}", _currentConfiguration.Nome, EnviromentInfo));

                this.Init();

                if (IsRunning)
                {
                    WriteLog(TipoLog.Informacao, "Execucao Encerrada. A rotina já está em execucão");
                    return;
                }
                IsRunning = true;

                this.Process();

                IsRunning = false;
            }
            catch (Exception e)
            {
                WriteLog(TipoLog.Erro, String.Format("Erro na execução. Detalhe: {0}", e.Message));
                ExceptionLogger.LogException(e);
                IsRunning = false;
                throw e;
            }
            finally
            {
                WriteLog(TipoLog.Informacao, "Execucao Finalizada: " + _currentConfiguration.Nome);
                CloseExecution();
                CloseIfOpen();
            }
        }

        protected virtual string EnviromentInfo
        {
            get
            {
                string applicationName = "";
                var applicationAlias = HostingEnvironment.ApplicationVirtualPath;
                if (!applicationAlias.IsEmpty())
                {
                    applicationName = applicationAlias.Substring(1);
                }
                return string.Format("[{0} | {1} | {2}]", Environment.MachineName, HostingEnvironment.SiteName, applicationName);
            }
        }
        protected virtual void BeginExecution(IJobExecutionContext context)
        {
            _currentConfiguration = GetQuartzConfiguration(context);
            ITransaction transaction = _statelessSession.BeginTransaction();
            _currentExecution = _execucaoService.IniciarExecucao(_currentConfiguration.Id);
            transaction.Commit();
            _statelessSession.Clear();
        }

        protected virtual void CloseExecution()
        {
            ITransaction transaction = _statelessSession.BeginTransaction();
            _execucaoService.FinalizarExecucao(_currentExecution.Id);
            transaction.Commit();
            _statelessSession.Clear();
        }

        protected virtual void WriteLog(TipoLog tipo, string log)
        {
            if (log.Length > DatabaseStandardDefinitions.FiveHundredTwelveLength)
            {
                log = log.Substring(0, DatabaseStandardDefinitions.FiveHundredTwelveLength - 12) + "-truncate";
            }
            ITransaction transaction = _statelessSession.BeginTransaction();
            _execucaoService.CriarLog(log, _currentExecution.Id, tipo);
            transaction.Commit();
            _statelessSession.Clear();
        }

        protected QuartzConfiguration GetQuartzConfiguration(IJobExecutionContext context)
        {
            // Try Get From JobDataMap.config
            object objQuartz;
            if (context.JobDetail.JobDataMap.TryGetValue("config", out objQuartz))
            {
                return (QuartzConfiguration)objQuartz;
            }

            // Try get from JobDataMap.id
            long idQuartzConfiguration = context.JobDetail.JobDataMap.GetLongValue("id");
            QuartzConfiguration quartz = _statelessSession.Get<QuartzConfiguration>(idQuartzConfiguration);
            if (quartz != null)
            {
                _statelessSession.Evict(quartz);
            }
            return quartz;
        }

        protected void CloseIfOpen()
        {
            _session.CloseIfOpen();
            _statelessSession.CloseIfOpen();
        }

        public void Dispose()
        {
            CloseIfOpen();
        }
    }
}