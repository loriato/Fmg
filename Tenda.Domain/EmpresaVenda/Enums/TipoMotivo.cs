using Europa.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoMotivo
    {
        [Display(Name = "TipoMotivo_NegativaAnexarDocumentoProponente", ResourceType = typeof(GlobalMessages))]
        NegativaAnexarDocumentoProponente = 1,
    }
}
