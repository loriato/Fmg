using Europa.Commons;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS14")]
    public class RegraComissaoEvsController : BaseController
    {
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }

        [BaseAuthorize("EVS14", "DownloadRegraComissao")]
        public FileContentResult DownloadPdfRegraComissaoEvs(long idRegra, long idEmpresaVenda)
        {
            var regra = _regraComissaoEvsRepository.Buscar(idEmpresaVenda, idRegra);
            return File(regra.Arquivo.Content, regra.Arquivo.ContentType, regra.Arquivo.Nome);
        }

        [BaseAuthorize("EVS14", "DownloadRegraComissao")]
        public FileContentResult DownloadPdfRegraComissaoEspecifica(long idRegra)
        {
            var regra = _regraComissaoEvsRepository.FindById(idRegra);
            return File(regra.Arquivo.Content, regra.Arquivo.ContentType, regra.Arquivo.Nome);
        }
    }
}