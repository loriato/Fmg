using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ItemPontuacaoFidelidadeValidator:AbstractValidator<ItemPontuacaoFidelidade>
    {
        public ItemPontuacaoFidelidadeValidator()
        {
            RuleFor(x => x).Must(x => ValidarItemPadraoFixo(x)).WithMessage(x=>string.Format(GlobalMessages.MsgItemPontuacaoFidelidadeInvalido,x.Empreendimento.Nome,x.EmpresaVenda.NomeFantasia))
                .Must(x => ValidarItemPadraoNominal(x)).WithMessage(x=>string.Format(GlobalMessages.MsgItemPontuacaoFidelidadeInvalido,x.Empreendimento.Nome,x.EmpresaVenda.NomeFantasia))
                .Must(x => ValidarItemCampanhaFixo(x)).WithMessage(x=>string.Format(GlobalMessages.MsgItemPontuacaoFidelidadeInvalido,x.Empreendimento.Nome,x.EmpresaVenda.NomeFantasia))
                .Must(x => ValidarCampanhaNominal(x)).WithMessage(x=>string.Format(GlobalMessages.MsgItemPontuacaoFidelidadeInvalido,x.Empreendimento.Nome,x.EmpresaVenda.NomeFantasia))
                .Must(x => ValidarQuantidadeMinima(x)).WithMessage(x=>string.Format(GlobalMessages.MsgItemPontuacaoFidelidadeInvalido,x.Empreendimento.Nome,x.EmpresaVenda.NomeFantasia));
        }
        public bool ValidarItemPadraoNominal(ItemPontuacaoFidelidade item)
        {
            if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
            {
                return true;
            }

            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                return true;
            }

            if (item.PontuacaoFaixaUmMeioSeca < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaUmMeioNormal < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaUmMeioTurbinada < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisSeca < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisNormal < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisTurbinada < 1)
            {
                return false;
            }

            if (item.PontuacaoPNESeca < 1)
            {
                return false;
            }

            if (item.PontuacaoPNENormal < 1)
            {
                return false;
            }

            if (item.PontuacaoPNETurbinada < 1)
            {
                return false;
            }

            return true;
        }
        public bool ValidarItemPadraoFixo(ItemPontuacaoFidelidade item)
        {
            if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
            {
                return true;
            }

            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Nominal)
            {
                return true;
            }

            if (item.PontuacaoFaixaUmMeio < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDois < 1)
            {
                return false;
            }

            return true;
        }
        public bool ValidarItemCampanhaFixo(ItemPontuacaoFidelidade item)
        {
            if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                return true;
            }

            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Nominal)
            {
                return true;
            }

            if (item.FatorUmMeio < 1)
            {
                return false;
            }

            if (item.PontuacaoPadraoUmMeio < 1)
            {
                return false;
            }

            if (item.FatorDois < 1)
            {
                return false;
            }

            if (item.PontuacaoPadraoDois < 1)
            {
                return false;
            }

            return true;
        }
        public bool ValidarCampanhaNominal(ItemPontuacaoFidelidade item)
        {
            if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                return true;
            }

            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                return true;
            }

            if (item.FatorUmMeioSeca < 1)
            {
                return false;
            }

            if(item.PontuacaoFaixaUmMeioSeca < 1)
            {
                return false;
            }

            if (item.FatorUmMeioNormal < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaUmMeioNormal < 1)
            {
                return false;
            }

            if (item.FatorUmMeioTurbinada < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaUmMeioTurbinada < 1)
            {
                return false;
            }

            if (item.FatorDoisSeca < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisSeca < 1)
            {
                return false;
            }

            if (item.FatorDoisNormal < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisNormal < 1)
            {
                return false;
            }

            if (item.FatorDoisTurbinada < 1)
            {
                return false;
            }

            if (item.PontuacaoFaixaDoisTurbinada < 1)
            {
                return false;
            }

            if (item.FatorPNESeca < 1)
            {
                return false;
            }

            if (item.PontuacaoPNESeca < 1)
            {
                return false;
            }

            if (item.FatorPNENormal < 1)
            {
                return false;
            }

            if (item.PontuacaoPNENormal < 1)
            {
                return false;
            }

            if (item.FatorPNETurbinada < 1)
            {
                return false;
            }

            if (item.PontuacaoPNETurbinada < 1)
            {
                return false;
            }

            return true;
        }
        public bool ValidarQuantidadeMinima(ItemPontuacaoFidelidade item)
        {
            if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                return true;
            }

            if (item.PontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVenda)
            {
                return true;
            }

            if (item.QuantidadeMinima < 1)
            {
                return false;
            }

            return true;
        }
    }
}
