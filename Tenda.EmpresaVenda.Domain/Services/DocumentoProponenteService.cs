using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators.DocumentoProponente;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class DocumentoProponenteService : BaseService
    {
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        public ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }
        public MotivoRepository _motivoRepository { get; set; }

        public DocumentoProponente AprovarDocumento(DocumentoProponenteDTO documentoDto)
        {
            var bre = new BusinessRuleException();
            var prprResult = new AprovacaoDocumentoProponenteValidator(_documentoProponenteRepository).Validate(documentoDto);
            bre.WithFluentValidation(prprResult);
            bre.ThrowIfHasError();

            var documentoReg = _documentoProponenteRepository.FindById(documentoDto.IdDocumentoProponente);

            if (documentoDto.Parecer.HasValue())
            {
                var parecer = new ParecerDocumentoProponente();
                parecer.Parecer = documentoDto.Parecer;
                parecer.DocumentoProponente = documentoReg;
                _parecerDocumentoProponenteRepository.Save(parecer);
            }

            documentoReg.Situacao = SituacaoAprovacaoDocumento.Aprovado;
            documentoReg.DataExpiracao = documentoDto.DataExpiracao;
            _documentoProponenteRepository.Save(documentoReg);

            return documentoReg;
        }

        public void AtualizarDocumentoRelacionado(DocumentoProponenteDTO documentoDto, SituacaoAprovacaoDocumento situacao)
        {
            var documentoOrigem = _documentoProponenteRepository.FindById(documentoDto.IdDocumentoProponente);
            var motivo = _motivoRepository.FindById(ProjectProperties.MotivoDocumentoAnexadoOutroProponente);
            var documentos = _documentoProponenteRepository.DocumentosAssociadosDeOutroProponente(documentoOrigem.PreProposta.Id,
                documentoOrigem.Proponente.Id, documentoOrigem.TipoDocumento.Id, documentoOrigem.Id,motivo.Descricao);


            if (situacao == SituacaoAprovacaoDocumento.Aprovado)
            {
                AprovarDocumentosRelacionados(documentos, documentoDto);
            }
            else
            {
                PendenciarDocumentosRelacionados(documentos,documentoDto);
            }
        }

        private void PendenciarDocumentosRelacionados(List<DocumentoProponente> documentos, DocumentoProponenteDTO documentoDto)
        {
            foreach (var documentoAssociado in documentos)
            {
                documentoDto.IdDocumentoProponente = documentoAssociado.Id;
                PendenciarDocumento(documentoDto);
            }
        }

        private void AprovarDocumentosRelacionados(List<DocumentoProponente> documentos, DocumentoProponenteDTO documentoDto)
        {
            foreach (var documentoAssociado in documentos)
            {
                documentoDto.IdDocumentoProponente = documentoAssociado.Id;
                AprovarDocumento(documentoDto);
            }
        }

        public DocumentoProponente PendenciarDocumento(DocumentoProponenteDTO documentoDto)
        {
            var bre = new BusinessRuleException();
            var prprResult = new PendenciaDocumentoProponenteValidator(_documentoProponenteRepository).Validate(documentoDto);
            bre.WithFluentValidation(prprResult);
            bre.ThrowIfHasError();

            var documentoReg = _documentoProponenteRepository.FindById(documentoDto.IdDocumentoProponente);

            var parecer = new ParecerDocumentoProponente();
            parecer.Parecer = documentoDto.Parecer;
            parecer.DocumentoProponente = documentoReg;
            _parecerDocumentoProponenteRepository.Save(parecer);

            documentoReg.Situacao = SituacaoAprovacaoDocumento.Pendente;
            documentoReg.DataExpiracao = documentoDto.DataExpiracao;
            _documentoProponenteRepository.Save(documentoReg);
            return documentoReg;
        }

        public byte[] ExportarTodosDocumentos(long idPreProposta, string codigoUf)
        {
            var notAllowedChars = new List<string>() { "<", ">", ":", "/", "/", "|", "?", "*","\"" };

            var documentos = _documentoProponenteRepository.BuscarDocumentosComArquivosAnexadosPorIdPreProposta(idPreProposta);

            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (var doc in documentos)
            {
                // Removendo caracteres inv�lidos para windows
                var normalizedName = doc.Arquivo.Nome;

                var name = normalizedName.ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                normalizedName = name[1] + " - " + name[2];

                if (normalizedName.Contains("("))
                    normalizedName = normalizedName.ToString().Split(new string[] { " (" }, StringSplitOptions.None)[0] + ".pdf";
                if (normalizedName.Contains("N�o"))
                    normalizedName = normalizedName.ToString().Split(new string[] { " - N�o" }, StringSplitOptions.None)[0] + ".pdf";
                if (!normalizedName.Contains(".pdf"))
                    normalizedName = normalizedName + ".pdf";
                if (codigoUf.HasValue() && codigoUf.Contains("REL01"))
                    normalizedName = normalizedName.Insert(0, "Aprovar - ");

                notAllowedChars.ForEach(reg => normalizedName = normalizedName.Replace(reg, " "));

                var nomeArquivoMetadados = normalizedName.Replace(".pdf", "");

                var memoryStream = new MemoryStream(doc.Arquivo.Content);
                var entry = new ZipEntry(normalizedName);
                entry.IsUnicodeText = true;
                entry.DateTime = DateTime.Now;
                zipOutputStream.PutNextEntry(entry);
                StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
                zipOutputStream.CloseEntry();

                if(doc.Arquivo.Metadados.IsEmpty() == false)
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

        public byte[] ExportarDocumento(long Id)
        {
            var notAllowedChars = new List<string>() { "<", ">", ":", "/", "/", "|", "?", "*", "\"" };

            var documento = _documentoProponenteRepository.FindById(Id);

            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression


            // Removendo caracteres inv�lidos para windows
            var normalizedName = documento.Arquivo.Nome;

            var name = normalizedName.ToString().Split(new string[] { " - " }, StringSplitOptions.None);

            normalizedName = name[1] + " - " + name[2];

            if (normalizedName.Contains("("))
                normalizedName = normalizedName.ToString().Split(new string[] { " (" }, StringSplitOptions.None)[0] + ".pdf";
            if (normalizedName.Contains("N�o"))
                normalizedName = normalizedName.ToString().Split(new string[] { " - N�o" }, StringSplitOptions.None)[0] + ".pdf";
            if (!normalizedName.Contains(".pdf"))
                normalizedName = normalizedName + ".pdf";

            notAllowedChars.ForEach(reg => normalizedName = normalizedName.Replace(reg, " "));

            var nomeArquivoMetadados = normalizedName.Replace(".pdf", "");

            var memoryStream = new MemoryStream(documento.Arquivo.Content);
            var entry = new ZipEntry(normalizedName);
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            if (documento.Arquivo.Metadados.IsEmpty() == false)
            {
                var ms = new MemoryStream();
                var metadadosStream = new StreamWriter(ms, new UnicodeEncoding());
                metadadosStream.Write(documento.Arquivo.Metadados);
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
            
            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }

        public void ValidarDocumento(DocumentoProponenteDTO documento,SituacaoAprovacaoDocumento situacao)
        {
            var documentoReg = _documentoProponenteRepository.FindById(documento.IdDocumentoProponente);
            var motivo = _motivoRepository.FindById(ProjectProperties.MotivoDocumentoAnexadoOutroProponente);
            if (documentoReg.HasValue() && documentoReg.Arquivo.IsNull() && documentoReg.Motivo.Equals(motivo.Descricao))
            {
                throw new BusinessRuleException(string.Format(GlobalMessages.MsgDocumentoAnexadoOutroProponente,
                                                              documentoReg.TipoDocumento.Nome,
                                                              situacao == SituacaoAprovacaoDocumento.Aprovado ? GlobalMessages.Aprovado : GlobalMessages.Pendenciado));
            }
        }
    }
}