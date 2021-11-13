using Europa.Web.Menu;
using System;
using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Login
{
    public class LoginResponseDto : EntityDto
    {
        //public long Id { get; set; } //TODO Futuramente não iremos utilizar. Manter até integrar todo o adm por token.
        public string Login { get; set; }
        //public string Nome { get; set; }
        public string Email { get; set; }
        public string Autorizacao { get; set; }
        public bool PerfilInicial { get; set; }
        public long IdAcesso { get; set; }
        public DateTime InicioAcesso { get; set; }
        public List<EntityDto> Perfis { get; set; }
        public List<LoginPermissaoDto> Permissoes { get; set; }
        public MenuItem Menu { get; set; }
        public long IdLoja { get; set; }
        public string NomeLoja { get; set; }
        public string EstadoLoja { get; set; }
        public string TokenIntegracaoConecta { get; set; }
        public string TokenAcessoSimulador { get; set; }
        public List<HouseDto> Lojas { get; set; }

        public LoginResponseDto()
        {
            Lojas = new List<HouseDto>();
        }
    }
}
