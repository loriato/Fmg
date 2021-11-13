using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AreaKanbanPrePropostaValidator:AbstractValidator<AreaKanbanPreProposta>
    {
        private AreaKanbanPrePropostaRepository _areaKanbanPrePropostaRepository { get; set; }
        public AreaKanbanPrePropostaValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().OverridePropertyName("Descricao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(x => x.Cor).NotEmpty().OverridePropertyName("Cor").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cor));
            
            RuleFor(x => x).Must(x => DescricaoUnica(x)).OverridePropertyName("Descricao").WithMessage(string.Format("Descrição já existente"));
        }

        public bool DescricaoUnica(AreaKanbanPreProposta areaKanbanPreProposta)
        {
            if (areaKanbanPreProposta.Descricao.IsEmpty())
            {
                return true;
            }

            var valido = _areaKanbanPrePropostaRepository.ValidarDescricaoUnica(areaKanbanPreProposta.Descricao,areaKanbanPreProposta.Id);

            return valido;
        }
    }
}
