using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Europa.Extensions;

//TODO verificar a necessidade de descer para outra "camada"
namespace Tenda.EmpresaVenda.Portal.Models
{
    public class TreeViewModel
    {
        public long id { get; set; }
        public int nodeLevel { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string selectedIcon { get; set; }
        public string color { get; set; }
        public string backColor { get; set; }
        public string href { get; set; }
        public bool selectable { get; set; }
        public TreeViewState state { get; set; }
        public List<TreeViewModel> nodes { get; set; }

        public TreeViewModel()
        {
            selectable = false;
            nodeLevel = 0;
            state = new TreeViewState
            {
                @checked = false,
                disabled = false,
                expanded = false,
                selected = false
            };
        }
    }
}