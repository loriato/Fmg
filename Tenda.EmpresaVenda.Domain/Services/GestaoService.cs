using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class GestaoService : BaseService
    {
        private GestaoRepository _gestaoRepository { get; set; }

        public void Salvar(Gestao model, BusinessRuleException bre)
        {
            if (model.PontoVenda.IsEmpty() && model.PontoVenda.Id == 0)
            {
                model.PontoVenda = null;
            }
            if (model.TipoCusto.IsEmpty() && model.TipoCusto.Id == 0)
            {
                model.TipoCusto = null;
            }
            if (model.EmpresaVenda.IsEmpty() && model.EmpresaVenda.Id == 0)
            {
                model.EmpresaVenda = null;
            }
            if (model.Fornecedor.IsEmpty() && model.Fornecedor.Id == 0)
            {
                model.Fornecedor = null;
            }
            if (model.Classificacao.IsEmpty() && model.Classificacao.Id == 0)
            {
                model.Classificacao = null;
            }
            if (model.CentroCusto.IsEmpty() && model.CentroCusto.Id == 0)
            {
                model.CentroCusto = null;
            }
            var validation = new GestaoValidator(_gestaoRepository).Validate(model);
            bre.WithFluentValidation(validation);
            

            if (validation.IsValid)
            {
                _gestaoRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public Gestao Excluir(Gestao model)
        {
            var exc = new BusinessRuleException();
            try
            {
                _gestaoRepository.Delete(model);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(model.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
            return model;
        }

        public byte[] Exportar(DataSourceRequest request,GestaoDTO filtro)
        {
            var results = _gestaoRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());
            
            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.DataReferencia.ToDate().ToString()).Width(20)
                    .CreateCellValue(model.TipoCusto.Descricao).Width(30)
                    .CreateCellValue(model.Classificacao.Descricao).Width(30)
                    .CreateCellValue(model.Fornecedor.NomeFantasia).Width(30)
                    .CreateCellValue(model.Descricao).Width(70)
                    .CreateCellValue(model.EmpresaVenda.NomeFantasia).Width(30)
                    .CreateCellValue(model.PontoVenda.Nome).Width(30)
                    .CreateCellValue(model.CentroCusto.Codigo).Width(20)
                    .CreateMoneyCell(model.ValorBudgetEstimado).Width(20)
                    .CreateCellValue(model.NumeroChamado).Width(20)
                    .CreateDateTimeCell(model.DataCriacaoChamado).Width(20)
                    .CreateDateTimeCell(model.DataFarol).Width(20)
                    .CreateCellValue(model.NumeroRequisicaoCompra).Width(20)
                    .CreateMoneyCell(model.ValorGasto).Width(20)
                    .CreateCellValue(model.NumeroPedido).Width(20)
                    .CreateCellValue(model.Observacao).Width(70);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.DataReferencia,
                GlobalMessages.Custo,
                GlobalMessages.Classificacao,
                GlobalMessages.Fornecedor,
                GlobalMessages.DescricaoServico,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.PontoVenda,
                GlobalMessages.CentroCusto,
                GlobalMessages.BudgetEstimado,
                GlobalMessages.Numero,
                GlobalMessages.DataCriacao,
                GlobalMessages.DataFarol,
                GlobalMessages.RequisicaoCompra,
                GlobalMessages.ValorGasto,
                GlobalMessages.NumeroPedido,
                GlobalMessages.HistoricoObservacao
            };
            return header.ToArray();
        }
    }
}

