using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PrePropostaService : BaseService
    {
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public ProponenteRepository _proponenteRepository { get; set; }
        private PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        public ClienteRepository _clienteRepository { get; set; }
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        public HistoricoPrePropostaService _historicoPrePropostaService { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        private AceiteRegraComissaoRepository _aceiteRegraComissaoRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ConsolidadoPrePropostaRepository _consolidadoPrePropostaRepository { get; set; }
        private HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        private BoletoPrePropostaRepository _boletoPrePropostaRepository { get; set; }
        private ContratoPrePropostaRepository _contratoPrePropostaRepository { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }
        private ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private TransferenciaCarteiraService _transferenciaCarteiraService { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private ViewPlanoPagamentoRepository _viewPlanoPagamentoRepository { get; set; }
        public DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }
        private DocumentoFormularioRepository _documentoFormularioRepository { get; set; }
        private FilaEmailService _filaEmailService { get; set; }
        private PlanoPagamentoValidator _planoPagamentoValidator { get; set; }
        private ClienteService _clienteService { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private GrupoCCAPrePropostaRepository _grupoCCAPrePropostaRepository { get; set; }
        private UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        private ViewCCAPrePropostaRepository _viewCCAPrePropostaRepository { get; set; }
        private GrupoCCARepository _grupoCCARepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private ViewRateioComissaoRepository _viewRateioComissaoRepository { get; set; }
        private ViewAceiteRegraComissaoRepository _viewAceiteRegraComissaoRepository { get; set; }
        private RateioComissaoRepository _rateioComissaoRepository { get; set; }
        public PreProposta Salvar(PreProposta model)
        {
            var isInclusao = model.Id.IsEmpty();
            //Remover Mascara
            model.CpfIndicador = model.CpfIndicador?.OnlyNumber();
            BusinessRuleException bre = new BusinessRuleException();

            //Validar dados
            ValidarPreProposta(model, bre);

            // Verifica se retornou algum erro
            bre.ThrowIfHasError();

            // Salvar Pré Proposta
            if (model.Id.IsEmpty())
            {
                //Criando código da Pré Proposta
                var sequence = _prePropostaRepository.BuscarCodigoPreProposta();
                model.Codigo = "PPR" + DateTime.Now.ToString("yyMMdd") + sequence.PadLeft(5, '0');
                model.DataElaboracao = DateTime.Now;
                model.SituacaoProposta = SituacaoProposta.EmElaboracao;
                model.FaixaUmMeio = false;
                model.ClienteCotista = model.Cliente.MesesFGTS >= 36;
                model.FatorSocial = model.Cliente.QuantidadeFilhos > 0;
            }

            _prePropostaRepository.Save(model);

            if (isInclusao)
            {
                // Cria e salva novo Proponente
                Proponente proponente = new Proponente
                {
                    PreProposta = model,
                    Cliente = model.Cliente,
                    Participacao = 100,
                    Titular = true,
                    RendaFormal = model.Cliente.RendaFormal,
                    RendaInformal = model.Cliente.RendaInformal
                };
                _proponenteRepository.Save(proponente);
            }

            return model;
        }

        public PreProposta AlterarSicaq(PreProposta model, UsuarioPortal usuario, List<Perfil> perfis)
        {
            BusinessRuleException bre = new BusinessRuleException();
            var preProposta = _prePropostaRepository.FindById(model.Id);
            if (!preProposta.IsEmpty())
            {
                // Valida se os dados estão preenchidos
                ValidationResult result = new AlteracaoSicaqPrePropostaValidator().Validate(model);
                bre.WithFluentValidation(result);
                bre.ThrowIfHasError();

                preProposta.StatusSicaq = model.StatusSicaq;
                preProposta.DataSicaq = model.DataSicaq;
                preProposta.FaixaUmMeio = model.FaixaUmMeio;
                preProposta.ParcelaAprovada = model.ParcelaAprovada;
                _prePropostaRepository.Save(preProposta);
                SituacaoProposta situacaoDestino = preProposta.SituacaoProposta.Value;
                switch (model.StatusSicaq)
                {
                    case StatusSicaq.Aprovado:
                        situacaoDestino = SituacaoProposta.AguardandoIntegracao;
                        break;
                    case StatusSicaq.Condicionado:
                        situacaoDestino = SituacaoProposta.Condicionada;
                        break;
                    case StatusSicaq.SICAQComErro:
                        situacaoDestino = SituacaoProposta.SICAQComErro;
                        break;
                    case StatusSicaq.Reprovado:
                        situacaoDestino = SituacaoProposta.Reprovada;
                        break;
                    default:
                        bre.AddField("StatusSicaq");
                        bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, "StatusSicaq"));
                        bre.ThrowIfHasError();
                        break;
                }

                preProposta.ContadorSicaq++;

                MudarSituacaoProposta(preProposta, situacaoDestino, usuario, null, perfis);
            }
            return preProposta;
        }

        public PreProposta AlterarSicaqPrevio(PreProposta model, UsuarioPortal usuario, List<Perfil> perfis)
        {
            var bre = new BusinessRuleException();

            var preProposta = _prePropostaRepository.FindById(model.Id);
            if (!preProposta.IsEmpty())
            {
                preProposta.StatusSicaqPrevio = model.StatusSicaq;
                preProposta.DataSicaqPrevio = model.DataSicaq;
                preProposta.FaixaUmMeioPrevio = model.FaixaUmMeio;
                preProposta.ParcelaAprovadaPrevio = model.ParcelaAprovada;

                SituacaoProposta situacaoDestino = preProposta.SituacaoProposta.Value;

                switch (model.StatusSicaq)
                {
                    case StatusSicaq.Aprovado:
                        situacaoDestino = SituacaoProposta.AguardandoFluxo;
                        break;
                    case StatusSicaq.Condicionado:
                        situacaoDestino = SituacaoProposta.Condicionada;
                        break;
                    case StatusSicaq.SICAQComErro:
                        situacaoDestino = SituacaoProposta.SICAQComErro;
                        break;
                    case StatusSicaq.Reprovado:
                        situacaoDestino = SituacaoProposta.Reprovada;
                        break;
                    default:
                        bre.AddField("StatusSicaq");
                        bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, "StatusSicaq"));
                        bre.ThrowIfHasError();
                        break;
                }

                preProposta.ContadorSicaq++;

                MudarSituacaoProposta(preProposta, situacaoDestino, usuario, null, perfis);

            }
            return preProposta;
        }

        private void ValidarPreProposta(PreProposta model, BusinessRuleException bre)
        {
            // Realiza as validações de PreProposta
            var prprResult = new PrePropostaValidator().Validate(model);

            // Adiciona os erros, caso existam
            bre.WithFluentValidation(prprResult);
        }

        public void RecalcularValores(long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            if (preProposta == null) { return; }

            preProposta.TotalDetalhamentoFinanceiro = _planoPagamentoRepository.SomatorioDetalhamentoFinanceiro(idPreProposta);
            preProposta.TotalItbiEmolumento = _planoPagamentoRepository.SomatorioItbiEmolumentos(idPreProposta);
            preProposta.Valor = _planoPagamentoRepository.SomatorioTotal(idPreProposta);
        }

        public void CancelarPreProposta(PreProposta preProposta)
        {
            preProposta.SituacaoProposta = SituacaoProposta.Cancelada;
            _prePropostaRepository.Save(preProposta);
        }

        public void RevisarPreProposta(PreProposta preProposta)
        {
            preProposta.SituacaoProposta = SituacaoProposta.EmElaboracao;
            _prePropostaRepository.Save(preProposta);
        }

        public Corretor AlterarCorretor(long idPreProposta, long? idCorretor)
        {
            var proposta = _prePropostaRepository.FindById(idPreProposta);
            BusinessRuleException bre = new BusinessRuleException();
            if (idCorretor.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, "Novo" + GlobalMessages.Corretor))
                .Complete();
            }
            bre.ThrowIfHasError();
            var corretor = _corretorRepository.FindById(idCorretor.Value);

            proposta.Corretor = corretor;
            _prePropostaRepository.Save(proposta);
            return corretor;
        }


        /// <summary>
        /// Não aterar a situação da pré-proposta para a situação destino antes de chamar este método. A informação antiga é importante para entender que houve
        ///    alteração de situação, criar o histórico e validar as regras.
        /// </summary>
        /// <param name="preProposta"></param>
        /// <param name="situacao"></param>
        /// <param name="responsavel"></param>
        public HistoricoPreProposta MudarSituacaoProposta(PreProposta preProposta, SituacaoProposta situacao, UsuarioPortal responsavel, HistoricoPreProposta historicoAtual = null, List<Perfil> perfis = null)
        {
            // Cria uma nova instancia da regra que será aplicada.
            var regrasParaTransicao = PrePropostaWorkflowDeclaration.RuleFor(preProposta.SituacaoProposta.Value, situacao);

            if (regrasParaTransicao.IsEmpty())
            {
                BusinessRuleException bre = new BusinessRuleException();
                bre.AddError(GlobalMessages.PrePropostaOutroAnalista).Complete();
                bre.ThrowIfHasError();
            }
            ValidadeValorParcelaSolicitada(preProposta.ParcelaSolicitada);
            ValidateDocumentRule(preProposta, situacao);

            // Setando os repositórios que poderão ser utilizados. Possívelmente fazer isso via AutoFac. 
            regrasParaTransicao.ForEach(reg =>
                    reg.WithPrePropostaRepository(_prePropostaRepository)
                       .WithProponenteRepository(_proponenteRepository)
                       .WithPlanoPagamentoRepository(_planoPagamentoRepository)
                       .WithClienteRepository(_clienteRepository)
                       .WithDocumentoProponenteRepository(_documentoProponenteRepository)
                       .WithTipoDocumentoRepository(_tipoDocumentoRepository)
                       .WithRegraComissaoRepository(_regraComissaoRepository)
                       .WithAceiteRegraComissaoRepository(_aceiteRegraComissaoRepository)
                       .WithNotificacaoRepository(_notificacaoRepository)
                       .WithRegraComissaoEvsRepository(_regraComissaoEvsRepository)
                       .WithAceiteRegraComissaoEvsRepository(_aceiteRegraComissaoEvsRepository)
                       .WithItemRegraComissaoRepository(_itemRegraComissaoRepository)
                       .WithDocumentoRuleMachinePrePropostaRepository(_documentoRuleMachinePrePropostaRepository)
                       .WithDocumentoFormularioRepository(_documentoFormularioRepository));

            // Juntar tudo em uma exception
            foreach (var regra in regrasParaTransicao)
            {
                regra.Validate(preProposta);
            }

            // Se chegou aqui, está tudo OK
            preProposta.SituacaoProposta = situacao;

            //atribuindo CCA
            preProposta = AtribuirUltimoCCA(preProposta, responsavel.Id);

            _prePropostaRepository.Save(preProposta);

            return _historicoPrePropostaService.CriarOuAvancarHistoricoPreProposta(preProposta, responsavel, historicoAtual, perfis);
        }

        public PreProposta AtribuirUltimoCCA(PreProposta preProposta, long idUsuario)
        {
            var situacoes = new List<SituacaoProposta> {
                SituacaoProposta.EmAnaliseSimplificada,
                SituacaoProposta.EmAnaliseCompleta
            };

            if (situacoes.Contains(preProposta.SituacaoProposta.Value))
            {
                var cca = _usuarioGrupoCCARepository.Queryable()
                    .Where(x => x.Usuario.Id == idUsuario)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
                if (cca.HasValue())
                {
                    preProposta.UltimoCCA = cca.GrupoCCA?.Descricao;
                }
            }

            return preProposta;
        }

        public void RetrocederPreProposta(PreProposta preProposta)
        {
            var historicoAtual = _historicoPrePropostaService.RetrocederHistorico(preProposta.Id);
            preProposta.SituacaoProposta = historicoAtual.SituacaoInicio;
            _prePropostaRepository.Save(preProposta);
        }

        public void DesassociarUnidade(PreProposta preProposta)
        {
            preProposta.IdSuat = 0;
            preProposta.IdUnidadeSuat = 0;
            preProposta.IdentificadorUnidadeSuat = null;
            _prePropostaRepository.Save(preProposta);
        }

        public PreProposta ClonarPreProposta(long idPreProposta, long? idBreveLancamento, long? idTorre, string obsTorre, string nomeTorre)
        {
            BusinessRuleException bre = new BusinessRuleException();
            PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
            BreveLancamento breveLancamento = new BreveLancamento();

            if (preProposta.IsEmpty())
            {
                bre.AddError(GlobalMessages.MsgPrePropostaNaoEncontrada).Complete();
                bre.ThrowIfHasError();
            }

            //Se empresa de venda está com as regras de comissão aceitas
            long idEmpresaVenda = preProposta.EmpresaVenda.Id;
            long idRegraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda).Id;
            bool possuiAceiteRegraEvsVigente = _aceiteRegraComissaoEvsRepository.BuscarAceiteParaRegraEvsAndEmpresaVenda(idRegraEvsVigente, idEmpresaVenda);
            if (!possuiAceiteRegraEvsVigente)
            {
                bre.AddError(GlobalMessages.RegrasComissaoPendentesAprovacao).Complete();
                bre.ThrowIfHasError();
            }

            if (!idBreveLancamento.IsEmpty())
            {
                breveLancamento = _breveLancamentoRepository.FindById(idBreveLancamento.Value);
            }
            if (idBreveLancamento.IsEmpty() || breveLancamento.IsEmpty())
            {
                bre.AddError(GlobalMessages.BreveLancamentoNaoEncontrado).Complete();
                bre.ThrowIfHasError();
            }
            if (idTorre.IsEmpty())
            {
                bre.AddError(GlobalMessages.Torre + " não encontrada.").Complete();
                bre.ThrowIfHasError();
            }


            // Buscando todas as informações a serem clonadas
            var planosPagamentos = _planoPagamentoRepository.ListarParcelas(preProposta.Id);
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);
            var documentos = _documentoProponenteRepository.BuscarDocumentosPorPreProposta(preProposta.Id);
            var antigaPreProposta = _prePropostaRepository.FindById(preProposta.Id);
            var formularios = _documentoFormularioRepository.FindByIdPreProposta(preProposta.Id);

            // Clonando Pré-Proposta
            _session.Evict(preProposta);
            preProposta.IdOrigem = preProposta.Id;
            preProposta.Id = 0;
            preProposta.IdSuat = 0;
            preProposta.IdUnidadeSuat = 0;
            preProposta.IdentificadorUnidadeSuat = "";
            var sequence = _prePropostaRepository.BuscarCodigoPreProposta();
            preProposta.Codigo = "PPR" + DateTime.Now.ToString("yyMMdd") + sequence.PadLeft(5, '0');
            preProposta.DataElaboracao = DateTime.Now;
            preProposta.SituacaoProposta = SituacaoProposta.EmElaboracao;

            preProposta.FaixaUmMeio = false;
            preProposta.ClienteCotista = preProposta.Cliente.MesesFGTS >= 36;
            preProposta.FatorSocial = preProposta.Cliente.QuantidadeFilhos > 0;

            preProposta.IdTorre = idTorre.Value;
            preProposta.ObservacaoTorre = obsTorre;
            preProposta.NomeTorre = nomeTorre;

            preProposta.BreveLancamento = breveLancamento;
            preProposta.ParcelaAprovada = antigaPreProposta.ParcelaAprovada;
            _prePropostaRepository.Save(preProposta);

            // Clonando Planos de Pagamento
            foreach (var plano in planosPagamentos)
            {
                _session.Evict(plano);
                plano.Id = 0;
                plano.IdSuat = 0;
                plano.PreProposta = preProposta;
                _planoPagamentoRepository.Save(plano);
            }

            // Clonando Proponentes
            foreach (var proponente in proponentes)
            {
                _session.Evict(proponente);
                proponente.Id = 0;
                proponente.IdSuat = 0;
                proponente.PreProposta = preProposta;
                _proponenteRepository.Save(proponente);
            }

            // Clonando Documentos
            foreach (var documento in documentos)
            {
                _session.Evict(documento);
                documento.Id = 0;
                documento.PreProposta = preProposta;
                documento.Situacao = SituacaoAprovacaoDocumento.Aprovado;
                _documentoProponenteRepository.Save(documento);
            }

            // Clonando Formulários
            foreach (var formulario in formularios)
            {
                _session.Evict(formulario);
                formulario.Id = 0;
                formulario.PreProposta = preProposta;
                _documentoFormularioRepository.Save(formulario);
            }

            return preProposta;
        }

        public void ClonarPreProposta(PreProposta preProposta, UsuarioPortal usuario, HistoricoPreProposta historicoAtual, List<Perfil> perfis)
        {
            var situacaoAnterior = _historicoPrePropostaRepository.HistoricoAnterior(preProposta.IdOrigem)
                .SituacaoInicio;

            var elaboracao = MudarSituacaoProposta(preProposta, SituacaoProposta.EmElaboracao,
                    usuario, null, perfis);

            var aguardandoAnaliseSimplificada = MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseSimplificada,
                usuario, elaboracao, perfis);

            var emAnaliseSimplificada = MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseSimplificada,
                usuario, aguardandoAnaliseSimplificada, perfis);

            var analiseSimplificadaAprovada = MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseSimplificadaAprovada,
                usuario, emAnaliseSimplificada, perfis);

            if (situacaoAnterior == SituacaoProposta.AnaliseSimplificadaAprovada)
            {
                return;
            }

            var aguardandoFluxo = MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoFluxo,
                usuario, analiseSimplificadaAprovada, perfis);

            var fluxoEnviado = MudarSituacaoProposta(preProposta, SituacaoProposta.FluxoEnviado,
                usuario, aguardandoFluxo, perfis);

            var aguardandoAnaliseCompleta = MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                usuario, fluxoEnviado, perfis);

            var emAnaliseCompleta = MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseCompleta,
                usuario, aguardandoAnaliseCompleta, perfis);

            var analiseCompletaAprovada = MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseCompletaAprovada,
                usuario, emAnaliseCompleta, perfis);

        }

        /// <summary>
        /// Precisa remover todas as associações de uma preproposta e remover ela
        /// </summary>
        /// <param name="clone"></param>
        public void RemoverClonePorFalha(PreProposta clone)
        {
            if (clone == null || clone.Id.IsEmpty()) { return; }

            //Removendo Documentos
            var documentosPreProposta = _documentoProponenteRepository.BuscarDocumentosPorPreProposta(clone.Id);
            foreach (var documento in documentosPreProposta)
            {
                _documentoProponenteRepository.Delete(documento);
            }

            //Removendo Proponentes
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(clone.Id);
            foreach (var proponente in proponentes)
            {
                _proponenteRepository.Delete(proponente);
            }

            //Planos de Pagamento
            var planosPagamento = _planoPagamentoRepository.ListarParcelas(clone.Id);
            foreach (var planoPagamento in planosPagamento)
            {
                _planoPagamentoRepository.Delete(planoPagamento);
            }

            var historicosPreProposta = _historicoPrePropostaRepository.HistoricoDaPreProposta(clone.Id);
            foreach (var historico in historicosPreProposta)
            {
                _historicoPrePropostaRepository.Delete(historico);
            }

            var consolidado = _consolidadoPrePropostaRepository.FindByPrePropostaId(clone.Id);
            //Pode ainda não ter sido criado
            if (consolidado != null)
            {
                _consolidadoPrePropostaRepository.Delete(consolidado);
            }

            var formularios = _documentoFormularioRepository.FindByIdPreProposta(clone.Id);
            foreach (var form in formularios)
            {
                _documentoFormularioRepository.Delete(form);
            }

            _prePropostaRepository.Delete(clone);
        }

        public byte[] BaixarBoleto(long idProposta)
        {
            var doc = _boletoPrePropostaRepository.BuscarBoletoMaisRecente(idProposta);

            if (doc.IsNull())
            {
                return null;
            }

            return doc.Boleto;
        }

        public byte[] BaixarContrato(long idProposta)
        {
            var doc = _contratoPrePropostaRepository.BuscarContratoMaisRecente(idProposta);

            if (doc.IsNull())
            {
                return null;
            }

            return doc.Contrato;
        }

        public void ValidarEmpreendimento(long idPreProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();

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

            ValidarRateioComissao(preProposta);

        }

        public void ValidarRateioComissao(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();

            if (preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                var empresaVenda = preProposta.EmpresaVenda;
                if(preProposta.BreveLancamento.Empreendimento == null)
                {
                    return;
                }
                var idEmpreendimento = preProposta.BreveLancamento.Empreendimento.Id;

                //Reteios existentes ativos para as evs envolvidas
                var rateios = _rateioComissaoRepository.Queryable()
                    .Where(x => x.Contratada.Id == empresaVenda.Id || x.Contratante.Id == empresaVenda.Id)
                    .Where(x => x.Empreendimento == null || x.Empreendimento.Id == idEmpreendimento)
                    .Where(x => x.Situacao == SituacaoRateioComissao.Ativo)
                    .ToList();

                var evs = rateios.Select(x => x.Contratada).ToList();
                evs.AddRange(rateios.Select(x => x.Contratante));

                //garantindo que a EV principal esteja no processo
                evs.Add(empresaVenda);

                // regras vigentes para as evs encontradas
                var regrasVigentes = _regraComissaoEvsRepository.Queryable()
                    .Where(x => evs.Contains(x.EmpresaVenda))
                    .Where(x=>x.Situacao==SituacaoRegraComissao.Ativo)
                    .ToList();

                //verificando se as evs possuem regras vigentes
                foreach(var ev in evs)
                {
                    var existe = regrasVigentes.Where(x => x.EmpresaVenda.Id == ev.Id).Any();

                    if (!existe)
                    {
                        bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, ev.NomeFantasia)).Complete();
                        bre.ThrowIfHasError();
                    }
                }

                var idRegras = regrasVigentes.Select(x => x.RegraComissao.Id).ToList();

                //lista de itens para as regras encontradas
                var itens = _itemRegraComissaoRepository.Queryable()
                    .Where(x => idRegras.Contains(x.RegraComisao.Id))
                    .Where(x => x.Empreendimento.Id == idEmpreendimento)
                    .ToList();

                //verificando se as evs envolvidas possuem item ativo
                foreach(var ev in evs)
                {
                    var itemExistente = itens.Where(x => x.Empreendimento.Id == idEmpreendimento)
                        .Where(x => x.EmpresaVenda.Id == ev.Id).Any();

                    if (!itemExistente)
                    {
                        bre.AddError(GlobalMessages.ErroItemRegraComissao).Complete();
                        bre.ThrowIfHasError();
                    }
                }

                var idRegrasEvs = regrasVigentes.Select(x => x.Id).ToList();
                var idEvs = evs.Select(x => x.Id).ToList();

                //listando aceites das evs envolvidas
                var aceites = _aceiteRegraComissaoEvsRepository.Queryable()
                    .Where(x => idRegrasEvs.Contains(x.RegraComissaoEvs.Id))
                    .Where(x => idEvs.Contains(x.EmpresaVenda.Id))
                    .Where(x => x.DataAceite != null)
                    .ToList();

                foreach(var ev in evs)
                {
                    var aceiteRegra = aceites.Where(x => x.EmpresaVenda.Id == ev.Id).Any();

                    if (!aceiteRegra)
                    {
                        bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, ev.NomeFantasia)).Complete();
                        bre.ThrowIfHasError();
                    }
                }




                //var rateioComissao = _viewRateioComissaoRepository.ListarPorEV(idEmpresaVenda);
                //RegraComissaoEvs regraEvsVigente = new RegraComissaoEvs();
                //ViewAceiteRegraComissao aceite = new ViewAceiteRegraComissao();
                //if (rateioComissao.HasValue())
                //{
                //    var rateiosComissaoContratada = _viewRateioComissaoRepository.ListarPorEVContratada(idEmpresaVenda);

                //    if (rateiosComissaoContratada.HasValue())
                //    {
                //        regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(rateiosComissaoContratada.FirstOrDefault().IdContratada);
                //        if (regraEvsVigente.IsNull())
                //        {
                //            bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratada.FirstOrDefault().NomeContratada)).Complete();
                //        }
                //        else
                //        {
                //           aceite =  _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(regraEvsVigente.Id)
                //                        .Where(reg => reg.IdEmpresaVenda == rateiosComissaoContratada.FirstOrDefault().IdContratada)
                //                        .Where (reg => reg.SituacaoRegraComissao == SituacaoRegraComissao.Ativo)
                //                        .Where (reg => reg.DataAceite != null)
                //                        .FirstOrDefault();
                //            if (aceite.IsNull())
                //            {
                //                bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratada.FirstOrDefault().NomeContratada)).Complete();
                //            }
                //        }
                        
                //        foreach (var rateio in rateiosComissaoContratada)
                //        {
                //            regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(rateio.IdContratante);
                //            if (regraEvsVigente.IsNull())
                //            {
                //                bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateio.NomeContratante)).Complete();
                //            }
                //            else
                //            {
                //                aceite = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(regraEvsVigente.Id)
                //                             .Where(reg => reg.IdEmpresaVenda == rateio.IdContratante)
                //                             .Where(reg => reg.SituacaoRegraComissao == SituacaoRegraComissao.Ativo)
                //                             .Where(reg => reg.DataAceite != null)
                //                             .FirstOrDefault();
                //                if (aceite.IsNull())
                //                {
                //                    bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratada.FirstOrDefault().NomeContratante)).Complete();
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var rateiosComissaoContratante = _viewRateioComissaoRepository.ListarPorEVContratante(idEmpresaVenda);

                //        regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(rateiosComissaoContratante.FirstOrDefault().IdContratante);
                //        if (regraEvsVigente.IsNull())
                //        {
                //            bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratante.FirstOrDefault().NomeContratante)).Complete();
                //        }
                //        else
                //        {
                //            aceite = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(regraEvsVigente.Id)
                //                         .Where(reg => reg.IdEmpresaVenda == rateiosComissaoContratante.FirstOrDefault().IdContratante)
                //                         .Where(reg => reg.SituacaoRegraComissao == SituacaoRegraComissao.Ativo)
                //                         .Where(reg => reg.DataAceite != null)
                //                         .FirstOrDefault();
                //            if (aceite.IsNull())
                //            {
                //                bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratada.FirstOrDefault().NomeContratante)).Complete();
                //            }
                //        }

                //        foreach (var rateio in rateiosComissaoContratante)
                //        {
                //            regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(rateio.IdContratada);
                //            if (regraEvsVigente.IsNull())
                //            {
                //                bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateio.NomeContratada)).Complete();
                //            }
                //            else
                //            {
                //                aceite = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(regraEvsVigente.Id)
                //                             .Where(reg => reg.IdEmpresaVenda == rateio.IdContratada)
                //                             .Where(reg => reg.SituacaoRegraComissao == SituacaoRegraComissao.Ativo)
                //                             .Where(reg => reg.DataAceite != null)
                //                             .FirstOrDefault();
                //                if (aceite.IsNull())
                //                {
                //                    bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, rateiosComissaoContratada.FirstOrDefault().NomeContratada)).Complete();
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);
                //    if (regraEvsVigente.IsNull())
                //    {
                //        bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, regraEvsVigente.EmpresaVenda.NomeFantasia)).Complete();
                //    }
                //    else
                //    {
                //        aceite = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(regraEvsVigente.Id)
                //                     .Where(reg => reg.IdEmpresaVenda == idEmpresaVenda)
                //                     .Where(reg => reg.SituacaoRegraComissao == SituacaoRegraComissao.Ativo)
                //                     .Where(reg => reg.DataAceite != null)
                //                     .FirstOrDefault();
                //        if (aceite.IsNull())
                //        {
                //            bre.AddError(string.Format(GlobalMessages.ErroRegraDeComissaoEV, regraEvsVigente.EmpresaVenda.NomeFantasia)).Complete();
                //        }
                //    }
                //}

                //    bre.ThrowIfHasError();

                //var itemRegraComissao = _itemRegraComissaoRepository.Buscar(regraEvsVigente.RegraComissao.Id, idEmpresaVenda, idEmpreendimento);
                //if (itemRegraComissao.IsEmpty())
                //{
                //    bre.AddError(GlobalMessages.ErroItemRegraComissao).Complete();
                //    bre.ThrowIfHasError();
                //}
            }

        }

        public void SalvarBreveLancamento(PrePropostaDTO prePropostaDTO)
        {
            var bre = new BusinessRuleException();

            var preProspota = _prePropostaRepository.FindById(prePropostaDTO.IdPreProposta);

            if (preProspota.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta)).Complete();
            }

            if (prePropostaDTO.IdBreveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Produto)).Complete();
            }

            if (prePropostaDTO.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }

            bre.ThrowIfHasError();

            var breveLancamento = _breveLancamentoRepository.FindById(prePropostaDTO.IdBreveLancamento);

            if (breveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.BreveLancamento)).Complete();
            }

            bre.ThrowIfHasError();

            preProspota.BreveLancamento = breveLancamento;
            preProspota.IdTorre = prePropostaDTO.IdTorre;
            preProspota.ObservacaoTorre = prePropostaDTO.ObservacaoTorre;
            preProspota.NomeTorre = prePropostaDTO.IdTorre == -1 ? "TORRE INEXISTENTE" : prePropostaDTO.NomeTorre;

            _prePropostaRepository.Save(preProspota);

        }
        public void SalvarNovoBreveLancamento(PreProposta preProposta, long? idBreveLancamento)
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (idBreveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, "Novo " + GlobalMessages.Produto)).Complete();
            }

            bre.ThrowIfHasError();

            preProposta.BreveLancamento = _breveLancamentoRepository.FindById(idBreveLancamento.Value);
            preProposta.PassoAtualSuat = null;
            preProposta.IdSuat = 0;
            preProposta.IdTorre = 0;
            preProposta.NomeTorre = null;
            preProposta.ObservacaoTorre = null;
            preProposta.IdUnidadeSuat = 0;
            preProposta.IdentificadorUnidadeSuat = null;

            _prePropostaRepository.Save(preProposta);

            var planosPagamento = _planoPagamentoRepository.ListarParcelas(preProposta.Id);
            foreach (var plan in planosPagamento)
            {
                plan.IdSuat = 0;
                _planoPagamentoRepository.Save(plan);
            }

            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);
            foreach (var prop in proponentes)
            {
                prop.IdSuat = 0;
                _proponenteRepository.Save(prop);
            }
        }

        public long TransferirCarteira(List<long> idsPreProposta, long idViabilizador)
        {
            var viabilizador = _usuarioPortalRepository.FindById(idViabilizador);
            var ListaPreproposta = _prePropostaRepository.BuscarPrePropostasAtivas(idsPreProposta);

            foreach (var preProposta in ListaPreproposta)
            {
                _transferenciaCarteiraService.Salvar(preProposta, preProposta.Viabilizador, viabilizador);

                preProposta.Viabilizador = viabilizador;
                _prePropostaRepository.Save(preProposta);
            }
            return ListaPreproposta.Count;
        }

        public PreProposta MudarFatorSocial(long idPreProposta, bool fatorSocial)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            preProposta.FatorSocial = fatorSocial;

            _prePropostaRepository.Save(preProposta);

            return preProposta;
        }

        #region Simulador

        public string MontarUrlSimulador(long idPreProposta)
        {
            var url = ProjectProperties.UrlSimulador + "Simulador/SimulacaoEmpresaVenda?";
            var bre = new BusinessRuleException();

            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            if (preProposta.IsEmpty())
            {
                bre.AddError("Pré-Proposta não encontrada").Complete();
            }
            bre.ThrowIfHasError();

            // FIXME: HMN: Usar dicionário ao invés de concatenar os registros desta forma
            //var parametros = new Dictionary<string, string>();
            //parametros.Add("returnUrl", "simulador");
            //parametros.Add("PreProposta", preProposta.Codigo);
            //parametros.Add("NomeCompleto", HttpUtility.UrlEncode(preProposta.Cliente.NomeCompleto));
            //var uri = string.Join("&", parametros.Select(reg => reg.Key + "=" + reg.Value));
            //url = url + uri;

            url += "PreProposta=" + preProposta.Codigo;
            url += "&NomeCompleto=" + HttpUtility.UrlEncode(preProposta.Cliente.NomeCompleto);
            url += "&Telefone=" + preProposta.Cliente.TelefoneResidencial;
            url += "&Email=" + preProposta.Cliente.Email;

            if (preProposta.Cliente.EstadoCivil.HasValue())
            {
                url += "&EstadoCivil=" + Convert.ToInt32(preProposta.Cliente.EstadoCivil);
            }
            if (preProposta.Cliente.DataNascimento.HasValue())
            {
                url += "&DataNascimento=" + preProposta.Cliente.DataNascimento.Value.ToDate();
            }

            var endereco = _enderecoClienteRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);

            if (endereco.HasValue())
            {
                url += "&Cep=" + endereco.Cep;
            }

            url += "&Cpf=" + preProposta.Cliente.CpfCnpj;
            url += "&PossuiFatorSocial=" + preProposta.FatorSocial.Value.ToString();

            var mesesFgts = preProposta.Cliente.MesesFGTS.HasValue() ? (preProposta.Cliente.MesesFGTS.Value >= 36 ? true : false) : false;

            url += "&PossuiTresAnosFgts=" + mesesFgts.ToString();
            url += "&Regional=" + preProposta.EmpresaVenda.Estado;

            if (preProposta.BreveLancamento.HasValue() && preProposta.BreveLancamento.Empreendimento.HasValue())
            {
                var divisao = preProposta.BreveLancamento.Empreendimento.IsEmpty() ? "" : preProposta.BreveLancamento.Empreendimento.Divisao;
                url += "&Divisao=" + divisao;
            }

            url += "&Fgts=" + preProposta.Cliente.FGTS.ToString().Replace(',', '.');
            url += "&RendaMensal=" + preProposta.Cliente.RendaMensal.ToString().Replace(',', '.');

            var request = new DataSourceRequest();
            var entrada = _viewPlanoPagamentoRepository
                .ListarDetalhamentoFinanceiro(request, preProposta.Id).records
                .Where(x => x.TipoParcela == TipoParcela.Ato).FirstOrDefault();

            if (entrada.HasValue())
            {
                url += "&Entrada=" + entrada.Total.ToString().Replace(',', '.');
            }

            return url;
        }

        public string MontarUrlSimulador(string tokenAcesso, long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            var bre = new BusinessRuleException();
            if (preProposta.IsEmpty())
            {
                new BusinessRuleException("Pré-Proposta não encontrada").ThrowIfHasError();
            }

            var url = ProjectProperties.UrlSimulador + "integracao/landing?token=" + HttpUtility.UrlEncode(tokenAcesso) + "&returnUrl=simulador";

            // FIXME: HMN: Usar dicionário ao invés de concatenar os registros desta forma
            //var parametros = new Dictionary<string, string>();
            //parametros.Add("returnUrl", "simulador");
            //parametros.Add("PreProposta", preProposta.Codigo);
            //parametros.Add("NomeCompleto", HttpUtility.UrlEncode(preProposta.Cliente.NomeCompleto));
            //var uri = string.Join("&", parametros.Select(reg => reg.Key + "=" + reg.Value));
            //url = url + uri;

            url += "&PreProposta=" + preProposta.Codigo;
            url += "&NomeCompleto=" + HttpUtility.UrlEncode(preProposta.Cliente.NomeCompleto);
            url += "&Telefone=" + preProposta.Cliente.TelefoneResidencial;
            url += "&Email=" + preProposta.Cliente.Email;

            if (preProposta.Cliente.EstadoCivil.HasValue())
            {
                url += "&EstadoCivil=" + Convert.ToInt32(preProposta.Cliente.EstadoCivil);
            }
            if (preProposta.Cliente.DataNascimento.HasValue())
            {
                // Não usar ToString, preferir sempre o .ToDate()
                url += "&DataNascimento=" + preProposta.Cliente.DataNascimento.Value.ToString("yyyy-MM-dd");
            }

            if (preProposta.Cliente.TipoSexo.HasValue())
            {
                if (preProposta.Cliente.TipoSexo.Value == TipoSexo.Masculino) {
                    url += "&Genero=1";
                }
                else if(preProposta.Cliente.TipoSexo.Value == TipoSexo.Feminino)
                {
                    url += "&Genero=2";
                }                
            }

            var endereco = _enderecoClienteRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);

            if (endereco.HasValue())
            {
                url += "&Cep=" + endereco.Cep;
            }

            url += "&Cpf=" + preProposta.Cliente.CpfCnpj;
            url += "&PossuiFatorSocial=" + preProposta.FatorSocial.Value.ToString();

            var mesesFgts = preProposta.Cliente.MesesFGTS.HasValue() ? (preProposta.Cliente.MesesFGTS.Value >= 36 ? true : false) : false;

            url += "&PossuiTresAnosFgts=" + mesesFgts.ToString();
            url += "&Regional=" + preProposta.EmpresaVenda.Estado;

            if (preProposta.BreveLancamento.HasValue() && preProposta.BreveLancamento.Empreendimento.HasValue())
            {
                var divisao = preProposta.BreveLancamento.Empreendimento.IsEmpty() ? "" : preProposta.BreveLancamento.Empreendimento.Divisao;
                url += "&Divisao=" + divisao;
            }

            url += "&Fgts=" + preProposta.Cliente.FGTS.ToString().Replace(',', '.');
            url += "&RendaMensal=" + preProposta.Cliente.RendaMensal.ToString().Replace(',', '.');

            var request = new DataSourceRequest();
            var entrada = _viewPlanoPagamentoRepository
                .ListarDetalhamentoFinanceiro(request, preProposta.Id).records
                .Where(x => x.TipoParcela == TipoParcela.Ato).FirstOrDefault();

            if (entrada.HasValue())
            {
                url += "&Entrada=" + entrada.Total.ToString().Replace(',', '.');
            }

            return url;
        }

        public string ParametroMatrizOferta(long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            if (preProposta.IsEmpty())
            {
                new BusinessRuleException(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.PreProposta)).ThrowIfHasError();
            }

            // FIXME: Ver sugestão de dicionário em MontarUrlSimulador
            var parametro = "&";
            parametro += "PreProposta=" + preProposta.Codigo;
            parametro += "&Cpf=" + preProposta.Cliente.CpfCnpj.OnlyNumber();
            parametro += "&NomeCompleto=" + HttpUtility.UrlEncode(preProposta.Cliente.NomeCompleto);

            if (preProposta.Cliente.EstadoCivil.HasValue())
            {
                parametro += "&EstadoCivil=" + preProposta.Cliente.EstadoCivil.AsString();
            }

            if (preProposta.Cliente.TipoSexo.HasValue())
            {
                if (preProposta.Cliente.TipoSexo.Value == TipoSexo.Masculino)
                {
                    parametro += "&Genero=1";
                }
                else if (preProposta.Cliente.TipoSexo.Value == TipoSexo.Feminino)
                {
                    parametro += "&Genero=2";
                }
            }

            if (preProposta.Cliente.DataNascimento.HasValue())
            {
                // Não usar ToString, preferir sempre o .ToDate()
                parametro += "&DataNascimento=" + preProposta.Cliente.DataNascimento.Value.ToString("yyyy-MM-dd");
            }

            parametro += "&Telefone=" + preProposta.Cliente.TelefoneResidencial;
            parametro += "&Email=" + preProposta.Cliente.Email;

            var enderecoCliente = _enderecoClienteRepository.FindByCliente(preProposta.Cliente.Id);

            if (enderecoCliente.HasValue() && enderecoCliente.Cep.HasValue())
            {
                parametro += "&Cep=" + enderecoCliente.Cep.OnlyNumber();
            }

            parametro += "&PossuiFilhos=" + preProposta.Cliente.QuantidadeFilhos.HasValue();

            if (preProposta.Cliente.QuantidadeFilhos.HasValue())
            {
                parametro += "&QtdFilhos=" + preProposta.Cliente.QuantidadeFilhos;
            }

            if (preProposta.Cliente.TipoResidencia.HasValue())
            {
                parametro += "&TipoResidencia=" + preProposta.Cliente.TipoResidencia.AsString();
            }

            var mesesFgts = preProposta.Cliente.MesesFGTS.HasValue() ? (preProposta.Cliente.MesesFGTS.Value >= 36 ? true : false) : false;
            parametro += "&PossuiTresAnosFgts=" + mesesFgts.ToString();

            if (preProposta.Cliente.FGTS.HasValue())
            {
                parametro += "&Fgts=" + preProposta.Cliente.FGTS.ToString().Replace(',', '.');
            }

            if (preProposta.Cliente.RendaMensal.HasValue())
            {
                parametro += "&RendaMensal=" + preProposta.Cliente.RendaMensal.ToString().Replace(',', '.');
            }

            var request = new DataSourceRequest();
            var entrada = _viewPlanoPagamentoRepository
                .ListarDetalhamentoFinanceiro(request, preProposta.Id).records
                .Where(x => x.TipoParcela == TipoParcela.Ato).FirstOrDefault();

            if (entrada.HasValue())
            {
                parametro += "&Entrada=" + entrada.Total.ToString().Replace(',', '.');
            }

            parametro += "&Regional=" + preProposta.EmpresaVenda.Estado;

            return parametro;
        }

        public DataSourceResponse<ViewPlanoPagamento> DetalhamentoFinanceiroBySimulador(DataSourceRequest request, SimuladorDto parametro)
        {
            var response = new List<ViewPlanoPagamento>();

            var simuladorService = new SimuladorService();
            var simulacao = simuladorService.BuscarSimulacaoPorCodigo(parametro);

            if (simulacao.HasValue())
            {
                response = MontarPlanoPagamentoSimulador(simulacao,parametro.IdPreProposta);
            }

            return CastDatatablePlanoPagamento(response, request);
        }

        public DataSourceResponse<ViewPlanoPagamento> CastDatatablePlanoPagamento(List<ViewPlanoPagamento> planoPagamento, DataSourceRequest request)
        {
            var query = planoPagamento.AsQueryable();

            if (request.order.HasValue())
            {
                var orderTipoParcela = request.order.Where(x => x.column.Equals("TipoParcela")).SingleOrDefault();

                if (orderTipoParcela.HasValue())
                {
                    query = orderTipoParcela.IsAsc() ? query.OrderBy(x => x.TipoParcela.AsString()) : query.OrderByDescending(x => x.TipoParcela.AsString());
                }

            }

            var filtered = query.Count();
            var total = query.Count();

            query = query.Skip(request.start);
            if (request.pageSize > 0)
            {
                query = query.Take(request.pageSize);
            }

            return new DataSourceResponse<ViewPlanoPagamento> { records = query.ToList(), total = total, filtered = filtered };
        }

        public DataSourceResponse<ViewPlanoPagamento> ItbiEmolumento(DataSourceRequest request, long idPreProposta)
        {
            var lista = _viewPlanoPagamentoRepository.ListarItbiEmolumentos(request, idPreProposta).records.ToList();

            return CastDatatablePlanoPagamento(lista, request);
        }

        public DataSourceResponse<ViewPlanoPagamento> DetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            var lista = _viewPlanoPagamentoRepository.ListarDetalhamentoFinanceiro(request, idPreProposta).records.ToList();

            return CastDatatablePlanoPagamento(lista, request);
        }

        public List<ViewPlanoPagamento>MontarPlanoPagamentoSimulador(SimuladorDto simulacao,long idPreProposta)
        {
            var id = 1;
            //var preProposta = _prePropostaRepository.BuscarPorCodigo(simulacao.CodigoPreProposta);

            var planoPagamento = new List<ViewPlanoPagamento>();

            #region Detalhamento Financeiro

            //Entrada
            if (simulacao.ValorEntrada.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorEntrada;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataVencimentoEntrada;
                parcela.TipoParcela = TipoParcela.Ato;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;
                parcela.IdPreProposta = idPreProposta;

                planoPagamento.Add(parcela);
            }

            //FGTS
            if (simulacao.ValorFgts.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorFgts;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataVencimentoEntrada.AddDays(59).Date;
                parcela.TipoParcela = TipoParcela.FGTS;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Financiamento
            if (simulacao.FinanciamentoPreAprovado.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.FinanciamentoPreAprovado;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataVencimentoEntrada.AddDays(59).Date;
                parcela.TipoParcela = TipoParcela.Financiamento;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Pós-Chaves
            if (simulacao.ValorParcelaNegociadaPosChaves.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaNegociadaPosChaves;
                parcela.NumeroParcelas = simulacao.QuantidadeParcelaNegociadaPosChaves;
                parcela.DataVencimento = simulacao.DataVencimentoPos;
                parcela.TipoParcela = TipoParcela.PosChaves;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Pré-Chaves
            if (simulacao.ValorParcelaNegociadaPreChaves.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaNegociadaPreChaves;
                parcela.NumeroParcelas = simulacao.QuantidadeParcelaNegociadaPreChaves;
                parcela.DataVencimento = simulacao.DataVencimentoPre;
                parcela.TipoParcela = TipoParcela.PreChaves;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Pré-Cahves Intermediária
            if (simulacao.ValorPrimeiraIntermediaria.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorPrimeiraIntermediaria;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataPrimeiraIntermediaria.GetValueOrDefault();
                parcela.TipoParcela = TipoParcela.PreChavesIntermediaria;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            if (simulacao.ValorSegundaIntermediaria.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorSegundaIntermediaria;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataSegundaIntermediaria.GetValueOrDefault();
                parcela.TipoParcela = TipoParcela.PreChavesIntermediaria;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Subsídio
            if (simulacao.FaixaUmMeioPotencial && simulacao.SubsidioFaixaUmMeio.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.SubsidioFaixaUmMeio;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataVencimentoEntrada.AddDays(59).Date;
                parcela.TipoParcela = TipoParcela.Subsidio;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }
            else if (!simulacao.FaixaUmMeioPotencial && simulacao.SubsidioFaixaDois.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.SubsidioFaixaDois;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.DataVencimentoEntrada.AddDays(59).Date;
                parcela.TipoParcela = TipoParcela.Subsidio;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;

                planoPagamento.Add(parcela);
            }

            //Premiada Tenda
            if (simulacao.ValorParcelaPremiadaTenda.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaPremiadaTenda;
                parcela.NumeroParcelas = 1;
                parcela.DataVencimento = simulacao.InicioPagamentoPosChaves.Value;
                parcela.TipoParcela = TipoParcela.PremiadaTenda;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;

                planoPagamento.Add(parcela);
            }
            #endregion

            #region ITBI/Emolumentos

            //Pós-Chave ITBI
            if (simulacao.PagamentoItbiParcelado && simulacao.ValorParcelaPosChavesItbi.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaPosChavesItbi;
                parcela.NumeroParcelas = simulacao.QuantidadeParcelaPosChavesItbi;
                parcela.DataVencimento = simulacao.DataVencimentoPosItbi;
                parcela.TipoParcela = TipoParcela.PosChavesItbi;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }

            //Pré - Chaves Intermediária
            if (simulacao.ValorParcelaIntermediariaItbi.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaIntermediariaItbi;
                parcela.NumeroParcelas = simulacao.QuantidadeParcelaIntermediariaItbi;
                parcela.DataVencimento = DateTime.MinValue;
                parcela.TipoParcela = TipoParcela.PreChavesIntermediariaItbi;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }
            //Pré - Chaves ITBI
            if (simulacao.PagamentoItbiParcelado && simulacao.ValorParcelaPreChavesItbi.HasValue())
            {
                var parcela = new ViewPlanoPagamento();
                parcela.Id = id++;

                parcela.ValorParcela = simulacao.ValorParcelaPreChavesItbi;
                parcela.NumeroParcelas = simulacao.QuantidadeParcelaPreChavesItbi;
                parcela.DataVencimento = simulacao.DataVencimentoPreItbi.HasValue()?simulacao.DataVencimentoPreItbi:DateTime.MinValue;
                parcela.TipoParcela = TipoParcela.PreChavesItbi;
                parcela.IdPreProposta = idPreProposta;
                parcela.Total = parcela.ValorParcela * parcela.NumeroParcelas;


                planoPagamento.Add(parcela);
            }
            #endregion

            return planoPagamento;
        }

        public List<string> AplicarDetalhamentoFinanceiro(SimuladorDto parametro)
        {
            var bre = new BusinessRuleException();
            var msgs = new List<string>();

            var simuladorService = new SimuladorService();
            var simulacao = simuladorService.BuscarSimulacaoFinalizadaEmpresaVenda(parametro);

            if (simulacao.IsEmpty() || (simulacao.HasValue() && !simulacao.CodigoPreProposta.Equals(parametro.CodigoPreProposta)))
            {
                simulacao = simuladorService.BuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda(parametro);
            }

            if (simulacao.IsEmpty())
            {
                bre.AddError(string.Format("A simulação {0}-{1} não foi encontrada.", parametro.Codigo, parametro.Digito)).Complete();
                bre.ThrowIfHasError();
            }

            simulacao.IdProposta = parametro.IdProposta;

            //removendo parcelas anteriores
            var detalhamentoFinanceiro = MontarPlanoPagamentoSimulador(simulacao,parametro.IdProposta.Value);
            var idPreProposta = detalhamentoFinanceiro.First().IdPreProposta;
            var tipos = detalhamentoFinanceiro.Select(x => x.TipoParcela);

            var antigo = _planoPagamentoRepository.Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => tipos.Contains(x.TipoParcela));

            foreach (var plano in antigo)
            {
                _planoPagamentoRepository.Delete(plano);
            }

            //atualizando dados do plano de pagamento
            foreach (var parcela in detalhamentoFinanceiro)
            {
                var plano = new PlanoPagamento();
                plano.IdSuat = 0;
                plano.ValorParcela = parcela.ValorParcela;
                plano.NumeroParcelas = parcela.NumeroParcelas;

                if (parcela.DataVencimento.IsEmpty())
                {
                    msgs.Add(string.Format("Verifique da data de vencimento da {0} antes de avançar.", parcela.TipoParcela.AsString()));
                    parcela.DataVencimento = DateTime.Now.AddDays(1);
                }

                plano.DataVencimento = parcela.DataVencimento;
                plano.TipoParcela = parcela.TipoParcela;
                plano.PreProposta = new PreProposta { Id = parcela.IdPreProposta };
                plano.Total = parcela.Total;

                var validate = _planoPagamentoValidator.Validate(plano);
                bre.WithFluentValidation(validate);
                bre.ThrowIfHasError();

                _planoPagamentoRepository.Save(plano);

            }

            var preProposta = _prePropostaRepository.BuscarPorCodigo(simulacao.CodigoPreProposta);

            preProposta.TotalDetalhamentoFinanceiro = detalhamentoFinanceiro
                .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
                .Sum(x => x.Total);

            preProposta.TotalItbiEmolumento = detalhamentoFinanceiro
                .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
                .Sum(x => x.Total);

            preProposta.Valor = detalhamentoFinanceiro.Sum(x => x.Total);

            _prePropostaRepository.Save(preProposta);

            //atualizando dados pre-proposta ao aplicar simulação
            preProposta= AtualizarDadosPrePropostaPorSimulacao(preProposta,simulacao, ref msgs);

            //Atualizando dados do cliente
            var cliente = _prePropostaRepository.Queryable()
                .Where(x => x.Id == simulacao.IdProposta)
                .Select(x => x.Cliente)
                .SingleOrDefault();

            AtualizarClientePorSimulacao(cliente, simulacao, ref msgs);

            return msgs;
        }

        public List<string> AplicarSimulacaoAtual(SimuladorDto parametro)
        {
            var bre = new BusinessRuleException();

            if (parametro.IdProposta.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta)).Complete();
                bre.ThrowIfHasError();
            }

            var preProposta = _prePropostaRepository.FindById(parametro.IdProposta.Value);

            if (preProposta.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta)).Complete();
                bre.ThrowIfHasError();
            }

            parametro.Codigo = preProposta.CodigoSimulacao;
            parametro.Digito = preProposta.DigitoSimulacao;
            parametro.CodigoPreProposta = preProposta.Codigo;

            var msgs = AplicarDetalhamentoFinanceiro(parametro);

            return msgs;
        }

        //FIX-ME: Tenho que unificar o objeto da simulação
        private PreProposta AtualizarDadosPrePropostaPorSimulacao(PreProposta preProposta,SimuladorDto simulacao, ref List<string> msgs)
        {
            if (simulacao.Divisao.HasValue())
            {
                var regraComissao = ValidarRegraComissao(preProposta.EmpresaVenda, simulacao);

                var breveLancamento = _breveLancamentoRepository.FindByDivisao(simulacao.Divisao);

                if (breveLancamento.IsEmpty())
                {
                    msgs.Add(string.Format("O Produto da simulação, {0}, não foi encontrado neste ambiente", simulacao.Produto));
                }
                else
                {
                    preProposta.BreveLancamento = breveLancamento;
                }

                if (simulacao.Torre.HasValue() && simulacao.Unidade.HasValue())
                {
                    if (!preProposta.BreveLancamento.IsNull())
                    {
                        var service = new SuatService();
                        var estoqueTorre = service.EstoqueTorre(preProposta.BreveLancamento.Empreendimento.Divisao);
                        var torre = estoqueTorre.Find(x => x.NomeTorre.ToLower().Equals(simulacao.Torre.ToLower()));

                        if (torre.HasValue())
                        {
                            preProposta.IdTorre = torre.IdTorre;
                            preProposta.NomeTorre = torre.NomeTorre;
                            preProposta.ObservacaoTorre = String.Format("{0}, unidade {1}", simulacao.Torre, simulacao.Unidade);
                        }
                        else
                        {
                            msgs.Add(string.Format("Durante a aplicação da Simulação não foi encontrado a Torre {0} nas EVS, foi selecionado Torre inexistente.", simulacao.Torre));
                            preProposta.IdTorre = -1;
                            preProposta.NomeTorre = "TORRE INEXISTENTE";
                            preProposta.ObservacaoTorre = null;
                        }
                    }
                }
            }

            _prePropostaRepository.Save(preProposta);

            return preProposta;
        }

        public ItemRegraComissao ValidarRegraComissao(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,SimuladorDto simulacao)
        {
            if (empresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
            {
                return null;
            }

            var apiEx = new ApiException();

            var regraComissaoEvs = _regraComissaoEvsRepository.BuscarRegraEvsVigente(empresaVenda.Id);

            if (regraComissaoEvs.IsEmpty())
            {
                apiEx.AddError(string.Format("Empresa de Vendas {0} não possui regra de comissão ativa", empresaVenda.NomeFantasia));
                apiEx.ThrowIfHasError();
            }

            var itemRegraComissao = _itemRegraComissaoRepository.Queryable()
                .Where(x => x.RegraComisao.Id == regraComissaoEvs.RegraComissao.Id)
                .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                .Where(x => x.Empreendimento.Divisao.Equals(simulacao.Divisao))
                .SingleOrDefault();

            if (itemRegraComissao.IsEmpty())
            {
                apiEx.AddError(string.Format("A Empresa de Venda {0} não possui regra de comissao ativa para o Empreendimento {1}", empresaVenda.NomeFantasia, simulacao.Produto));
                apiEx.ThrowIfHasError();
            }

            return itemRegraComissao;
        }

        public List<string> AplicarDetalhamentoFinanceiroPorIntegracao(Tenda.EmpresaVenda.ApiService.Models.Simulador.SimuladorDto parametro, PreProposta preProposta)
        {
            var bre = new BusinessRuleException();
            var msgs = new List<string>();

            var parametroAuxiliar = new SimuladorDto();
            parametroAuxiliar.Codigo = parametro.Codigo;
            parametroAuxiliar.Digito = parametro.Digito;

            var simulacao = new SimuladorDto();
            var simuladorService = new SimuladorService();

            if (parametro.MatrizOferta)
            {
                simulacao = simuladorService.BuscarSimulacaoMatrizOfertaFinalizadaEmpresaVenda(parametroAuxiliar);
            }
            else
            {
                simulacao = simuladorService.BuscarSimulacaoFinalizadaEmpresaVenda(parametroAuxiliar);
            }

            if (simulacao.IsEmpty())
            {
                bre.AddError(string.Format("A simulação {0}-{1} não foi encontrada.", parametro.Codigo, parametro.Digito)).Complete();
                bre.ThrowIfHasError();
            }

            //removendo parcelas anteriores

            var idPreProposta = preProposta.Id;
            var detalhamentoFinanceiro = MontarPlanoPagamentoSimulador(simulacao,idPreProposta);
            var tipos = detalhamentoFinanceiro.Select(x => x.TipoParcela);

            var antigo = _planoPagamentoRepository.Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => tipos.Contains(x.TipoParcela));

            foreach (var plano in antigo)
            {
                _planoPagamentoRepository.Delete(plano);
            }

            //atualizando dados do plano de pagamento
            foreach (var parcela in detalhamentoFinanceiro)
            {
                var plano = new PlanoPagamento();
                plano.IdSuat = 0;
                plano.ValorParcela = parcela.ValorParcela;
                plano.NumeroParcelas = parcela.NumeroParcelas;

                if (parcela.DataVencimento.IsEmpty())
                {
                    msgs.Add(string.Format("Verifique da data de vencimento da {0} antes de avançar.", parcela.TipoParcela.AsString()));
                    parcela.DataVencimento = DateTime.Now.AddDays(1);
                }

                plano.DataVencimento = parcela.DataVencimento;
                plano.TipoParcela = parcela.TipoParcela;
                plano.PreProposta = new PreProposta { Id = idPreProposta };
                plano.Total = parcela.Total;

                var validate = _planoPagamentoValidator.Validate(plano);
                bre.WithFluentValidation(validate);
                bre.ThrowIfHasError();

                _planoPagamentoRepository.Save(plano);
            }

            var totalDetalhamentoFinanceiro = detalhamentoFinanceiro
                .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
                .ToList();

            var totalItbi = detalhamentoFinanceiro
                .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
                .ToList();

            preProposta.TotalDetalhamentoFinanceiro = totalDetalhamentoFinanceiro.Any() ? totalDetalhamentoFinanceiro.Sum(x => x.Total) : 0;
            preProposta.TotalItbiEmolumento = totalItbi.Any() ? totalItbi.Sum(x => x.Total) : 0;
            preProposta.Valor = detalhamentoFinanceiro.Any() ? detalhamentoFinanceiro.Sum(x => x.Total) : 0;

            AtualizarClientePorSimulacao(preProposta.Cliente, parametro, ref msgs);

            preProposta = AtualizarDadosPrePropostaPorSimulacao(preProposta,simulacao, ref msgs);

            _prePropostaRepository.Save(preProposta);

            return msgs;
        }

        public void AtualizarTotalFinanceiro(long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            if (preProposta == null) { return; }

            preProposta.TotalDetalhamentoFinanceiro = _planoPagamentoRepository.SomatorioDetalhamentoFinanceiro(idPreProposta);
            preProposta.TotalItbiEmolumento = _planoPagamentoRepository.SomatorioItbiEmolumentos(idPreProposta);
            preProposta.Valor = _planoPagamentoRepository.SomatorioTotal(idPreProposta);

            _prePropostaRepository.Save(preProposta);
        }

        public PreProposta PrePropostaSimulador(PrePropostaDTO prePropostaDTO)
        {

            var apiEx = new ApiException();

            var preProposta = _prePropostaRepository.Queryable()
                .Where(x => x.CodigoSimulacao.Equals(prePropostaDTO.SimuladorDto.Codigo))
                .Where(x => x.DigitoSimulacao.Equals(prePropostaDTO.SimuladorDto.Digito))
                .SingleOrDefault();

            try
            {
                if (preProposta.HasValue())
                {
                    AplicarDetalhamentoFinanceiroPorIntegracao(prePropostaDTO.SimuladorDto, preProposta);

                    return preProposta;
                }

                if (prePropostaDTO.ClienteDto.CpfCnpj.IsEmpty())
                {
                    apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cpf));
                    apiEx.ThrowIfHasError();
                }

                var breveLancamento = new BreveLancamento();
                preProposta = new PreProposta();

                var cliente = _clienteRepository.ListarClientes(null, null, null, prePropostaDTO.ClienteDto.CpfCnpj.OnlyNumber(), prePropostaDTO.Corretor.EmpresaVenda.Id, prePropostaDTO.Corretor.Id).FirstOrDefault();
                preProposta.Cliente = cliente;
                if (cliente.IsEmpty())
                {
                    var bre = new BusinessRuleException();
                    preProposta.Cliente = new Cliente();

                    preProposta.Cliente.CpfCnpj = prePropostaDTO.ClienteDto.CpfCnpj;
                    preProposta.Cliente.NomeCompleto = prePropostaDTO.ClienteDto.Nome;
                    //cliente.TipoSexo = TipoSexo.Outros;//"Retirar a obrigatoriedade"
                    preProposta.Cliente.TelefoneResidencial = prePropostaDTO.ClienteDto.TelefoneResidencial;
                    preProposta.Cliente.TelefoneComercial = prePropostaDTO.ClienteDto.TelefoneCelular;
                    preProposta.Cliente.Email = prePropostaDTO.ClienteDto.EmailPrincipal;
                    //cliente.DataAdmissao = DateTime.Now;//"Retirar a obrigatoriedade"

                    preProposta.Cliente.TipoPessoa = TipoPessoa.Fisica;
                    preProposta.Cliente.DataNascimento = prePropostaDTO.ClienteDto.DataNascimento;
                    preProposta.Cliente.EstadoCivil = prePropostaDTO.ClienteDto.EstadoCivil;
                    preProposta.Cliente.TipoSexo = prePropostaDTO.ClienteDto.Genero;
                    preProposta.Cliente.Corretor = prePropostaDTO.Corretor.Id;
                    preProposta.Cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = prePropostaDTO.Corretor.EmpresaVenda.Id };

                    preProposta.Cliente.FGTS = prePropostaDTO.SimuladorDto.ValorFgts;

                    preProposta.Cliente = _clienteService.Salvar(preProposta.Cliente, bre);

                    apiEx.AddError(bre.Errors);

                    apiEx.ThrowIfHasError();

                }

                if (prePropostaDTO.Divisao.HasValue())
                {
                    breveLancamento = _breveLancamentoRepository.Queryable()
                        .Where(x => x.Empreendimento.Divisao.Equals(prePropostaDTO.Divisao))
                        .SingleOrDefault();
                }

                if (breveLancamento.IsEmpty())
                {
                    breveLancamento = _enderecoBreveLancamentoRepository.Queryable()
                        .Where(x => x.Estado.Equals(prePropostaDTO.Corretor.EmpresaVenda.Estado))
                        .Where(x => x.BreveLancamento != null)
                        .Where(x => x.BreveLancamento.DisponivelCatalogo)
                        .Where(x => x.BreveLancamento.Nome.ToUpper().Equals(prePropostaDTO.NomeEmpreendimento.ToUpper()))
                        .Select(x => x.BreveLancamento)
                        .SingleOrDefault();
                }

                if (breveLancamento.IsEmpty())
                {
                    apiEx.AddError(string.Format("Empreendimento não cadastrado"));
                    apiEx.ThrowIfHasError();
                }

                preProposta.BreveLancamento = new BreveLancamento { Id = breveLancamento.Id };

                if (prePropostaDTO.Torre.HasValue() && prePropostaDTO.Unidade.HasValue())
                {
                    if (!preProposta.BreveLancamento.IsNull())
                    {
                        var service = new SuatService();
                        var estoqueTorre = service.EstoqueTorre(prePropostaDTO.Divisao);
                        var torre = estoqueTorre.Find(x => x.NomeTorre.ToLower().Equals(prePropostaDTO.Torre.ToLower()));

                        preProposta.IdTorre = torre.IdTorre;
                        preProposta.NomeTorre = torre.NomeTorre;
                    }

                    preProposta.ObservacaoTorre = String.Format("{0}, unidade {1}", prePropostaDTO.Torre, prePropostaDTO.Unidade);
                }

                if (preProposta.BreveLancamento.HasValue() && preProposta.IdTorre.IsEmpty())
                {
                    preProposta.IdTorre = -1;
                    preProposta.NomeTorre = "TORRE INEXISTENTE";
                }

                preProposta.OrigemCliente = TipoOrigemCliente.Simulador;
                preProposta.CodigoSimulacao = prePropostaDTO.SimuladorDto.Codigo;
                preProposta.DigitoSimulacao = prePropostaDTO.SimuladorDto.Digito;

                preProposta.Elaborador = prePropostaDTO.Corretor;

                //preProposta.Regiao = _estadoCidadeRepository.FindByEstado(prePropostaDTO.EmpresaVenda.Estado)
                //    .FirstOrDefault();//"colocar a primeira região";
                preProposta.ParcelaSolicitada = 1;
                preProposta.PontoVenda = _pontoVendaRepository.BuscarPorEmpresaVenda(prePropostaDTO.EmpresaVenda.Id)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault(); //"colocar o primeiro";

                if (preProposta.PontoVenda.HasValue())
                {
                    preProposta.Viabilizador = preProposta.PontoVenda.Viabilizador;
                }

                //preProposta.Cliente = new Cliente { Id = cliente.Id };
                preProposta.Corretor = prePropostaDTO.Corretor;
                preProposta.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = prePropostaDTO.EmpresaVenda.Id };

                Salvar(preProposta);

                MudarSituacaoProposta(preProposta, SituacaoProposta.EmElaboracao,
                            prePropostaDTO.UsuarioPortal);

                AplicarDetalhamentoFinanceiroPorIntegracao(prePropostaDTO.SimuladorDto, preProposta);

                //var webRoot = GetWebAppRootAdmin();
                //_hierarquiaCoordenadorService.NotificarClienteDuplicado(webRoot, preProposta);
            }
            catch (BusinessRuleException bre)
            {
                apiEx.AddError(bre.Errors);
            }
            finally
            {
                apiEx.ThrowIfHasError();
            }

            return preProposta;
        }

        public void AtualizarClientePorSimulacao(Cliente cliente, SimuladorDto simulacao,ref List<string> msgs)
        {
            var bre = new BusinessRuleException();

            cliente.NomeCompleto = simulacao.NomeCliente.HasValue() && !cliente.NomeCompleto.Equals(simulacao.NomeCliente) ? simulacao.NomeCliente : cliente.NomeCompleto;
            cliente.TelefoneResidencial = simulacao.Telefone.OnlyNumber().HasValue() && !cliente.TelefoneResidencial.OnlyNumber().Equals(simulacao.Telefone.OnlyNumber()) ? simulacao.Telefone.OnlyNumber() : cliente.TelefoneResidencial.OnlyNumber();
            cliente.Email = simulacao.Email.HasValue() && !cliente.Email.Equals(simulacao.Email) ? simulacao.Email : cliente.Email;
            cliente.EstadoCivil = simulacao.EstadoCivil.HasValue() && !cliente.EstadoCivil.Equals(simulacao.EstadoCivil) ? simulacao.EstadoCivil : cliente.EstadoCivil;
            cliente.DataNascimento = simulacao.DataNascimento.HasValue() && !cliente.DataNascimento.Equals(simulacao.DataNascimento) ? simulacao.DataNascimento : cliente.DataNascimento;
            cliente.CpfCnpj = simulacao.Cpf.HasValue() && !cliente.CpfCnpj.OnlyNumber().Equals(simulacao.Cpf.OnlyNumber()) ? simulacao.Cpf.OnlyNumber() : cliente.CpfCnpj.OnlyNumber();
            cliente.FGTS = simulacao.ValorFgts.HasValue() ? simulacao.ValorFgts : cliente.FGTS;

            _clienteService.Salvar(cliente, bre);

            msgs.AddRange(bre.Errors);
        }

        public PreProposta AtualizarCodigoSimulacaoPreProposta(SimuladorDto parametro)
        {
            var apiEx = new ApiException();

            if (parametro.CodigoPreProposta.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta));
                apiEx.ThrowIfHasError();
            }

            var preProposta = _prePropostaRepository.BuscarPorCodigo(parametro.CodigoPreProposta);

            if (preProposta.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.PreProposta));
                apiEx.ThrowIfHasError();
            }

            if (parametro.Codigo.IsEmpty() || parametro.Digito.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Codigo + "/Dígito"));
                apiEx.ThrowIfHasError();
            }

            preProposta.CodigoSimulacao = parametro.Codigo;
            preProposta.DigitoSimulacao = parametro.Digito;

            _prePropostaRepository.Save(preProposta);

            return preProposta;
        }

        ///////////

        public void AplicarSimulacao(SimuladorDto parametro)
        {

        }

        ///////////

        #endregion

        public SituacaoProposta? SituacaoAnterior(long idPreProposta)
        {
            var situacao = _historicoPrePropostaRepository.BuscarStatusAnterior(idPreProposta).SituacaoInicio;
            return situacao;
        }


        //Validação de documentos a cada transição
        public void ValidateDocumentRule(PreProposta preProposta, SituacaoProposta destino)
        {
            if (preProposta.SituacaoProposta == SituacaoProposta.EmElaboracao && destino == SituacaoProposta.EmElaboracao)
            {
                return;
            }

            if (destino == SituacaoProposta.Cancelada)
            {
                return;
            }

            var bre = new BusinessRuleException();

            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            const int participacaoEsperada = 100;
            int somatorioParticipacao = proponentes.Sum(reg => reg.Participacao);
            if (participacaoEsperada != somatorioParticipacao)
            {
                bre.AddError(GlobalMessages.ErroParticipacaoDeveSerCem).Complete();
            }

            foreach (var prop in proponentes)
            {
                bool EvLoja = prop.Cliente.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja;
                // Realiza as validações dos proponentes
                var anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, destino, EvLoja).Validate(prop);

                // Verifica se existe algum erro e retorna exceção se necessário
                bre.WithFluentValidation(anprResult);
            }

            bre.ThrowIfHasError();
        }
        //Validadando Valor Parcela Solicitada
        public void ValidadeValorParcelaSolicitada(decimal parcelaSolicitado)
        {
            var bre = new BusinessRuleException();
            if (parcelaSolicitado <= 0)
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.ParcelaSolicitada)).Complete();
            }
            bre.ThrowIfHasError();
        }

        //Método alterado para adicionar o e-mail na fila de envio
        public void EnviarEmailAguardandoAnalise(PreProposta proposta)
        {
            if (proposta.SituacaoProposta.IsEmpty())
            {
                return;
            }

            //Criação do item fila de email
            var email = new FilaEmail();
            email.Titulo = String.Empty;
            email.Mensagem = String.Empty;
            if (proposta.SituacaoProposta == SituacaoProposta.AguardandoAnaliseCompleta)
            {

                switch (proposta.EmpresaVenda.TipoEmpresaVenda)
                {
                    case TipoEmpresaVenda.Loja:
                        email.Titulo = "PORTAL HOUSE/" + DateTime.Now.ToString("HH:mm");
                        email.Mensagem = string.Format(GlobalMessages.EmailAtendimentoUnificadoCorpo,
                                proposta.Cliente.NomeCompleto,
                                proposta.Cliente.TelefonePrimeiraReferencia.ToPhoneFormat(),
                                "PORTAL HOUSE",
                                proposta.Cliente.CpfCnpj.ToCPFFormat(),
                                proposta.Codigo);
                        break;
                    default:
                        email.Titulo = "PORTAL EV/" + DateTime.Now.ToString("HH:mm");
                        email.Mensagem = string.Format(GlobalMessages.EmailAtendimentoUnificadoCorpo,
                                proposta.Cliente.NomeCompleto,
                                proposta.Cliente.TelefonePrimeiraReferencia.ToPhoneFormat(),
                                "PORTAL EV",
                                proposta.Cliente.CpfCnpj.ToCPFFormat(),
                                proposta.Codigo);
                        break;
                }

                if (proposta.Cliente.HasValue())
                {
                    email.Titulo += "/" + proposta.Cliente.NomeCompleto;
                }

                if (proposta.EmpresaVenda.HasValue())
                {
                    email.Titulo += "/" + proposta.EmpresaVenda.Estado + "/" + proposta.EmpresaVenda.NomeFantasia;
                }

                email.Titulo += "/COMPLETA";
            }
            else
            {

                switch (proposta.EmpresaVenda.TipoEmpresaVenda)
                {
                    case TipoEmpresaVenda.Loja:
                        email.Titulo = "PORTAL HOUSE/" + DateTime.Now.ToString("HH:mm");
                        email.Mensagem = string.Format(GlobalMessages.EmailAtendimentoUnificadoCorpo,
                                proposta.Cliente.NomeCompleto,
                                proposta.Cliente.TelefonePrimeiraReferencia.ToPhoneFormat(),
                                "PORTAL HOUSE",
                                proposta.Cliente.CpfCnpj.ToCPFFormat(),
                                proposta.Codigo);
                        break;
                    default:
                        email.Titulo = "PORTAL EV/" + DateTime.Now.ToString("HH:mm");
                        email.Mensagem = string.Format(GlobalMessages.EmailAtendimentoUnificadoCorpo,
                                proposta.Cliente.NomeCompleto,
                                proposta.Cliente.TelefonePrimeiraReferencia.ToPhoneFormat(),
                                "PORTAL EV",
                                proposta.Cliente.CpfCnpj.ToCPFFormat(),
                                proposta.Codigo);
                        break;
                }

                if (proposta.Cliente.HasValue())
                {
                    email.Titulo += "/" + proposta.Cliente.NomeCompleto;
                }

                if (proposta.EmpresaVenda.HasValue())
                {
                    email.Titulo += "/" + proposta.EmpresaVenda.Estado + "/" + proposta.EmpresaVenda.NomeFantasia;
                }

                email.Titulo += "/SIMPLIFICADA";

            }
            email.Destinatario = ProjectProperties.EmailConfigurations.EmailAtendimentoUnificado;
            email.SituacaoEnvio = SituacaoEnvioFila.Pendente;
            _filaEmailService.PushEmail(email);

        }

        public string AlterarCCA(GrupoCCAPrePropostaDTO grupoCCAPrePropostaDTO, UsuarioPortal usuario, List<Perfil> perfis)
        {

            var bre = new BusinessRuleException();

            var transfList = _grupoCCAPrePropostaRepository.FindByPreProposta(grupoCCAPrePropostaDTO.IdPreProposta);
            var preProposta = _prePropostaRepository.FindById(grupoCCAPrePropostaDTO.IdPreProposta);
            var grupoCCAUsuário = _usuarioGrupoCCARepository.Queryable()
                         .Where(x => x.Usuario.Id == usuario.Id)
                         .Select(x => x.GrupoCCA.Id)
                         .FirstOrDefault();
            grupoCCAPrePropostaDTO.IdCCAOrigem = grupoCCAUsuário;
            if (transfList.HasValue())
            {

                foreach (var transf in transfList)
                {
                    if (grupoCCAPrePropostaDTO.IdCCAOrigem == transf.GrupoCCADestino.Id)
                    {
                        transf.Situacao = Situacao.Cancelado;
                        _grupoCCAPrePropostaRepository.Save(transf);
                    }
                }
            }

            if (grupoCCAPrePropostaDTO.IdCCAOrigem == grupoCCAPrePropostaDTO.IdCCADestino)
            {
                bre.AddError("A pré proposta já pertence ao CCA selecionado").Complete();
                bre.ThrowIfHasError();
            }

            //Comentado pois IdCCAOrigem sempre será o CCA do usuário.

            //if (grupoCCAPrePropostaDTO.IdCCAOrigem.IsEmpty()|| grupoCCAPrePropostaDTO.IdCCAOrigem==0)
            //{ 
            //    grupoCCAPrePropostaDTO.IdCCAOrigem = _grupoCCAEmpresaVendaRepository.BuscarGruposPorEv(preProposta.EmpresaVenda.Id).FirstOrDefault().GrupoCCA.Id;
            //}

            var novo = new GrupoCCAPreProposta();
            novo.GrupoCCAOrigem.Id = grupoCCAPrePropostaDTO.IdCCAOrigem;
            novo.GrupoCCADestino.Id = grupoCCAPrePropostaDTO.IdCCADestino;
            novo.IdPreProposta = grupoCCAPrePropostaDTO.IdPreProposta;
            novo.Situacao = Situacao.Ativo;

            _grupoCCAPrePropostaRepository.Save(novo);


            if (preProposta.SituacaoProposta == SituacaoProposta.EmAnaliseSimplificada || preProposta.SituacaoProposta == SituacaoProposta.EmAnaliseCompleta)
            {
                if (preProposta.HasValue())
                {
                    switch (preProposta.SituacaoProposta)
                    {
                        case SituacaoProposta.EmAnaliseSimplificada:
                            MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseSimplificada,
                            usuario, null, perfis);
                            EnviarEmailAguardandoAnalise(preProposta);
                            break;
                        case SituacaoProposta.EmAnaliseCompleta:
                            MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                            usuario, null, perfis);
                            EnviarEmailAguardandoAnalise(preProposta);
                            break;
                    }
                }
                return string.Format("O CCA da pré proposta foi modificado com sucesso, e o status retornado!");
            }

            return "O CCA da pré proposta foi modificado com sucesso!";
        }


        public DataSourceResponse<ViewCCAPreProposta> ListarCCAsPPR(DataSourceRequest request, long IdPreProposta)
        {
            var bre = new BusinessRuleException();
            var CCAs = new DataSourceResponse<ViewCCAPreProposta>();

            try
            {
                CCAs = _viewCCAPrePropostaRepository.BuscarCCAsPreProposta(request, IdPreProposta);
            }
            catch (Exception ex)
            {
                bre.AddError(ex.Message).Complete();
                bre.ThrowIfHasError();
            }
            return CCAs;
        }

        public List<GrupoCCA> ListarCCAsDestinoPPR(long IdPreProposta)
        {
            var bre = new BusinessRuleException();
            var todosCCAs = new List<GrupoCCA>();
            var CCAsPPR = new List<ViewCCAPreProposta>().AsQueryable();

            try
            {
                var todosCCASQueryable = _grupoCCARepository.ListarTodosCCAs().AsQueryable();
                CCAsPPR = _viewCCAPrePropostaRepository.BuscarCCAsPreProposta(IdPreProposta).AsQueryable();

                if (CCAsPPR.HasValue())
                {
                    var CCAsPPRIds = CCAsPPR.Select(y => y.IdGrupoCCA).ToList();
                    foreach (var CCA in CCAsPPRIds)
                    {
                        todosCCASQueryable = todosCCASQueryable.Where(x => x.Id != CCA);
                    }
                }

                todosCCAs = todosCCASQueryable.ToList();
            }
            catch (Exception ex)
            {
                bre.AddError(ex.Message).Complete();
                bre.ThrowIfHasError();
            }
            //um pequeno ajuste no SICAQ
            return todosCCAs;
        }

        public HistoricoPreProposta ReenviarAnaliseCompletaAprovada(PreProposta preProposta, SituacaoProposta situacao, UsuarioPortal responsavel, List<Perfil> perfis)
        {
            var validation = new BusinessRuleException();
            if (preProposta.JustificativaReenvio.IsEmpty())
            {
                validation.AddError("Justificativa não preenchida!").Complete();
                validation.ThrowIfHasError();
            }
            return MudarSituacaoProposta(preProposta, situacao, responsavel, null, perfis);
        }

        public bool VerificaDocumentosEmAnaliseCompletaParaAnaliseCompletaAprovada(Proponente proponente)
        {
            if (proponente.Cliente.IsEmpty())
            {
                return false;
            }

            var permitidos = new List<SituacaoAprovacaoDocumento>() {
                SituacaoAprovacaoDocumento.Aprovado,
                SituacaoAprovacaoDocumento.Anexado,
                SituacaoAprovacaoDocumento.Informado
            };

            // Verificar se todos os documentos estão nas situações corretas para avançõ
            var temNaoPermitido = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == proponente.PreProposta.Id)
                .Where(x => x.Proponente.Cliente.Id == proponente.Cliente.Id)
                .Where(x => !permitidos.Contains(x.Situacao))
                .Any();

            var todosQuePrecisamEstarPreenchidos = new List<string>();

            var filtroObrigatoriedade = _documentoRuleMachinePrePropostaRepository.Queryable()
            .Where(x => x.RuleMachinePreProposta.Origem == SituacaoProposta.EmAnaliseCompleta)
            .Where(x => x.RuleMachinePreProposta.Destino == SituacaoProposta.AnaliseCompletaAprovada)
            .Where(x => x.TipoDocumento.Situacao == Situacao.Ativo);

            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioPortal);
            }
            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioHouse);
            }
            todosQuePrecisamEstarPreenchidos = filtroObrigatoriedade
                                    .Select(x => x.TipoDocumento.Nome)
                                    .ToList();
            if (todosQuePrecisamEstarPreenchidos.IsEmpty())
            {
                return true;
            }
            var contagemDocumentos = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == proponente.PreProposta.Id)
                .Where(x => x.Proponente.Cliente.Id == proponente.Cliente.Id)
                .Where(x => todosQuePrecisamEstarPreenchidos.Contains(x.TipoDocumento.Nome))
                .Where(x => x.Situacao == SituacaoAprovacaoDocumento.Aprovado).ToList();

            return !temNaoPermitido && contagemDocumentos.Count() >= todosQuePrecisamEstarPreenchidos.Count();
        }

    }
}
