using Europa.Commons;
using Europa.Extensions;
using FluentValidation.Results;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoBreveLancamentoService : BaseService
    {
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }

        public EnderecoBreveLancamento Salvar(EnderecoBreveLancamento enderecoBreveLancamento, BusinessRuleException bre)
        {
            // Remove máscaras
            enderecoBreveLancamento.Cep = enderecoBreveLancamento.Cep.OnlyNumber();

            // Realiza as validações de BreveLancamento
            ValidationResult encoResult = new EnderecoBreveLancamentoValidator(_enderecoBreveLancamentoRepository).Validate(enderecoBreveLancamento);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(encoResult);

            if (encoResult.IsValid)
            {
                _enderecoBreveLancamentoRepository.Save(enderecoBreveLancamento);
            }

            return enderecoBreveLancamento;
        }
    }
}
