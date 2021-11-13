using Europa.Resources;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Motivo;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class MotivoController : BaseController
    {
        public ActionResult RenderMotivosNegativosAnexo()
        {
            var filtro = new FiltroMotivoDto
            {
                TipoMotivo = TipoMotivo.NegativaAnexarDocumentoProponente,
                Situacao = Situacao.Ativo
            };

            var results = EmpresaVendaApi.ListarMotivos(filtro);
            results.Add(new EntityDto
            {
                Id = 0,
                Nome = GlobalMessages.NaoQueroAnexar
            });

            var list = results.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString(),
            });

            return PartialView("~/Views/PreProposta/_MotivoDropDownList.cshtml", list);
        }
    }
}