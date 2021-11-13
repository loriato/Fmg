using Europa.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoCorretor
    {
        [Display(Name = "TipoCorretor_Corretor", ResourceType = typeof(GlobalMessages))]
        Corretor = 1,
        [Display(Name = "TipoCorretor_AgenteVenda", ResourceType = typeof(GlobalMessages))]
        AgenteVenda = 2
    }
}
