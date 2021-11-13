using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class FechamentoContabilValidator:AbstractValidator<FechamentoContabilDto>
    {
        private FechamentoContabilRepository _fechamentoContabilRepository { get; set; }
        public FechamentoContabilValidator()
        {
            RuleFor(x => x).Must(x => CheckPeriodo(x))
                .WithMessage(GlobalMessages.DataInicioTerminoInvalida);
            RuleFor(x => x.InicioFechamento).Must(x => CheckDataInicio(x))
                .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.InicioFechamento))
                .WithName("InicioFechamento");
            RuleFor(x => x.TerminoFechamento).Must(x => CheckDataTermino(x))
                .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.TerminoFechamento))
                .WithName("TerminoFechamento");
            RuleFor(x => x.Descricao).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                .WithName("Descricao");
            RuleFor(x => x.QuantidadeDiasLembrete).Must(x => CheckQtDiasLembrete(x))
                .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.QuantidadeDiasLembrete))
                .WithName("Descricao");
            RuleFor(x => x).Must(x => CheckPeriodoValido(x.InicioFechamento, x.TerminoFechamento))
                .WithMessage(GlobalMessages.PeriodoInvalido);
        }
        
        public bool CheckPeriodo(FechamentoContabilDto fechamentoDto)
        {
            if (fechamentoDto.InicioFechamento.IsEmpty())
            {
                return true;
            }

            if (fechamentoDto.TerminoFechamento.IsEmpty())
            {
                return true;
            }

            if (fechamentoDto.InicioFechamento >= fechamentoDto.TerminoFechamento)
            {
                return false;
            }

            return true;
        }
        public bool CheckDataInicio(DateTime inicioFechamento)
        {
            if(inicioFechamento.Date < DateTime.Now.Date)
            {
                return false;
            }

            return _fechamentoContabilRepository.CheckIntersecaoData(inicioFechamento);
        }

        public bool CheckDataTermino(DateTime terminoFechamento)
        {
            if(terminoFechamento.Date < DateTime.Now.Date)
            {
                return false;
            }

            return _fechamentoContabilRepository.CheckIntersecaoData(terminoFechamento);
        }

        public bool CheckPeriodoValido(DateTime inicio, DateTime termino)
        {
            return _fechamentoContabilRepository.CheckPeriodoValido(inicio, termino);
        }

        public bool CheckQtDiasLembrete(int dias)
        {
            if(dias < 31 && dias >= 0)
            {
                return true;
            }
            return false;
        }
    }    
}
