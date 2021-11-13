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
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AvalistaValidator:AbstractValidator<Avalista>
    {
        public AvalistaRepository _avalistaRepository { get; set; }

        public AvalistaValidator(AvalistaRepository avalistaRepository)
        {
            _avalistaRepository = avalistaRepository;

            RuleFor(avalista => avalista.Nome).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(avalista => avalista.CPF).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cpf));
            //RuleFor(avalista => avalista.Email).Must(email => CheckIfEmailIsValid(email)).WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Email));

        }

        public bool CheckIfEmailIsValid(string email)
        {
            if (email.HasValue())
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    return false;
                }
            }
            if (email.IsEmpty())
            {
                return true;
            }
            return email.IsValidEmail();
        }
    }
}
