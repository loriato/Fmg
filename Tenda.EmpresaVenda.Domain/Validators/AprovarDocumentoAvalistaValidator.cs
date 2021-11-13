using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AprovarDocumentoAvalistaValidator : AbstractValidator<DocumentoAvalistaDTO>
    {
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }

        public AprovarDocumentoAvalistaValidator(DocumentoAvalistaRepository documentoAvalistaRepository)
        {
            _documentoAvalistaRepository = documentoAvalistaRepository;

            RuleFor(doc => doc).Must(x=>x.ExisteDocumento).WithMessage(x=>string.Format(GlobalMessages.CampoObrigatorio, x.NomeTipoDocumento));
        }

        public bool CheckExpirationDate(DocumentoAvalistaDTO doc)
        {
            var documentoReg = _documentoAvalistaRepository.FindById(doc.IdDocumentoAvalista);
            if (documentoReg.HasValue())
            {
                if (doc.DataExpiracao.HasValue() && doc.DataExpiracao.Date >= DateTime.Now.Date)
                {
                    return true;
                }

                return false;
            }
            return true;
        }
    }
}
