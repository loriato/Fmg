using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class FamiliarDto
    {
        public TipoFamiliaridade? Familiaridade { get; set; }
        public EntityDto Cliente1 { get; set; }
        public EntityDto Cliente2 { get; set; }

        public FamiliarDto()
        {
            Cliente1 = new EntityDto();
            Cliente2 = new EntityDto();
        }

        public void FromDomain(Familiar familiar)
        {
            Familiaridade = familiar.Familiaridade;
            
            Cliente1.Id = familiar.Cliente1.Id;
            Cliente1.Nome = familiar.Cliente1.NomeCompleto;

            Cliente2.Id = familiar.Cliente2.Id;
            Cliente2.Nome = familiar.Cliente2.NomeCompleto;

        }
    }
}
