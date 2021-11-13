using Europa.Commons;
using Europa.Extensions;
using FluentValidation.Results;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoClienteService : BaseService
    {
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }

        public EnderecoCliente Salvar(EnderecoCliente enderecoCliente, BusinessRuleException bre)
        {
            // Remove máscaras
            enderecoCliente.Cep = enderecoCliente.Cep.OnlyNumber();

            _enderecoClienteRepository.Save(enderecoCliente);

            return enderecoCliente;
        }
        public bool ValidarEndereco(EnderecoCliente enderecoCliente, BusinessRuleException bre)
        {
            // Realiza as validações de Corretor
            ValidationResult enclResult = new EnderecoClienteValidator(_enderecoClienteRepository).Validate(enderecoCliente);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(enclResult);

            if (enclResult.IsValid)
            {
                return true;
            }
            return false;
        }
    }
}
