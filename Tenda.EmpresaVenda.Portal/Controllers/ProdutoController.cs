using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Controllers;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class ProdutoController : BaseController
    {
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private ArquivoBreveLancamentoRepository _arquivoBreveLancamentoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private ArquivoEmpreendimentoRepository _arquivoEmpreendimentoRepository { get; set; }
        private ViewArquivoEmpreendimentoRepository _viewArquivoEmpreendimentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private ViewArquivoBreveLancamentoRepository _viewArquivoBreveLancamentoRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }

        [BaseAuthorize("EVS07", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        private List<ProdutoViewModel> ListarProdutos()
        {
            var empresaUsuario = SessionAttributes.Current().EmpresaVenda;
            var brevesLancamentos = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaUsuario);
            var enderecos =
                _enderecoBreveLancamentoRepository.EnderecosDeBrevesLancamentos(
                    brevesLancamentos.Select(reg => reg.Id).ToList(), empresaUsuario);
            enderecos.ForEach(reg => _session.Evict(reg.Value));
            enderecos.ForEach(reg => _session.Evict(reg.Value.BreveLancamento));
            enderecos.ForEach(reg => reg.Value.BreveLancamento = null);

            List<ProdutoViewModel> produtos = new List<ProdutoViewModel>(brevesLancamentos.Count);

            var defaultRequest = DataTableExtensions.DefaultRequest();
            defaultRequest.pageSize = 20;
            string webRoot = GetWebAppRoot();

            string imageNotFound = _staticResourceService.CreateImageUrl(webRoot, "not-found.png");

            foreach (var breveLancamento in brevesLancamentos)
            {
                ProdutoViewModel produto = new ProdutoViewModel();

                produto.GeneratedAt = DateTime.Now;
                produto.IdBreveLancamento = breveLancamento.Id;
                produto.Nome = breveLancamento.Nome;
                produto.DisponivelParaVenda = breveLancamento.DisponivelCatalogo;
                produto.Informacoes = breveLancamento.Informacoes;
                produto.Endereco = enderecos.Single(reg => reg.Key == breveLancamento.Id).Value;
                produto.Sequencia = breveLancamento.Sequencia;

                var arquivosDoEmprendimento =
                    _viewArquivoBreveLancamentoRepository.Listar(defaultRequest, breveLancamento.Id);
                foreach (var arquivo in arquivosDoEmprendimento.records)
                {
                    ArquivoDto arquivoDto = new ArquivoDto();
                    if (arquivo.ContentType.Equals("video"))
                    {
                        arquivoDto.Nome = arquivo.Nome;
                        arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                    }
                    else
                    {
                        string filename = _staticResourceService.LoadResource(arquivo.IdArquivo);
                        arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                        arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                    }

                    arquivoDto.FileExtension = arquivo.FileExtension;
                    arquivoDto.ContentType = arquivo.ContentType;
                    produto.Book.Add(arquivoDto);
                }

                var imagemPrincipal = arquivosDoEmprendimento.records
                    .Where(reg => reg.ContentType.ToLower().Contains("image"))
                    .OrderByDescending(reg => reg.AtualizadoEm)
                    .FirstOrDefault();
                if (imagemPrincipal.HasValue())
                {
                    string filename = _staticResourceService.LoadResource(imagemPrincipal.IdArquivo);
                    ArquivoDto arquivoDto = new ArquivoDto();
                    arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                    arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                    arquivoDto.FileExtension = imagemPrincipal.FileExtension;
                    arquivoDto.ContentType = imagemPrincipal.ContentType;
                    produto.ImagemPrincipal = arquivoDto;
                }

                if (breveLancamento.Empreendimento.HasValue())
                {
                    if (breveLancamento.Empreendimento.RegistroIncorporacao.HasValue())
                    {
                        produto.Endereco = _enderecoEmpreendimentoRepository.Queryable()
                            .Where(x => x.Empreendimento.Id == breveLancamento.Empreendimento.Id).FirstOrDefault();

                        produto.VerificarEmpreendimento = true;

                        produto.Nome = breveLancamento.Empreendimento.Nome;
                        produto.Informacoes = breveLancamento.Empreendimento.Informacoes;
                        produto.ImagemPrincipal = null;
                    }

                    //var arquivos = _arquivoEmpreendimentoRepository.Queryable().Where(x => x.Empreendimento.Id == empreendimento.Empreendimento.Id).ToList();

                    var arquivos = _viewArquivoEmpreendimentoRepository.Listar(breveLancamento.Empreendimento.Id);

                    var principalEmpre = _arquivoEmpreendimentoRepository.Queryable()
                        .Where(x => x.Empreendimento.Id == breveLancamento.Empreendimento.Id)
                        .Where(reg => reg.Arquivo.ContentType.ToLower().Contains("image"))
                        .OrderByDescending(reg => reg.AtualizadoEm)
                        .FirstOrDefault();
                    produto.IdEmpreendimento = breveLancamento.Empreendimento.Id;
                    if (principalEmpre.HasValue())
                    {
                        string filename = _staticResourceService.LoadResource(principalEmpre.Arquivo.Id);
                        ArquivoDto arquivoDto = new ArquivoDto();
                        arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                        arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                        arquivoDto.FileExtension = principalEmpre.Arquivo.FileExtension;
                        arquivoDto.ContentType = principalEmpre.Arquivo.ContentType;
                        produto.ImagemPrincipal = arquivoDto;
                    }

                    foreach (var arquivo in arquivos)
                    {
                        ArquivoDto arquivoDto = new ArquivoDto();
                        if (arquivo.ContentType.Equals("video"))
                        {
                            arquivoDto.Nome = arquivo.Nome;
                            arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                            arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                            arquivoDto.FileExtension = arquivo.FileExtension;
                            arquivoDto.ContentType = arquivo.ContentType;
                        }
                        else
                        {
                            var metadata = _arquivoRepository.WithNoContentAndNoThumbnail(arquivo.IdArquivo);
                            string filename = _staticResourceService.LoadResource(arquivo.IdArquivo);
                            arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                            arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                            arquivoDto.FileExtension = metadata.FileExtension;
                            arquivoDto.ContentType = metadata.ContentType;
                        }

                        produto.Book.Add(arquivoDto);
                    }
                }
                else
                {
                    produto.VerificarEmpreendimento = false;
                }
                //Filtro realizado na pesquisa do empreendimento
                //if (produto.Endereco.Estado == empresaUsuario.Estado)
                //{
                //    produtos.Add(produto);
                //}
                produtos.Add(produto);
            }

            return produtos.OrderByDescending(x => x.Sequencia != null)
                            .ThenBy(x => x.VerificarEmpreendimento)
                            .ThenBy(x => x.Sequencia)
                            .ThenBy(x => x.Nome)
                            .ToList();
        }

        [OutputCache(Duration = 300, VaryByParam = "none")]
        public JsonResult Produtos()
        {
            List<ProdutoViewModel> produtos = ListarProdutos();
            return Json(produtos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObterBl(long idBl, bool empreendimento)
        {
            var json = new JsonResponse();
            var listaProduto = new List<ArquivoDto>();
            var objeto = new InfoBreveLancamentoViewModel();
            if (!empreendimento)
            {
                objeto.ArquivoBreveLancamento = _viewArquivoBreveLancamentoRepository.Queryable().Where(x =>
                        x.IdBreveLancamento == idBl &&
                        (x.ContentType.Contains("image") || x.ContentType.Equals("video")))
                    .ToList();
                objeto.Endereco = _enderecoBreveLancamentoRepository.FindByBreveLancamento(idBl);
                var defaultRequest = DataTableExtensions.DefaultRequest();
                defaultRequest.pageSize = 20;
                string webRoot = GetWebAppRoot();
                var arquivosDoEmprendimento = _viewArquivoBreveLancamentoRepository.Queryable()
                    .Where(x => x.IdBreveLancamento == idBl &&
                                (x.ContentType.Contains("image") || x.ContentType.Equals("video")))
                    .ToDataRequest(defaultRequest);
                foreach (var arquivo in arquivosDoEmprendimento.records)
                {
                    ArquivoDto arquivoDto = new ArquivoDto();
                    if (arquivo.ContentType.Equals("video"))
                    {
                        arquivoDto.Nome = arquivo.Nome;
                        arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.FileExtension = arquivo.FileExtension;
                        arquivoDto.ContentType = arquivo.ContentType;
                    }
                    else
                    {
                        string filename = _staticResourceService.LoadResource(arquivo.IdArquivo);
                        arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                        arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                    }

                    arquivoDto.FileExtension = arquivo.FileExtension;
                    arquivoDto.ContentType = arquivo.ContentType;
                    listaProduto.Add(arquivoDto);
                }
            }
            else
            {
                objeto.EnderecoEmpreendimento = _enderecoEmpreendimentoRepository.Queryable()
                    .Where(x => x.Empreendimento.Id == idBl).FirstOrDefault();
                var defaultRequest = DataTableExtensions.DefaultRequest();
                defaultRequest.pageSize = 20;
                string webRoot = GetWebAppRoot();
                var arquivosDoEmprendimento = _viewArquivoEmpreendimentoRepository.Queryable()
                    .Where(x => x.IdEmprendimento == idBl &&
                                (x.ContentType.Contains("image") || x.ContentType.Equals("video")))
                    .ToDataRequest(defaultRequest);
                foreach (var arquivo in arquivosDoEmprendimento.records)
                {
                    ArquivoDto arquivoDto = new ArquivoDto();
                    if (arquivo.ContentType.Equals("video"))
                    {
                        arquivoDto.Nome = arquivo.Nome;
                        arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.FileExtension = arquivo.FileExtension;
                        arquivoDto.ContentType = arquivo.ContentType;
                    }
                    else
                    {
                        string filename = _staticResourceService.LoadResource(arquivo.IdArquivo);
                        arquivoDto.Url = _staticResourceService.CreateUrl(webRoot, filename);
                        arquivoDto.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                    }

                    arquivoDto.FileExtension = arquivo.FileExtension;
                    arquivoDto.ContentType = arquivo.ContentType;
                    listaProduto.Add(arquivoDto);
                }
            }

            objeto.ListaArquivo = listaProduto;
            json.Sucesso = true;
            json.Objeto = RenderRazorViewToString("_ModalInfo", objeto, false);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult SalvarBook(long idBl, bool empre)
        {
            if (empre)
            {
                var book = _arquivoEmpreendimentoRepository.Queryable()
                    .Where(x => x.Empreendimento.Id == idBl && x.Arquivo.FileExtension.Contains(".pdf"))
                    .FirstOrDefault();
                if (book != null)
                {
                    string nomeArquivo = GlobalMessages.Book;
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    return File(book.Arquivo.Content, book.Arquivo.ContentType, $"{nomeArquivo}_{date}.pdf");
                }
            }
            else
            {
                var book = _arquivoBreveLancamentoRepository.Queryable()
                    .Where(x => x.BreveLancamento.Id == idBl && x.Arquivo.FileExtension.Contains(".pdf"))
                    .FirstOrDefault();
                if (book != null)
                {
                    string nomeArquivo = GlobalMessages.Book;
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    return File(book.Arquivo.Content, book.Arquivo.ContentType, $"{nomeArquivo}_{date}.pdf");
                }
            }

            return null;
        }

        public object DownloadSimulador(long idBl)
        {
            var book = _arquivoBreveLancamentoRepository.Queryable()
                .Where(x => x.BreveLancamento.Id == idBl && x.Arquivo.FileExtension.Contains(".xlsx")).FirstOrDefault();
            if (book != null)
            {
                string nomeArquivo = GlobalMessages.Simulador;
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                return File(book.Arquivo.Content, book.Arquivo.ContentType, $"{nomeArquivo}_{date}.xlsx");
            }

            return null;
        }

        public JsonResult VerificarArquivo(long idBl, bool tpArquivo, bool empre)
        {
            var json = new JsonResponse();
            if (empre)
            {
                var book = _arquivoEmpreendimentoRepository.Queryable()
                    .Where(x => x.Empreendimento.Id == idBl && x.Arquivo.FileExtension.Contains(".pdf"))
                    .FirstOrDefault();
                json.Sucesso = book == null ? false : true;
            }
            else
            {
                if (tpArquivo)
                {
                    var book = _arquivoBreveLancamentoRepository.Queryable().Where(x =>
                        x.BreveLancamento.Id == idBl && x.Arquivo.FileExtension.Contains(".xlsx")).FirstOrDefault();
                    json.Sucesso = book == null ? false : true;
                }
                else
                {
                    var book = _arquivoBreveLancamentoRepository.Queryable().Where(x =>
                        x.BreveLancamento.Id == idBl && x.Arquivo.FileExtension.Contains(".pdf")).FirstOrDefault();
                    json.Sucesso = book == null ? false : true;
                }
            }

            if (!json.Sucesso)
            {
                var bus = new BusinessRuleException();
                bus.AddError(GlobalMessages.NaoExisteDocumentos).WithParam(empre == true
                    ? GlobalMessages.Empreendimento.ToLower()
                    : GlobalMessages.BreveLancamento.ToLower()).Complete();
                json.Mensagens.AddRange(bus.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}