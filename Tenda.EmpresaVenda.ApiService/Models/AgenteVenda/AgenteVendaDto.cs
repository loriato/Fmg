using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.AgenteVenda
{
    public class AgenteVendaDto
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public long IdLoja { get; set; }
        public bool Ativo { get; set; }
        public string Estado { get; set; }
        public string Email { get; set; }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Views.ViewLojaPortalUsuario model)
        {
            Id = model.Id;
            IdUsuario = model.IdUsuario;
            NomeUsuario = model.NomeUsuario;
            IdLoja = model.IdLojaPortal;
            Ativo = model.Ativo;
            Estado = model.Estado;
            Email = model.Email;
        }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            IdLoja = cliente.EmpresaVenda.Id;
        }

        public Corretor ToDomain()
        {
            var model = new Corretor();
            model.Usuario = new UsuarioPortal() { Id = IdUsuario };
            model.Nome = NomeUsuario;
            model.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda() { Id = IdLoja };
            model.TipoCorretor = TipoCorretor.AgenteVenda;
            model.Estado = Estado;
            model.Email = Email;

            return model;
        }
    }
}
