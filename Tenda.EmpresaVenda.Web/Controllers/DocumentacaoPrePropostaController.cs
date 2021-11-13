using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class DocumentacaoPrePropostaController : BaseController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private ProponenteRepository _proponenteRepository { get; set; }
        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private DocumentoProponenteService _documentoProponenteService { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }
        private ConsolidadoPrePropostaRepository _consolidadoPrePropostaRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private DocumentoFormularioRepository _documentoFormularioRepository { get; set; }

        [BaseAuthorize("EVS12", "VisualizarDocumento")]
        public ActionResult Index(long? id)
        {
            var viewModel = new DocumentacaoPrePropostaViewModel();
            if (id.IsEmpty())
            {
                return View(viewModel);
            }
            var preProposta = _prePropostaRepository.FindById(id.Value);
            if (preProposta.IsEmpty())
            {
                return View(viewModel);
            }

            viewModel.PrePropostaEmAnalise = preProposta.SituacaoProposta.HasValue() &&
                                             (preProposta.SituacaoProposta.Value == SituacaoProposta.EmAnaliseSimplificada ||
                                             preProposta.SituacaoProposta.Value == SituacaoProposta.EmAnaliseCompleta);
            viewModel.IdPreProposta = preProposta.Id;
            viewModel.Codigo = preProposta.Codigo;
            viewModel.IdCliente = preProposta.Cliente.Id;
            viewModel.Cliente = preProposta.Cliente.NomeCompleto;
            viewModel.DataElaboracao = preProposta.DataElaboracao;
            viewModel.Status = _viewPrePropostaRepository.Queryable()
                                                 .Where(x => x.Id == id.Value).Select(reg => reg.SituacaoPrePropostaSuatEvs).SingleOrDefault();
            viewModel.SituacaoPreProposta = preProposta.SituacaoProposta.Value;
            viewModel.EmpresaDeVenda = preProposta.EmpresaVenda.NomeFantasia;
            viewModel.PontoDeVenda = preProposta.PontoVenda.Nome;
            viewModel.Corretor = preProposta.Corretor.Nome;
            viewModel.EmailCorretor = preProposta.Corretor.Email;
            viewModel.TelefoneCorretor = preProposta.Corretor.Telefone;
            viewModel.AgenteViabilizador = preProposta.Elaborador.Nome;
            viewModel.BreveLancamento = preProposta.BreveLancamento.HasValue() ? preProposta.BreveLancamento.ChaveCandidata() : String.Empty;
            viewModel.Endereco = preProposta.BreveLancamento.HasValue() ? _enderecoBreveLancamentoRepository.FindByBreveLancamento(preProposta.BreveLancamento.Id).ChaveCandidata() : String.Empty;
            viewModel.DocCompleta = preProposta.DocCompleta;
            viewModel.ClienteCotista = preProposta.ClienteCotista;
            viewModel.FatorSocial = preProposta.FatorSocial;
            viewModel.TotalDetalhamento = preProposta.TotalDetalhamentoFinanceiro;
            viewModel.TotalITBI = preProposta.TotalItbiEmolumento;
            viewModel.Total = preProposta.Valor;
            viewModel.Observacao = preProposta.Observacao;
            viewModel.PontoDeVenda = preProposta.PontoVenda.Nome;
            viewModel.Proponentes = ProponentesPreProposta(preProposta.Id);
            viewModel.NomeTorre = preProposta.NomeTorre;
            viewModel.ObservacaoTorre = preProposta.ObservacaoTorre;
            viewModel.Viabilizador = preProposta?.Viabilizador?.Nome;
            viewModel.ParcelaSolicitada = preProposta.ParcelaSolicitada;
            viewModel.FaixaEv = preProposta.FaixaEv;

            if (preProposta.UltimoCCA.HasValue())
            {
                //viewModel.NomeCCA = preProposta.UltimoCCA.GrupoCCA.Descricao;
                viewModel.NomeCCA = preProposta.UltimoCCA;
            }

            viewModel.PossuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

            DocumentosProponentes(viewModel, preProposta.Id);

            viewModel.PreProposta.SituacaoProposta = preProposta.SituacaoProposta;
            viewModel.PreProposta.StatusSicaqPrevio = preProposta.StatusSicaqPrevio;
            viewModel.PreProposta.ParcelaAprovadaPrevio = preProposta.ParcelaAprovadaPrevio;
            viewModel.PreProposta.DataSicaqPrevio = preProposta.DataSicaqPrevio;
            viewModel.PreProposta.FaixaUmMeioPrevio = preProposta.FaixaUmMeioPrevio;
            viewModel.PreProposta.JustificativaReenvio = preProposta.JustificativaReenvio;
            return View(viewModel);
        }

        [BaseAuthorize("EVS12", "AprovarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarDocumento(DocumentoProponenteDTO documento)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _documentoProponenteService.ValidarDocumento(documento,SituacaoAprovacaoDocumento.Aprovado);
                var docReg = _documentoProponenteService.AprovarDocumento(documento);
                _documentoProponenteService.AtualizarDocumentoRelacionado(documento,SituacaoAprovacaoDocumento.Aprovado);

                var conReg = _consolidadoPrePropostaRepository.FindById(docReg.PreProposta.Id);
                if (conReg.PendenciasParecer.HasValue())
                {
                    var name = conReg.PendenciasParecer;
                    name = name.Replace(conReg.PreProposta.Cliente.NomeCompleto + " | " + docReg.TipoDocumento.Nome + " : " + documento.Parecer + "\r\n", "");
                    conReg.PendenciasParecer = name;
                    _consolidadoPrePropostaRepository.Save(conReg);
                }

                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    docReg.TipoDocumento.Nome, GlobalMessages.Aprovado.ToLower()));
                jsonResponse.Objeto = new DocumentoProponente
                {
                    Id = docReg.Id,
                    Motivo = docReg.Motivo,
                    Situacao = docReg.Situacao,
                    DataExpiracao = docReg.DataExpiracao,
                    TipoDocumento = new TipoDocumento
                    {
                        Id = docReg.TipoDocumento.Id,
                        Nome = docReg.TipoDocumento.Nome
                    },
                    Proponente = new Proponente
                    {
                        Id = docReg.Proponente.Id,
                        Cliente = new Cliente
                        {
                            Id = docReg.Proponente.Cliente.Id,
                            NomeCompleto = docReg.Proponente.Cliente.NomeCompleto
                        }
                    }
                };
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS12", "PendenciarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult PendenciarDocumento(DocumentoProponenteDTO documento)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _documentoProponenteService.ValidarDocumento(documento,SituacaoAprovacaoDocumento.Pendente);
                var docReg = _documentoProponenteService.PendenciarDocumento(documento);
                _documentoProponenteService.AtualizarDocumentoRelacionado(documento,SituacaoAprovacaoDocumento.Pendente);

                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    docReg.TipoDocumento.Nome, GlobalMessages.Pendenciado.ToLower()));
                jsonResponse.Objeto = new DocumentoProponente
                {
                    Id = docReg.Id,
                    Motivo = docReg.Motivo,
                    Situacao = docReg.Situacao,
                    DataExpiracao = docReg.DataExpiracao,
                    TipoDocumento = new TipoDocumento
                    {
                        Id = docReg.TipoDocumento.Id,
                        Nome = docReg.TipoDocumento.Nome
                    },
                    Proponente = new Proponente
                    {
                        Id = docReg.Proponente.Id,
                        Cliente = new Cliente
                        {
                            Id = docReg.Proponente.Cliente.Id,
                            NomeCompleto = docReg.Proponente.Cliente.NomeCompleto
                        }
                    }
                };
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS12", "BaixarTodosDocumentos")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarTodosDocumentos(long idPreProposta, string codigoUf)
        {
            byte[] file = _documentoProponenteService.ExportarTodosDocumentos(idPreProposta, codigoUf);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"{nomeArquivo}_Completo_{date}.zip");
        }

        private List<Proponente> ProponentesPreProposta(long idPreProposta)
        {
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(idPreProposta);
            return proponentes.Select(x => new Proponente
            {
                Id = x.Id,
                Cliente = new Cliente
                {
                    Id = x.Cliente.Id,
                    NomeCompleto = x.Cliente.NomeCompleto
                }
            }).ToList();
        }

        private void DocumentosProponentes(DocumentacaoPrePropostaViewModel viewModel, long idPreProposta)
        {
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(idPreProposta);
            var listaDocumentos = new List<DocumentoProponente>();
            var listaArquivos = new Dictionary<string, ArquivoUrlDTO>();
            string webRoot = GetWebAppRoot();

            foreach (var proponente in proponentes)
            {
                var documentos =
                    _documentoProponenteRepository.BuscarDocumentosPorIdPrePropostaIdProponente(idPreProposta, proponente.Id)
                    .Where(x => x.Situacao != SituacaoAprovacaoDocumento.Pendente).Where(x => x.Situacao != SituacaoAprovacaoDocumento.Aprovado);
                listaDocumentos.AddRange(documentos.OrderBy(x => x.TipoDocumento.Nome).ToList());
                foreach (var documento in documentos)
                {
                    if (documento.Arquivo.HasValue())
                    {
                        string metadados = "";
                        if(documento.Arquivo.Metadados.IsEmpty() == false)
                        {
                            var objMetadados = JsonConvert.DeserializeObject<Metadado>(documento.Arquivo.Metadados);
                            var objMetadadosFiltrado = new 
                            {
                                Titulo = objMetadados.Titulo,
                                Autor = objMetadados.Autor,
                                Assunto = objMetadados.Assunto,
                                CriadoPor = objMetadados.CriadoPor,
                                CriadoEm = objMetadados.CriadoEm,
                                ModificadoEm = objMetadados.ModificadoEm,
                                ProduzidoPor = objMetadados.ProduzidoPor,
                                PalavrasChave = objMetadados.PalavrasChave
                            };

                            metadados = JsonConvert.SerializeObject(objMetadadosFiltrado, Formatting.Indented);
                        }
                        
                        string filename = _staticResourceService.LoadResource(documento.Arquivo.Id);
                        listaArquivos.Add(documento.Id.ToString(), new ArquivoUrlDTO
                        {
                            Url = _staticResourceService.CreateUrl(webRoot, filename),
                            UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename),
                            FileExtension = documento.Arquivo.FileExtension,
                            ContentType = documento.Arquivo.ContentType,
                            Metadados = metadados
                        });
                    }
                }
            }

            viewModel.Arquivos = listaArquivos;
            viewModel.Documentos = listaDocumentos.Select(x => new DocumentoProponente
            {
                Id = x.Id,
                Proponente = new Proponente
                {
                    Id = x.Proponente.Id,
                    Cliente = new Cliente
                    {
                        Id = x.Proponente.Cliente.Id,
                        NomeCompleto = x.Proponente.Cliente.NomeCompleto
                    }
                },
                TipoDocumento = x.TipoDocumento,
                Motivo = x.Motivo,
                Situacao = x.Situacao,
                DataExpiracao = x.DataExpiracao,
            }).ToList();

            foreach (var documento in viewModel.Documentos)
            {
                var parecer = _parecerDocumentoProponenteRepository.BuscarUltimoParecerDocumento(documento.Id);
                if (parecer.HasValue())
                {
                    viewModel.Pareceres.Add(new ParecerDocumentoProponente
                    {
                        Id = parecer.Id,
                        DocumentoProponente = new DocumentoProponente
                        {
                            Id = parecer.DocumentoProponente.Id
                        },
                        Parecer = parecer.Parecer
                    });
                }
            }
        }


        [BaseAuthorize("EVS12", "VisualizarDocumento")]
        public ActionResult AtualizarViewModel(long idPreProposta, bool somenteSemAnalise)
        {
            var json = new JsonResponse();
            var viewModel = new DocumentacaoPrePropostaViewModel();
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(idPreProposta);
            var listaDocumentos = new List<DocumentoProponente>();
            var listaArquivos = new Dictionary<string, ArquivoUrlDTO>();
            string webRoot = GetWebAppRoot();

            foreach (var proponente in proponentes)
            {
                var documentos =
                    _documentoProponenteRepository.BuscarDocumentosPorIdPrePropostaIdProponente(idPreProposta, proponente.Id);
                if (somenteSemAnalise)
                {
                    documentos = documentos.Where(x => x.Situacao != SituacaoAprovacaoDocumento.Pendente).Where(x => x.Situacao != SituacaoAprovacaoDocumento.Aprovado).ToList();
                }
                listaDocumentos.AddRange(documentos.OrderBy(x => x.TipoDocumento.Nome).ToList());
                foreach (var documento in documentos)
                {
                    if (documento.Arquivo.HasValue())
                    {
                        string filename = _staticResourceService.LoadResource(documento.Arquivo.Id);
                        listaArquivos.Add(documento.Id.ToString(), new ArquivoUrlDTO
                        {
                            Url = _staticResourceService.CreateUrl(webRoot, filename),
                            UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename),
                            FileExtension = documento.Arquivo.FileExtension,
                            ContentType = documento.Arquivo.ContentType
                        });
                    }
                }
            }

            viewModel.Arquivos = listaArquivos;
            viewModel.Documentos = listaDocumentos.Select(x => new DocumentoProponente
            {
                Id = x.Id,
                Proponente = new Proponente
                {
                    Id = x.Proponente.Id,
                    Cliente = new Cliente
                    {
                        Id = x.Proponente.Cliente.Id,
                        NomeCompleto = x.Proponente.Cliente.NomeCompleto
                    }
                },
                TipoDocumento = x.TipoDocumento,
                Motivo = x.Motivo,
                Situacao = x.Situacao,
                DataExpiracao = x.DataExpiracao
            }).ToList();

            foreach (var documento in viewModel.Documentos)
            {
                var parecer = _parecerDocumentoProponenteRepository.BuscarUltimoParecerDocumento(documento.Id);
                if (parecer.HasValue())
                {
                    viewModel.Pareceres.Add(new ParecerDocumentoProponente
                    {
                        Id = parecer.Id,
                        DocumentoProponente = new DocumentoProponente
                        {
                            Id = parecer.DocumentoProponente.Id
                        },
                        Parecer = parecer.Parecer
                    });
                }
            }
            return Json(viewModel,JsonRequestBehavior.AllowGet);
        }

    }
}