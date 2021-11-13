using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class UploadDocumentoEmpresaVendaValidator:AbstractValidator<DocumentoEmpresaVendaDto>
    {
        public UploadDocumentoEmpresaVendaValidator()
        {
            RuleFor(x => x.File).NotEmpty().WithMessage(GlobalMessages.Arquivo + " " + GlobalMessages.Obrigatorio);
            RuleFor(x => x.IdTipoDocumento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoDeDocumento));
            RuleFor(x => x.IdEmpresaVenda).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda));

        }
    }
}
