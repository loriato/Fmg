using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class HierarquiaModuloService : BaseService
    {
        public HierarquiaModuloRepository _hierarquiaModuloRepository { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }

        public void Salvar(HierarquiaModulo model)
        {
            Validate(model);
            _hierarquiaModuloRepository._session.Evict(model);
            _hierarquiaModuloRepository.Save(model);
        }

        public void Excluir(long id)
        {
            var exc = new BusinessRuleException();
            var model = _hierarquiaModuloRepository.FindById(id);
            if (model.HasValue())
            {
                _hierarquiaModuloRepository.Delete(model);
                _hierarquiaModuloRepository.Flush();
            }
            else
            {
                exc.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.Modulo.ToLower(), id.ToString()).Complete();
            }
            exc.ThrowIfHasError();
        }

        public void Validate(HierarquiaModulo model)
        {
            var ex = new BusinessRuleException();

            if (model.Nome.IsEmpty())
            {
                ex.AddError(GlobalMessages.CampoObrigatorioVazio).WithParam(GlobalMessages.Nome.ToLower()).Complete();
            }
            if (model.Sistema.IsEmpty())
            {
                ex.AddError(GlobalMessages.CampoObrigatorioVazio).WithParam(GlobalMessages.Sistema.ToLower()).Complete();
            }
            else
            {
                var sistema = _sistemaRepository.FindById(model.Sistema.Id);
                if (sistema.IsNull())
                {
                    ex.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.Sistema.ToLower(), model.Sistema.Id.ToString()).Complete();
                }
            }
            if (model.Pai.HasValue())
            {
                var moduloPai = _hierarquiaModuloRepository.FindById(model.Pai.Id);
                if (moduloPai.IsNull())
                {
                    ex.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.ModuloPai.ToLower(), model.Pai.Id.ToString()).Complete();
                }
                else if (model.Id == moduloPai.Id)
                {
                    ex.AddError(GlobalMessages.ErroReferenciaCiclica).WithParam(model.Nome).Complete();
                }
            }
            else
            {
                model.Pai = null;
            }

            ex.ThrowIfHasError();
        }
    }
}
