using Europa.Commons;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ParametroRequisicaoCompraService : BaseService
    {
        private ParametroRequisicaoCompraRepository _parametroRequisicaoCompraRepository { get; set; }
        public void Salvar(ParametroRequisicaoCompra model, BusinessRuleException bre)
        {
            var validation = new ParametroRequisicaoCompraValidator().Validate(model);
            bre.WithFluentValidation(validation);

            if (validation.IsValid)
            {
                _parametroRequisicaoCompraRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }
    }
}
