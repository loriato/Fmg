using System.Collections.Generic;
using System.Web.Mvc;
using Europa.Resources;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class DropDownViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectListItem> List { get; set; }
        public string OptionLabel { get; set; }
        public long? SelectedId { get; set; }
        public bool IsMutiple { get; set; }

        public string Param { get; set; }

        public DropDownViewModel()
        {
            List = new List<SelectListItem>();
            OptionLabel = GlobalMessages.Selecione;
        }
    }
}