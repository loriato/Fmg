using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PontuacaoFidelidadeValidator : AbstractValidator<PontuacaoFidelidade>
    {
        public PontuacaoFidelidadeRepository _pontuacaoFidelidadeRepository { get; set; }

        public PontuacaoFidelidadeValidator(PontuacaoFidelidadeRepository pontuacaoFidelidadeRepository)
        {
            _pontuacaoFidelidadeRepository = pontuacaoFidelidadeRepository;

            RuleFor(x => x.Regional).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional)).OverridePropertyName("Regional");
            RuleFor(x => x.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao)).OverridePropertyName("Descricao");
            RuleFor(x => x).Must(x => ValidarCampanha(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, "Inicio e Término de Vigência"));
            RuleFor(x => x.InicioVigencia).Must(x => ValidarDataInicio(x)).WithMessage(string.Format(GlobalMessages.DadoInvalido,GlobalMessages.DataInicio));
            RuleFor(x => x).Must(x => ValidarPeriodo(x)).WithMessage(string.Format(GlobalMessages.DataInicioTerminoInvalida));
        }

        public bool ValidarCampanha(PontuacaoFidelidade pontuacaoFidelidade)
        {
            if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                return true;
            }

            if (pontuacaoFidelidade.InicioVigencia.IsEmpty())
            {
                return false;
            }

            if (pontuacaoFidelidade.TerminoVigencia.IsEmpty())
            {
                return false;
            }

            return true;
        }

        public bool ValidarDataInicio(DateTime? data)
        {
            if (data.IsEmpty())
            {
                return true;
            }

            if (data.Value.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }

        public bool ValidarPeriodo(PontuacaoFidelidade pontuacao)
        {
            if (pontuacao.InicioVigencia.IsEmpty())
            {
                return true;
            }

            if (pontuacao.TerminoVigencia.IsEmpty())
            {
                return true;
            }

            if (pontuacao.InicioVigencia.Value.Date > pontuacao.TerminoVigencia.Value.Date)
            {
                return false;
            }

            return true;
        }
    }
}
