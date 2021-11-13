using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class RecuperarContaController : Controller
    {
        public UsuarioPortalRepository _usuarioPortalRepository{get;set ;}
        public CorretorRepository _corretorRepository { get; set; }
        public CorretorService _corretorService { get; set; }
        
        // GET: RecuperarConta
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MensagemEnvio()
        {
            return View("MensagemEnvio");
        }
        
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ReenviarTokenAtivacao(string Username)
        {
            var jsonResponse = new JsonResponse();
            var bre = new BusinessRuleException();

            using (var session = _usuarioPortalRepository._session)
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        if (Username.IsEmpty()) bre.AddError(String.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Email)).Complete();
                        bre.ThrowIfHasError();

                        if (!Username.IsValidEmail()) bre.AddError(GlobalMessages.EmailInvalido).Complete();
                        bre.ThrowIfHasError();

                        var corretor = _corretorRepository.FindByEmail(Username);
                        if (corretor.HasValue()) _corretorService.CriarTokenAtivacaoEEnviarEmailSenha(corretor);

                        jsonResponse.Sucesso = true;
                        transaction.Commit();
                    }
                    catch (BusinessRuleException ex)
                    {
                        jsonResponse.Mensagens.AddRange(ex.Errors);
                        transaction.Rollback();
                        
                    }
                }
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}