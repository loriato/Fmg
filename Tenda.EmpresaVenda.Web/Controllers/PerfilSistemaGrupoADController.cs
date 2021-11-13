using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class PerfilSistemaGrupoADController : BaseController
    {
        private PerfilSistemaGrupoActiveDiretoryRepository _perfilSistemaGrupoActiveDiretoryRepository { get; set; }
        private PerfilSistemaGrupoActiveDiretoryService _perfilSistemaGrupoActiveDiretoryService { get; set; }
        private ViewPerfilSistemaGrupoActiveDirectoryRepository _viewPerfilSistemaGrupoActiveDirectoryRepository { get; set; }
        private SistemaService _sistemaService { get; set; }
        private PerfilRepository _perfilRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("SEG13", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request)
        {

            var sistema = _sistemaService.FindByCodigo(ApplicationInfo.CodigoSistema);

            var result = _viewPerfilSistemaGrupoActiveDirectoryRepository.Listar(request, sistema.Id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("SEG13", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(PerfilSistemaGrupoAdDTO perfilSistemaGrupoAdDTO)
        {
            return Salvar(perfilSistemaGrupoAdDTO);
        }

        [HttpPost]
        [BaseAuthorize("SEG13", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Alterar(PerfilSistemaGrupoAdDTO perfilSistemaGrupoAdDTO)
        {
            return Salvar(perfilSistemaGrupoAdDTO);
        }

        private JsonResult Salvar(PerfilSistemaGrupoAdDTO perfilSistemaGrupoAdDTO)
        {
            var novo = new PerfilSistemaGrupoActiveDirectory
            {
                Id = perfilSistemaGrupoAdDTO.Id,
                Perfil = _perfilRepository.FindById(perfilSistemaGrupoAdDTO.IdPerfil),
                Sistema = _sistemaService.FindByCodigo(ApplicationInfo.CodigoSistema),
                GrupoActiveDirectory = perfilSistemaGrupoAdDTO.GrupoAd
            };

            string operacao = novo.Id.HasValue() ? GlobalMessages.Alterado : GlobalMessages.Incluido;

            var resposta = new JsonResponse();

            try
            {
                var bre = new BusinessRuleException();
                _perfilSistemaGrupoActiveDiretoryService.Salvar(novo, bre);
                bre.ThrowIfHasError();
                resposta.Sucesso = true;
                resposta.Objeto = novo;
                resposta.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, "", operacao));
            }
            catch (BusinessRuleException bre)
            {
                resposta.Mensagens.AddRange(bre.Errors);
                resposta.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("SEG13", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Excluir(long id)
        {
            var resposta = new JsonResponse();

            var obj = _perfilSistemaGrupoActiveDiretoryRepository.FindById(id);

            try
            {
                _perfilSistemaGrupoActiveDiretoryRepository.Delete(obj);

                resposta.Mensagens.Add(GlobalMessages.RegistroRemovidoSucesso);
                resposta.Sucesso = true;
            }

            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroViolacaoConstraint, ""));
                }
                else
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }
                CurrentTransaction().Rollback();
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG13", "ExportarPagina")]
        public ActionResult ExportarPagina(DataSourceRequest request)
        {
            return Exportar(request);
        }

        [BaseAuthorize("SEG13", "ExportarTodos")]
        public ActionResult ExportarTodos(DataSourceRequest request)
        {
            request.start = 0;
            request.pageSize = 0;

            return Exportar(request);
        }

        private ActionResult Exportar(DataSourceRequest request)
        {
            var sistema = _sistemaService.FindByCodigo(ApplicationInfo.CodigoSistema);

            byte[] file = _perfilSistemaGrupoActiveDiretoryService.Exportar(request, sistema.Id);
            string nomeArquivo = GlobalMessages.AssociacoesPerfilSistemaGrupoAd;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

    }
}