using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class SupervisorViabilizadorValidator : AbstractValidator<SupervisorViabilizador>
    {
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get;set;}
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        public SupervisorViabilizadorValidator()
        {
            RuleFor(x => x.Supervisor).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Supervisor));
            RuleFor(x => x.Viabilizador).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Viabilizador));
            RuleFor(x => x).Must(x => CruzamentoUnico(x.Viabilizador.Id)).WithMessage(x=>MsgCruzamentoUnico(x.Viabilizador.Id));
            RuleFor(x => x.Viabilizador).Must(x => CoordenadorViabilizadorUnico(x.Id)).WithMessage(x => MsgErroCoordenadorViabilizadorUnico(x.Viabilizador.Id));

        }

        public bool CruzamentoUnico(long idViabilizador)
        {
            return _supervisorViabilizadorRepository.CruzamentoUnicoPorViabilizador(idViabilizador).IsNull();
        }

        public string MsgCruzamentoUnico(long idViabilizador)
        {
            var crz = _supervisorViabilizadorRepository.CruzamentoUnicoPorViabilizador(idViabilizador);

            return string.Format("O viabilizador {0} já está associado ao supervisor {1}",crz.Viabilizador.Nome,crz.Supervisor.Nome);
        }

        public bool CoordenadorViabilizadorUnico(long idViabilizador)
        {
            return !_coordenadorViabilizadorRepository.CoordenadorViabilizadorUnico(idViabilizador);
        }

        public string MsgErroCoordenadorViabilizadorUnico(long idViabilizador)
        {
            var coordenadorViabilizador = _coordenadorViabilizadorRepository.Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .SingleOrDefault();

            return coordenadorViabilizador.HasValue() ? string.Format("O viabilizador já está associado ao coordenador {0}", coordenadorViabilizador.Coordenador.Nome) : "";

        }
    }
}
