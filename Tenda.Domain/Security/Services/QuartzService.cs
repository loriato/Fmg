using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class QuartzService : BaseService
    {
        public QuartzConfigurationRepository _quartzRepository { get; set; }

        public void SalvarSemValidar(QuartzConfiguration model)
        {
            _quartzRepository.Save(model);
        }

        public void Salvar(QuartzConfiguration model)
        {
            var exc = new BusinessRuleException();
            Validar(model, exc);
            exc.ThrowIfHasError();
            _quartzRepository.Save(model);
        }

        public void Validar(QuartzConfiguration quartz, BusinessRuleException exception)
        {
            if (quartz.IsNull())
            {
                quartz = new QuartzConfiguration();
            }
            if (quartz.Nome.IsEmpty())
            {
                exception.AddError(GlobalMessages.CampoObrigatorioVazio).WithParams(GlobalMessages.Nome).Complete();
            }
            else
            {
                if (_quartzRepository.Queryable().Any(x => x.Nome.ToLower().Equals(quartz.Nome.ToLower()) && x.Id != quartz.Id))
                {
                    exception.AddError(GlobalMessages.RegistroNaoUnico).WithParams(GlobalMessages.Nome).WithParams(quartz.Nome).Complete();
                }
            }
            if (quartz.CaminhoCompleto.IsEmpty())
            {
                exception.AddError(GlobalMessages.CampoObrigatorioVazio).WithParams(GlobalMessages.CaminhoCompleto).Complete();
            }
            if (quartz.ServidorExecucao.IsEmpty())
            {
                exception.AddError(GlobalMessages.CampoObrigatorioVazio).WithParams(GlobalMessages.ServidorExecucao).Complete();
            }
            if (quartz.Cron.IsEmpty())
            {
                exception.AddError(GlobalMessages.CampoObrigatorioVazio).WithParams(GlobalMessages.HorarioExecucao).Complete();
            }
            if (quartz.SiteExecucao != null && quartz.SiteExecucao.Length > 128)
            {
                exception.AddError(GlobalMessages.TamanhoCampoExcedido).WithParams(GlobalMessages.SiteExecucao, "128").Complete();
            }
            if (quartz.Observacoes != null && quartz.Observacoes.Length > 256)
            {
                exception.AddError(GlobalMessages.TamanhoCampoExcedido).WithParams(GlobalMessages.Observacoes, "256").Complete();
            }
        }

        public IQueryable<QuartzConfiguration> ListarQuartz()
        {
            var result = _quartzRepository.Queryable();
            return result;
        }

        public QuartzConfiguration BuscarQuartz(long idQuartz)
        {
            var result = new QuartzConfiguration();
            if (idQuartz > 0)
            {
                result = _quartzRepository.FindById(idQuartz);
            }
            return result;
        }

        public QuartzConfiguration BuscarQuartz(string nome)
        {
            var result = new QuartzConfiguration();
            if (!nome.IsEmpty())
            {
                result = _quartzRepository.FindByName(nome);
            }
            return result;
        }

    }
}
