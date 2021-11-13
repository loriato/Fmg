using Europa.Commons;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoAvalistaService:BaseService
    {
        private EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }
        public void Salvar(EnderecoAvalista endereco)
        {
            var bre = new BusinessRuleException();

            var result = new EnderecoAvalistaValidator(_enderecoAvalistaRepository).Validate(endereco);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();

            _enderecoAvalistaRepository.Save(endereco);
        }
    }
}
