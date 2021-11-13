using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class LojaService : BaseService
    {
        private LojaRepository _lojaRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public IQueryable<Loja> ListarDisponiveis(LojaDto dto)
        {
            var loja = _lojaRepository.Queryable()
                .Where(reg => !(_empresaVendaRepository.Queryable()
                        .Where(empr => empr.Loja.Id == reg.Id).Any()) &&
                        (reg.Nome.ToLower().Contains(dto.Nome.ToLower()) || reg.NomeFantasia.ToLower().Contains(dto.Nome.ToLower())));
            return loja;
        }
    }
}
