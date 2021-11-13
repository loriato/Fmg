using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation;
using System;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class CoordenadorSupervisorHouseValidator:AbstractValidator<CoordenadorSupervisorHouse>
    {
        private CoordenadorSupervisorHouseRepository _coordenadorSupervisorHouseRepository { get; set; }
        public CoordenadorSupervisorHouseValidator()
        {
            RuleFor(x => x.Supervisor.Id).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Supervisor));
            RuleFor(x => x.Coordenador.Id).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Coordenador));

            When(x => x.Inicio.IsEmpty(), () =>
            {
                RuleFor(x => x.Inicio).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Inicio));
            }).Otherwise(() =>
            {
                RuleFor(x => x.Inicio).Must(x => ValidarData(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.MsgDataPeriodoDeAteInvalida, GlobalMessages.Inicio, GlobalMessages.Hoje));
            });

            RuleFor(x => x).Must(x => ValidarVinculoCoordenadorSupervisorHouse(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(x => MsgValidarVinculoCoordenadorSupervisorHouse(x.Supervisor.Id));
        }

        public bool ValidarData(DateTime? data)
        {
            if (data.IsEmpty())
            {
                return true;
            }

            return data.Value.Date >= DateTime.Now.Date;
        }

        public bool ValidarVinculoCoordenadorSupervisorHouse(CoordenadorSupervisorHouse coordenadorSupervisorHouse)
        {
            if (coordenadorSupervisorHouse.Coordenador.IsEmpty() || coordenadorSupervisorHouse.Coordenador.Id.IsEmpty())
            {
                return true;
            }

            if (coordenadorSupervisorHouse.Supervisor.IsEmpty() || coordenadorSupervisorHouse.Supervisor.Id.IsEmpty())
            {
                return true;
            }

            var validate = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(coordenadorSupervisorHouse.Supervisor.Id).HasValue();

            return !validate;
        }

        public string MsgValidarVinculoCoordenadorSupervisorHouse(long idSupervisor)
        {
            var coordenadorSupervisorHouse = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(idSupervisor);

            var msg = string.Format("Supervisor(a) {0} já está vinculado ao Coordenador(a) {1}", coordenadorSupervisorHouse.Supervisor.Nome, coordenadorSupervisorHouse.Coordenador.Nome);

            return msg;
        }
    }
}
