using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
//TODO verificar a necessidade de descer para outra "camada"

namespace Tenda.EmpresaVenda.Web.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TreeViewState
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}