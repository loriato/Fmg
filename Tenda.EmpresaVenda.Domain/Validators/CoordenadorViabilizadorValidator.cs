using Europa.Extensions;
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
    public class CoordenadorViabilizadorValidator : AbstractValidator<CoordenadorViabilizador>
    {
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        public CoordenadorViabilizadorValidator()
        {
            RuleFor(x => x.Viabilizador).Must(x => CoordenadorViabilizadorUnico(x.Id)).WithMessage(x => MsgErroCoordenadorViabilizadorUnico(x.Viabilizador.Id));
            RuleFor(x => x.Viabilizador).Must(x => SupervisorViabilizadorUnico(x.Id)).WithMessage(x => MsgErroSupervisorViabilizadorUnico(x.Viabilizador.Id));
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

        public bool SupervisorViabilizadorUnico(long idViabilizador)
        {
            return _supervisorViabilizadorRepository.CruzamentoUnicoPorViabilizador(idViabilizador).IsEmpty();
        }

        public string MsgErroSupervisorViabilizadorUnico(long idViabilizador)
        {
            var supervisorViabilizador = _supervisorViabilizadorRepository.CruzamentoUnicoPorViabilizador(idViabilizador);

            return string.Format("O viabilizador já está associado ao supervisor {0}", supervisorViabilizador.Supervisor.Nome);
        }
    }
}
