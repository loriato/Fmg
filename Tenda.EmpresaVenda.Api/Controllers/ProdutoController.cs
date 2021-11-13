using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.Produto;
using Tenda.EmpresaVenda.ApiService.Models.StaticResource;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/produtos")]
    public class ProdutoController : BaseApiController
    {
        private ViewProdutoRepository ViewProdutoRepository { get; set; }
        private CorretorRepository CorretorRepository { get; set; }
        private StaticResourceService StaticResourceService { get; set; }
        private ArquivoRepository ArquivoRepository { get; set; }
        private ViewArquivoBreveLancamentoRepository ViewArquivoBreveLancamentoRepository { get; set; }
        private ViewArquivoEmpreendimentoRepository ViewArquivoEmpreendimentoRepository { get; set; }

        [HttpGet]
        [Route("listar")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage Listar()
        {
            var estadoUsuario = CorretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id).Estado;

            var list = ViewProdutoRepository.Queryable().Where(x => x.Estado == estadoUsuario
                                                                    && x.DisponivelCatalogo);
            var listOrdenada = list.OrderByDescending(x => x.Sequencia != null)
                .ThenBy(x => x.EmpreendimentoVerificado)
                .ThenBy(x => x.Sequencia)
                .ThenBy(x => x.Nome)
                .ToList();
            listOrdenada.ForEach(x => { x.UrlImagemPrincipal = GetStaticResource(x.IdImagemPrincipal)?.Url; });
            return Response(listOrdenada);
        }
        
        [HttpGet]
        [Route("breveLancamento/{id}")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage DetalharBreveLancamento(long id)
        {
            var model = ViewProdutoRepository.Queryable().FirstOrDefault(x => x.Id == id);
            var arquivos = ViewArquivoBreveLancamentoRepository.ListarImagensVideos(id);
            var arquivosDto = new List<ArquivoDto>();
            
            foreach (var arquivo in arquivos)
            {
                var arquivoDto = new ArquivoDto();
                if (arquivo.IsVideo())
                {
                    arquivoDto.FromDomainVideo(arquivo);
                }
                else
                {
                    var staticResource = GetStaticResource(arquivo.IdArquivo);
                    arquivoDto.FromDto(staticResource, arquivo.ContentType, arquivo.FileExtension);
                }
                arquivosDto.Add(arquivoDto);
            }

            var detalhe = MontarDetalheDto(model, arquivosDto);
            return Response(detalhe);
        }
        
        [HttpGet]
        [Route("breveLancamento/{id}/book")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage BookBreveLancamento(long id)
        {
            var documento = ViewArquivoBreveLancamentoRepository.BuscarPdf(id);

            if (documento.IsEmpty())
            {
                var exc = new ApiException();
                exc.AddError(string.Format(GlobalMessages.NaoExisteDocumentos, GlobalMessages.BreveLancamento.ToLower()));
                exc.ThrowIfHasError();
            }

            var arquivo = ArquivoRepository.FindById(documento.IdArquivo);
            var dto = new FileDto();
            dto.FromDomain(arquivo);
            return Response(dto);
        }
        
        [HttpGet]
        [Route("empreendimento/{id}")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage DetalharEmpreendimento(long id)
        {
            var model = ViewProdutoRepository.Queryable().FirstOrDefault(x => x.Id == id);
            var arquivos = ViewArquivoEmpreendimentoRepository.ListarImagensVideos(id);
            var arquivosDto = new List<ArquivoDto>();
            
            foreach (var arquivo in arquivos)
            {
                var arquivoDto = new ArquivoDto();
                if (arquivo.IsVideo())
                {
                    arquivoDto.FromDomainVideo(arquivo);
                }
                else
                {
                    var staticResource = GetStaticResource(arquivo.IdArquivo);
                    arquivoDto.FromDto(staticResource, arquivo.ContentType, arquivo.FileExtension);
                }
                arquivosDto.Add(arquivoDto);
            }

            var detalhe = MontarDetalheDto(model, arquivosDto);
            return Response(detalhe);
        }
        
        [HttpGet]
        [Route("empreendimento/{id}/book")]
        [AuthenticateUserByToken("EVS07", "Visualizar")]
        public HttpResponseMessage BookEmpreendimento(long id)
        {
            var documento = ViewArquivoEmpreendimentoRepository.BuscarPdf(id);

            if (documento.IsEmpty())
            {
                var exc = new ApiException();
                exc.AddError(string.Format(GlobalMessages.NaoExisteDocumentos, GlobalMessages.Empreendimento.ToLower()));
                exc.ThrowIfHasError();
            }

            var arquivo = ArquivoRepository.FindById(documento.IdArquivo);
            var dto = new FileDto();
            dto.FromDomain(arquivo);
            return Response(dto);
        }

        private DetalheProdutoDto MontarDetalheDto(ViewProduto viewProduto, List<ArquivoDto> arquivos)
        {
            var detalhe = new DetalheProdutoDto();
            detalhe.FromDomain(viewProduto);
            detalhe.Arquivos = arquivos;
            return detalhe;
        }

        private StaticResourceDTO GetStaticResource(long? idArquivo)
        {
            if (idArquivo.IsEmpty()) return null;
            var filename = StaticResourceService.LoadResource(idArquivo.Value);
            var staticResource = new StaticResourceDTO();
            staticResource.Id = idArquivo.Value;
            staticResource.FileName = filename;
            staticResource.Url = StaticResourceService.CreateUrlApi(staticResource.FileName);
            staticResource.UrlThumbnail = StaticResourceService.CreateThumbnailUrlApi(staticResource.FileName);
            return staticResource;
        }
    }
}