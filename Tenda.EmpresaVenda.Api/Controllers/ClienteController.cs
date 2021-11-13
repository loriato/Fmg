using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Conecta;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/clientes")]
    public class ClienteController : BaseApiController
    {
        private ClienteRepository _clienteRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ClienteService _clienteService { get; set; }
        private ConectaService _conectaService { get; set; }

        [HttpGet]
        [Route("{id}/dados")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage Dados(long id)
        {
            var cliente = _clienteRepository.FindById(id);

            if (cliente.IsEmpty())
            {
                return Response(HttpStatusCode.NotFound);
            }
            var dto = new DadosClienteDto().FromDomain(cliente);

            return Response(dto);
        }

        [HttpPost]
        [Route("listar")]
        [AuthenticateUserByToken("EVS05", "Visualizar")]
        public HttpResponseMessage Listar(FiltroClienteDto filtro)
        {
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            var dataSource = _clienteRepository.ListarClientes(filtro.Nome, filtro.Email, filtro.Telefone.OnlyNumber(), 
                filtro.CpfCnpj.OnlyNumber(), corretor.EmpresaVenda.Id, corretor.Id).ToDataRequest(filtro.DataSourceRequest);
            var viewModels = dataSource.records.Select(x => new DadosClienteDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpGet]
        [Route("{id}")]
        [AuthenticateUserByToken("EVS05", "Visualizar")]
        public HttpResponseMessage Buscar(long id)
        {
            if (id.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var model = _clienteService.MontarClienteDto(id);
            return Response(model);
        }

        [HttpPost]
        [Route("incluir")]
        [AuthenticateUserByToken("EVS05", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Incluir(ClienteDto dto)
        {            
            var response = new BaseResponse();
            try
            {
                var result = Salvar(dto);
                response.Data = result;
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, dto.InformacoesGeraisDto.NomeCompleto,
                    GlobalMessages.Incluido.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
                _session.Transaction.Rollback();
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                response.ErrorResponse(ex.Message);
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("alterar")]
        [AuthenticateUserByToken("EVS05", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Alterar(ClienteDto dto)
        {
            if (dto.IdCliente.IsEmpty()) return Response(HttpStatusCode.NotFound);
            var response = new BaseResponse();

            try
            {
                var result = Salvar(dto);
                response.Data = result;
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSalvo, dto.InformacoesGeraisDto.NomeCompleto,
                    GlobalMessages.Alterado.ToLower()));
            }catch(BusinessRuleException bre)
            {
                response.ErrorResponse(bre.Errors);
                _session.Transaction.Rollback();
            }
            catch(ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                response.ErrorResponse(ex.Message);
                _session.Transaction.Rollback();
            }

            return Response(response);
        }

        public ClienteDto Salvar(ClienteDto clienteDto)
        {
            //Define o Corretor que criou esse cliente
            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            clienteDto.AgenteVendaDto.Id = corretor.Id;
            clienteDto.AgenteVendaDto.IdLoja = corretor.EmpresaVenda.Id;
            clienteDto.Vendedor = RequestState.UsuarioPortal.Login;

            if (clienteDto.NovoCliente)
            {
                clienteDto.OrigemContato = "Portal Loja";    
            }

            var cliente = _clienteService.SalvarCliente(clienteDto);
                        
            return clienteDto;
        }

        [HttpPost]
        [Route("validar")]
        [AuthenticateUserByToken("EVS05", "ValidarDados")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage ValidarDadosIntegracao(ClienteDto clienteDto)
        {
            SuatService service = new SuatService();
            var bre = new BusinessRuleException();
            var response = new BaseResponse();
            try
            {
                var cliente = clienteDto.ToCliente();
                var enderecoCliente = clienteDto.ToEnderecoCliente();
                var enderecoEmpresa = clienteDto.ToEnderecoEmpresa();

                var dadosIntegracao = new ClienteSuatDTO().FromModel(cliente, enderecoCliente,enderecoEmpresa);
                var result = service.ValidarCliente(dadosIntegracao);

                if (clienteDto.IdCliente.IsEmpty())
                {
                    Salvar(clienteDto);
                }
                bre.ThrowIfHasError();
                response.Data = clienteDto.IdCliente;
                response.SuccessResponse(result.Mensagens);
            }
            catch (BusinessRuleException bre2)
            {
                response.ErrorResponse(bre2.Errors);
                _session.Transaction.Rollback();
            }
            catch (ApiException apiEx)
            {
                response = apiEx.GetResponse();
                _session.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                response.ErrorResponse(ex.Message);
                _session.Transaction.Rollback();
            }

            return Response(response);
        }
    }
}