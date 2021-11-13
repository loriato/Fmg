using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC11")]
    public class TermoAceiteProgramaFidelidadeController : BaseController
    {
        private TermoAceiteProgramaFidelidadeRepository _termoAceiteProgramaFidelidadeRepository { get; set; }
        private TermoAceiteProgramaFidelidadeService _termoAceiteProgramaFidelidadeService { get; set; }
        private ViewTermoAceiteProgramaFidelidadeRepository _viewTermoAceiteProgramaFidelidadeRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }

        [BaseAuthorize("GEC11", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar(DataSourceRequest request)
        {
            var results = _viewTermoAceiteProgramaFidelidadeRepository.Listar();
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RenderTermoAceiteProgramaFidelidade()
        {
            var dto = _termoAceiteProgramaFidelidadeRepository.BuscarUltimoTermoAceite();
            var viewModel = RenderRazorViewToString("_TermoAceiteProgramaFidelidade", dto, false);
            var result = new JsonResponse();
            result.Sucesso = true;
            result.Objeto = viewModel;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC11", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarTermoAceite(HttpPostedFileBase file)
        {
            var result = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                if (file == null)
                {
                    bre.AddError(GlobalMessages.SelecioneArquivoParaEnviar).Complete();
                }
                else if (file.ContentType != "application/pdf")
                {
                    bre.AddError(string.Format(GlobalMessages.MsgUploadFormatoInvalido, GlobalMessages.PDF)).Complete();
                }

                bre.ThrowIfHasError();

                var termo = _termoAceiteProgramaFidelidadeService.SalvarTermoAceite(file);

                EnviarNotificação();

                result.Sucesso = true;
                var viewModel = RenderRazorViewToString("_TermoAceiteProgramaFidelidade", termo, false);
                result.Objeto = viewModel;
                result.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    termo.NomeDoubleCheck, GlobalMessages.Incluido));
            }
            catch (BusinessRuleException exc)
            {
                result.Sucesso = false;
                result.Mensagens.AddRange(exc.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadTermoAceite(long idArquivo)
        {
            var arquivo = _arquivoRepository.FindById(idArquivo);
            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        private void EnviarNotificação()
        {
            var diretores = _corretorRepository.ListarTodosDiretoresAtivos();

            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = GlobalMessages.NovoTermoAceite_Titulo,
                    Conteudo = GlobalMessages.NovoTermoAceite_Conteudo,
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    Link = ProjectProperties.EvsBaseUrl + "/programafidelidade",
                    NomeBotao = GlobalMessages.Visualizar,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);
            }

        }
    }
}