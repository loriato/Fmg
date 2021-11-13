using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Europa.Extensions;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Funcionalidade;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/unidadesFuncionais")]
    public class UnidadeFuncionalController : BaseApiController
    {
        private LogAcaoService _logAcaoService { get; set; }
        private UnidadeFuncionalRepository _unidadeFuncionalRepository { get; set; }

        //[HttpPost]
        //[Route("autocomplete")]
        //[AuthenticateUserByToken]
        //public HttpResponseMessage ListarUnidadeFuncionalAutocomplete(DataSourceRequest request)
        //{
        //    var dataSource = LogAcaoService.ListarUnidadeFuncionalAutocomplete(request);
        //    var viewModels = dataSource.records.Select(MontarDtoAutocomplete).AsQueryable();
        //    var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
        //    return Response(response);
        //}

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarUnidadesFuncionaisDoSistema([FromBody] string codigo)
        {
            if (codigo.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var dataSource = _unidadeFuncionalRepository.ListarCodigoNomeUnidadesFuncionaisDoSistema(codigo)
                .Select(MontarDto);
            return Response(dataSource.ToList());
        }

        private FuncionalidadeDto MontarDto(KeyValuePair<string, string> keyValuePair)
        {
            var dto = new FuncionalidadeDto();
            dto.FromKeyValuePair(keyValuePair);
            return dto;
        }

        private EntityDto MontarDtoAutocomplete(UnidadeFuncional model)
        {
            var dto = new EntityDto {Id = model.Id, Nome = model.Modulo.Sistema.Nome + " - " + model.Nome};
            return dto;
        }
    }
}