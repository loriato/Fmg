using Europa.Extensions;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class IntegracaoPrePropostaValidator : AbstractValidator<PreProposta>
    {

        public IntegracaoPrePropostaValidator()
        {
            RuleFor(prpr => prpr).Must(prpr => BreveLancamentoPossuiEmpreendimento(prpr)).WithName("Empreendimento")
                .WithMessage(prpr => string.Format("O Breve Lançamento da Pré-Proposta {0} não possui empreendimento definido", prpr.Codigo));
            RuleFor(prpr => prpr).Must(prpr => PontoVendaPossuiViabilizador(prpr)).WithName("Viabilizador")
                .WithMessage(prpr => string.Format("O Ponto de Venda da Pré-Proposta {0} não possui Viabilizador definido", prpr.Codigo));
            RuleFor(prpr => prpr).Must(prpr => UnidadeDefinida(prpr)).WithName("Unidade")
                .WithMessage(prpr => string.Format("A Pré-Proposta {0} não unidade destino definida", prpr.Codigo));
            RuleFor(prpr => prpr).Must(prpr => EmpresaVendaPossuiCentralImobiliaria(prpr)).WithName("CentralImobiliaria")
               .WithMessage(prpr => string.Format("A Empresa de Venda {0} da Pré-Proposta {1} não possui central imobiliária definida", prpr.EmpresaVenda.NomeFantasia, prpr.Codigo));
            RuleFor(prpr => prpr).Must(prpr => VerificarParametroMidia(ProjectProperties.SuatMidiaProposta)).WithName("Midia")
                .WithMessage(prpr => string.Format("O parâmetro {0}, referente a mídia da empresa de vendas não foi configurado", ProjectProperties.SuatMidiaPropostaProperty));
        }

        public bool BreveLancamentoPossuiEmpreendimento(PreProposta preProposta)
        {
            return preProposta.BreveLancamento.Empreendimento != null;
        }

        public bool PontoVendaPossuiViabilizador(PreProposta preProposta)
        {
            return preProposta.PontoVenda.Viabilizador != null;
        }

        public bool EmpresaVendaPossuiCentralImobiliaria(PreProposta preProposta)
        {
            return preProposta.EmpresaVenda.Loja != null;
        }

        public bool UnidadeDefinida(PreProposta preProposta)
        {
            return preProposta.IdUnidadeSuat.HasValue();
        }

        public bool VerificarParametroMidia(long idMidiaDestino)
        {
            return idMidiaDestino.HasValue();
        }
    }
}