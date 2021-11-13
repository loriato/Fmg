using Europa.Resources;
using FluentValidation;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PendenciaDocumentoAvalistaValidator : AbstractValidator<DocumentoAvalistaDTO>
    {
        public DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }

        public PendenciaDocumentoAvalistaValidator(DocumentoAvalistaRepository documentoAvalistaRepository)
        {
            _documentoAvalistaRepository = documentoAvalistaRepository;

            RuleFor(doc => doc.Parecer).NotEmpty().WithName("Parecer").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Parecer));
        }
    }
}
