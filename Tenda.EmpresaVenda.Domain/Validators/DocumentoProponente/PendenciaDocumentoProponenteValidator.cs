using System;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.DocumentoProponente
{
    public class PendenciaDocumentoProponenteValidator : AbstractValidator<DocumentoProponenteDTO>
    {
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }

        public PendenciaDocumentoProponenteValidator(DocumentoProponenteRepository documentoProponenteRepository)
        {
            _documentoProponenteRepository = documentoProponenteRepository;
          
            RuleFor(doc => doc.Parecer).NotEmpty().WithName("Parecer").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Parecer));
        }
    }
}