using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class RegraComissaoValidator : AbstractValidator<RegraComissao>
    {
        private RegraComissaoRepository _regraComissaoRepository { get; set; }

        public RegraComissaoValidator(RegraComissaoRepository regraComissaoRepository)
        {
            _regraComissaoRepository = regraComissaoRepository;

            RuleFor(regra=>regra.Tipo).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoRegraComissao));
            RuleFor(regra => regra.Regional).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
            RuleFor(regra => regra.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(regra => regra).Must(regra => CheckVigencia(regra, regra.InicioVigencia)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.InicioVigencia));
            RuleFor(regra => regra).Must(regra => CheckVigencia(regra, regra.TerminoVigencia)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TerminoVigencia));
            RuleFor(regra => regra).Must(regra => CheckVigencia(regra)).WithMessage(string.Format(GlobalMessages.MsgErroRangeDataMaior,GlobalMessages.TerminoVigencia,GlobalMessages.InicioVigencia));
        }

        public bool CheckVigencia(RegraComissao regra, DateTime? data)
        {
            if (regra.Tipo.IsEmpty())
            {
                return true;
            }

            if(regra.Tipo == TipoRegraComissao.Campanha)
            {
                if (data.IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckVigencia(RegraComissao regraComissao)
        {
            if (regraComissao.InicioVigencia.IsEmpty())
            {
                return true;
            }

            if (regraComissao.TerminoVigencia.IsEmpty())
            {
                return true;
            }

            if(regraComissao.InicioVigencia > regraComissao.TerminoVigencia)
            {
                return false;
            }

            return true;
        }
    }
}
