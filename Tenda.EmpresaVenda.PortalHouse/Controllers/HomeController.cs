using Europa.Commons;
using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Web.Mvc;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (SessionAttributes.Current().UsuarioPortal.IdLoja == 0)
            {
                return RedirectToAction("Index", "HierarquiaHouse");
            }

            GenericFileLogUtil.DevLogWithDateOnBegin("Acessando a Home");
            var chaveConecta = SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta;

            if (chaveConecta.IsEmpty())
            {
                //aqui garante que o usuário foi autenticado pelo sistema
                try
                {
                    var login = SessionAttributes.Current().UsuarioPortal.Login;
                    var gerarTokenAcessoResponseDto = EmpresaVendaApi.GerarTokenAcessoConecta(login);

                    if (gerarTokenAcessoResponseDto.HasValue())
                    {
                        SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta = gerarTokenAcessoResponseDto.Token;
                    }
                }catch(Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    SessionAttributes.Current().UsuarioPortal.TokenIntegracaoConecta = null;
                }
            }

            return View();
        }
    }
}