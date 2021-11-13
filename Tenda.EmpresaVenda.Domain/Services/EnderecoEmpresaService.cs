using Europa.Commons;
using Europa.Extensions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{

    public class EnderecoEmpresaService : BaseService
    {
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }

        public EnderecoEmpresa Salvar(EnderecoEmpresa enderecoEmpresa, BusinessRuleException bre)
        {
            // Remove máscaras
            enderecoEmpresa.Cep = enderecoEmpresa.Cep.OnlyNumber();
            _enderecoEmpresaRepository.Save(enderecoEmpresa);

            return enderecoEmpresa;
        }
    }
}
