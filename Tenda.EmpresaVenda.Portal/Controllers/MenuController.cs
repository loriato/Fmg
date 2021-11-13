using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class MenuController : BaseController
    {
        private StaticResourceService _staticResourceService { get; set; }
        private EmpresaVendaService _empresaVendaService { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private CorretorService _corretorService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }


        [HttpPost]
        //[BaseAuthorize("EVS01", "UploadFoto")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult UploadFoto(FotoEmpresaVendaDTO dto)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var idArquivo = _empresaVendaService.UploadFoto(dto.Foto, dto.IdEmpresaVenda);
                // Para garantir que a imagem seja carregada de forma estática
                CurrentTransaction().Commit();
                var fileName = _staticResourceService.LoadResource(idArquivo);
                var urlPath = _staticResourceService.CreateUrl(GetWebAppRoot(), fileName);
                SessionAttributes.Current().FotoFachada = urlPath;
                jsonResponse.Sucesso = true;
                jsonResponse.Objeto = urlPath;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarSenha(long idCorretor)
        {
            var jsonResponse = new JsonResponse();
            var corretor = _corretorRepository.FindByIdUsuario(idCorretor);
            var linkAtivacao = _corretorService.CriarTokenAtivacao(corretor);
            return Redirect(linkAtivacao);
        }

        [HttpPost]
        public JsonResult VerificarNovasNotificacoes()
        {
            SessionAttributes current = SessionAttributes.Current();
            current.NotificacoesNaoLidas = _notificacaoRepository.NaoLidasDoUsuario(current.UsuarioPortal.Id, current.UsuarioPortal.UltimaLeituraNotificacao, DateTime.Now).Count();
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}