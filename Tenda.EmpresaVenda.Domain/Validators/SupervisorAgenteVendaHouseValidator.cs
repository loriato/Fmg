using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class SupervisorAgenteVendaHouseValidator:AbstractValidator<HierarquiaHouseDto>
    {
        private CoordenadorSupervisorHouseRepository _coordenadorSupervisorHouseRepository { get; set; }
        private SupervisorAgenteVendaHouseRepository _supervisorAgenteVendaHouseRepository { get; set; }
        private AgenteVendaHouseRepository _agenteVendaHouseRepository { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }
        public SupervisorAgenteVendaHouseValidator()
        {
            RuleFor(x => x.IdUsuarioAgenteVenda).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.UsuarioAgenteVenda));
            RuleFor(x => x.NomeUsuarioAgenteVenda).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.UsuarioAgenteVenda));
            RuleFor(x => x.EmailUsuarioAgenteVenda).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmailAgenteVenda));
            
            RuleFor(x => x.IdSupervisorHouse).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Supervisor));
            RuleFor(x => x).Must(x=> ValidarVinculoCoordenadorSupervisorHouse(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(x=> MsgValidarVinculoCoordenadorSupervisorHouse(x));
            RuleFor(x => x).Must(x => ValidarSupervisor(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format("Supervisor(a) não possui vinculo com algum coordenador"));
            RuleFor(x => x.NomeSupervisorHouse).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Supervisor));
            
            RuleFor(x => x.IdHouse).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.House));
            
            RuleFor(x => x.IdCoordenadorHouse).NotEmpty().WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Coordenador));

            RuleFor(x => x).Must(x => ValidarVinculoSupervisorAgenteVendaHouse(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(x => MsgValidarVinculoSupervisorAgenteVendaHouse(x));
            
            RuleFor(x => x).Must(x => ValidarAgenteHouse(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(x => MsgValidarAgenteHouse(x));

            RuleFor(x => x).Must(x => ValidarHierarquia(x)).WithName(RestDefinitions.GlobalMessageErrorKey).WithMessage(x => MsgValidarHierarquia(x));

        }

        public bool ValidarVinculoCoordenadorSupervisorHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            if (hierarquiaHouseDto.IdCoordenadorHouse.IsEmpty() || hierarquiaHouseDto.IdSupervisorHouse.IsEmpty())
            {
                return true;
            }

            var coordenadorSupervisor = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(hierarquiaHouseDto.IdSupervisorHouse);

            if (coordenadorSupervisor.IsEmpty())
            {
                return false;
            }

            if (hierarquiaHouseDto.IsSuperiorHouse)
            {
                return true;
            }

            if (!hierarquiaHouseDto.IsCoordenadorHouse)
            {
                return true;
            }

            if (coordenadorSupervisor.HasValue() && coordenadorSupervisor.Coordenador.Id == hierarquiaHouseDto.IdCoordenadorHouse)
            {
                return true;
            }

            return false;
        }

        public string MsgValidarVinculoCoordenadorSupervisorHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var coordenadorSupervisorHouse = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(hierarquiaHouseDto.IdSupervisorHouse);

            if (coordenadorSupervisorHouse.IsEmpty())
            {
                return string.Format("Supervisor(a) {0} não possui vinculo com algum coordenador",hierarquiaHouseDto.NomeSupervisorHouse);
            }
            return string.Format("Alteração só pode ser realizada pelo(a) Supervisor(a) {0} ou Coordenador(a) {1}", coordenadorSupervisorHouse.Supervisor.Nome, coordenadorSupervisorHouse.Coordenador.Nome);
        }

        public bool ValidarSupervisor(HierarquiaHouseDto hierarquiaHouseDto)
        {
            if (!hierarquiaHouseDto.IsSupervisorHouse)
            {
                return true;
            }

            if (hierarquiaHouseDto.IdSupervisorHouse.IsEmpty())
            {
                return true;
            }

            var coordenadorSupervisor = _coordenadorSupervisorHouseRepository.BuscarVinculoSupervisorAtivo(hierarquiaHouseDto.IdSupervisorHouse);

            if (coordenadorSupervisor.HasValue())
            {
                return true;
            }

            return false;
        }

        public bool ValidarVinculoSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            if (hierarquiaHouseDto.IdUsuarioAgenteVenda.IsEmpty())
            {
                return true;
            }

            var supervisorAgenteVenda = _supervisorAgenteVendaHouseRepository.BuscarVinculoSupervisorAgenteVendaAtivo(hierarquiaHouseDto.IdUsuarioAgenteVenda);

            if (supervisorAgenteVenda.IsEmpty())
            {
                return true;
            }

            if (supervisorAgenteVenda.Supervisor.Id == hierarquiaHouseDto.IdSupervisorHouse)
            {
                return true;
            }

            return false;
        }

        public string MsgValidarVinculoSupervisorAgenteVendaHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var supervisorAgenteVenda = _supervisorAgenteVendaHouseRepository.BuscarVinculoSupervisorAgenteVendaAtivo(hierarquiaHouseDto.IdUsuarioAgenteVenda);

            var msg = string.Format("Agente de Venda {0} já vinculado ao Supervisor(a) {1}", supervisorAgenteVenda.AgenteVenda.Nome, supervisorAgenteVenda.Supervisor.Nome);

            return msg;
        }
        
        public bool ValidarAgenteHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            if (hierarquiaHouseDto.IdUsuarioAgenteVenda.IsEmpty())
            {
                return true;
            }

            var agenteVendaHouse = _agenteVendaHouseRepository.Queryable()
                .Where(x => x.UsuarioAgenteVenda.Id == hierarquiaHouseDto.IdUsuarioAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            if (agenteVendaHouse.IsEmpty())
            {
                return true;
            }

            return false;
        }

        public string MsgValidarAgenteHouse(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var agenteVendaHouse = _agenteVendaHouseRepository.Queryable()
                .Where(x => x.UsuarioAgenteVenda.Id == hierarquiaHouseDto.IdUsuarioAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            var msg = string.Format("Agente de Venda {0} já vinculado a Loja {1}",agenteVendaHouse.AgenteVenda.Nome,agenteVendaHouse.House.NomeFantasia);

            return msg;
        }

        public bool ValidarHierarquia(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var hierarquia = _hierarquiaHouseRepository.Queryable()
                .Where(x => x.AgenteVenda.Id == hierarquiaHouseDto.IdUsuarioAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Any();

            return !hierarquia;
        }      
        
        public string MsgValidarHierarquia(HierarquiaHouseDto hierarquiaHouseDto)
        {
            var hierarquia = _hierarquiaHouseRepository.Queryable()
                .Where(x => x.AgenteVenda.Id == hierarquiaHouseDto.IdUsuarioAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            return string.Format("O Agente {0} pertence a hierarquia do Coordenador {1}", hierarquia.AgenteVenda.Nome, hierarquia.Coordenador.Nome);
        }


    }
}
