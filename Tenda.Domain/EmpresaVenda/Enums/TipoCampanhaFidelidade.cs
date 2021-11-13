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
    public enum TipoCampanhaFidelidade
    {
        [Display(Name = "TipoCampanhaFidelidade_PorVenda", ResourceType = typeof(GlobalMessages))]
        PorVenda = 1,
        [Display(Name = "TipoCampanhaFidelidade_PorVendaMinima", ResourceType = typeof(GlobalMessages))]
        PorVendaMinima = 2,
        [Display(Name = "TipoCampanhaFidelidade_PorVendaMinimaEmpreendimento", ResourceType = typeof(GlobalMessages))]
        PorVendaMinimaEmpreendimento = 3
    }
}
