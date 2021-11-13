using Flurl.Http;
using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.Funcionalidade;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {

        public List<FuncionalidadeDto> ListarCodigoNomeUnidadesFuncionaisDoSistema(string codigo)
        {
            var request = GetBaseRequest().AppendPathSegments("unidadesFuncionais", "listar");
            var response = request.PostJsonAsync(codigo).Result;
            return HandleResponse<List<FuncionalidadeDto>>(response);
        }
    }
}
