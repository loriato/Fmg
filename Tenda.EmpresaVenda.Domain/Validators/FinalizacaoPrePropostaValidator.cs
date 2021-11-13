using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class FinalizacaoPrePropostaValidator : AbstractValidator<PreProposta>
    {
        public FinalizacaoPrePropostaValidator()
        {
            RuleFor(prop => prop).Must(prop => VerificarStatusSicaq(prop)).WithMessage(prop => string.Format(GlobalMessages.MsgTodosDocumentosAprovados, prop.Cliente.NomeCompleto));
        }

        public bool VerificarStatusSicaq(PreProposta preProposta)
        {
            // Incluir validação Status Sicaq
            if (preProposta.IsEmpty())
            {
                return false;
            }
            return true;
        }
    }
}