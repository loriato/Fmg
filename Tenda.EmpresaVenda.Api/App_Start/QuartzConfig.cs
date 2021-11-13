

using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Data;

namespace Tenda.EmpresaVenda.Api
{
    public class QuartzConfig
    {
        private const string TriggerName = "TRIGGER";
        private const string TriggerGroup = "JOBS";
        private const string TriggerNameImmediate = "TRIGGER_IMMEDIATE";
        private const string TriggerGroupImmediate = "JOBS_IMMEDIATE";
        private const string Immediate = "_IMMEDIATE";
        private readonly ISession _session;

        public QuartzConfig(ISession session)
        {
            _session = session;
        }

        public async Task<IScheduler> GetScheduler()
        {
            return await new StdSchedulerFactory().GetScheduler();
        }

        public static void Config()
        {
            ISession session = null;
            try
            {
                session = NHibernateSession.Session();
                var config = new QuartzConfig(session);
                config.InitConfig();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
            finally
            {
                NHibernateSession.CloseIfOpen(session);
            }
        }

        public async Task InitConfig()
        {
            var scheduler = GetScheduler();
            var repo = new QuartzConfigurationRepository(_session);

            IList<QuartzConfiguration> jobsConf = repo.Queryable().Where(reg => reg.IniciarAutomaticamente).ToList();

            foreach (var job in jobsConf)
                if (CanRunInThisServer(job))
                    try
                    {
                        ScheduleJob(job);
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                    }
                else
                    ExceptionLogger.LogException(new Exception(MensagemServidorErrado(job)));

            await scheduler.Result.Start();
        }

        private bool CanRunInThisServer(QuartzConfiguration quartz)
        {
            var applicationName = "";
            var applicationAlias = HostingEnvironment.ApplicationVirtualPath;
            if (!applicationAlias.IsEmpty()) applicationName = applicationAlias.Substring(1);

            return
                // Está no servidor indicado
                Environment.MachineName.Equals(quartz.ServidorExecucao) &&
                // O site de execução está vazio ou então é o configurado
                (quartz.SiteExecucao.IsEmpty() ||
                 HostingEnvironment.SiteName.ToLower().Equals(quartz.SiteExecucao.ToLower())) &&
                // 
                (quartz.AplicacaoExecucao.IsEmpty() ||
                 applicationName.ToLower().Equals(quartz.AplicacaoExecucao.ToLower()));
        }

        private string MensagemServidorErrado(QuartzConfiguration job)
        {
            return
                $"Job configurado para executar em [{job.ServidorExecucao} | {job.SiteExecucao}]. Servidor atual [{Environment.MachineName} | {HostingEnvironment.SiteName}]. Job: {job.Nome}";
        }

        private async Task ScheduleJob(QuartzConfiguration job)
        {
            var scheduler = GetScheduler();
            var classe = Type.GetType(job.CaminhoCompleto);
            if (classe == null)
                throw new ArgumentNullException("job",
                    string.Format("Classe não encontrada: {0}", job.CaminhoCompleto));

            var jobDetail = new JobDetailImpl(job.Nome, classe);
            jobDetail.JobDataMap.Put("id", job.Id);
            jobDetail.JobDataMap.Put("config", job);
            var triggerKey = new TriggerKey(job.Nome + TriggerName, TriggerGroup);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .WithCronSchedule(job.Cron)
                .Build();
            await scheduler.Result.ScheduleJob(jobDetail, trigger);
        }


        private void FireJobNow(QuartzConfiguration job)
        {
            if (!Environment.MachineName.Equals(job.ServidorExecucao) ||
                !job.SiteExecucao.IsEmpty() && !HostingEnvironment.SiteName.Equals(job.SiteExecucao))
            {
                var bre = new BusinessRuleException(MensagemServidorErrado(job));
                bre.ThrowIfHasError();
            }

            var scheduler = GetScheduler().Result;

            var classe = Type.GetType(job.CaminhoCompleto);
            if (classe == null)
            {
                var exc = new BusinessRuleException();
                exc.AddError("Job {0} não encontrado. Esse job provavelmente pertence a outro projeto.")
                    .WithParam(job.CaminhoCompleto).Complete();
                exc.ThrowIfHasError();
            }

            var jobDetail = new JobDetailImpl(job.Nome + Immediate, classe);
            jobDetail.JobDataMap.Put("id", job.Id);
            var triggerKey = new TriggerKey(job.Nome + TriggerNameImmediate, TriggerGroupImmediate);
            if (scheduler.CheckExists(triggerKey).Result &&
                scheduler.GetCurrentlyExecutingJobs().Result.Any(x =>
                    x.Trigger.Key.Name == triggerKey.Name && x.Trigger.Key.Group == triggerKey.Group))
            {
                var exc = new BusinessRuleException();
                exc.AddError(GlobalMessages.MsgErroExecucaoJob).WithParam(job.Nome).Complete();
                exc.ThrowIfHasError();
            }

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.Start();
        }

        public void PararJob(QuartzConfiguration job)
        {
            var schedduler = GetScheduler();
            var triggerKey = new TriggerKey(job.Nome + TriggerName, TriggerGroup);
            schedduler.Result.UnscheduleJob(triggerKey);
        }

        public void ExecutarJobAgora(QuartzConfiguration jobConf)
        {
            FireJobNow(jobConf);
        }

        public void ReconfigurarJob(QuartzConfiguration jobConf)
        {
            try
            {
                if (jobConf != null)
                {
                    var sched = GetScheduler();
                    var classe = Type.GetType(jobConf.CaminhoCompleto);
                    var jobDetail = new JobDetailImpl(jobConf.Nome, classe);
                    jobDetail.JobDataMap.Put("id", jobConf.Id);
                    var triggerKey = new TriggerKey(jobConf.Nome + TriggerName, TriggerGroup);
                    if (jobConf.IniciarAutomaticamente)
                    {
                        if (sched.Result.CheckExists(triggerKey).Result)
                        {
                            var trigger = TriggerBuilder.Create()
                                .WithIdentity(triggerKey)
                                .WithCronSchedule(jobConf.Cron)
                                .Build();
                            sched.Result.RescheduleJob(triggerKey, trigger);
                        }
                        else
                        {
                            ScheduleJob(jobConf);
                        }
                    }
                    else
                    {
                        PararJob(jobConf);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw e;
            }
        }
    }
}