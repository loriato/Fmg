using Europa.Commons;
using Europa.Extensions;
using FluentValidation.Results;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoEmpreendimentoService : BaseService
    {
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }

        public EnderecoEmpreendimento Salvar(EnderecoEmpreendimento enderecoEmpreendimento, BusinessRuleException bre)
        {
            // Remove máscaras
            enderecoEmpreendimento.Cep = enderecoEmpreendimento.Cep.OnlyNumber();

            // Realiza as validações de Empreendimento
            ValidationResult encoResult = new EnderecoEmpreendimentoValidator(_enderecoEmpreendimentoRepository).Validate(enderecoEmpreendimento);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(encoResult);

            if (encoResult.IsValid)
            {
                _enderecoEmpreendimentoRepository.Save(enderecoEmpreendimento);
            }

            return enderecoEmpreendimento;
        }
    }
}
