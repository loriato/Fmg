using Europa.Commons;
using Europa.Extensions;
using FluentValidation.Results;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoCorretorService : BaseService
    {
        private EnderecoCorretorRepository _enderecoCorretorRepository { get; set; }

        public EnderecoCorretor Salvar(EnderecoCorretor enderecoCorretor, BusinessRuleException bre)
        {
            // Remove máscaras
            enderecoCorretor.Cep = enderecoCorretor.Cep.OnlyNumber();

            // Realiza as validações de Corretor
            ValidationResult encoResult = new EnderecoCorretorValidator(_enderecoCorretorRepository).Validate(enderecoCorretor);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(encoResult);

            if (encoResult.IsValid)
            {
                _enderecoCorretorRepository.Save(enderecoCorretor);
            }

            return enderecoCorretor;
        }
    }
}
