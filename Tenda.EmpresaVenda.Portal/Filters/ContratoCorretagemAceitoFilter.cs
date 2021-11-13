using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Filters
{
    public class ContratoCorretagemAceitoFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // O Contrato está aceito. Vida que segue
            if (SessionAttributes.Current().ContratoAssinado) { return; }

            string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
            const string destinationController = "contratocorretagem";
            // O controller destino é o do fluxo de aceite
            if (destinationController == controllerName) { return; }

            const string alteracaoSenhaController = "alteracaosenha";
            if (alteracaoSenhaController == controllerName) { return; }

            const string alteracaoSenhaEmailController = "alteracaosenhaemail";
            if (alteracaoSenhaEmailController == controllerName) { return; }

            const string preCadastro = "precadastro";
            if (preCadastro == controllerName) { return; }

            // O usuário logado não é diretor
            if (TipoFuncao.Diretor != SessionAttributes.Current().Corretor.Funcao)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", destinationController },
                        { "Action", "ContratoNaoAceite" }
                });
            }

            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", destinationController },
                        { "Action", "Index" }
                });

        }
    }
}