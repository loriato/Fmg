using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.Avalista;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.DocumentoProponente;
using Tenda.EmpresaVenda.ApiService.Models.PlanoPagamento;
using Tenda.EmpresaVenda.ApiService.Models.PontoVenda;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Proponente;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/preProposta")]
    public class PrePropostaController : BaseApiController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private PropostaSuatRepository _propostaSuatRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private ViewProponenteRepository _viewProponenteRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private DocumentoFormularioRepository _documentoFormularioRepository { get; set; }
        private HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        private PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private AvalistaRepository _avalistaRepository { get; set; }
        private EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }
        private ViewPlanoPagamentoRepository _viewPlanoPagamentoRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        private ViewDocumentoProponenteRepository _viewDocumentoProponenteRepository { get; set; }
        private HierarquiaCoordenadorService _hierarquiaCoordenadorService { get; set; }
        private PlanoPagamentoService _planoPagamentoService { get; set; }
        private ProponenteService _proponenteService { get; set; }
        private ProponenteRepository _proponenteRepository { get; set; }
        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private DocumentoProponenteService _documentoProponenteService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private MotivoRepository _motivoRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private PrePropostaReintegradaRepository _prePropostaReintegradaRepository { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }
        private FamiliarRepository _familiarRepository { get; set; }
        private PrePropostaReintegradaService _prePropostaReintegradaService { get; set; }
        private ViewHistoricoPrePropostaRepository _viewHistoricoPrePropostaRepository { get; set; }
        private AgrupamentoProcessoPrePropostaService _AgrupamentoProcessoPrePropostaService { get; set; }
        private HistoricoRecusaIndicacaoService _historicoRecusaIndicacaoService { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private IndiqueService _indiqueService { get; set; }

        [HttpPost]
        [Route("buscar")]
        [AuthenticateUserByToken("EVS10", "Visualizar")]
        public HttpResponseMessage Buscar(FiltroPrePropostaDto filtro)
        {
            PreProposta preProposta = null;
            PropostaSuat proposta = new PropostaSuat();
            PrePropostaCreateDto dto = new PrePropostaCreateDto();

            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);

            if (filtro.Id.HasValue)
            {
                preProposta = _prePropostaRepository.FindById(filtro.Id.Value);
                proposta = _propostaSuatRepository.FindByIdPreProposta(preProposta.Id);
            }

            if (filtro.IdEmpreendimento.HasValue)
            {
                var empre = _breveLancamentoRepository.FindById(filtro.IdEmpreendimento.Value);
                dto.Empreendimento = new EntityDto
                {
                    Id = empre.Id,
                    Nome = empre.Nome
                };
            }

            if (preProposta == null)
            {
                // Elaboração, Status e outros só são definidos ao salvar
                preProposta = new PreProposta();
                preProposta.Corretor = corretor;
                preProposta.Elaborador = corretor;
                preProposta.EmpresaVenda = corretor.EmpresaVenda;
                dto.ListProponentes = new List<EntityDto>();
            }
            else
            {
                dto.ListProponentes = ProponentesPreProposta(preProposta.Id);
                dto.SituacaoPrepropostaSuatEvs = preProposta.SituacaoProposta.Value == SituacaoProposta.Integrada ?
                                                _viewPrePropostaRepository.Queryable().Where(x => preProposta.IdSuat == x.IdSuat)
                                                .Where(x => x.Id == filtro.Id.Value).Select(reg => reg.SituacaoPrePropostaSuatEvs).SingleOrDefault()
                                                : preProposta.SituacaoProposta.AsString();
                var situacaoPortalHouse = _AgrupamentoProcessoPrePropostaService.BuscarNomeAgrupamentoPorSistemaESituacao((long)preProposta.SituacaoProposta.Value, filtro.CodigoSistema);
                dto.SituacaoPrePropostaPortalHouse = (situacaoPortalHouse.IsEmpty() ? dto.SituacaoPrepropostaSuatEvs : situacaoPortalHouse);
                dto.PossuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

                var situacaoAnterior = _historicoPrePropostaRepository.Queryable()
                .Where(x => x.PreProposta.Id == preProposta.Id)
                .Where(x => x.Anterior != null);

                if (situacaoAnterior.HasValue())
                {
                    dto.SituacaoAnterior = (int)situacaoAnterior.FirstOrDefault().SituacaoInicio;
                }
            }

            if (!proposta.IsEmpty())
            {
                dto.KitCompleto = proposta.KitCompleto;
                var pagamento = _planoPagamentoRepository.ListarParcelas(preProposta.Id);
                dto.PermissaoAvalistaPreChaves = _planoPagamentoRepository.RegraAvalistaPreChaves(preProposta.Id);
            }

            if (!proposta.IsEmpty() && !proposta.StatusConformidade.IsEmpty())
            {
                dto.StatusConformidade = proposta.StatusConformidade.ToLower();
            }

            dto.PreProposta = new PrePropostaDto().FromDomain(preProposta);
            if (preProposta.BreveLancamento != null)
            {
                var enderecoBreveLancamento =
                    _enderecoBreveLancamentoRepository.FindByBreveLancamento(preProposta.BreveLancamento.Id);
                dto.EnderecoBreveLancamento = enderecoBreveLancamento.ChaveCandidata();
            }

            if (preProposta.Avalista != null)
            {
                dto.AvalistaDto = new AvalistaDto();

                var avalista = _avalistaRepository.FindById(preProposta.Avalista.Id);
                dto.AvalistaDto.Avalista = avalista;

                var enderecoAvalista = _enderecoAvalistaRepository.FindByIdAvalista(avalista.Id);
                dto.AvalistaDto.Endereco = enderecoAvalista;

                var situacao = new List<SituacaoAprovacaoDocumentoAvalista> {
                    SituacaoAprovacaoDocumentoAvalista.Anexado,
                    SituacaoAprovacaoDocumentoAvalista.Informado,
                    SituacaoAprovacaoDocumentoAvalista.NaoAnexado,
                    SituacaoAprovacaoDocumentoAvalista.Pendente
                };

                var docsPendentes = _documentoAvalistaRepository.Queryable()
                    .Where(x => x.Avalista.Id == avalista.Id)
                    .Where(x => situacao.Contains(x.Situacao))
                    .Any();

                dto.AvalistaDto.DocsPendentes = !docsPendentes;
            }
            else
            {
                dto.AvalistaDto = new AvalistaDto();
            }

            // Lista os pontos de venda da Empresa de Venda do usuário logado
            dto.ListPontosVenda = _pontoVendaRepository
                .BuscarAtivosPorEmpresaVenda(corretor.EmpresaVenda.Id).Select(x =>
                    new EntityDto
                    {
                        Id = x.Id,
                        Nome = x.Nome
                    });

            if (dto.PreProposta.PontoVenda.HasValue() && dto.PreProposta.PontoVenda.Situacao != Situacao.Ativo)
            {
                var pontoVenda = new List<PontoVendaDto>();
                pontoVenda.Add(dto.PreProposta.PontoVenda);

                dto.ListPontosVenda = pontoVenda.Select(x => new EntityDto
                {
                    Id = x.Id,
                    Nome = x.Nome
                });

            }

            // Lista os Breves Lançamentos da Empresa de Venda do usuário logado
            dto.ListBrevesLancamentos = _enderecoBreveLancamentoRepository
                .BuscarPorEstado(corretor.EmpresaVenda.Estado).Select(x =>
                    new EntityDto
                    {
                        Id = x.BreveLancamento.Id,
                        Nome = x.BreveLancamento.Nome
                    });
            if (filtro.IdCliente.HasValue)
            {
                var cliente = _clienteRepository.FindById(filtro.IdCliente.Value);
                dto.PreProposta.Cliente = new DadosClienteDto().FromDomain(cliente);
            }

            dto.ListCidade = _estadoCidadeRepository.Queryable()
                .Where(x => x.Estado.Equals(corretor.EmpresaVenda.Estado))
                .Select(x =>
                new EntityDto
                {
                    Id = x.Id,
                    Nome = x.Cidade
                });

            if (dto.PreProposta.Regiao.HasValue() &&
                dto.PreProposta.SituacaoProposta != SituacaoProposta.EmElaboracao &&
                dto.PreProposta.SituacaoProposta != SituacaoProposta.DocsInsuficientesSimplificado)
            {

                //var regiao = new List<EstadoCidadeDto>();
                //regiao.Add(dto.PreProposta.Regiao);
                //
                //dto.ListCidade = regiao.Select(x => new EntityDto
                //{
                //    Id = x.Id,
                //    Nome = x.Cidade
                //});
            }

            if (preProposta.ParcelaAprovadaPrevio.HasValue())
            {
                dto.PreProposta.ParcelaAprovadaPrevio = preProposta.ParcelaAprovadaPrevio;
            }

            dto.PrePropostaReintegrada = _prePropostaReintegradaRepository.Queryable().Where(x => x.PreProposta.Id == filtro.Id).Any();

            dto.UrlDetalharEmpreendimento = ProjectProperties.SuatBaseUrl + ProjectProperties.SuatUrlDetalharEmpreendimento;
            dto.UrlDetalharTorre = ProjectProperties.SuatBaseUrl + ProjectProperties.SuatUrlDetalharTorre;
            dto.UrlDetalharUnidade = ProjectProperties.SuatBaseUrl + ProjectProperties.SuatUrlDetalharUnidade;

            //verifica se a prop tem formularios anexados e docs aprovados
            var possuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

            return Response(dto);
        }

        [HttpPost]
        [Route("")]
        [AuthenticateUserByToken("EVS10", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Incluir(PrePropostaCreateDto dto)
        {
            return Salvar(dto);
        }

        [HttpPost]
        [Route("alterar")]
        [AuthenticateUserByToken("EVS10", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Alterar(PrePropostaCreateDto dto)
        {
            return Salvar(dto);
        }

        private HttpResponseMessage Salvar(PrePropostaCreateDto dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            var response = new BaseResponse();
            var isInclusao = dto.PreProposta.Id.IsEmpty();
            var aux = dto.PreProposta;
            var proposta = dto.PreProposta.ToDomain();

            // Quando edição, só posso mexer no breve lançamento
            if (!isInclusao)
            {
                proposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                if (dto.PreProposta.BreveLancamento.HasValue() && dto.PreProposta.BreveLancamento.Id.HasValue())
                {
                    proposta.BreveLancamento = _breveLancamentoRepository.FindById(dto.PreProposta.BreveLancamento.Id);
                }
                else
                {
                    proposta.BreveLancamento = null;
                }

                //var regiao = _estadoCidadeRepository.FindById(dto.PreProposta.Regiao.Id);
                //proposta.Regiao = regiao;

                proposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;
                //dto.PreProposta = proposta;
            }
            else if (!dto.PreProposta.PontoVenda.Id.IsEmpty())
            {
                var pontovenda = _pontoVendaRepository.FindById(dto.PreProposta.PontoVenda.Id);
                proposta.Viabilizador = pontovenda.Viabilizador;
            }

            if (aux.IdTorre != proposta.IdTorre)
            {
                proposta.IdTorre = aux.IdTorre;
                proposta.NomeTorre = aux.NomeTorre;
                proposta.ObservacaoTorre = aux.ObservacaoTorre;
            }

            try
            {

                proposta.FaixaEv = aux.FaixaEv;

                if (!proposta.BreveLancamento.IsEmpty())
                {
                    proposta.BreveLancamento =
                        _breveLancamentoRepository.FindById(proposta.BreveLancamento.Id);
                }
                else
                {
                    proposta.BreveLancamento = null;
                }

                if (!proposta.PontoVenda.IsEmpty())
                {
                    proposta.PontoVenda = _pontoVendaRepository.FindById(proposta.PontoVenda.Id);
                }
                else
                {
                    proposta.PontoVenda = null;
                }

                if (!proposta.Cliente.IsEmpty())
                {
                    proposta.Cliente = _clienteRepository.FindById(proposta.Cliente.Id);
                }
                else
                {
                    proposta.Cliente = null;
                }

                proposta = _prePropostaService.Salvar(proposta);

                // Cria histórico da Pré Proposta
                if (isInclusao)
                {
                    _prePropostaService.MudarSituacaoProposta(proposta, SituacaoProposta.EmElaboracao,
                        RequestState.UsuarioPortal);
                    var webRoot = GetWebAppRootAdmin();
                    _hierarquiaCoordenadorService.NotificarClienteDuplicado(webRoot, proposta);

                }

                var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
                // Lista os pontos de venda da Empresa de Venda do usuário logado
                dto.ListPontosVenda = _pontoVendaRepository
                    .BuscarAtivosPorEmpresaVenda(corretor.EmpresaVenda.Id).Select(x =>
                        new EntityDto
                        {
                            Id = x.Id,
                            Nome = x.Nome
                        });

                // Lista os Breves Lançamentos da Empresa de Venda do usuário logado
                dto.ListBrevesLancamentos = _enderecoBreveLancamentoRepository
                    .BuscarPorEstado(corretor.EmpresaVenda.Estado).Select(x =>
                        new EntityDto
                        {
                            Id = x.BreveLancamento.Id,
                            Nome = x.BreveLancamento.Nome
                        });

                //Lista de cidade
                dto.ListCidade = _estadoCidadeRepository.Queryable()
                .Where(x => x.Estado.Equals(corretor.EmpresaVenda.Estado))
                .Select(x =>
                new EntityDto
                {
                    Id = x.Id,
                    Nome = x.Cidade
                });

                dto.PreProposta.FromDomain(proposta);

                response.SuccessResponse(string.Format(GlobalMessages.MsgPrePropostaSucesso, dto.PreProposta.Codigo,
                isInclusao ? GlobalMessages.Incluida.ToLower() : GlobalMessages.Alterada.ToLower()));

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
        [Route("enviar")]
        [AuthenticateUserByToken("EVS10", "Enviar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Enviar(PrePropostaCreateDto dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            var response = new BaseResponse();

            BusinessRuleException error = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                //Salvando valor parcela solicitada se o usuario clickou em enviar direto
                preProposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;
                var brevaLancamaento = _breveLancamentoRepository.FindById(dto.IdBreveLancamento);
                dto.PossuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

                preProposta.BreveLancamento = brevaLancamaento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;
                preProposta.FaixaEv = dto.PreProposta.FaixaEv;

                //se possui formularios anexados verifica os docs do proponente estão aprovados pro fluxo reduzido
                if (dto.PossuiFormulario && (preProposta.SituacaoProposta == SituacaoProposta.FluxoEnviado || preProposta.SituacaoProposta == SituacaoProposta.AguardandoFluxo))
                {
                    var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

                    foreach (var prop in proponentes)
                    {
                        if (_prePropostaService.VerificaDocumentosEmAnaliseCompletaParaAnaliseCompletaAprovada(prop))
                        {
                            dto.hasAprovedDocs = true;
                        }
                    }
                }

                SituacaoProposta situacaoDestino = preProposta.SituacaoProposta.Value;

                switch (preProposta.SituacaoProposta)
                {
                    case SituacaoProposta.EmElaboracao:
                        situacaoDestino = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;

                    case SituacaoProposta.AguardandoFluxo:
                        if (preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
                        {
                            situacaoDestino = SituacaoProposta.AguardandoAnaliseCompleta;
                            if (dto.hasAprovedDocs)
                            {
                                situacaoDestino = SituacaoProposta.AguardandoIntegracao;
                                preProposta.DataSicaq = preProposta.DataSicaqPrevio;
                                preProposta.StatusSicaq = preProposta.StatusSicaqPrevio;
                                preProposta.ParcelaAprovada = preProposta.ParcelaAprovadaPrevio;
                                preProposta.FaixaUmMeio = preProposta.FaixaUmMeioPrevio;
                            }
                        }
                        else
                        {
                            situacaoDestino = SituacaoProposta.FluxoEnviado;
                        }
                        break;

                    default:
                        error.AddError(string.Format(GlobalMessages.PreProposta_Inconsistente, preProposta.Codigo));
                        error.ThrowIfHasError();
                        break;
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, situacaoDestino,
                    RequestState.UsuarioPortal);

                if (preProposta.SituacaoProposta == SituacaoProposta.AguardandoAnaliseSimplificada)
                {
                    _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                }
                response.SuccessResponse(string.Format(GlobalMessages.PreProposta_EnviadaSucesso,
                    preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }
        [HttpPost]
        [Route("retornar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Retornar(PrePropostaCreateDto dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            var response = new BaseResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                var brevaLancamaento = _breveLancamentoRepository.FindById(dto.IdBreveLancamento);

                preProposta.BreveLancamento = brevaLancamaento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;
                preProposta.FaixaEv = dto.PreProposta.FaixaEv;

                //Salvando valor parcela solicitada se o usuario clickou em enviar direto
                preProposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;
                var situacao = _prePropostaService.SituacaoAnterior(dto.PreProposta.Id);

                switch (situacao)
                {
                    case SituacaoProposta.EmAnaliseCompleta:
                        situacao = SituacaoProposta.AguardandoAnaliseCompleta;
                        break;
                    case SituacaoProposta.EmAnaliseSimplificada:
                        situacao = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;
                    case SituacaoProposta.AnaliseSimplificadaAprovada:
                        situacao = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;
                    default:
                        situacao = SituacaoProposta.EmElaboracao;
                        break;
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, situacao.Value,
                     RequestState.UsuarioPortal);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                response.Success = true;
                response.Messages.Add(string.Format("A proposta de código {0} foi enviada com sucesso!",
                    preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                response.Messages.AddRange(bre.Errors);
                response.Messages.AddRange(bre.ErrorsFields);
            }

            return Response(response);
        }
        [HttpPost]
        [Route("aguardandoAnaliseCompleta")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AguardandoAnaliseCompleta(PrePropostaCreateDto dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            var response = new BaseResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                var breveLancamento = _breveLancamentoRepository.FindById(dto.PreProposta.BreveLancamento.Id);

                preProposta.BreveLancamento = breveLancamento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                    RequestState.UsuarioPortal);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);

                response.SuccessResponse(string.Format("A proposta de código {0} foi enviada com sucesso!", preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }


        [HttpPost]
        [Route("{id}/AprovarAnaliseSimplificadaAprovadaProposta")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AprovarAnaliseSimplificadaAprovadaProposta(long id)
        {
            var response = new BaseResponse();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(id);

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseSimplificadaAprovada, RequestState.UsuarioPortal, null, RequestState.Perfis);

                response.SuccessResponse(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AnaliseSimplificadaAprovada.AsString()));
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }

            return Response(response);

        }

        [HttpPost]
        [Route("{id}/AprovarAnaliseCompletaAprovadaProposta")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AprovarAnaliseCompletaAprovadaProposta(long id)
        {
            var response = new BaseResponse();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(id);

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseCompletaAprovada, RequestState.UsuarioPortal, null, RequestState.Perfis);

                response.SuccessResponse(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AnaliseCompletaAprovada.AsString()));
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }

            return Response(response);

        }

        [HttpPost]
        [Route("{id}/cancelar")]
        [AuthenticateUserByToken("EVS10", "Cancelar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage Cancelar(long id)
        {
            if (id.IsEmpty())
            {
                return Response(HttpStatusCode.NotFound);
            }
            CanAccessPreProposta(id);

            var response = new BaseResponse();

            var preProposta = _prePropostaRepository.FindById(id);

            _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Cancelada,
                RequestState.UsuarioPortal);

            response.SuccessResponse(string.Format("A proposta de código {0} foi cancelada com sucesso!",
                preProposta.Codigo));

            return Response(response);
        }

        private bool CanAccessPreProposta(long idPreProposta)
        {
            var apiEx = new ApiException();
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            // Não foi informada pré-proposta
            if (idPreProposta.IsEmpty())
            {
                return true;
            }

            // Não existe a pré-proposta informada
            if (preProposta == null)
            {
                apiEx.AddError("Pré Proposta não encontrada.");
                apiEx.ThrowIfHasError();
                return false;
            }

            var corretor = _corretorRepository.FindByIdUsuario(RequestState.UsuarioPortal.Id);
            var empresaVendaId = corretor?.EmpresaVenda?.Id;
            // A pré-proposta é da mesma empresa de venda, pode exibir
            if (preProposta.EmpresaVenda.Id == empresaVendaId)
            {
                return true;
            }

            // A idéia é tratar com WhiteList (Bloqueio tudo, exceto o que explicitamente estiver liberado
            else
            {
                apiEx.AddError("Não é possível alterar a Pré Proposta. Somente agentes da Loja que criou a Pré Proposta podem alterar.");
                apiEx.ThrowIfHasError();
                return false;
            }
        }

        private IList<EntityDto> ProponentesPreProposta(long idPreProposta)
        {
            var request = DataTableExtensions.DefaultRequest();
            var proponentes = _viewProponenteRepository.ProponentesDaProposta(request, idPreProposta);
            IList<EntityDto> proponentesSelectList = new List<EntityDto>();
            proponentesSelectList.Add(new EntityDto() { Id = 0, Nome = GlobalMessages.Proponente });
            foreach (var proponente in proponentes.records.ToList())
            {
                proponentesSelectList.Add(new EntityDto() { Id = proponente.IdCliente, Nome = proponente.NomeCliente });
            }

            return proponentesSelectList;
        }

        [HttpPost]
        [Route("integrarPrePropostaSimulador")]
        [AuthenticateUserByToken("EVS10", "Simulacao")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IntegrarPrePropostaSimulador(PrePropostaRequestDto prePropostaRequestDto)
        {
            var response = new BaseResponse();
            var apiEx = new ApiException();
            try
            {
                var prePropostaDTO = new PrePropostaDTO();

                var usuario = _usuarioPortalRepository.UsuarioPorLogin(prePropostaRequestDto.LoginDto.Username);

                if (usuario.IsEmpty())
                {
                    apiEx.AddError(string.Format("Usuário {0} não encontrado", prePropostaRequestDto.LoginDto.Username));
                    apiEx.ThrowIfHasError();
                }

                var corretor = _corretorRepository.Queryable()
                    .Where(x => x.Usuario.Id == usuario.Id)
                    .SingleOrDefault();

                if (corretor.IsEmpty())
                {
                    apiEx.AddError(string.Format("O usuário {0} não é um corretor", usuario.Nome));
                    apiEx.ThrowIfHasError();
                }

                var empresaVenda = corretor.EmpresaVenda;

                prePropostaDTO.UsuarioPortal = usuario;
                prePropostaDTO.EmpresaVenda = empresaVenda;
                prePropostaDTO.Corretor = corretor;

                prePropostaDTO.ClienteDto = prePropostaRequestDto.ClienteDto;
                prePropostaDTO.SimuladorDto = prePropostaRequestDto.SimuladorDto;
                prePropostaDTO.Divisao = prePropostaRequestDto.EmpreendimentoDto.Divisao;
                prePropostaDTO.NomeEmpreendimento = prePropostaRequestDto.EmpreendimentoDto.NomeEmpreendimento;

                prePropostaDTO.SimuladorDto.MatrizOferta = prePropostaRequestDto.MatrizOferta;

                var preProposta = _prePropostaService.PrePropostaSimulador(prePropostaDTO);

                var urlPreProposta = ProjectProperties.EvsBaseUrl + "PreProposta/Index/" + preProposta.Id;

                var prePropostaResponseDto = new PrePropostaResponseDto();
                prePropostaResponseDto.CodigoPreProposta = preProposta.Codigo;
                prePropostaResponseDto.Link = urlPreProposta;

                response.Data = prePropostaResponseDto;
                response.SuccessResponse(string.Format("Simulação {0}-{1} vinculada a Pré-Proposta {2}", preProposta.CodigoSimulacao, preProposta.DigitoSimulacao, preProposta.Codigo));

            }
            catch (ApiException ex)
            {
                CurrentTransaction().Rollback();
                response = ex.GetResponse();
            }

            return Response(response);
        }

        [HttpGet]
        [Route("{idPreProposta}/validarEmpreendimento")]
        public HttpResponseMessage ValidarEmpreendimento(long idPreProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(idPreProposta);

                if (preProposta.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.RegistroNaoEncontrado, GlobalMessages.PreProposta, idPreProposta)).Complete();
                    bre.ThrowIfHasError();
                }

                if (preProposta.BreveLancamento.Empreendimento.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.BreveLancamentoSemEmpreendimento, preProposta.BreveLancamento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }
                if (preProposta.EmpresaVenda.TipoEmpresaVenda != TipoEmpresaVenda.Loja)
                {
                    var idEmpresaVenda = preProposta.EmpresaVenda.Id;
                    var regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);

                    if (regraEvsVigente.IsNull())
                    {
                        bre.AddError(GlobalMessages.ErroRegraComissao).Complete();
                        bre.ThrowIfHasError();
                    }

                    var idEmpreendimento = preProposta.BreveLancamento.Empreendimento.Id;

                    var itemRegraComissao = _itemRegraComissaoRepository.Buscar(regraEvsVigente.RegraComissao.Id, idEmpresaVenda, idEmpreendimento);
                    if (itemRegraComissao.IsEmpty())
                    {
                        bre.AddError(GlobalMessages.ErroItemRegraComissao).Complete();
                        bre.ThrowIfHasError();
                    }
                }

                response.Code = (int)HttpStatusCode.OK;
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }


            return Response(response);
        }

        [HttpPost]
        [Route("integracaoSuat/definirUnidade")]
        [Transaction(TransactionAttributeType.Required)]
        [AuthenticateUserByToken("EVS10", "IntegrarPrePropostaSUAT")]
        public HttpResponseMessage DefinirUnidadePreProposta(FiltroIntegracaoSuatDto dto)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.IdPreProposta);
                if (preProposta.IdTorre.IsEmpty())
                {
                    preProposta.IdTorre = dto.IdTorre;
                    preProposta.NomeTorre = dto.NomeTorre;
                }

                // Verificar se a proposta já possui unidade associada
                if (preProposta.IdUnidadeSuat.HasValue() && preProposta.IdUnidadeSuat != dto.IdUnidadeSuat)
                {
                    throw new BusinessRuleException(string.Format("A pré-proposta {0} já está associada a unidade de identificador {1}", preProposta.Codigo, preProposta.IdUnidadeSuat));
                }

                preProposta.IdUnidadeSuat = dto.IdUnidadeSuat;
                preProposta.IdentificadorUnidadeSuat = dto.IdentificadorUnidadeSuat;
                _prePropostaRepository.Save(preProposta);

                response.Code = (int)HttpStatusCode.OK;
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }

            return Response(response);
        }

        [HttpPost]
        [Route("salvarBreveLancamento")]
        [Transaction(TransactionAttributeType.Required)]
        [AuthenticateUserByToken("EVS10", "ReintegrarPrePropostaSUAT")]
        public HttpResponseMessage SalvarNovoBreveLancamento(FiltroPrePropostaDto filtro)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(filtro.Id.Value);

                //Salvando dados da preprosta q serão apagados
                _prePropostaReintegradaService.SalvarDadosReintegracao(preProposta);

                //Salva novo breve lançamento e limpa os campos preenchidos pela integraçãoklk
                _prePropostaService.SalvarNovoBreveLancamento(preProposta, filtro.IdBreveLancamento);

                if (preProposta.SituacaoProposta == SituacaoProposta.Integrada)
                {
                    _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoIntegracao, RequestState.UsuarioPortal, null, RequestState.Perfis);
                }

                var model = new
                {
                    Divisao = preProposta.BreveLancamento.Empreendimento.HasValue() ? preProposta.BreveLancamento.Empreendimento.Divisao : null,
                    NomeEmpreendimento = preProposta.BreveLancamento.Empreendimento.HasValue() ? preProposta.BreveLancamento.Empreendimento.Nome : preProposta.BreveLancamento.Nome,
                    NomeBreveLancamento = preProposta.BreveLancamento.Nome
                };

                response.Messages.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso, preProposta.Codigo, GlobalMessages.Alterado.ToLower()));
                response.SuccessResponse(model);
            }
            catch (BusinessRuleException e)
            {
                CurrentTransaction().Rollback();
                FromBusinessRuleException(e);
            }

            return Response(response);
        }

        [HttpPost]
        [Route("integracaoSuat/preProposta/{id}")]
        [Transaction(TransactionAttributeType.Required)]
        [AuthenticateUserByToken("EVS10", "IntegrarPrePropostaSUAT")]
        public HttpResponseMessage IntegrarPrePropostaSUAT(long id)
        {
            var response = new BaseResponse();

            try
            {
                var preProposta = _prePropostaRepository.FindById(id);
                // Call Validate

                var validationResult = new IntegracaoPrePropostaValidator().Validate(preProposta);
                new BusinessRuleException().WithFluentValidation(validationResult).ThrowIfHasError();

                var dto = MontarPropostaDTO(id);
                SuatService service = new SuatService();
                var transaction = CurrentSession().BeginTransaction();

                var result = service.IntegrarPreProposta(dto);
                if (result == null)
                {
                    throw new BusinessRuleException("A conexão sofreu timeout");
                }

                if (result.Sucesso)
                {
                    response.SuccessResponse(GlobalMessages.IntegracaoPropostaSucesso);
                }
                else
                {
                    response.ErrorResponse(result.Mensagens);
                }

                // Atualiza os IDS SUAT
                if (result.Objeto != null)
                {
                    AtualizarDadosSuat(result.Objeto, transaction);
                }
            }
            catch (BusinessRuleException e)
            {
                FromBusinessRuleException(e);
            }

            return Response(response);
        }

        [HttpPost]
        [Route("integracaoSuat/logIntegracao")]
        [AuthenticateUserByToken("EVS10", "VisualizarLogIntegracao")]
        public HttpResponseMessage LogIntegracao(FiltroLogIntegracaoDto filtro)
        {
            SuatService service = new SuatService();
            var result = service.LogIntegracao(filtro.CodigoPreProposta, 0);
            var response = result.Select(MontarDto).AsQueryable().ToDataRequest(filtro.DataSourceRequest);
            return Response(response);
        }

        private LogIntegracaoPrePropostaDto MontarDto(LogIntegracaoPrePropostaDTO model)
        {
            var dto = new LogIntegracaoPrePropostaDto();
            dto.Id = model.Id;
            dto.CriadoPor = model.CriadoPor;
            dto.CriadoEm = model.CriadoEm;
            dto.IdProposta = model.IdProposta;
            dto.Codigo = model.Codigo;
            dto.PreProposta = model.PreProposta;
            dto.Mensagem = model.Mensagem;

            return dto;
        }

        private PropostaDTO MontarPropostaDTO(long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            var planosPagamento = _planoPagamentoRepository.ListarParcelas(idPreProposta);
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(idPreProposta);
            var endereco = _enderecoClienteRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);
            var enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);
            var clienteSuatDTO = new ClienteSuatDTO().FromModel(preProposta.Cliente, endereco, enderecoEmpresa);

            PropostaDTO proposta = new PropostaDTO(preProposta, clienteSuatDTO, planosPagamento, proponentes);

            var sicaqEnquadrado = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);
            proposta.SicaqEnquadrado = sicaqEnquadrado;

            foreach (var prop in proposta.Proponentes)
            {
                var cliente = _clienteRepository.FindById(prop.IdCliente);
                if (!cliente.EmpresaVenda.IsEmpty())
                {
                    cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = cliente.EmpresaVenda.Id };
                }
                endereco = _enderecoClienteRepository.BuscarSomenteEndereco(prop.IdCliente);
                enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(prop.IdCliente);
                proposta.Clientes.Add(new ClienteSuatDTO().FromModel(cliente, endereco, enderecoEmpresa));
            }
            foreach (var clie in proposta.Clientes)
            {
                var clienteConjuge = new Cliente();
                var conjuge = _familiarRepository.BuscarConjugePorCliente(clie.Id);
                if (!conjuge.IsEmpty())
                {
                    clienteConjuge = conjuge.Cliente1.Id == clie.Id ? conjuge.Cliente2 : conjuge.Cliente1;
                    endereco = _enderecoClienteRepository.BuscarSomenteEndereco(clienteConjuge.Id);
                    enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(clienteConjuge.Id);
                    proposta.Conjuges.Add(new ClienteSuatDTO().FromModel(clienteConjuge, endereco, enderecoEmpresa, clie.Id));
                    clie.IdConjuge = clienteConjuge.Id;
                }
            }

            proposta.Cliente.PossuiComprometimentoFinanceiro = false;
            proposta.Cliente.PossuiContaBanco = false;
            proposta.Cliente.PossuiFinanciamento = false;
            proposta.Cliente.PossuiVeiculo = false;

            return proposta;
        }

        private void AtualizarDadosSuat(PropostaDTO dto, ITransaction transaction)
        {
            // Atualizando ID SUAT dos Clientes
            foreach (var clie in dto.Clientes)
            {
                if (!clie.IdSuat.IsEmpty())
                {
                    var cliente = _clienteRepository.FindById(clie.Id);
                    cliente.IdSuat = clie.IdSuat;
                    _clienteRepository.Save(cliente);
                }
            }

            // Atualizando ID SUAT dos Conjuges
            if (dto.Conjuges.HasValue())
            {
                foreach (var clie in dto.Conjuges)
                {
                    if (!clie.IdSuat.IsEmpty())
                    {
                        var cliente = _clienteRepository.FindById(clie.Id);
                        cliente.IdSuat = clie.IdSuat;
                        _clienteRepository.Save(cliente);
                    }
                }
            }

            // Verifica se a Pré-Proposta já tem ID SUAT
            if (!dto.IdSuat.IsEmpty())
            {
                // Atualizando ID SUAT da Pré-Proposta
                var preProposta = _prePropostaRepository.BuscarPorCodigo(dto.ProtocoloGA);
                preProposta.IdSuat = dto.IdSuat;

                //Atualizando situação do passo atual SUAT
                var suatService = new SuatService();
                var resultSituacaoSuat = suatService.BuscarSituacaoProposta(dto.IdSuat);

                if (resultSituacaoSuat.Sucesso)
                {
                    preProposta.PassoAtualSuat = resultSituacaoSuat.Mensagens.First();
                }

                _prePropostaRepository.Save(preProposta);

                // Atualizando ID SUAT dos Proponentes
                foreach (var prop in dto.Proponentes)
                {
                    if (!prop.IdSuat.IsEmpty())
                    {
                        var proponente = _proponenteRepository.FindById(prop.Id);
                        proponente.IdSuat = prop.IdSuat;
                        _proponenteRepository.Save(proponente);
                    }
                }

                // Atualizando ID SUAT dos Planos de Pagamento
                if (dto.PlanosPagamento.HasValue())
                {
                    foreach (var plano in dto.PlanosPagamento)
                    {
                        if (!plano.IdSuat.IsEmpty())
                        {
                            var planoPagamento = _planoPagamentoRepository.FindById(plano.Id);
                            planoPagamento.IdSuat = plano.IdSuat;
                            _planoPagamentoRepository.Save(planoPagamento);
                        }
                    }
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Integrada, RequestState.UsuarioPortal, null, RequestState.Perfis);

            }
        }

        #region Detalhamento Financeiro / ITBI / Emolumentos

        [HttpPost]
        [Route("detalhamentosFinanceiros/listar")]
        [AuthenticateUserByToken("EVS10", "VisualizarDetalhamentoFinanceiro")]
        public HttpResponseMessage ListarDetalhamentoFinanceiro(FilterIdDto filtro)
        {
            //var statusCode = CanAccessPreProposta(filtro.Id);
            //if (statusCode != HttpStatusCode.OK)
            //{
            //    return Response(statusCode);
            //}

            var dataSource = _viewPlanoPagamentoRepository.ListarDetalhamentoFinanceiro(filtro.DataSourceRequest, filtro.Id);
            var viewModels = dataSource.records.Select(x => new PlanoPagamentoDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpPost]
        [Route("itbiEmolumentos/listar")]
        [AuthenticateUserByToken("EVS10", "VisualizarItbiEmolumento")]
        public HttpResponseMessage ListarItbiEmolumentos(FilterIdDto filtro)
        {
            //var statusCode = CanAccessPreProposta(filtro.Id);
            //if (statusCode != HttpStatusCode.OK)
            //{
            //    return Response(statusCode);
            //}

            var dataSource = _viewPlanoPagamentoRepository.ListarItbiEmolumentos(filtro.DataSourceRequest, filtro.Id);
            var viewModels = dataSource.records.Select(x => new PlanoPagamentoDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }


        [HttpPost]
        [Route("detalhamentosFinanceiros")]
        [AuthenticateUserByToken("EVS10", "IncluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IncluirDetalhamentoFinanceiro(PlanoPagamentoDto dto)
        {
            return SalvarPlanoPagamento(dto);
        }

        [HttpPost]
        [Route("detalhamentosFinanceiros/alterar")]
        [AuthenticateUserByToken("EVS10", "AlterarDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AlterarDetalhamentoFinanceiro(PlanoPagamentoDto dto)
        {
            return SalvarPlanoPagamento(dto);
        }

        private HttpResponseMessage SalvarPlanoPagamento(PlanoPagamentoDto planoPagamento)
        {
            CanAccessPreProposta(planoPagamento.IdPreProposta);

            var response = new BaseResponse();
            try
            {
                var inclusao = planoPagamento.Id == 0;
                _planoPagamentoService.Salvar(planoPagamento.ToDomain(), planoPagamento.IdPreProposta);
                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    planoPagamento.TipoParcela.AsString(),
                    inclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));

                RecalcularTotalFinanceiro(planoPagamento.IdPreProposta);
            }
            catch (BusinessRuleException bre)
            {
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }

        [HttpPost]
        [Route("detalhamentosFinanceiros/excluir/{id}")]
        [AuthenticateUserByToken("EVS10", "ExcluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage ExcluirDetalhamentoFinanceiro(long id)
        {
            return ExcluirPlanoFinanceiro(id);
        }

        private HttpResponseMessage ExcluirPlanoFinanceiro(long id)
        {
            var model = _planoPagamentoRepository.FindById(id);
            if (model.IsNull())
            {
                return Response(HttpStatusCode.NotFound);
            }
            else
            {
                CanAccessPreProposta(model.PreProposta.Id);
            }

            var transaction = _session.BeginTransaction();
            var response = new BaseResponse();
            try
            {
                var planoPagamento = _planoPagamentoService.Excluir(id);
                transaction.Commit();
                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    planoPagamento.TipoParcela.AsString(), GlobalMessages.Excluido.ToLower()));
            }
            catch (GenericADOException exp) when (
                ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
            {
                response.ErrorResponse(string.Format(GlobalMessages.RemovidoSemSucessoEstaEmUso, model.TipoParcela.AsString()));
                transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("{id}/recalcularTotalFinanceiro")]
        [AuthenticateUserByToken("EVS10", "VisualizarDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage RecalcularTotalFinanceiro(long id)
        {
            CanAccessPreProposta(id);

            PreProposta preProposta = _prePropostaRepository.FindById(id);
            _prePropostaService.RecalcularValores(id);
            var dto = new PrePropostaCreateDto() { PreProposta = new PrePropostaDto().FromDomain(preProposta) };

            return Response(dto);
        }

        [HttpPost]
        [Route("itbiEmolumentos/incluir")]
        [AuthenticateUserByToken("EVS10", "IncluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IncluirItbiEmolumento(PlanoPagamentoDto dto)
        {
            return SalvarPlanoPagamento(dto);
        }

        [HttpPost]
        [Route("itbiEmolumentos/alterar")]
        [AuthenticateUserByToken("EVS10", "AlterarDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AlterarItbiEmolumento(PlanoPagamentoDto dto)
        {
            return SalvarPlanoPagamento(dto);
        }

        [HttpPost]
        [Route("itbiEmolumentos/excluir/{id}")]
        [AuthenticateUserByToken("EVS10", "ExcluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage ExcluirItbiEmolumento(long id)
        {
            return ExcluirPlanoFinanceiro(id);
        }

        #endregion

        #region Proponentes

        [HttpPost]
        [Route("proponentes/listar")]
        [AuthenticateUserByToken("EVS10", "VisualizarProponente")]
        public HttpResponseMessage ListarProponentes(FilterIdDto filtro)
        {
            //var statusCode = CanAccessPreProposta(filtro.Id);
            //if (statusCode != HttpStatusCode.OK)
            //{
            //    return Response(statusCode);
            //}

            var dataSource = _viewProponenteRepository.ProponentesDaProposta(filtro.DataSourceRequest, filtro.Id);
            var viewModels = dataSource.records.Select(x => new ProponenteDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpPost]
        [Route("proponentes")]
        [AuthenticateUserByToken("EVS10", "IncluirProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IncluirProponente(ProponenteCreateDto dto)
        {
            return SalvarProponente(dto);
        }

        [HttpPost]
        [Route("proponentes/alterar")]
        [AuthenticateUserByToken("EVS10", "AlterarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage AlterarProponente(ProponenteCreateDto dto)
        {
            return SalvarProponente(dto);
        }

        private HttpResponseMessage SalvarProponente(ProponenteCreateDto dto)
        {
            CanAccessPreProposta(dto.IdPreProposta);

            var response = new BaseResponse();
            try
            {
                var inclusao = dto.Proponente.Id == 0;
                var proponente = dto.Proponente.ToDomain();
                _proponenteService.Salvar(proponente, dto.IdPreProposta, dto.IdCliente);
                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    proponente.Cliente.NomeCompleto,
                    inclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));
                response.Data = proponente.Id;
            }
            catch (BusinessRuleException bre)
            {
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }

        [HttpPost]
        [Route("proponentes/{id}")]
        [AuthenticateUserByToken("EVS10", "ExcluirProponente")]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage ExcluirProponente(long id)
        {
            var model = _proponenteRepository.FindById(id);
            if (model.IsNull())
            {
                return Response(HttpStatusCode.NotFound);
            }
            else
            {
                CanAccessPreProposta(model.PreProposta.Id);
            }

            var transaction = _session.BeginTransaction();
            var response = new BaseResponse();
            var apiEx = new ApiException();
            try
            {
                var proponente = _proponenteService.Excluir(id);
                transaction.Commit();

                response.SuccessResponse(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    proponente.Cliente.NomeCompleto, GlobalMessages.Excluido.ToLower()));
            }
            catch (GenericADOException exp) when (
                ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
            {
                response.ErrorResponse(string.Format(GlobalMessages.RemovidoSemSucessoEstaEmUso, model.Cliente.NomeCompleto));
                transaction.Rollback();
            }

            return Response(response);
        }

        #endregion

        #region Documentação de Proponentes

        [HttpGet]
        [Route("documentos/listar/{codigoProposta}")]
        [AuthenticateUserByToken("EVS10", "BaixarTodosDocumentos")]
        public HttpResponseMessage Documento([FromUri] string codigoProposta)
        {
            var proposta = _propostaSuatRepository.FindByCodigoProposta(codigoProposta);

            if (proposta == null || proposta.PreProposta == null)
            {
                return Response(new List<ViewDocumentoProponente>());
            }

            var documentos = _viewDocumentoProponenteRepository
                .ListarPreProposta(proposta.PreProposta.Id, null, SituacaoAprovacaoDocumento.Aprovado)
                .Where(reg => reg.IdArquivo != 0)
                .Select(reg => new DocumentoProponenteRepasseDto
                {
                    NomeCliente = reg.NomeCliente,
                    NomeTipoDocumento = reg.NomeTipoDocumento,
                    IdArquivo = reg.IdArquivo
                }).ToList();

            foreach (var documento in documentos)
            {
                var metadado = _arquivoRepository.WithNoContentAndNoThumbnail(documento.IdArquivo);
                if (metadado != null)
                {
                    var dataSource = _staticResourceService.LoadResource(documento.IdArquivo);

                    documento.FileExtension = metadado.FileExtension;
                    documento.Url = _staticResourceService.CreateUrlApi(dataSource);
                    documento.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(dataSource);
                }
            }

            return Response(documentos);
        }


        /// <summary>
        /// Listar os documentos possíveis e faz um chaveamento entre os documentos preenchidos do proponente
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("proponentes/documentos/listar")]
        [AuthenticateUserByToken("EVS10", "VisualizarProponente")]
        public HttpResponseMessage ListarDocumentacaoProponentes(FiltroDocumentoProponenteDto filtro)
        {
            var tiposDocumento = _tipoDocumentoRepository.ListarTodosHouse();
            tiposDocumento.ForEach(reg => _session.Evict(reg));

            var proponentes = _viewProponenteRepository.ListarDaPreProposta(filtro.Id, filtro.IdCliente);
            proponentes.ForEach(reg => _session.Evict(reg));

            var documentosDaProposta =
                _viewDocumentoProponenteRepository.ListarPreProposta(filtro.Id, filtro.IdCliente, null);
            documentosDaProposta.ForEach(reg => _session.Evict(reg));

            foreach (var proponente in proponentes)
            {
                foreach (var tipoDocumento in tiposDocumento)
                {
                    // Verifico se existe documento de determinado tipo para o proponente
                    var documentoProponente = documentosDaProposta
                        .Where(reg => reg.IdTipoDocumento == tipoDocumento.Id)
                        .Where(reg => reg.IdCliente == proponente.IdCliente)
                    .SingleOrDefault();

                    if (tipoDocumento.Nome.Contains(" - Não obrigatório"))
                        tipoDocumento.Nome = tipoDocumento.Nome.ToString().Replace(" - Não obrigatório", "");

                    // se não existir, então crio e adiciono na listagem a ser retornada
                    if (documentoProponente == null)
                    {
                        documentoProponente = new ViewDocumentoProponente();
                        documentoProponente.IdCliente = proponente.IdCliente;
                        documentoProponente.NomeCliente = proponente.NomeCliente;
                        documentoProponente.IdProponente = proponente.Id;
                        documentoProponente.IdTipoDocumento = tipoDocumento.Id;
                        documentoProponente.IdPreProposta = filtro.Id;
                        documentoProponente.NomeTipoDocumento = tipoDocumento.Nome;
                        documentoProponente.Situacao = SituacaoAprovacaoDocumento.NaoAnexado;
                        documentoProponente.Anexado = false;
                        if (filtro.SituacaoDocumento.IsEmpty())
                        {
                            documentosDaProposta.Add(documentoProponente);
                        }
                        else if (filtro.SituacaoDocumento.Value == documentoProponente.Situacao)
                        {
                            documentosDaProposta.Add(documentoProponente);
                        }
                    }
                    else
                    {
                        if (documentoProponente.NomeTipoDocumento.Contains(" - Não obrigatório"))
                            documentoProponente.NomeTipoDocumento = documentoProponente.NomeTipoDocumento.ToString().Replace(" - Não obrigatório", "");
                        documentoProponente.Anexado = !documentoProponente.IdArquivo.IsEmpty();
                        if (filtro.SituacaoDocumento.HasValue() && filtro.SituacaoDocumento.Value != documentoProponente.Situacao)
                        {
                            documentosDaProposta.Remove(documentoProponente);
                        }
                    }
                }
            }

            var response = documentosDaProposta.Select(x => new DocumentoProponenteDto().FromDomain(x))
                .AsQueryable().ToDataRequest(filtro.DataSourceRequest);

            return Response(response);
        }

        [HttpPost]
        [Route("proponentes/documentos")]
        [AuthenticateUserByToken("EVS10", "VisualizarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage IncluirDocumentoProponente(DocumentoProponenteCreateDto dto)
        {
            CanAccessPreProposta(dto.Documento.IdPreProposta);

            var response = new BaseResponse();
            try
            {
                var errors = new BusinessRuleException();
                if (dto.File == null || dto.Documento == null ||
                    dto.Documento.IdPreProposta.IsEmpty() ||
                    dto.Documento.IdTipoDocumento.IsEmpty() ||
                    dto.Documento.IdProponente.IsEmpty())
                {
                    errors.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
                }

                errors.ThrowIfHasError();

                var preProposta = _prePropostaRepository.FindById(dto.Documento.IdPreProposta);
                var proponente = _proponenteRepository.FindById(dto.Documento.IdProponente);
                var tipoDocumento = _tipoDocumentoRepository.FindById(dto.Documento.IdTipoDocumento);

                if (preProposta == null)
                {
                    //errors.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.PreProposta, documento.IdPreProposta.ToString()).Complete();
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.PreProposta, dto.Documento.IdPreProposta.ToString()).Complete();
                }

                if (proponente == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.Proponente, dto.Documento.IdProponente.ToString()).Complete();
                }

                if (tipoDocumento == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.TipoDocumento,
                        dto.Documento.IdTipoDocumento.ToString()).Complete();
                }

                errors.ThrowIfHasError();

                // Se houver documento anexado, ele deve ser utilizado novamente e não criado um novo
                // TODO: Está deixando Arquivos orfãos. Decido manter para histórico.
                DocumentoProponente documentoProponente =
                    _documentoProponenteRepository.BuscarDoTipoParaProponente(dto.Documento.IdPreProposta,
                        dto.Documento.IdProponente, dto.Documento.IdTipoDocumento);
                if (documentoProponente == null)
                {
                    documentoProponente = new DocumentoProponente();
                }

                if (SituacaoAprovacaoDocumento.Aprovado == documentoProponente.Situacao)
                {
                    errors.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                        .WithParams(tipoDocumento.Nome, proponente.Cliente.NomeCompleto).Complete();
                }

                errors.ThrowIfHasError();

                var nomeArquivo = string.Format("{0} - {1} - {2}.pdf", preProposta.Codigo,
                    proponente.Cliente.NomeCompleto, tipoDocumento.Nome);
                var arquivo = _arquivoService.CreateFile(dto.File.ToDomain(), nomeArquivo);

                _arquivoService.PreencherMetadadosDePdf(ref arquivo);

                documentoProponente.PreProposta = preProposta;
                documentoProponente.Proponente = proponente;
                documentoProponente.Situacao = SituacaoAprovacaoDocumento.Anexado;
                documentoProponente.TipoDocumento = tipoDocumento;
                documentoProponente.Motivo = dto.Documento.Motivo;
                documentoProponente.Arquivo = arquivo;
                documentoProponente.DataExpiracao = null;

                _documentoProponenteRepository.Save(documentoProponente);

                response.SuccessResponse(string.Format(GlobalMessages.DocumentoProponenteCarregadoSucesso,
                    tipoDocumento.Nome, proponente.Cliente.NomeCompleto));
            }
            catch (BusinessRuleException bre)
            {
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }

        [HttpGet]
        [Route("proponentes/{id}/documentos/baixarTodos/{codigoUf?}")]
        [AuthenticateUserByToken("EVS10", "BaixarTodosDocumentos")]
        public HttpResponseMessage BaixarTodosDocumentos(long id, string codigoUf = null)
        {
            byte[] file = _documentoProponenteService.ExportarTodosDocumentos(id, codigoUf);
            var preProposta = _prePropostaRepository.FindById(id);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");

            var dto = new FileDto
            {
                Bytes = file,
                FileName = $"{nomeArquivo}_Completo_{date}",
                Extension = "zip",
                ContentType = MimeMappingWrapper.MimeType.Zip
            };

            return Response(dto);
        }

        [HttpPost]
        [Route("proponentes/documentos/{id}")]
        [AuthenticateUserByToken("EVS10", "VisualizarProponente")]
        [Transaction(TransactionAttributeType.None)]
        public HttpResponseMessage ExcluirDocumentoProponente(long id)
        {
            var model = _documentoProponenteRepository.FindById(id);
            if (model.IsNull())
            {
                return Response(HttpStatusCode.NotFound);
            }
            else
            {
                CanAccessPreProposta(model.PreProposta.Id);
            }

            var transaction = _session.BeginTransaction();
            var response = new BaseResponse();
            try
            {
                _documentoProponenteRepository.Delete(model);
                _arquivoRepository.Delete(model.Arquivo);
                transaction.Commit();

                response.SuccessResponse(string.Format(GlobalMessages.RegistroSucesso, model.TipoDocumento.Nome,
                    GlobalMessages.Excluido.ToLower()));
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    response.ErrorResponse(string.Format(GlobalMessages.ErroExcluirDocumentoPreProposta));
                }
                else
                {
                    response.ErrorResponse(string.Format(GlobalMessages.ErroNaoTratado, exp.Message));
                }
                transaction.Rollback();
            }

            return Response(response);
        }

        [HttpPost]
        [Route("proponentes/documentos/negativa")]
        [AuthenticateUserByToken("EVS10", "VisualizarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage NegativaAnexarDocumentoProponente(NegativaDocumentoProponenteDto dto)
        {
            CanAccessPreProposta(dto.Documento.IdPreProposta);

            var response = new BaseResponse();
            try
            {
                var errors = new BusinessRuleException();

                var preProposta = _prePropostaRepository.FindById(dto.Documento.IdPreProposta);
                var proponente = _proponenteRepository.FindById(dto.Documento.IdProponente);
                var tipoDocumento = _tipoDocumentoRepository.FindById(dto.Documento.IdTipoDocumento);

                if (preProposta == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.PreProposta, dto.Documento.IdPreProposta.ToString()).Complete();
                }

                if (proponente == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.Proponente, dto.Documento.IdProponente.ToString()).Complete();
                }

                if (tipoDocumento == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.TipoDocumento,
                        dto.Documento.IdTipoDocumento.ToString()).Complete();
                }

                errors.ThrowIfHasError();

                DocumentoProponente documentoProponente =
                    _documentoProponenteRepository.BuscarDoTipoParaProponente(dto.Documento.IdPreProposta,
                        dto.Documento.IdProponente, dto.Documento.IdTipoDocumento);
                if (documentoProponente == null)
                {
                    documentoProponente = new DocumentoProponente();
                }

                // Não permitir alterar caso o documento já tenha sido informado
                if (SituacaoAprovacaoDocumento.Aprovado == documentoProponente.Situacao)
                {
                    errors.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                        .WithParams(tipoDocumento.Nome, proponente.Cliente.NomeCompleto).Complete();
                }

                errors.ThrowIfHasError();

                // Verificando motivo
                ValidarMotivo(dto.IdMotivo, dto.DescricaoMotivo);
                if (dto.IdMotivo > 0)
                {
                    var motivo = _motivoRepository.FindById(dto.IdMotivo.Value);

                    // Verificando se o o motivo de anexado em outro proponente procede
                    if (motivo.Id == ProjectProperties.MotivoDocumentoAnexadoOutroProponente)
                    {
                        var possuiAnexado =
                            _documentoProponenteRepository.PossuiDocumentoAnexadoEmOutroProponente(preProposta.Id,
                                proponente.Id, tipoDocumento.Id);

                        if (!possuiAnexado)
                        {
                            errors.AddError(GlobalMessages.NaoFoiEncontradoDocumentoAnexado)
                                .WithParam(tipoDocumento.Nome).Complete();
                            errors.ThrowIfHasError();
                        }
                    }

                    documentoProponente.Motivo = motivo.Descricao;
                }
                else
                {
                    documentoProponente.Motivo = dto.DescricaoMotivo;
                }

                documentoProponente.PreProposta = preProposta;
                documentoProponente.Proponente = proponente;
                documentoProponente.Situacao = SituacaoAprovacaoDocumento.Informado;
                documentoProponente.TipoDocumento = tipoDocumento;
                documentoProponente.DataExpiracao = null;

                _documentoProponenteRepository.Save(documentoProponente);
                response.SuccessResponse(string.Format(GlobalMessages.RegistroSucesso, documentoProponente.TipoDocumento.Nome,
                    GlobalMessages.Atualizado.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                FromBusinessRuleException(bre);
            }

            return Response(response);
        }

        private void ValidarMotivo(long? idMotivo, string descricaoMotivo)
        {
            var bre = new BusinessRuleException();
            if (idMotivo.IsNull())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Motivo)).Complete();
            }

            bre.ThrowIfHasError();
            if (idMotivo == 0 && descricaoMotivo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Detalhe)).Complete();
            }

            bre.ThrowIfHasError();
            if (idMotivo.IsEmpty() && descricaoMotivo.Length < 2)
            {
                bre.AddError(string.Format(GlobalMessages.CampoTamanhoMinimo, GlobalMessages.Detalhe, 1)).Complete();
            }

            bre.ThrowIfHasError();
        }

        #endregion

        [HttpPost]
        [Route("historico/listar")]
        [AuthenticateUserByToken("EVS10", "VisualizarHistoricoPreProposta")]
        public HttpResponseMessage ListarHistoricoPreProposta(FilterIdDto filtro)
        {
            var dataSource = _viewHistoricoPrePropostaRepository.Listar(filtro.DataSourceRequest, filtro.Id);
            var viewModels = dataSource.records.Select(x => new HistoricoPrePropostaDto().FromDomain(x)).AsQueryable();
            var response = dataSource.CloneDataSourceResponse(viewModels.ToList());
            return Response(response);
        }

        [HttpPost]
        [Route("{id}/fatorSocial")]
        [AuthenticateUserByToken("EVS10", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage MudarFatorSocial(long id, [FromBody] bool? fatorSocial)
        {
            var response = new BaseResponse();

            var exc = new ApiException();
            if (fatorSocial.IsEmpty())
            {
                exc.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FatorSocial));
            }
            exc.ThrowIfHasError();

            var result = _prePropostaService.MudarFatorSocial(id, fatorSocial.Value);
            response.SuccessResponse(string.Format(GlobalMessages.MsgPrePropostaSucesso, result.Codigo, GlobalMessages.Alterada.ToLower()));

            return Response(response);
        }

        [HttpPost]
        [Route("autocompleteEstadoIndique")]
        [AuthenticateUserByToken(true)]
        public HttpResponseMessage ListarEstadoIndiqueAutoComplete(DataSourceRequest request)
        {
            var _indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var result = _indiqueService.ListarEstadoAutocomplete(request);
            return Response(result);
        }
        [HttpPost]
        [Route("autocompleteCidadeIndique")]
        [AuthenticateUserByToken(true)]
        public HttpResponseMessage ListarCidadeIndiqueAutoComplete(DataSourceRequest request)
        {
            var _indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var result = _indiqueService.ListarCidadeAutocomplete(request);
            return Response(result);
        }
        [HttpPost]
        [Route("enviarIndicacao")]
        public HttpResponseMessage EnviarIndicacao(List<Tenda.EmpresaVenda.Domain.Integration.Indique.Models.IndicadoDto> indicacaoDto, long idCliente)
        {
            var baseResponse = new List<BaseResponse>();

            var _indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var cliente = _clienteRepository.FindById(idCliente);
            var clienteMgm = _indiqueService.BuscarClienteLandingPage(cliente.CpfCnpj);

            foreach (var indicacao in indicacaoDto)
            {
                indicacao.IdCliente = clienteMgm.Id;
                var result = _indiqueService.CadastrarIndicado(indicacao);
                baseResponse.Add(result);
            }
            return Response(baseResponse);
        }

        [HttpPost]
        [Route("salvarRecusaIndicacao")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage SalvarRecusaIndicacao(long idPreProposta)
        {
            var baseResponse = new BaseResponse();
            var usuario = RequestState.UsuarioPortal;
            var preproposta = _prePropostaRepository.FindById(idPreProposta);
            var model = new HistoricoRecusaIndicacao
            {
                PreProposta = preproposta,
                Responsavel = usuario,
                DataMomento = DateTime.Now,
                SituacaoPreProposta = preproposta.SituacaoProposta.Value
            };
            var recusaIndicacao = _historicoRecusaIndicacaoService.Salvar(model);
            baseResponse.Success = true;
            baseResponse.Data = recusaIndicacao.Id;

            return Response(baseResponse);
        }

        [HttpPost]
        [Route("enviarWhatsBotmakerIndicado")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage EnviarWhatsBotmakerIndicado(long id, string nome, string telefone)
        {
            var baseResponse = _indiqueService.EnviarWhatsBotmakerIndicado(id, nome, telefone);
            return Response(baseResponse);
        }

        [HttpPost]
        [Route("enviarWhatsBotmakerIndicador")]
        [Transaction(TransactionAttributeType.Required)]
        public HttpResponseMessage EnviarWhatsBotmakerIndicador(long id)
        {
            var baseResponse = _indiqueService.EnviarWhatsBotmakerIndicador(id);
            return Response(baseResponse);
        }
    }
}