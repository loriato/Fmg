using System.Collections.Generic;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;

namespace Europa.Fmg.Portal.Models.PreferenciasUsuarioViewModel
{
    public class PreferenciasUsuarioViewModel
    {
        public UsuarioPortal Usuario { get; set; }
        public List<SelectListItem> Fila { get; set; }

        public PreferenciasUsuarioViewModel()
        {
            Fila = new List<SelectListItem>();
        }
        public long PerfilId { get; set; }
        public string NomePerfil { get; set; }
    }
}