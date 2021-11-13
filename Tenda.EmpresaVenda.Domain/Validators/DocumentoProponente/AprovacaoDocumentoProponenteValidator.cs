using System;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.DocumentoProponente
{
    public class AprovacaoDocumentoProponenteValidator : AbstractValidator<DocumentoProponenteDTO>
    {
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }

        public AprovacaoDocumentoProponenteValidator(DocumentoProponenteRepository documentoProponenteRepository)
        {
            _documentoProponenteRepository = documentoProponenteRepository ;

            RuleFor(doc => doc).Must(CheckExpirationDate).When(doc => doc.ExisteDocumento).WithName("DataExpiracao").WithMessage(string.Format(GlobalMessages.CampoObrigatorioEFuturo, GlobalMessages.DataVencimento));
        }

        public bool CheckExpirationDate(DocumentoProponenteDTO doc)
        {
            var documentoReg = _documentoProponenteRepository.FindById(doc.IdDocumentoProponente);

            if (documentoReg.HasValue() && 
                ProjectProperties.IdsDocumentosProponenteComDataExpiracaoObrigatoria.Contains(documentoReg.TipoDocumento.Id)&&
                doc.DataExpiracao.IsEmpty())
            {
                return false;
            }

            if (documentoReg.HasValue() &&
                ProjectProperties.IdsDocumentosProponenteComDataExpiracaoObrigatoria.Contains(documentoReg.TipoDocumento.Id) &&
                doc.DataExpiracao.Value.Date <= DateTime.Now.Date)
            {
                return false;
            }

            if(documentoReg.HasValue()&&
                doc.DataExpiracao.HasValue()&&
                doc.DataExpiracao.Value.Date <= DateTime.Now.Date)
            {
                return false;
            }
                        
            return true;
        }
    }
}