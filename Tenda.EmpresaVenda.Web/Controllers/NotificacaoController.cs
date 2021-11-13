using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class NotificacaoController : BaseController
    {
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private NotificacaoService _notificacaoService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }

        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _notificacaoRepository
                .NotificacoesDoUsuarioAdm(SessionAttributes.Current().UsuarioPortal.Id)
                .Select(reg => new Notificacao()
                {
                    Titulo = reg.Titulo,
                    Conteudo = reg.Conteudo,
                    Link = reg.Link,
                    DataLeitura = reg.DataLeitura,
                    NomeBotao = reg.NomeBotao,
                    TipoNotificacao = reg.TipoNotificacao
                }).ToList();
            DefinirUltimaDataLeitura();
            return PartialView("~/Views/Shared/_ModalNotificacao/_ListaNotificacoes.cshtml", results);
        }

        public JsonResult DefinirUltimaDataLeitura()
        {
            var usuario = _usuarioPortalRepository.FindById(SessionAttributes.Current().UsuarioPortal.Id);
            // Marca todas as notificações de UltimaLeitura até a data de agora como lidas
            _notificacaoService.MarcarComoLidas(usuario.Id, usuario.UltimaLeituraNotificacao, DateTime.Now);
            usuario.UltimaLeituraNotificacao = DateTime.Now;
            _usuarioPortalRepository.Save(usuario);
            _usuarioPortalRepository.Flush();
            SessionAttributes current = SessionAttributes.Current();
            current.NotificacoesNaoLidas = 0;
            JsonResponse response = new JsonResponse();
            response.Sucesso = true;
            return Json(response);
        }
    }
}