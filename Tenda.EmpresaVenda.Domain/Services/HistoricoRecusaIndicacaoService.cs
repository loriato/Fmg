using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class HistoricoRecusaIndicacaoService : BaseService
    {
        private HistoricoRecusaIndicacaoRepository _historicoRecusaIndicacaoRepository { get; set; }
        public HistoricoRecusaIndicacao Salvar(HistoricoRecusaIndicacao model)
        {
            _historicoRecusaIndicacaoRepository.Save(model);
            return model;
        }
    }
}
