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
    [BaseAuthorize("EVS18")]
    public class GestaoController : BaseController
    {
        private GestaoRepository _gestaoRepository { get; set; }
        private GestaoService _gestaoService { get; set; }

        [BaseAuthorize("EVS18", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS18", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request,GestaoDTO filtro)
        {
            var results = _gestaoRepository.Listar(filtro).Select(x => new Gestao
            {
                Id = x.Id,
                Descricao = x.Descricao,
                DataReferencia = x.DataReferencia,
                ValorBudgetEstimado = x.ValorBudgetEstimado,
                NumeroChamado = x.NumeroChamado,
                DataCriacaoChamado = x.DataCriacaoChamado,
                DataFarol = x.DataFarol,
                NumeroRequisicaoCompra = x.NumeroRequisicaoCompra,
                ValorGasto = x.ValorGasto,
                NumeroPedido = x.NumeroPedido,
                Observacao = x.Observacao,
                TipoCusto = new TipoCusto
                {
                    Id = x.TipoCusto.Id,
                    Descricao = x.TipoCusto.Descricao
                },
                Classificacao = new Classificacao
                {
                    Id = x.Classificacao.Id,
                    Descricao = x.Classificacao.Descricao
                },
                Fornecedor = new Fornecedor
                {
                    Id = x.Fornecedor.Id,
                    NomeFantasia = x.Fornecedor.NomeFantasia
                },
                EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                {
                    Id = x.EmpresaVenda.Id,
                    NomeFantasia = x.EmpresaVenda.NomeFantasia
                },
                PontoVenda = new PontoVenda
                {
                    Id = x.PontoVenda.Id,
                    Nome = x.PontoVenda.Nome
                },
                CentroCusto = new CentroCusto
                {
                    Id = x.CentroCusto.Id,
                    Codigo = x.CentroCusto.Codigo
                }

            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS18", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(Gestao model)
        {
            return Salvar(model, false);
        }

        [BaseAuthorize("EVS18", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Alterar(Gestao model)
        {
            return Salvar(model, true);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Salvar(Gestao model, bool isUpdate)
        {
            var json = new JsonResponse();
            try
            {
                var bre = new BusinessRuleException();
                _gestaoService.Salvar(model, bre);
                bre.ThrowIfHasError();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, model.ChaveCandidata(), isUpdate ? GlobalMessages.Alterado : GlobalMessages.Incluido));
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

        [BaseAuthorize("EVS18", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Remover(long id)
        {
            var json = new JsonResponse();
            var obj = _gestaoRepository.FindById(id);
            try
            {
                _gestaoService.Excluir(obj);
                CurrentTransaction().Commit();
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, obj.ChaveCandidata(), GlobalMessages.Removido));
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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, obj.Descricao));
                    json.Sucesso = false;
                }
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult ExportarTodos(DataSourceRequest request, GestaoDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _gestaoService.Exportar(modifiedRequest,filtro);
            string nomeArquivo = GlobalMessages.Gestao;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public FileContentResult ExportarPagina(DataSourceRequest request, GestaoDTO filtro)
        {
            byte[] file = _gestaoService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.Gestao;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    } 
}