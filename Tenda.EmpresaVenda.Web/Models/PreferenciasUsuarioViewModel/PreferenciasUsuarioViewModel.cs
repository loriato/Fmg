using System.Collections.Generic;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Web.Models.PreferenciasUsuarioViewModel
{
    public class PreferenciasUsuarioViewModel
    {
        public UsuarioPortal Usuario { get; set; }
        public List<SelectListItem> Fila { get; set; }
        public Sistema Sistema { get; set; }
        public List<Sistema> Sistemas { get; set; }

        public PreferenciasUsuarioViewModel()
        {
            Fila = new List<SelectListItem>();
            Sistemas = new List<Sistema>();
        }
        public long PerfilId { get; set; }
        public string NomePerfil { get; set; }
    }
}