using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.WebPages;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.StatusPreProposta;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class StatusPrePropostaValidator:AbstractValidator<StatusPrePropostaDto>
    {
        public StatusPrePropostaRepository _StatusPrePropostaRepository { get; set; }

        public StatusPrePropostaValidator(StatusPrePropostaRepository StatusPrePropostaRepository)
        {
            RuleFor(Status => Status.StatusPadrao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Status));
            RuleFor(Status => Status.StatusPortalHouse).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Status));
        }

    }
}
