using Europa.Extensions;
using Europa.Web;
using System.Linq;
using System.Web.Mvc;
using Tenda.EmpresaVenda.PortalHouse.Security;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Europa.Rest;
using Newtonsoft.Json.Linq;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    [BaseAuthorize("EVS05")]
    public class ClienteController : BaseController
    {

        [BaseAuthorize("EVS05", "Visualizar")]
        public ActionResult Index(long? id)
        {
            var model = id > 0 ? EmpresaVendaApi.BuscarCliente(id.Value) : new ClienteDto();
            //if (!id.IsEmpty() && (model.Cliente.IsEmpty() || model.Cliente.EmpresaVenda?.Id != SessionAttributes.Current().UsuarioPortal.Loja.Id))
            if (!id.IsEmpty() && model.IdCliente.IsEmpty())
            { 
                RouteData.Values.Remove("id");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [BaseAuthorize("EVS05", "Incluir")]
        public JsonResult Incluir(ClienteDto model)
        {
            return Salvar(model);
        }

        [HttpPost]
        [BaseAuthorize("EVS05", "Atualizar")]
        public JsonResult Atualizar(ClienteDto model)
        {
            return Salvar(model);
        }

        private JsonResult Salvar(ClienteDto model)
        {
            var result = new JsonResponse();
            try
            {
                var response = model.IdCliente > 0
                    ? EmpresaVendaApi.EditarCliente(model)
                    : EmpresaVendaApi.IncluirCliente(model);
                result.FromBaseResponse(response);
                if (response.Success && response.Data != null)
                {
                    var dto = ((JObject)response.Data).ToObject<ClienteDto>();
                    result.Objeto = RenderRazorViewToString("_Conteudo", dto);
                    TempData["SucessMessage"] = response.Messages.FirstOrDefault();
                }
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS05", "ValidarDados")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ValidarDadosIntegracao(ClienteDto dto)
        {
            if (dto.AgenteVendaDto.IdLoja.IsEmpty())
            {
                dto.AgenteVendaDto.IdLoja = SessionAttributes.Current().UsuarioPortal.IdLoja;
            }

            var response = EmpresaVendaApi.ValidarDadosIntegracao(dto);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarClientes(DataSourceRequest request, string nome, string email, string telefone, string cpfCnpj)
        {
            var filtro = new FiltroClienteDto
            {
                DataSourceRequest = request,
                Nome = nome,
                Email = email,
                Telefone = telefone,
                CpfCnpj = cpfCnpj
            };

            var results = EmpresaVendaApi.ListarClientes(filtro);

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
