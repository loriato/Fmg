using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class DocumentoEmpresaVendaService : BaseService
    {
        private DocumentoEmpresaVendaRepository _documentoEmpresaVendaRepository { get; set; }
        private UploadDocumentoEmpresaVendaValidator _uploadDocumentoEmpresaVendaValidator { get; set; }
        private ResponsavelAceiteRegraComissaoService _responsavelAceiteRegraComissaoService { get; set; }
        private ArquivoService _arquivoService { get; set; }
        public DocumentoEmpresaVenda UploadDocumentoEmpresaVenda(DocumentoEmpresaVendaDto upload)
        {
            var bre = new BusinessRuleException();
            var documento = new DocumentoEmpresaVenda();

            var validate = _uploadDocumentoEmpresaVendaValidator.Validate(upload);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            documento.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = upload.IdEmpresaVenda };
            documento.TipoDocumento = new TipoDocumentoEmpresaVenda { Id = upload.IdTipoDocumento };
            documento.Situacao = SituacaoAprovacaoDocumento.Anexado;

            var arquivo = _arquivoService.CreateFile(upload.File, upload.NomeArquivo);

            documento.Arquivo = arquivo;

            _documentoEmpresaVendaRepository.Save(documento);

            return documento;
        }

        public void UploadDocumentosEmpresaVenda(List<DocumentoEmpresaVendaDto> uploads)
        {
            foreach(var upload in uploads)
            {
                UploadDocumentoEmpresaVenda(upload);
            }
        }

        public void ExcluirDocumentoEmpresaVenda(long idDocumento)
        {
            var bre = new BusinessRuleException();
            var documento = _documentoEmpresaVendaRepository.FindById(idDocumento);

            try
            {              

                if (documento.IsEmpty())
                {
                    bre.AddError(GlobalMessages.DocumentoInexistente).Complete();
                }

                bre.ThrowIfHasError();

                //verifica se a EV nao tem procuraçoes anexadas e se sim exclui os responsaveis ativos
                if(documento.TipoDocumento.Nome == "Procuração")
                {
                    if (!_documentoEmpresaVendaRepository.EvTemMaisDeUmaProcuracao(documento.EmpresaVenda.Id))
                    {
                        _responsavelAceiteRegraComissaoService.SuspenderResposaveisAtivos(documento.EmpresaVenda.Id);
                    }
                }

                _documentoEmpresaVendaRepository.Delete(documento);

            }catch(GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(documento.ChaveCandidata()).Complete();
                }
            }
            finally
            {
                bre.ThrowIfHasError();
            }
        }
    }
}
