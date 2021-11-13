using FluentValidation;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AgenteVendaValidator : AbstractValidator<AgenteVendaDto>
    {
        private AgenteVendaHouseRepository _agenteVendaHouseRepository { get; set; }
        public AgenteVendaValidator()
        {
            RuleFor(x => x).Must(x => !_agenteVendaHouseRepository.AgenteVendaValido(x.IdUsuario,x.IdLoja))
                .WithMessage(x =>
                    string.Format("O Agente de Venda {0} já pertence à outra loja.", x.NomeUsuario));
        }
    }
}
