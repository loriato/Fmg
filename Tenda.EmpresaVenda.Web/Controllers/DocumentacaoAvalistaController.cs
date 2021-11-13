using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS17")]
    public class DocumentacaoAvalistaController : BaseController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private ParecerDocumentoAvalistaRepository _parecerDocumentoAvalistaRepository { get; set; }
        private DocumentoAvalistaService _documentoAvalistaService { get; set; }
                
        public ActionResult Index(long? id)
        {

            var view = new DocumentacaoAvalistaViewModel();

            var preProposta = new PreProposta();

            if (id.HasValue())
            {
                preProposta = _prePropostaRepository.FindById(id.Value);
                view.IdPreProposta = preProposta.Id;
            }

            var endereco = new EnderecoAvalista();
            if (!preProposta.Avalista.IsEmpty())
            {
                view.IdAvalista = preProposta.Avalista.Id;
                view.NomeAvalista = preProposta.Avalista.Nome;
                view.EmailAvalista = preProposta.Avalista.Email;
                view.RG = preProposta.Avalista.RG;
                view.OrgaoExpedicao = preProposta.Avalista.OrgaoExpedicao;
                view.CPF = preProposta.Avalista.CPF;
                view.Profissao = preProposta.Avalista.Profissao;
                view.Empresa = preProposta.Avalista.Empresa;
                view.TempoEmpresa = preProposta.Avalista.TempoEmpresa;
                view.RendaDeclarada = preProposta.Avalista.RendaDeclarada;
                view.OutrasRendas = preProposta.Avalista.OutrasRendas;
                view.Banco = preProposta.Avalista.Banco;
                view.ContatoGerente = preProposta.Avalista.ContatoGerente;
                view.ValorTotalBens = preProposta.Avalista.ValorTotalBens;
                view.PossuiImovel = preProposta.Avalista.PossuiImovel;
                view.PossuiVeiculo = preProposta.Avalista.PossuiVeiculo;

                endereco = _enderecoAvalistaRepository.FindByIdAvalista(preProposta.Avalista.Id);
                if (endereco.HasValue())
                { 
                    view.IdEndereco = endereco.Id;
                    view.Cidade = endereco.Cidade;
                    view.Logradouro = endereco.Logradouro;
                    view.Bairro = endereco.Bairro;
                    view.Numero = endereco.Numero;
                    view.Cep = endereco.Cep;
                    view.Complemento = endereco.Complemento;
                    view.Estado = endereco.Estado;
                    view.Pais = endereco.Pais;
                }
            }
            
            DocumentosAvalista(view, preProposta.Id);

            return View(view);
        }
        
        private void DocumentosAvalista(DocumentacaoAvalistaViewModel viewModel, long idPreProposta)
        {
            var avalistas = _prePropostaRepository.ListarDaPreProposta(idPreProposta,0);
            viewModel.Avalistas = avalistas;
            var listaDocumentos = new List<DocumentoAvalista>();
            var listaArquivos = new Dictionary<string, ArquivoUrlDTO>();
            string webRoot = GetWebAppRoot();

            foreach (var avalista in avalistas)
            {
                var documentos =
                    _documentoAvalistaRepository.BuscarDocumentosAvalista(idPreProposta, avalista.Id);

                listaDocumentos.AddRange(documentos.OrderBy(x => x.TipoDocumento.Nome).ToList());
                foreach (var documento in documentos)
                {
                    if (documento.Arquivo.HasValue())
                    {
                        string metadados = "";
                        if (documento.Arquivo.Metadados.IsEmpty() == false)
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
            viewModel.Documentos = listaDocumentos.Select(x=> new DocumentoAvalista
            {
                Id=x.Id,
                Avalista = new Avalista
                {
                    Id = x.Avalista.Id,
                    Nome = x.Avalista.Nome
                },
                TipoDocumento = x.TipoDocumento,
                Motivo = x.Motivo,
                Situacao = x.Situacao,
                //DataExpiracao = x.DataExpiracao,
            }).ToList();

            foreach (var documento in viewModel.Documentos)
            {
                var parecer = _parecerDocumentoAvalistaRepository.BuscarUltimoParecerDocumento(documento.Id);
                if (parecer.HasValue())
                {
                    viewModel.Pareceres.Add(new ParecerDocumentoAvalista
                    {
                        Id = parecer.Id,
                        DocumentoAvalista = new DocumentoAvalista
                        {
                            Id = parecer.DocumentoAvalista.Id
                        },
                        Parecer = parecer.Parecer
                    });
                }
            }
        }

        public ActionResult AtualizarViewModel(long idPreProposta, bool somenteSemAnalise)
        {
            var json = new JsonResponse();
            var viewModel = new DocumentacaoAvalistaViewModel();
            var avalistas = _prePropostaRepository.ListarDaPreProposta(idPreProposta, 0);
            var listaDocumentos = new List<DocumentoAvalista>();
            var listaArquivos = new Dictionary<string, ArquivoUrlDTO>();
            string webRoot = GetWebAppRoot();

            foreach (var avalista in avalistas)
            {
                var documentos =
                    _documentoAvalistaRepository.BuscarDocumentosParaAnalise(idPreProposta, avalista.Id);

                if (somenteSemAnalise)
                {
                    documentos = _documentoAvalistaRepository.BuscarDocumentosNaoAnalisados(idPreProposta, avalista.Id);
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
            viewModel.Documentos = listaDocumentos.Select(x => new DocumentoAvalista
            {
                Id = x.Id,
                Avalista = new Avalista
                {
                    Id = x.Avalista.Id,
                    Nome = x.Avalista.Nome
                },
                TipoDocumento = x.TipoDocumento,
                Motivo = x.Motivo,
                Situacao = x.Situacao,
                //DataExpiracao = x.DataExpiracao,
            }).ToList();

            foreach (var documento in viewModel.Documentos)
            {
                var parecer = _parecerDocumentoAvalistaRepository.BuscarUltimoParecerDocumento(documento.Id);
                if (parecer.HasValue())
                {
                    viewModel.Pareceres.Add(new ParecerDocumentoAvalista
                    {
                        Id = parecer.Id,
                        DocumentoAvalista = new DocumentoAvalista
                        {
                            Id = parecer.DocumentoAvalista.Id
                        },
                        Parecer = parecer.Parecer
                    });
                }
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS17", "AprovarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarDocumentacao(DocumentoAvalistaDTO documento)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _documentoAvalistaService.AprovarDocumentacao(documento);
                
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    documento.NomeTipoDocumento, GlobalMessages.Aprovado.ToLower()));
                jsonResponse.Objeto = documento;
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS17", "PendenciarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult PendenciarDocumentacao(DocumentoAvalistaDTO documento)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _documentoAvalistaService.PendenciarDocumentacao(documento);

                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    documento.NomeTipoDocumento, GlobalMessages.Pendenciado.ToLower()));
                jsonResponse.Objeto = documento;
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS17", "BaixarTodosDocumentos")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarTodosDocumentos(DocumentoAvalistaDTO documento)
        {
            byte[] file = _documentoAvalistaService.ExportarTodosDocumentos(documento.IdPreProposta, documento.IdAvalista);
            string nomeArquivo = documento.NomeAvalista;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"{nomeArquivo}_Completo_{date}.zip");
        }

        [BaseAuthorize("EVS17", "AprovarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarAvalista(DocumentoAvalistaDTO documento)
        {
            var json = new JsonResponse();
            try
            {
                _documentoAvalistaService.AprovarAvalista(documento);
                json.Sucesso = true;
                json.Mensagens.Add(GlobalMessages.AvalistaPreAprovado);
            }
            catch(BusinessRuleException bre)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS17", "PendenciarDocumento")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult PendenciarAvalista(DocumentoAvalistaDTO documento)
        {
            var json = new JsonResponse();
            try
            {
                _documentoAvalistaService.PendenciarAvalista(documento);
                json.Sucesso = true;
                json.Mensagens.Add(GlobalMessages.AvalistaPendenciado);
            }
            catch (BusinessRuleException bre)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}