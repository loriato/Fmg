using Tenda.Domain.Core.Models;
using Tenda.EmpresaVenda.ApiService.Models.GrupoCCA;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.UsuarioGrupoCCA
{
    public class UsuarioGrupoCCADto
    {
        public long Id { get; set; }
        public EntityDto Usuario { get; set; }
        public GrupoCCADto GrupoCCA { get; set; }

        public UsuarioGrupoCCADto FromDomain(Tenda.Domain.EmpresaVenda.Models.UsuarioGrupoCCA model)
        {
            Id = model.Id;
            Usuario = new EntityDto(model.Usuario.Id, model.Usuario.Nome);
            GrupoCCA = new GrupoCCADto().FromDomain(model.GrupoCCA);

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.UsuarioGrupoCCA ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.UsuarioGrupoCCA();
            model.Id = Id;
            model.Usuario = new UsuarioPortal { Id = model.Usuario.Id, Nome = model.Usuario.Nome };
            model.GrupoCCA = GrupoCCA.ToDomain();

            return model;
        }
    }
}
