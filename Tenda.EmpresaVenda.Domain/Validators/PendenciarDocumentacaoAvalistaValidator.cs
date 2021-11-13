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
    public class PendenciarDocumentacaoAvalistaValidator : AbstractValidator<DocumentoAvalista>
    {
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }
        public PendenciarDocumentacaoAvalistaValidator(DocumentoAvalistaRepository documentoAvalistaRepository)
        {
            _documentoAvalistaRepository = documentoAvalistaRepository;

            RuleFor(x => x).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DocumentoAvalista));
            RuleFor(x => x.PreProposta).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta));
            RuleFor(x => x.Avalista).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Avalista));
            RuleFor(x => x.Arquivo).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Arquivo));
            RuleFor(x => x.TipoDocumento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoDocumentoAvalista));

            RuleFor(x => x).Must(x => VerificarPendenciado(x)).WithMessage(GlobalMessages.MsgDocumentoPendente);
        }

        public bool VerificarPendenciado(DocumentoAvalista documento)
        {
            var lista = _documentoAvalistaRepository.BuscarDocumentosParaAnalise(documento.PreProposta.Id, documento.Avalista.Id);
            foreach (var doc in lista)
            {
                if (doc.Situacao == SituacaoAprovacaoDocumentoAvalista.Pendente)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
