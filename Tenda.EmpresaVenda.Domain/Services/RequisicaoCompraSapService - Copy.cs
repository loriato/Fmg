using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RequisicaoCompraSapService : BaseService
    {
        public RequisicaoCompraSapRepository _requisicaoCompraSapRepository { get; set; }
        public void Salvar(RequisicaoCompraSap model)
        {
            _requisicaoCompraSapRepository.Save(model);
        }
    }
}
