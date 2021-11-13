using System.Collections.Generic;
using Flurl.Http;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.Produto;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public partial class EmpresaVendaService
    {
        public List<ViewProduto> ListarProdutos()
        {
            var request = GetBaseRequest().AppendPathSegments("produtos", "listar");
            var response = request.GetAsync().Result;
            return HandleResponse<List<ViewProduto>>(response);
        }
        
        public DetalheProdutoDto DetalharProdutoBreveLancamento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("produtos", "breveLancamento", id);
            var response = request.GetAsync().Result;
            return HandleResponse<DetalheProdutoDto>(response);
        }
        
        public FileDto BookProdutoBreveLancamento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("produtos", "breveLancamento", id, "book");
            var response = request.GetAsync().Result;
            return HandleResponse<FileDto>(response);
        }
        
        public FileDto BookProdutoEmpreendimento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("produtos", "empreendimento", id, "book");
            var response = request.GetAsync().Result;
            return HandleResponse<FileDto>(response);
        }
        
        public DetalheProdutoDto DetalharProdutoEmpreendimento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("produtos", "empreendimento", id);
            var response = request.GetAsync().Result;
            return HandleResponse<DetalheProdutoDto>(response);
        }
    }
}
