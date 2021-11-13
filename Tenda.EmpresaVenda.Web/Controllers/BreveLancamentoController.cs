using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Web.Controllers
{

    [BaseAuthorize("EVS09")]
    public class BreveLancamentoController : BaseController
    {
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private EnderecoBreveLancamentoService _enderecoBreveLancamentoService { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private ViewBreveLancamentoEnderecoRepository _viewBreveLancamentoEnderecoRepository { get; set; }
        private RegionaisRepository _regionaisRepository { get; set; }

        [BaseAuthorize("EVS09", "Visualizar")]
        public ActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Regionais = _regionaisRepository.getAll();
            mymodel.BreveLancamento = new BreveLancamento();
            return View(mymodel);
        }

        [BaseAuthorize("EVS09", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, BreveLancamentoListarDTO filtro)
        {
            var results = _viewBreveLancamentoEnderecoRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS09", "Visualizar")]
        public ActionResult ListarPrioridade(DataSourceRequest request, string nome)
        {
            var results = _breveLancamentoRepository.Listar(nome);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        private JsonResult Salvar(EnderecoBreveLancamento enderecoBreveLancamento, BreveLancamento breveLancamento)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();

            try
            {
                breveLancamento = _breveLancamentoService.Salvar(breveLancamento, validation);

                enderecoBreveLancamento.BreveLancamento = breveLancamento;
                _enderecoBreveLancamentoService.Salvar(enderecoBreveLancamento, validation);

                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, breveLancamento.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS09", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(EnderecoBreveLancamento enderecoBreveLancamento, BreveLancamento breveLancamento)
        {
            return Salvar(enderecoBreveLancamento, breveLancamento);
        }

        [BaseAuthorize("EVS09", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(EnderecoBreveLancamento enderecoBreveLancamento, BreveLancamento breveLancamento)
        {
            return Salvar(enderecoBreveLancamento, breveLancamento);
        }

        [HttpGet]
        [BaseAuthorize("EVS09", "Visualizar")]
        public ActionResult BuscarFormulario(long idBreveLancamento)
        {
            var jsonResponse = new JsonResponse();
            if (idBreveLancamento == 0)
            {
                var breveLancamento = new BreveLancamento
                {
                    DisponivelCatalogo = true
                };
                var enderecoBreveLancamento = new EnderecoBreveLancamento
                {
                    BreveLancamento = breveLancamento
                };

                var result = new
                {
                    htmlBreveLancamento = RenderRazorViewToString("_Formulario", breveLancamento, false),
                    htmlEnderecoBreveLancamento = RenderRazorViewToString("_FormularioEndereco", enderecoBreveLancamento, false),
                };

                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
            }
            else
            {
                try
                {
                    var breveLancamento = _breveLancamentoRepository.FindById(idBreveLancamento);
                    var enderecoBreveLancamento = _enderecoBreveLancamentoRepository.FindByBreveLancamento(idBreveLancamento);

                    var result = new
                    {
                        htmlBreveLancamento = RenderRazorViewToString("_Formulario", breveLancamento, false),
                        htmlEnderecoBreveLancamento = RenderRazorViewToString("_FormularioEndereco", enderecoBreveLancamento, false)
                    };

                    jsonResponse.Objeto = result;
                    jsonResponse.Sucesso = true;
                }
                catch (BusinessRuleException bre)
                {
                    jsonResponse.Mensagens.AddRange(bre.Errors);
                    jsonResponse.Sucesso = false;

                }
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS09", "AssociarEmpreendimento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AssociarComEmpreendimento(long idBreveLancamento, long idEmpreendimento)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();

            try
            {
                var breveLancamento = _breveLancamentoService.AssociarComEmpreendimento(idBreveLancamento, idEmpreendimento, validation);
                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, breveLancamento.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS09", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Excluir(long idBreveLancamento)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            var entidade = _breveLancamentoRepository.FindById(idBreveLancamento);

            try
            {
                _breveLancamentoService.ExcluirPorId(idBreveLancamento);
                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, entidade.Nome, GlobalMessages.Excluido.ToLower()));

            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, entidade.ChaveCandidata()));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarDaRegional(DataSourceRequest request)
        {
            var result = _enderecoBreveLancamentoRepository.DisponiveisParaRegional(request, null);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaRegionalSemEmpreendimento(DataSourceRequest request)
        {
            var regional = request.filter.FirstOrDefault(x => x.column.Equals("regional")).value;
            //@REVER
            var result = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(null);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS09", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarPrioridade(BreveLancamento breveLancamento)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();

            try
            {
                breveLancamento = _breveLancamentoService.SalvarPrioridade(breveLancamento, validation);

                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, breveLancamento.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTorre(DataSourceRequest request)
        {
            //FIX-ME: Verificar com o Conecta a solução adequada
            if (request.HasValue())
            {
                request.pageSize = 100;
            }

            var filtroBreveLancamento = request.filter.FirstOrDefault(x => x.column.Equals("idBreveLancamento"));
            if (filtroBreveLancamento.IsNull())
            {
                return Json(new List<TorreDTO>().AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
            }

            var breveLancamento = _breveLancamentoRepository.FindById(Convert.ToInt32(filtroBreveLancamento.value));
            if (breveLancamento == null || breveLancamento.Empreendimento == null)
            {
                var listTorre = new List<TorreDTO>();
                var torreDto = new TorreDTO();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                return Json(listTorre.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
            }

            var results = ConsultaEstoqueService.EstoqueTorre(request, breveLancamento.Empreendimento.Divisao);
            if (results.records.IsEmpty())
            {
                var listTorre = new List<TorreDTO>();
                var torreDto = new TorreDTO();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                results = listTorre.AsQueryable().ToDataRequest(request);
            }
            else
            {
                var filtroNomeTorre = request.filter.FirstOrDefault(x => x.column.Equals("NomeTorre"));
                if (filtroNomeTorre.HasValue())
                {
                    results = results.records.Where(x => x.NomeTorre.ToLower().Contains(filtroNomeTorre.value.ToLower())).AsQueryable().ToDataRequest(request);
                }
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPorRegionalDisponiveisParaCatalogoNoEstado(DataSourceRequest request)
        {
            var regional = request.filter.FirstOrDefault(x => x.column.Equals("regional")).value;
            //@REVER
            var result = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(null);

            var list = result.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}