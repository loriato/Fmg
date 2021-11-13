using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models
{
    public class MidasApiResponseDto<T>
    {
        public List<T> ErrorList { get; set; }
        public List<T> SuccessList { get; set; }
    }
}
