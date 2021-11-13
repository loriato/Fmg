using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.RegraComissao;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/regraComissao")]
    public class RegraComissaoController : BaseApiController
    {
        public CorretorRepository _corretorRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public ResponsavelAceiteRegraComissaoRepository _responsavelAceiteRegraComissaoRepository { get; set; }
        public ViewResponsavelAceiteRegraComissaoRepository _viewResponsavelAceiteRegraComissaoRepository { get; set; }
        public ResponsavelAceiteRegraComissaoService _responsavelAceiteRegraComissaoService { get; set; }


        [HttpPost]
        [Route("listar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Listar(RegraComissaoDto dto)
        {
            var result = _viewResponsavelAceiteRegraComissaoRepository.Listar(dto);
            return Response(result);
        }

        [HttpPost]
        [Route("")]
        [AuthenticateUserByToken("EVS01", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Incluir(RegraComissaoDto dto)
        {
            var response = new BaseResponse();

            var corretor = _corretorRepository.FindById(dto.IdCorretor);
            var empresaVenda = _empresaVendaRepository.FindById(dto.IdEmpresaVenda);

            //_responsavelAceiteRegraComissaoService.SuspenderResposaveisAtivos(dto.IdEmpresaVenda);

            try
            {
                var responsavel = new ResponsavelAceiteRegraComissao
                {
                    EmpresaVenda = empresaVenda,
                    Corretor = corretor,
                    Inicio = DateTime.Now,
                    Situacao = Situacao.Ativo
                };

                _responsavelAceiteRegraComissaoRepository.Save(responsavel);
                

                response.SuccessResponse(string.Format(GlobalMessages.MsgResponsavelSucesso,
                    GlobalMessages.Incluido.ToLower()));
                response.Data = dto;
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                FromBusinessRuleException(bre);
            }
            

            return Response(response);
        }

        [HttpPost]
        [Route("suspender")]
        [AuthenticateUserByToken("EVS01", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage SuspenderResponsavel(RegraComissaoDto dto)
        {
            var response = new BaseResponse();

            var responsavel = _responsavelAceiteRegraComissaoRepository.FindById(dto.IdResponsavelRegraComissao);
            try
            {
                _responsavelAceiteRegraComissaoService.SuspenderResponsavelAtivo(responsavel);
                response.SuccessResponse(string.Format(GlobalMessages.MsgResponsavelSucesso,
                    GlobalMessages.Suspenso.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                FromBusinessRuleException(bre);
            }


            return Response(response);
        }


    }
}