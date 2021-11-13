using Europa.Commons;
using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.BreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque;
using Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoBreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoEmpreendimento;
using Tenda.EmpresaVenda.ApiService.Models.Funcionalidade;
using Tenda.EmpresaVenda.ApiService.Models.Motivo;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.Planta3D;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Proponente;
using Tenda.EmpresaVenda.ApiService.Models.StaticResource;
using Tenda.EmpresaVenda.ApiService.Models.Torre;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using ViewArquivoBreveLancamento = Tenda.EmpresaVenda.ApiService.Models.Arquivo.ViewArquivoBreveLancamento;
using ViewArquivoEmpreendimento = Tenda.EmpresaVenda.ApiService.Models.Arquivo.ViewArquivoEmpreendimento;

namespace Tenda.EmpresaVenda.ApiService.Services
{
    public abstract partial class EmpresaVendaService
    {
        private const string UrlApiEmpresaVendaProperty = "url_api_empresa_venda";
        private readonly IFlurlClient _flurlClient;

        public EmpresaVendaService(IFlurlClientFactory flurlClientFac)
        {
            var baseUrl = ConfigurationWrapper.GetStringProperty(UrlApiEmpresaVendaProperty);
            _flurlClient = flurlClientFac.Get(baseUrl);
            // configure _flurlClient as needed
        }

        private IFlurlRequest GetBaseRequest()
        {
            return _flurlClient.Request().WithAuthorization(GetAuthorizationToken()).AllowAnyHttpStatus().AppendPathSegment("api");
        }

        private T HandleResponse<T>(IFlurlResponse responseMessage) where T : new()
        {
            var response = responseMessage.ResponseMessage.Content.ReadAsStringAsync().Result;

            switch (responseMessage.ResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Task.FromResult(responseMessage).ReceiveJson<T>().Result;
                case HttpStatusCode.Forbidden: //sem permissão para ufs
                    var forbiddenResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    var data = JsonConvert.DeserializeObject<SemPermissaoDto>(
                        JsonConvert.SerializeObject(forbiddenResponse.Data));
                    throw new UnauthorizedPermissionException(forbiddenResponse.Messages.FirstOrDefault(),
                        data.UnidadeFuncional, data.Funcionalidade);
                case HttpStatusCode.Unauthorized: //token inválido
                    var unauthorizedResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new UnauthorizedAccessException(unauthorizedResponse.Messages.FirstOrDefault());
                case HttpStatusCode.NotFound: //Not found
                    throw new NotFoundException();
                case HttpStatusCode.BadRequest:
                    var errorResponse = Task.FromResult(responseMessage).ReceiveJson<BaseResponse>().Result;
                    throw new ApiException(errorResponse);
                default: //outros erros
                    HandleError(responseMessage);
                    return default;
            }
        }

        private void HandleError(IFlurlResponse responseMessage)
        {
            var response = responseMessage.ResponseMessage.Content.ReadAsStringAsync().Result;
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ExceptionDto>(response);
                throw new InternalServerException(errorResponse);
            }
            catch (FlurlParsingException)
            {
                throw new Exception(response);
            }
        }

        protected abstract string GetAuthorizationToken();

        #region Pré Proposta

        public BaseResponse EnviarPreProposta(PrePropostaCreateDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "enviar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse RetornarPreProposta(PrePropostaCreateDto dto)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "retornar");
            var response = request.PostJsonAsync(dto).Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse AguardandoAnaliseCompletaPreProposta(PrePropostaCreateDto dto)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "aguardandoAnaliseCompleta");
            var response = request.PostJsonAsync(dto).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse ValidarEmpreendimento(long idPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", idPreProposta, "validarEmpreendimento");
            var response = request.GetAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse DefinirUnidadePreProposta(FiltroIntegracaoSuatDto dto)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("preProposta", "integracaoSuat", "definirUnidade");
            var response = requisicao.PostJsonAsync(dto).Result;
            var result = HandleResponse<BaseResponse>(response);
            return result;
        }
        
