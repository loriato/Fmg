using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoDocumentoEnum
    {
        [Display(Name = "TipoDocumento_Rg", ResourceType = typeof(GlobalMessages))]
        Rg = 1,
        [Display(Name = "TipoDocumento_Cnh", ResourceType = typeof(GlobalMessages))]
        Cnh = 2,
        [Display(Name = "TipoDocumento_Passaporte", ResourceType = typeof(GlobalMessages))]
        Passaporte = 3,
        [Display(Name = "TipoDocumento_NumSegurancaSocial", ResourceType = typeof(GlobalMessages))]
        NumSegurancaSocial = 4,
        [Display(Name = "TipoDocumento_CarteiraCivil", ResourceType = typeof(GlobalMessages))]
        CarteiraCivil = 5,
        [Display(Name = "TipoDocumento_Rne", ResourceType = typeof(GlobalMessages))]
        Rne = 6,
        [Display(Name = "TipoDocumento_Crea", ResourceType = typeof(GlobalMessages))]
        Crea = 7,
        [Display(Name = "TipoDocumento_Cra", ResourceType = typeof(GlobalMessages))]
        Cra = 8,
        [Display(Name = "TipoDocumento_Crm", ResourceType = typeof(GlobalMessages))]
        Crm = 9,
        [Display(Name = "TipoDocumento_Crc", ResourceType = typeof(GlobalMessages))]
        Crc = 10,
        [Display(Name = "TipoDocumento_OAB", ResourceType = typeof(GlobalMessages))]
        OAB = 11
    }
}
