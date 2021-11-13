using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators.EnvioPreProposta
{
    public class PrePropostaValidator : AbstractValidator<PreProposta>
    {
        private readonly int SomatorioParticipacao = 100;

        public ProponenteRepository _proponenteRepository { get; set; }

        public PrePropostaValidator(ProponenteRepository proponenteRepository)
        {
            _proponenteRepository = proponenteRepository;

            RuleFor(prpr => prpr).Must(prpr => VerificarSomatorioParticipacao(prpr.Id)).WithName("Somatório")
                    .WithMessage("O somatório de participação dos proponentes da proposta deve ser de 100%");
        }

        public bool VerificarSomatorioParticipacao(long idPreProposta)
        {
            return _proponenteRepository.SomatorioParticipacaoProponentes(idPreProposta) == SomatorioParticipacao;
        }
    }
}