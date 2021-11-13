using Europa.Commons;
using Europa.Data;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System.Web.Mvc;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EstadoAvalistaService : BaseService
    {
        private EstadoAvalistaRepository _estadoAvalistaRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        public void Save(EstadoAvalista estadoAvalista)
        {
            var bre = new BusinessRuleException();

            var result = new EstadoAvalistaValidator(_estadoAvalistaRepository).Validate(estadoAvalista);

            bre.WithFluentValidation(result);

            bre.ThrowIfHasError();

            _estadoAvalistaRepository.Save(estadoAvalista);
        }

        public void Delete(EstadoAvalista estadoAvalista)
        {
            var bre = new BusinessRuleException();

            try
            {
                _estadoAvalistaRepository.Delete(estadoAvalista);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(estadoAvalista.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();
        }
    }
}
