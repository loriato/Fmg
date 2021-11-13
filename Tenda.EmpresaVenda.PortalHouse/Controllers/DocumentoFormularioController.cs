using System.Web.Mvc;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class DocumentoFormularioController : BaseController
    {
        [HttpGet]
        [BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        public FileContentResult BaixarFormularios(long idPreProposta)
        {
            var result = EmpresaVendaApi.BaixarFormularios(idPreProposta);

            return File(result.Bytes, result.ContentType, $"{result.FileName}.{result.Extension}");
        }
    }
}