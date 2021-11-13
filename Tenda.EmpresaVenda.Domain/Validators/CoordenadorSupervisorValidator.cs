using Europa.Extensions;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class CoordenadorSupervisorValidator : AbstractValidator<CoordenadorSupervisor>
    {
        private CoordenadorSupervisorRepository _coordenadorSupervisorRepository { get; set; }
        public CoordenadorSupervisorValidator()
        {
            RuleFor(x => x.Supervisor).Must(x => CoordenadorSupervisorUnico(x.Id)).WithMessage(x=> MsgErroSupervisor(x.Supervisor.Id));
        }

        public bool CoordenadorSupervisorUnico(long idSupervisor)
        {
            return !_coordenadorSupervisorRepository.CoordenadorSupervisorUnico(idSupervisor);
        }

        public string MsgErroSupervisor(long idSupervisor)
        {
            var coordenadorSupervisor = _coordenadorSupervisorRepository.Queryable()
                .Where(x => x.Supervisor.Id == idSupervisor)
                .SingleOrDefault();

            return coordenadorSupervisor.HasValue() ? string.Format("O supervisor já está associado ao coordenador {1}",coordenadorSupervisor.Supervisor.Nome,coordenadorSupervisor.Coordenador.Nome) : "";

        }
    }
}
