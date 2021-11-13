using Europa.Commons;
using Europa.Data;
using Europa.Resources;
using NHibernate.Exceptions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class UsuarioGrupoCCAService:BaseService
    {
        private UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        public void Save(UsuarioGrupoCCA usuarioGrupoCCA)
        {
            var bre = new BusinessRuleException();

            var result = new UsuarioGrupoCCAValidator(_usuarioGrupoCCARepository).Validate(usuarioGrupoCCA);

            bre.WithFluentValidation(result);

            bre.ThrowIfHasError();

            _usuarioGrupoCCARepository.Save(usuarioGrupoCCA);
        }

        public void Delete(UsuarioGrupoCCA usuarioGrupoCCA)
        {
            var bre = new BusinessRuleException();

            try
            {
                _usuarioGrupoCCARepository.Delete(usuarioGrupoCCA);
                _session.Transaction.Commit();
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(usuarioGrupoCCA.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();
        }
    }
}
