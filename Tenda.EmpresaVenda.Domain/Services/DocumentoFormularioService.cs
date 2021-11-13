using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class DocumentoFormularioService : BaseService
    {
        private ArquivoService _arquivoService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private DocumentoFormularioDTOValidator _documentoFormularioDTOValidator { get; set; }
        private DocumentoFormularioRepository _documentoFomrularioRepository { get; set; }
        public DocumentoFormulario UploadDocumento(DocumentoFormularioDTO documento)
        {
            var bre = new BusinessRuleException();

            var validate = _documentoFormularioDTOValidator.Validate(documento);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            var arquivo = _arquivoService.CreateFile(documento.Formulario);

            var formulario = new DocumentoFormulario();
            formulario.PreProposta = new PreProposta { Id = documento.IdPreProposta };
            formulario.Responsavel = new UsuarioPortal { Id = documento.IdResponsavel };
            formulario.Arquivo = arquivo;
            formulario.Situacao = SituacaoAprovacaoDocumento.Anexado;

            _documentoFomrularioRepository.Save(formulario);

            return formulario;
        }
        public DocumentoFormulario ExcluirDocumento(DocumentoFormularioDTO formulario)
        {
            var bre = new BusinessRuleException();

            var documento = _documentoFomrularioRepository.FindById(formulario.Id);

            if (documento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInexistente, formulario.NomeDocumento)).Complete();
                bre.ThrowIfHasError();
            }

            try
            {
                _documentoFomrularioRepository.Delete(documento);
                _arquivoRepository.Delete(documento.Arquivo);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(documento.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();

            return documento;
        }
        public byte[] BaixarFormularios(long idPreProposta)
        {
            var documentos = _documentoFomrularioRepository.Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .ToList();

            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (var doc in documentos)
            {                
                var nomeArquivoMetadados = doc.Arquivo.Nome;

                var memoryStream = new MemoryStream(doc.Arquivo.Content);
                var entry = new ZipEntry(nomeArquivoMetadados);
                entry.IsUnicodeText = true;
                entry.DateTime = DateTime.Now;
                zipOutputStream.PutNextEntry(entry);
                StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
                zipOutputStream.CloseEntry();

                if (doc.Arquivo.Metadados.IsEmpty() == false)
                {
                    var ms = new MemoryStream();
                    var metadadosStream = new StreamWriter(ms, new UnicodeEncoding());
                    metadadosStream.Write(doc.Arquivo.Metadados);
                    metadadosStream.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    var metadadosEntry = new ZipEntry($"{nomeArquivoMetadados}_metadados.txt");
                    metadadosEntry.IsUnicodeText = true;
                    metadadosEntry.DateTime = DateTime.Now;
                    zipOutputStream.PutNextEntry(metadadosEntry);
                    StreamUtils.Copy(ms, zipOutputStream, new byte[4096]);
                    zipOutputStream.CloseEntry();

                    metadadosStream.Dispose();
                }
            }
            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }
    }
}
