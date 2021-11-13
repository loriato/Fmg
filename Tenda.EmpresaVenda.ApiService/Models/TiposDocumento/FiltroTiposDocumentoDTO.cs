using Europa.Extensions;
using Tenda.Domain.Core.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.TiposDocumento
{
    public class FiltroTiposDocumentoDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Situacao Situacao { get; set; }
        public  bool Obrigatorio { get; set; }
        public  bool VisivelPortal { get; set; }
        public  bool VisivelLoja { get; set; }

        public DataSourceRequest Request { get; set; }
    }
}
