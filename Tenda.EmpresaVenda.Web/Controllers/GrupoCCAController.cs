using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD03")]
    public class GrupoCCAController : BaseController
    {
        private GrupoCCARepository _grupoCCARepository { get; set; }
        private GrupoCCAService _grupoCCAService { get; set; }
        private ViewGrupoCCAEmpresaVendaRepository _viewGrupoCCAEmpresaVendaRepository { get; set; }
        private GrupoCCAEmpresaVendaService _grupoCCAEmpresaVendaService { get; set; }
        private ViewGrupoCCAUsuarioRepository _viewGrupoCCAUsuarioRepository { get; set; }
        private UsuarioGrupoCCAService _usuarioGrupoCCAService { get; set; }

        [BaseAuthorize("CAD03", "VisualizarGrupoCCA")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD03", "VisualizarGrupoCCA")]
        public ActionResult ListarDatatableGrupoCCA(DataSourceRequest request, GrupoCCADTO filtro)
        {
            var result = _grupoCCARepository.ListarDatatable(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "IncluirGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirGrupoCCA(GrupoCCA grupo)
        {
            return SalvarGrupoCCA(grupo);
        }

        [BaseAuthorize("CAD03", "AtualizarGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AtualizarGrupoCCA(GrupoCCA grupo)
        {
            return SalvarGrupoCCA(grupo);
        }

        [BaseAuthorize("CAD03", "IncluirGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        private ActionResult SalvarGrupoCCA(GrupoCCA grupo)
        {
            var json = new JsonResponse();

            try
            {

                _grupoCCAService.SalvarGrupoCCA(grupo);

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, grupo.Descricao));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "RemoverGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ExcluirGrupoCCA(long id)
        {
            var json = new JsonResponse();

            try
            {
                var obj = _grupoCCARepository.FindById(id);

                _grupoCCAService.ExluirGrupoCCAPorId(obj);

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, obj.ChaveCandidata(), GlobalMessages.Removido));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "VisualizarGrupoCCA")]
        public ActionResult ListarDatatableGrupoEmpresaVenda(DataSourceRequest request, GrupoCCAEmpresaDTO filtro)
        {
            var result = _viewGrupoCCAEmpresaVendaRepository.ListarDatatable(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "IncluirGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AtribuirEmpresaVendaAGrupo(GrupoCCAEmpresaVenda grupoCCAEmpresa)
        {
            var json = new JsonResponse();

            try
            {
                if (grupoCCAEmpresa.Id == 0)
                {
                    _grupoCCAEmpresaVendaService.Save(grupoCCAEmpresa);
                }
                else
                {
                    _grupoCCAEmpresaVendaService.Delete(grupoCCAEmpresa);
                }

                json.Sucesso = true;

            }catch(BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(bre.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "VisualizarGrupoCCA")]
        public ActionResult ListarDatatableUsuario(DataSourceRequest request, GrupoCCADTO filtro)
        {
            var result = _viewGrupoCCAUsuarioRepository.ListarDatatable(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD03", "IncluirGrupoCCA")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AtribuirUsuarioAGrupo(UsuarioGrupoCCA usuarioGrupoCCA)
        {
            var json = new JsonResponse();

            try
            {
                if (usuarioGrupoCCA.Id == 0)
                {
                    _usuarioGrupoCCAService.Save(usuarioGrupoCCA);
                }
                else
                {
                    _usuarioGrupoCCAService.Delete(usuarioGrupoCCA);
                }

                json.Sucesso = true;

            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(bre.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}