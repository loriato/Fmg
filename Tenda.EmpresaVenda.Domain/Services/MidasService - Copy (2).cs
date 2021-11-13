using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Midas;
using Tenda.EmpresaVenda.Domain.Repository;


namespace Tenda.EmpresaVenda.Domain.Services
{
    public class MidasService
    {
        private OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }
        private PagamentoRepository _pagamentoRepository { get; set; }
        private NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository{ get; set; }
        private DocumentoMidasRepository _documentoMidasRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        public NotaFiscalPagamentoService _notaFiscalPagamentoService { get; set; }
        public string RegistrarOcorrencia(OcorrenciaRequestDto requestDto)
        {
            var apiException = new ApiException();
            try
            {
                if(requestDto == null)
                {
                    apiException.AddError("DTO Inválido.");
                }
                apiException.ThrowIfHasError();
                var LastOccurrence = _ocorrenciasMidasRepository.findByIdOcorrencia(requestDto.OccurrenceId);
                if (LastOccurrence != null)
                {
                    apiException.AddError("Ocorrência já inserida.");
                }
                apiException.ThrowIfHasError();

                if (requestDto.OccurrenceId == 0)
                {
                    apiException.AddError("OccurrenceId Nulo.");
                }
                if (String.IsNullOrEmpty(requestDto.TaxIdProvider))
                {
                    apiException.AddError("TaxIdProvider Nulo.");
                }
                if (String.IsNullOrEmpty(requestDto.TaxIdTaker))
                {
                    apiException.AddError("TaxIdTaker Nulo.");
                }
                if (String.IsNullOrEmpty(requestDto.Document.Number))
                {
                    apiException.AddError("Document Number Nulo.");
                }
                apiException.ThrowIfHasError();

                var ocorrencia = new OcorrenciasMidas();
                #region populandoOcorrencia
                ocorrencia.OccurenceId = requestDto.OccurrenceId;
                ocorrencia.TaxIdProvider = requestDto.TaxIdProvider;
                ocorrencia.TaxIdTaker = requestDto.TaxIdTaker;
                ocorrencia.Document.DocumentType = requestDto.Document.DocumentType ?? "";
                ocorrencia.Document.Number = requestDto.Document.Number;
                ocorrencia.Document.AccessKey = requestDto.Document.AccessKey ?? "";
                ocorrencia.Document.Serie = requestDto.Document.Serie ?? "";
                ocorrencia.Document.VerificationCode = requestDto.Document.VerificationCode ?? "";
                ocorrencia.Document.DateIssue = requestDto.Document.DateIssue; 
                ocorrencia.Document.MunicipalCode = requestDto.Document.MunicipalCode ?? "";
                ocorrencia.Document.ServiceValue = requestDto.Document.ServiceValue;
                ocorrencia.Document.TotalValue = requestDto.Document.TotalValue;
                ocorrencia.Document.DueDate = requestDto.Document.DueDate;
                #endregion
                if (_empresaVendaRepository.CheckIfExistsCNPJ(ocorrencia.TaxIdProvider))
                {
                    ocorrencia.CanBeCommissioned = true;
                }

                _documentoMidasRepository.Save(ocorrencia.Document);
                _ocorrenciasMidasRepository.Save(ocorrencia);
                return "Ocorrência Inserida!";
            }
            catch(Exception ex)
            {
                apiException.AddError(ex.Message);
                apiException.ThrowIfHasError();
                return "Ocorrência Não Inserida!";

            }


        }


        public string[] AutorizarPagamento(long idOcorrencia)
        {
            var apiException = new ApiException();
            string[] res = new string[2];
            try
            {
                OcorrenciasMidas ocorrencia = _ocorrenciasMidasRepository.findByIdOcorrencia(idOcorrencia);
                if(!ocorrencia.IsEmpty())
                {
                    if(ocorrencia.CanBeCommissioned == true)
                    {

                         NotaFiscalPagamentoOcorrencia nfpo = _notaFiscalPagamentoOcorrenciaRepository.FindComissionedByOccurrenceId(idOcorrencia);


                        if (!nfpo.IsEmpty())
                        {
                            res[0] = GlobalMessages.Commissioned;
                            res[1] = GlobalMessages.OcorrenciaCommissioned;
                        }
                        else
                        {
                            res[0] = GlobalMessages.None;
                            res[1] = GlobalMessages.OcorrenciaNone;
                        }
                    }
                    else
                    {
                        res[0] = GlobalMessages.NotCommissioned;
                        res[1] = GlobalMessages.OcorrenciaNotCommissioned;
                    }

                }
                else
                {
                    res[0] = "";
                    res[1] = GlobalMessages.OcorrenciaNaoCadastrada;
                }
            }
            catch (Exception ex)
            {
                apiException.AddError(ex.Message);
                apiException.ThrowIfHasError();
            }
            return res;
        }


        public bool ProvisionarPagamento(ProvisionarPagamentoRequestDto requestDto) 
        {
            var apiException = new ApiException();
            try
            {
                NotaFiscalPagamentoOcorrencia nfpo = _notaFiscalPagamentoOcorrenciaRepository.FindComissionedByOccurrenceId(requestDto.OccurrenceId);
                if (nfpo != null)
                {
                    List<Pagamento> pagamentos = _pagamentoRepository.BuscarPorIdNotaFiscalPagamento(nfpo.NotaFiscalPagamento.Id);
                    NotaFiscalPagamento nota = _notaFiscalPagamentoRepository.FindById(nfpo.NotaFiscalPagamento.Id);
                    
                    if (pagamentos != null && pagamentos.Count() > 0)
                    {
                        foreach(var pagamento in pagamentos)
                        {
                            pagamento.DataPrevisaoPagamento = requestDto.Date;
                            _pagamentoRepository.Save(pagamento);
                        }
                        if (nota.HasValue())
                        {
                            _notaFiscalPagamentoService.MudarSituacao(nota, SituacaoNotaFiscal.Aprovado);
                        }
                    }
                    else
                    {
                        apiException.AddError("Pagamento não identificado! ");
                        apiException.ThrowIfHasError();
                    }
                }
                else
                {
                    apiException.AddError("Nota Fiscal não identificada ou não aprovada para essa ocorrência! ");
                    apiException.ThrowIfHasError();
                }

            }
            catch (Exception ex)
            {
                apiException.AddError(ex.Message);
                apiException.ThrowIfHasError();
            }

            return true;
        }

    }
}
