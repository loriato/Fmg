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
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS08")]
    public class PontoVendaController : BaseController
    {
        private PontoVendaRepository _pontoVendaRepo { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private StandVendaEmpresaVendaRepository _standVendaEmpresaVendaRepository { get; set; }
        private ViewPontoVendaRepository _viewPontoVendaRepository { get; set; }
        private ViewPontoEmpresaVendaRepository _viewPontoEmpresaVendaRepository { get; set; }
        [BaseAuthorize("EVS08", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS08", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, PontoVendaDto filtro)
        {
            var response = _viewPontoVendaRepository.Listar(request, filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarAutoComplete(DataSourceRequest request)
        {
            PontoVendaDto dto = new PontoVendaDto();

            // Autocomplete 
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    dto.Nome = queryTerm;
                }
                var filtroEmpresaVenda = request.filter.FirstOrDefault(x => x.column.Equals("idEmpresaVenda"));
                if (filtroEmpresaVenda.HasValue() && !filtroEmpresaVenda.value.IsNull())
                {
                    dto.idEmpresaVenda = Convert.ToInt32(filtroEmpresaVenda.value);
                }

                var filtroStand = request.filter.FirstOrDefault(x => x.column.Equals("standVenda"));

                if(filtroStand.HasValue() && filtroStand.value.HasValue())
                {
                    dto.IdPontosVenda = _standVendaEmpresaVendaRepository.Queryable()
                        .Where(x => x.StandVenda.Id == long.Parse(filtroStand.value))
                        .Select(x=>x.PontoVenda.Id)
                        .ToList();
                }
            }

            var result = _viewPontoEmpresaVendaRepository.Listar(dto).Select(reg => new PontoVendaDto
            {
                Id = reg.IdPontoVenda,
                Nome = reg.NomePontoEmpresaVenda,
            });
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS08", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(PontoVenda model)
        {
            return Salvar(model, false);
        }

        [BaseAuthorize("EVS08", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(PontoVenda model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        private ActionResult Salvar(PontoVenda model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _pontoVendaService.Salvar(model, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, model.Nome, isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [BaseAuthorize("EVS08", "Remover")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _pontoVendaRepo.FindById(id);
            try
            {
                _pontoVendaService.ExcluirPorId(obj);
                CurrentTransaction().Commit();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, obj.Nome, GlobalMessages.Removido));
                json.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
                json.Sucesso = false;
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Nome));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS08", "Reativar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Ativar(long[] ids)
        {
            return AlterarSituacao(ids, Situacao.Ativo);
        }

        [BaseAuthorize("EVS08", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Suspender(long[] ids)
        {
            return AlterarSituacao(ids, Situacao.Suspenso);
        }

        [BaseAuthorize("EVS08", "Cancelar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Cancelar(long[] ids)
        {
            return AlterarSituacao(ids, Situacao.Cancelado);
        }

        [Transaction(TransactionAttributeType.Required)]
        private ActionResult AlterarSituacao(long[] ids, Situacao novaSituacao)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
                var numAlterados = _pontoVendaRepo.AlterarSituacaoEmLote(novaSituacao, ids, idUsuario);
                jsonResponse.Sucesso = true;
                String situacaoStr = "";
                switch (novaSituacao)
                {
                    case Situacao.Ativo:
                        situacaoStr = GlobalMessages.Ativados;
                        break;
                    case Situacao.Suspenso:
                        situacaoStr = GlobalMessages.Suspensos;
                        break;
                    case Situacao.Cancelado:
                        situacaoStr = GlobalMessages.Cancelados;
                        break;
                }
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, situacaoStr, numAlterados,
                    ids.Length));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult ExportarTodos(DataSourceRequest request, PontoVendaDto pontoVenda)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, pontoVenda);
            string nomeArquivo = GlobalMessages.PontoVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public FileContentResult ExportarPagina(DataSourceRequest request, PontoVendaDto pontoVenda)
        {
            byte[] file = Exportar(request, pontoVenda);
            string nomeArquivo = GlobalMessages.PontoVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, PontoVendaDto pontoVenda)
        {
            var results = _viewPontoVendaRepository.Listar(request, pontoVenda);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string formatRG = "00.000.000-00";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomePontoVenda).Width(20)
                    .CreateCellValue(model.NomeFantasia).Width(20)
                    .CreateCellValue(model.IniciativaTenda).Width(20)
                    .CreateCellValue(model.Situacao.AsString()).Width(20)
                    .CreateCellValue(model.NomeViabilizador).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.GerenciadoTenda,
                GlobalMessages.Situacao,
                GlobalMessages.Viabilizador
            };
            return header.ToArray();
        }

    }
}