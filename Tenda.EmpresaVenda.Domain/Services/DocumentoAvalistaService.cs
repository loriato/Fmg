using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Exceptions;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class DocumentoAvalistaService:BaseService
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private AvalistaRepository _avalistaRepository { get; set; }
        private TipoDocumentoAvalistaRepository _tipoDocumentoAvalistaRepository { get; set; }
        private DocumentoAvalistaRepository _documentoAvalistaRepository{ get; set; }
        private ArquivoService _arquivoService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        public MotivoRepository _motivoRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }

        public ParecerDocumentoAvalistaRepository _parecerDocumentoAvalistaRepository { get; set; }


        public void SalvarDocumento(DocumentoAvalistaDTO documento, HttpPostedFileBase file)
        {
            var bre = new BusinessRuleException();

            if (file == null || documento == null ||
                documento.IdPreProposta.IsEmpty() ||
                documento.IdTipoDocumento.IsEmpty())
            {
                bre.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
            }

            if (documento.IdAvalista.IsEmpty())
            {
                bre.AddError(GlobalMessages.MsgAvalistaNaoCadastrado).Complete();
            }

            bre.ThrowIfHasError();

            var preProposta = _prePropostaRepository.FindById(documento.IdPreProposta);
            var avalista = _avalistaRepository.FindById(documento.IdAvalista);
            var tipoDocumentoAvalista = _tipoDocumentoAvalistaRepository.FindById(documento.IdTipoDocumento);

            if (preProposta == null)
            {
                bre.AddError(GlobalMessages.RegistroNaoEncontrado)
                    .WithParams(GlobalMessages.PreProposta, documento.IdPreProposta.ToString()).Complete();
            }

            if (avalista == null)
            {
                bre.AddError(GlobalMessages.RegistroNaoEncontrado)
                    .WithParams(GlobalMessages.Avalista, documento.IdAvalista.ToString()).Complete();
            } 

            if (tipoDocumentoAvalista== null)
            {
                bre.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.TipoDocumentoAvalista,
                    documento.IdTipoDocumento.ToString()).Complete();
            }

            bre.ThrowIfHasError();

            var documentoAvalista = _documentoAvalistaRepository.BuscarDoTipoParaAvalista(documento.IdPreProposta, documento.IdAvalista, documento.IdTipoDocumento);

            if(documentoAvalista == null)
            {
                documentoAvalista = new DocumentoAvalista();
            }

            if (SituacaoAprovacaoDocumentoAvalista.PreAprovado == documentoAvalista.Situacao)
            {
                bre.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                    .WithParams(tipoDocumentoAvalista.Nome, avalista.Nome).Complete();
            }
            bre.ThrowIfHasError();

            var nomeArquivo = string.Format("{0} - {1} - {2}.pdf", preProposta.Codigo,
                    avalista.Nome, tipoDocumentoAvalista.Nome);
            var arquivo = _arquivoService.CreateFile(file, nomeArquivo);

            PreencherMetadadosDePdf(ref arquivo, arquivo.Content);

            documentoAvalista.PreProposta = preProposta;
            documentoAvalista.Avalista = avalista;
            documentoAvalista.Situacao = SituacaoAprovacaoDocumentoAvalista.Anexado;
            documentoAvalista.Avalista.SituacaoAvalista = SituacaoAvalista.DocumentosAnexados;
            documentoAvalista.TipoDocumento = tipoDocumentoAvalista;
            documentoAvalista.Motivo = documento.Motivo;
            documentoAvalista.Arquivo = arquivo;

            _avalistaRepository.Save(documentoAvalista.Avalista);
            _documentoAvalistaRepository.Save(documentoAvalista);

        }

        //FIXME: Deveria estar no SERVICE DE ARQUIVO
        private void PreencherMetadadosDePdf(ref Arquivo arquivo, byte[] bytes)
        {
            var stream = new MemoryStream(bytes);

            try
            {
                var documentoArquivo = PdfReader.Open(stream);
                var objMetadados = documentoArquivo.IsEmpty() ? null : new Metadado
                {
                    Titulo = documentoArquivo.Info.Title,
                    Autor = documentoArquivo.Info.Author,
                    Assunto = documentoArquivo.Info.Subject,
                    CriadoPor = documentoArquivo.Info.Creator,
                    CriadoEm = documentoArquivo.Info.CreationDate.IsEmpty() ? "" : documentoArquivo.Info.CreationDate.ToDateTimeSeconds(),
                    ModificadoEm = documentoArquivo.Info.ModificationDate.IsEmpty() ? "" : documentoArquivo.Info.ModificationDate.ToDateTimeSeconds(),
                    ProduzidoPor = documentoArquivo.Info.Producer,
                    PalavrasChave = documentoArquivo.Info.Keywords,
                    ExtraidoPor = "PdfSharp",
                    Raw = JsonConvert.SerializeObject(documentoArquivo.Info, Formatting.Indented)
                };

                var metadadosSerializados = objMetadados.IsEmpty() ? "" : JsonConvert.SerializeObject(objMetadados, Formatting.Indented);
                arquivo.Metadados = metadadosSerializados;
            }
            catch (Exception e)
            {
                ExceptionUtility.LogException(e);
                arquivo.Metadados = "";
                arquivo.FalhaExtracaoMetadados = true;
            }
        }

        public void Delete(DocumentoAvalista documento)
        {
            var bre = new BusinessRuleException();
            try
            {
                _documentoAvalistaRepository.Delete(documento);
                _arquivoRepository.Delete(documento.Arquivo);
                _session.Transaction.Commit();
                
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    bre.AddError(String.Format(GlobalMessages.ErroExcluirDocumentoPreProposta)).Complete();
                }
                else
                {
                    bre.AddError(String.Format(GlobalMessages.ErroNaoTratado, ex.Message)).Complete();
                }
                bre.ThrowIfHasError();
                _session.Transaction.Rollback();
            }
            finally
            {
                bre.ThrowIfHasError();
            }
        }

        public byte[] ExportarTodosDocumentos(long idPreProposta, long idAvalista)
        {
            var notAllowedChars = new List<string>() { "<", ">", ":", "/", "/", "|", "?", "*", "\"" };

            var documentos = _documentoAvalistaRepository.BuscarDocumentosComArquivosAnexadosPorIdPrePropostaEIdAvalista(idPreProposta,idAvalista);

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
                //if (codigoUf.HasValue() && codigoUf.Contains("REL01"))
                //    normalizedName = normalizedName.Insert(0, "Aprovar - ");

                notAllowedChars.ForEach(reg => normalizedName = normalizedName.Replace(reg, " "));

                var nomeArquivoMetadados = normalizedName.Replace(".pdf", "");

                var memoryStream = new MemoryStream(doc.Arquivo.Content);
                var entry = new ZipEntry(normalizedName);
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

        public void EnviarDocumentosAvalista(DocumentoAvalistaDTO documento)
        {
            var bre = new BusinessRuleException();

            if (documento.IdAvalista.IsEmpty())
            {
                bre.AddError(GlobalMessages.MsgAvalistaNaoCadastrado).Complete();
            }

            if (documento.IdPreProposta.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.RegistroInexistente,GlobalMessages.PreProposta)).Complete();
            }

            bre.ThrowIfHasError();
            
            var doc = _documentoAvalistaRepository.BuscarPOrIdPrePropostaEIdAvalista(documento.IdPreProposta, documento.IdAvalista);
            _session.Evict(doc);

            if (doc == null)
            {
                bre.AddError(GlobalMessages.DocumentoInexistente).Complete();
            }

            bre.ThrowIfHasError();

            doc.Situacao = SituacaoAprovacaoDocumentoAvalista.Enviado;
            doc.Avalista.SituacaoAvalista = SituacaoAvalista.AguardandoAnalise;

            _avalistaRepository.Save(doc.Avalista);
            _documentoAvalistaRepository.Save(doc);

        }

        public void ValidarDocumento(DocumentoAvalistaDTO documento, SituacaoAprovacaoDocumento situacao)
        {
            var documentoReg = _documentoAvalistaRepository.FindById(documento.IdDocumentoAvalista);
            if (documentoReg.HasValue() && documentoReg.Arquivo.IsNull())
            {
                throw new BusinessRuleException(string.Format(GlobalMessages.MsgDocumentoInexistente,documentoReg.TipoDocumento.Nome));
            }
        }

        public void AprovarDocumento(DocumentoAvalistaDTO documentoDto)
        {
            var bre = new BusinessRuleException();
            var prprResult = new AprovarDocumentoAvalistaValidator(_documentoAvalistaRepository).Validate(documentoDto);
            bre.WithFluentValidation(prprResult);
            bre.ThrowIfHasError();

            var documentoReg = _documentoAvalistaRepository.FindById(documentoDto.IdDocumentoAvalista);

            if (documentoDto.Parecer.HasValue())
            {
                var parecer = new ParecerDocumentoAvalista();
                parecer.Parecer = "";
                parecer.DocumentoAvalista = new DocumentoAvalista
                {
                    Id = documentoReg.Id
                };
                _parecerDocumentoAvalistaRepository.Save(parecer);
            }

            documentoReg.Situacao = SituacaoAprovacaoDocumentoAvalista.PreAprovado;
            documentoReg.Avalista.SituacaoAvalista = SituacaoAvalista.AvalistaAprovado;
            documentoDto.NomeTipoDocumento = documentoReg.TipoDocumento.Nome;
            _avalistaRepository.Save(documentoReg.Avalista);
            _documentoAvalistaRepository.Save(documentoReg);
        }

        private void AprovarDocumentosRelacionados(List<DocumentoAvalista> documentos, DocumentoAvalistaDTO documentoDto)
        {
            foreach (var documentoAssociado in documentos)
            {
                documentoDto.IdDocumentoAvalista = documentoAssociado.Id;
                AprovarDocumento(documentoDto);
            }
        }

        private void PendenciarDocumentosRelacionados(List<DocumentoAvalista> documentos, DocumentoAvalistaDTO documentoDto)
        {
            foreach (var documentoAssociado in documentos)
            {
                documentoDto.IdDocumentoAvalista = documentoAssociado.Id;
                PendenciarDocumento(documentoDto);
            }
        }

        public void PendenciarDocumento(DocumentoAvalistaDTO documentoDto)
        {
            var bre = new BusinessRuleException();
            var prprResult = new PendenciaDocumentoAvalistaValidator(_documentoAvalistaRepository).Validate(documentoDto);
            bre.WithFluentValidation(prprResult);
            bre.ThrowIfHasError();

            var documentoReg = _documentoAvalistaRepository.FindById(documentoDto.IdDocumentoAvalista);

            var parecer = new ParecerDocumentoAvalista();
            parecer.Parecer = documentoDto.Parecer;
            parecer.DocumentoAvalista = documentoReg;
            _parecerDocumentoAvalistaRepository.Save(parecer);

            documentoReg.Situacao = SituacaoAprovacaoDocumentoAvalista.Pendente;
            documentoReg.Avalista.SituacaoAvalista = SituacaoAvalista.AvalistaReprovado;
            documentoDto.NomeTipoDocumento = documentoReg.TipoDocumento.Nome;
            _avalistaRepository.Save(documentoReg.Avalista);
            _documentoAvalistaRepository.Save(documentoReg);
        }

        public void AprovarDocumentacao(DocumentoAvalistaDTO documento)
        {
            ValidarDocumento(documento, SituacaoAprovacaoDocumento.Aprovado);
            AprovarDocumento(documento);            
        }

        public void PendenciarDocumentacao(DocumentoAvalistaDTO documento)
        {
            ValidarDocumento(documento, SituacaoAprovacaoDocumento.Pendente);
            PendenciarDocumento(documento);
        }

        public void AprovarAvalista(DocumentoAvalistaDTO documento)
        {
            var bre = new BusinessRuleException();

            var documentoAvalista = _documentoAvalistaRepository.BuscarPOrIdPrePropostaEIdAvalista(documento.IdPreProposta,documento.IdAvalista);

            var result = new AprovarDocumentacaoAvalistaValidator(_documentoAvalistaRepository).Validate(documentoAvalista);
            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();

        }

        public void PendenciarAvalista(DocumentoAvalistaDTO documento)
        {
            var bre = new BusinessRuleException();

            var documentoAvalista = _documentoAvalistaRepository.BuscarPOrIdPrePropostaEIdAvalista(documento.IdPreProposta, documento.IdAvalista);

            var result = new PendenciarDocumentacaoAvalistaValidator(_documentoAvalistaRepository).Validate(documentoAvalista);
            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();
            var preproposta = _prePropostaRepository.FindById(documento.IdPreProposta);
            EnviarNotificacaoPortal(preproposta);
            EnviarNotificacaoAdm(preproposta);
        }

        public void EnviarNotificacaoPortal(PreProposta preproposta)
        {
            var notificacao = new Notificacao
            {
                Titulo = string.Format(GlobalMessages.AvalistaReprovado_Titulo, preproposta.Codigo, preproposta.Cliente.NomeCompleto),
                Conteudo = GlobalMessages.AvalistaReprovado_Corpo,
                Usuario = preproposta.Corretor.Usuario,
                EmpresaVenda = preproposta.EmpresaVenda,
                TipoNotificacao = TipoNotificacao.Comum,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);
        }

        public void EnviarNotificacaoAdm(PreProposta preproposta)
        {
            var notificacao = new Notificacao
            {
                Titulo = string.Format(GlobalMessages.AvalistaReprovado_Titulo, preproposta.Codigo, preproposta.Cliente.NomeCompleto),
                Conteudo = GlobalMessages.AvalistaReprovado_Corpo_ADM,
                Usuario = preproposta.Viabilizador,
                EmpresaVenda = preproposta.EmpresaVenda,
                TipoNotificacao = TipoNotificacao.Comum,
                DestinoNotificacao = DestinoNotificacao.Adm,
            };

            _notificacaoRepository.Save(notificacao);
        }
    }
}
