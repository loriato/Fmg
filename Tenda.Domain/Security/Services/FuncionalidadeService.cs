using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class FuncionalidadeService : BaseService
    {
        public FuncionalidadeRepository _funcionalidadeRepository { get; set; }

        public Funcionalidade AlterarStatusLog(long funcionalidadeId, bool log)
        {
            var func = _funcionalidadeRepository.FindById(funcionalidadeId);
            func.Logar = log;
            _funcionalidadeRepository.Save(func);
            _funcionalidadeRepository.Flush();
            return func;
        }

        public Funcionalidade Salvar(Funcionalidade model)
        {
            Validar(model);
            if (model.Id.HasValue())
            {
                var atualizar = _funcionalidadeRepository.FindById(model.Id);
                atualizar.Nome = model.Nome;
                atualizar.Logar = model.Logar;
                atualizar.UnidadeFuncional = model.UnidadeFuncional;
                atualizar.Comando = model.Comando;
                atualizar.Crud = model.Crud;
                _funcionalidadeRepository.Save(atualizar);
                _funcionalidadeRepository.Flush();
                return atualizar;
            }
            else
            {
                _funcionalidadeRepository.Save(model);
            }
            return model;
        }

        public void Remover(long id)
        {
            var bre = new BusinessRuleException();
            var model = _funcionalidadeRepository.FindById(id);
            if (model.IsNull())
            {
                bre.AddError(GlobalMessages.RegistroNaoExiste).Complete();
            }
            bre.ThrowIfHasError();
            try
            {
                _funcionalidadeRepository.Delete(model);
                _funcionalidadeRepository.Flush();
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso)
                        .WithParam(model.Nome)
                        .Complete();
                }
                else
                {
                    bre.AddError(GlobalMessages.ErroNaoTratado).WithParam(exp.Message);
                }
            }
            bre.ThrowIfHasError();
        }

        public void Validar(Funcionalidade model)
        {
            var bre = new BusinessRuleException();
            if (model.Nome.IsEmpty())
            {
                bre.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Nome).Complete();
            }
            if (model.UnidadeFuncional.IsNull())
            {
                bre.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.UnidadeFuncional).Complete();
            }
            if (model.Comando.IsEmpty())
            {
                bre.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Comando).Complete();
            }
            if (model.Logar.IsEmpty())
            {
                bre.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Logar).Complete();
            }
            bre.ThrowIfHasError();
            //verificar unicidade
            if (_funcionalidadeRepository.VerificarUnicidadeNome(model))
            {
                bre.AddError(GlobalMessages.ErrorUnicidadeFuncionalidade).WithParams(GlobalMessages.Nome, model.Nome, model.UnidadeFuncional.Nome).Complete();
            }
            if (_funcionalidadeRepository.VerificarUnicidadeComando(model))
            {
                bre.AddError(GlobalMessages.ErrorUnicidadeFuncionalidade).WithParams(GlobalMessages.Comando, model.Comando, model.UnidadeFuncional.Nome).Complete();
            }
            bre.ThrowIfHasError();
        }
    }
}
