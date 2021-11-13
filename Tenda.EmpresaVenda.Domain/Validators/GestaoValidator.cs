using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class GestaoValidator : AbstractValidator<Gestao>
    {
        public GestaoRepository _gestaoRepository { get; set; }

        public GestaoValidator(GestaoRepository gestaoRepository)
        {
            _gestaoRepository = gestaoRepository;
            RuleFor(ges => ges.DataReferencia).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataReferencia));
            RuleFor(ges => ges.TipoCusto).NotEmpty().OverridePropertyName("TipoCusto").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(ges => ges.Classificacao).NotEmpty().OverridePropertyName("Classificacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Classificacao));
            RuleFor(ges => ges.Fornecedor).NotEmpty().OverridePropertyName("Fornecedor").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Fornecedor));
            RuleFor(ges => ges.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoServico));
            RuleFor(ges => ges.EmpresaVenda).NotEmpty().OverridePropertyName("EmpresaVenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda));
            RuleFor(ges => ges.PontoVenda).NotEmpty().OverridePropertyName("PontoVenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PontoVenda));
            RuleFor(ges => ges.CentroCusto).NotEmpty().OverridePropertyName("CentroCusto").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentroCusto));
            RuleFor(ges => ges.ValorBudgetEstimado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.BudgetEstimado));
            RuleFor(ges => ges.NumeroChamado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(ges => ges.DataCriacaoChamado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataCriacao));
            RuleFor(ges => ges.DataFarol).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataFarol));
            RuleFor(ges => ges.NumeroRequisicaoCompra).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RequisicaoCompra));
            RuleFor(ges => ges.ValorGasto).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorGasto));
            RuleFor(ges => ges.NumeroPedido).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NumeroPedido));
            RuleFor(ges => ges.Observacao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.HistoricoObservacao));

        }
    }
}
