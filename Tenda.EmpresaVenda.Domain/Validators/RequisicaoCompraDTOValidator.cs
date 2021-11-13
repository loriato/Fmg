using Europa.Resources;
using FluentValidation;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class RequisicaoCompraDTOValidator : AbstractValidator<RequisicaoCompraDTO>
    {
        public RequisicaoCompraDTOValidator()
        {
            //Pagamento
            RuleFor(x => x.Pagamento.CodigoProposta).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Proposta));
            RuleFor(x => x.Pagamento.ValorAPagar).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorPagar));
            RuleFor(x => x.Pagamento.CodigoFornecedor).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CodigoFornecedor));
            RuleFor(x => x.Pagamento.CodigoEmpresa).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CodigoEmpresa));
            RuleFor(x => x.Pagamento.DivisaoEmpreendimento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Divisao));
            RuleFor(x => x.Pagamento.IdProposta).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Proposta));

            //Item requisição de compra
            RuleFor(x => x.ItemRequisicao.TextoBreve).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.TextoBreve));
            RuleFor(x=>x.ItemRequisicao.DataLiberacao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.DataLiberacao));
            RuleFor(x=>x.ItemRequisicao.DataRemessaItem).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataRemessaItem));
            RuleFor(x=>x.ItemRequisicao.DataSolicitacao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataSolicitacao));
            RuleFor(x=>x.ItemRequisicao.PrecoUnidade).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, "Preço Unidade"));
            RuleFor(x=>x.ItemRequisicao.CentroDeCusto).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentroCusto));
            RuleFor(x=>x.ItemRequisicao.Preco).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Preco));
            RuleFor(x=>x.ItemRequisicao.FornecedorPretendido).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.FornecedorPrentendido));
            RuleFor(x=>x.ItemRequisicao.TipoDocumento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.TipoDocumento));
            RuleFor(x=>x.ItemRequisicao.NumeroItem).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,"Numero Item"));
            RuleFor(x=>x.ItemRequisicao.GrupoCompradores).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.GrupoCompradores));
            RuleFor(x=>x.ItemRequisicao.NomeRequisitante).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.NomeRequisitante));
            RuleFor(x=>x.ItemRequisicao.NumeroMaterial).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,"Numero Material"));
            RuleFor(x=>x.ItemRequisicao.GrupoMercadorias).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.GrupoMercadorias));
            RuleFor(x=>x.ItemRequisicao.Quantidade).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.Quantidade));
            RuleFor(x=>x.ItemRequisicao.UnidadeMedida).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.UnidadeMedida));
            RuleFor(x=>x.ItemRequisicao.TipoData).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.TipoData));
            RuleFor(x=>x.ItemRequisicao.CategoriaClassificacaoContabil).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.CategoriaClassificacaoContabil));
            RuleFor(x=>x.ItemRequisicao.CodigoEntradaMercadorias).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.CodigoEntradaMercadorias));
            RuleFor(x=>x.ItemRequisicao.CodigoEntradaFaturas).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.CodigoEntradaFaturas));
            RuleFor(x=>x.ItemRequisicao.OrganizacaoDeCompras).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,"Organização de Compras"));
            RuleFor(x=>x.ItemRequisicao.CodigoMoeda).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,"Código Moeda"));
            RuleFor(x=>x.ItemRequisicao.AreaContabilidadeCustos).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.AreaContabilidadeCustos));
            RuleFor(x=>x.ItemRequisicao.NumeroSeqSegmentoClasseContabil).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.NumeroSeqSegmentoClasseContabil));
            RuleFor(x=>x.ItemRequisicao.NumeroContaRazao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.NumeroContaRazao));

            //Contabilização Requisição
            RuleFor(x=>x.ContabilizacaoRequisicao.NumeroOrdem).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NumeroOrdem));
            RuleFor(x=>x.ContabilizacaoRequisicao.Divisao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Divisao));
            RuleFor(x=>x.ContabilizacaoRequisicao.Quantidade).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Quantidade));
            RuleFor(x=>x.ContabilizacaoRequisicao.NumeroItem).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio,"Numero Item"));
        }
    }
}
