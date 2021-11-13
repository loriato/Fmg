using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Financeiro;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/financeiro")]
    public class FinanceiroController : BaseApiController
    {
        private NotaFiscalPagamentoService _notaFiscalPagamentoService { get; set; }

        [HttpPost]
        [Route("atualizarNotaFiscal")]
        [AuthenticateUserByToken(true)]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AtualizarNotaFiscal(NotaFiscalRequestDto notaFiscalRequestDto)
        {
            if (notaFiscalRequestDto.IsEmpty()) return Response(HttpStatusCode.NotFound);

            var response = new BaseResponse();

            try
            {
                _notaFiscalPagamentoService.AtualizarNotaFiscal(notaFiscalRequestDto);
                response.SuccessResponse(string.Format("Nota fiscal {0} atualizada com sucesso",notaFiscalRequestDto.Chave));
            }
            catch(BusinessRuleException bre)
            {
                response.SuccessResponse(bre.Errors);
            }
            
            return Response(response);
        }
    }
}