        public BaseResponse IntegrarPrePropostaSUAT(long id)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("preProposta", "integracaoSuat", "preProposta", id);
            var response = requisicao.PostAsync().Result;
            var result = HandleResponse<BaseResponse>(response);
            return result;
        }
        
        public DataSourceResponse<LogIntegracaoPrePropostaDto> ListarLogIntegracao(FiltroLogIntegracaoDto filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("preProposta", "integracaoSuat", "logIntegracao");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<LogIntegracaoPrePropostaDto>>(response);
            return result;
        }

        public BaseResponse SalvarNovoBreveLancamento(FiltroPrePropostaDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "salvarBreveLancamento");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public DataSourceResponse<HistoricoPrePropostaDto> ListarHistoricoPreProposta(FilterIdDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "historico", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<HistoricoPrePropostaDto>>(response);
        }

        public BaseResponse MudarFatorSocialPreProposta(long id, bool? fatorSocial)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("preProposta", id, "fatorSocial");
            var response = requisicao.PostJsonAsync(fatorSocial).Result;
            var result = HandleResponse<BaseResponse>(response);
            return result;
        }

        public DataSourceResponse<EntityDto> ListarEstadoIndiqueAutoComplete(DataSourceRequest dataSourceRequest)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "autocompleteEstadoIndique");
            var response = request.PostJsonAsync(dataSourceRequest).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(response);
        }
        public DataSourceResponse<EntityDto> ListarCidadeIndiqueAutoComplete(DataSourceRequest dataSourceRequest)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "autocompleteCidadeIndique");
            var response = request.PostJsonAsync(dataSourceRequest).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(response);
        }
        public List<BaseResponse> EnviarIndicacao(List<IndicadoDto> indicacaoDto, long idCliente)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "enviarIndicacao").SetQueryParam("idCliente", idCliente);
            var response = request.PostJsonAsync(indicacaoDto).Result;
            return HandleResponse<List<BaseResponse>>(response);
        }
        public BaseResponse SalvarRecusaIndicacao(long idPreProposta)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "salvarRecusaIndicacao").SetQueryParam("idPreProposta", idPreProposta);
            var response = request.PostJsonAsync(idPreProposta).Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse EnviarWhatsBotmakerIndicador(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "enviarWhatsBotmakerIndicador").SetQueryParam("id", id);
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }
        public BaseResponse EnviarWhatsBotmakerIndicado(long id, string nome, string telefone)
        {
            var request = GetBaseRequest().AppendPathSegments("preProposta", "enviarWhatsBotmakerIndicado").SetQueryParams(new { id = id, nome = nome, telefone = telefone });
            var response = request.PostAsync().Result;
            return HandleResponse<BaseResponse>(response);
        }
        #endregion

        #region Agente Venda



        #endregion

        #region Breve Lançamento/Empreendimento

        public DataSourceResponse<BreveLancamentoDto> ListarBreveLancamentoDaRegionalSemEmpreendimento(FilterDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("brevesLancamentos", "listarDaRegionalSemEmpreendimento");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<BreveLancamentoDto>>(response);
        }

        public DataSourceResponse<BreveLancamentoDto> ListarBreveLancamentoDaRegional(FilterDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("brevesLancamentos", "listarDaRegional");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<BreveLancamentoDto>>(response);
        }

        public List<BreveLancamentoDto> ListarBrevesLancamentosDisponiveisEstado()
        {
            var request = GetBaseRequest().AppendPathSegments("brevesLancamentos", "listarDisponiveisEstado");
            var response = request.GetAsync().Result;
            return HandleResponse<List<BreveLancamentoDto>>(response);
        }

        public DataSourceResponse<TorreDto> ListarTorres(FilterDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("brevesLancamentos", "torres", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<TorreDto>>(response);
        }
        public List<EnderecoBreveLancamentoDTO> ListarEnderecosDeBrevesLancamentos(List<long> idsBrevesLancamentos)
        {
            var request = GetBaseRequest().AppendPathSegments("enderecosBrevesLancamentos", "enderecosDeBrevesLancamentos");
            var response = request.PostJsonAsync(idsBrevesLancamentos).Result;
            return HandleResponse<List<EnderecoBreveLancamentoDTO>>(response);
        }
        public EnderecoEmpreendimentoDTO BuscarEnderecoPorEmpreendimento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("enderecosEmpreendimentos").AppendPathSegment("buscar").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<EnderecoEmpreendimentoDTO>(response);
        }
        public EnderecoBreveLancamentoDTO BuscarEnderecoPorBreveLancamento(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("enderecosBrevesLancamentos").AppendPathSegment("buscar").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<EnderecoBreveLancamentoDTO>(response);
        }

        public List<ViewArquivoBreveLancamento> ListarArquivosBreveLancamentos(long idBreveLancamento)
        {
            var request = GetBaseRequest().AppendPathSegments("arquivosBrevesLancamentos").AppendPathSegment("listar").AppendPathSegment(idBreveLancamento);
            var response = request.GetAsync().Result;
            return HandleResponse<List<ViewArquivoBreveLancamento>>(response);
        }
        public List<ViewArquivoEmpreendimento> ListarArquivosEmpreendimentos(long idEmpreendimento)
        {
            var request = GetBaseRequest().AppendPathSegments("arquivosEmpreendimentos").AppendPathSegment("listar").AppendPathSegment(idEmpreendimento);
            var response = request.GetAsync().Result;
            return HandleResponse<List<ViewArquivoEmpreendimento>>(response);
        }


        #endregion

        #region Consulta Pré Proposta

        public DataSourceResponse<Tenda.Domain.EmpresaVenda.Models.Views.ViewPreProposta> ListarPrepropostas(DataSourceRequest request, FiltroConsultaPrePropostaDto filtro)
        {
            filtro.DataSourceRequest = request;
            var requisicao = GetBaseRequest().AppendPathSegments("consultaPreproposta", "listar");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<Tenda.Domain.EmpresaVenda.Models.Views.ViewPreProposta>>(response);
            return result;
        }
        public FileDto ExportarPaginaConsultarPreProposta(FiltroConsultaPrePropostaDto filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("consultaPreproposta")
                .AppendPathSegment("exportarPagina");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }

        public FileDto ExportarTodosConsultarPreProposta(FiltroConsultaPrePropostaDto filtro)
        {
            var request = GetBaseRequest().WithTimeout(TimeSpan.FromMinutes(10)).AppendPathSegment("consultaPreproposta")
                .AppendPathSegment("exportarTodos");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<FileDto>(response);
        }
        #endregion

        #region Clientes
        public DadosClienteDto BuscarDadosCliente(long id)
        {
            var request = GetBaseRequest().AppendPathSegments("clientes", id, "dados");
            var response = request.GetAsync().Result;
            return HandleResponse<DadosClienteDto>(response);
        }

        public DataSourceResponse<DadosClienteDto> ListarClientes(FiltroClienteDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("clientes", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<DataSourceResponse<DadosClienteDto>>(response);
        }

        public BaseResponse IncluirCliente(ClienteDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("clientes", "incluir");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public BaseResponse EditarCliente(ClienteDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("clientes", "alterar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        public ClienteDto BuscarCliente(long id)
        {
            var request = GetBaseRequest().AppendPathSegment("clientes").AppendPathSegment(id);
            var response = request.GetAsync().Result;
            return HandleResponse<ClienteDto>(response);
        }

        public BaseResponse ValidarDadosIntegracao(ClienteDto model)
        {
            var request = GetBaseRequest().AppendPathSegments("clientes", "validar");
            var response = request.PostJsonAsync(model).Result;
            return HandleResponse<BaseResponse>(response);
        }

        #endregion

        #region Motivos

        public List<EntityDto> ListarMotivos(FiltroMotivoDto filtro)
        {
            var request = GetBaseRequest().AppendPathSegments("motivos", "listar");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<List<EntityDto>>(response);
        }

        #endregion

        #region Static Resource
        public StaticResourceDTO LoadResource(long id)
        {
            FiltroStaticResourceDTO filtro = new FiltroStaticResourceDTO() { Id = id };
            var request = GetBaseRequest().AppendPathSegments("staticResources", "loadResource");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<StaticResourceDTO>(response);
        }
        public ArquivoDto WithNoContentAndNoThumbnail(long id)
        {
            FiltroStaticResourceDTO filtro = new FiltroStaticResourceDTO() { Id = id };
            var request = GetBaseRequest().AppendPathSegments("staticResources", "withNoContentAndNoThumbnail");
            var response = request.PostJsonAsync(filtro).Result;
            return HandleResponse<ArquivoDto>(response);
        }

        #endregion

        #region Plantas 3D

        public PlantaTresDLinksDto BuscarLinksPlantasTresD()
        {
            var request = GetBaseRequest().AppendPathSegments("plantasTresD", "links");
            var response = request.GetAsync().Result;
            return HandleResponse<PlantaTresDLinksDto>(response);
        }

        #endregion

        #region Profissão

        public DataSourceResponse<EntityDto> ListarProfissaoAutocomplete(DataSourceRequest dataSourceRequest)
        {
            var request = GetBaseRequest().AppendPathSegments("profissoes", "autocomplete");
            var response = request.PostJsonAsync(dataSourceRequest).Result;
            return HandleResponse<DataSourceResponse<EntityDto>>(response);
        }

        #endregion

        #region Consulta Estoque

        public DataSourceResponse<EstoqueEmpreendimentoDto> EstoqueEmpreendimento(FiltroConsultaEstoqueDto filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("consultaEstoque", "empreendimento");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<EstoqueEmpreendimentoDto>>(response);
            return result;
        }
        
        public DataSourceResponse<EstoqueUnidadeDto> EstoqueUnidade(FiltroConsultaEstoqueDto filtro)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("consultaEstoque", "unidade");
            var response = requisicao.PostJsonAsync(filtro).Result;
            var result = HandleResponse<DataSourceResponse<EstoqueUnidadeDto>>(response);
            return result;
        }

        #endregion

        #region DocumentoFormulario

        public FileDto BaixarFormularios(long idPreProposta)
        {
            var requisicao = GetBaseRequest().AppendPathSegments("documentoFormulario", idPreProposta, "baixarFormularios");
            var response = requisicao.GetAsync().Result;
            var result = HandleResponse<FileDto>(response);
            return result;
        }

        #endregion

    }
}
