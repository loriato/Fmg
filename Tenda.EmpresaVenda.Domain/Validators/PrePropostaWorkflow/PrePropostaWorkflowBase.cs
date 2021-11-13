using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class PrePropostaWorkflowBase : ICloneable
    {
        protected PrePropostaRepository _prePropostaRepository;
        protected ProponenteRepository _proponenteRepository;
        protected PlanoPagamentoRepository _planoPagamentoRepository;
        protected ClienteRepository _clienteRepository;
        protected DocumentoProponenteRepository _documentoProponenteRepository;
        protected TipoDocumentoRepository _tipoDocumentoRepository;
        protected RegraComissaoRepository _regraComissaoRepository;
        protected AceiteRegraComissaoRepository _aceiteRegraComissaoRepository;
        protected NotificacaoRepository _notificacaoRepository;
        protected RegraComissaoEvsRepository _regraComissaoEvsRepository;
        protected AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository;
        protected ItemRegraComissaoRepository _itemRegraComissaoRepository;
        protected DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository;
        protected DocumentoFormularioRepository _documentoFormularioRepository;

        public SituacaoProposta Origem { get; set; }
        public SituacaoProposta Destino { get; set; }

        public string Verbo { get; set; }

        protected PrePropostaWorkflowBase() { }

        public PrePropostaWorkflowBase(SituacaoProposta origem, SituacaoProposta destino)
        {
            Origem = origem;
            Destino = destino;
        }

        /// <summary>
        /// Sobreescreva este método para para implementar a sua regra específica de validação
        /// </summary>
        /// <param name="preProposta"></param>
        /// <returns></returns>
        public virtual bool Validate(PreProposta preProposta) { return false; }

        // Fluent Methods

        public PrePropostaWorkflowBase WithItemRegraComissaoRepository(ItemRegraComissaoRepository itemRegraComissaoRepository)
        {
            _itemRegraComissaoRepository = itemRegraComissaoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithPrePropostaRepository(PrePropostaRepository prePropostaRepository)
        {
            _prePropostaRepository = prePropostaRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithProponenteRepository(ProponenteRepository proponenteRepository)
        {
            _proponenteRepository = proponenteRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithPlanoPagamentoRepository(PlanoPagamentoRepository planoPagamentoRepository)
        {
            _planoPagamentoRepository = planoPagamentoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithClienteRepository(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithDocumentoProponenteRepository(DocumentoProponenteRepository documentoProponenteRepository)
        {
            _documentoProponenteRepository = documentoProponenteRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithTipoDocumentoRepository(TipoDocumentoRepository tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithRegraComissaoRepository(RegraComissaoRepository regraComissaoRepository)
        {
            _regraComissaoRepository = regraComissaoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithRegraComissaoEvsRepository(RegraComissaoEvsRepository regraComissaoEvsRepository)
        {
            _regraComissaoEvsRepository = regraComissaoEvsRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithAceiteRegraComissaoRepository(AceiteRegraComissaoRepository aceiteRegraComissaoRepository)
        {
            _aceiteRegraComissaoRepository = aceiteRegraComissaoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithAceiteRegraComissaoEvsRepository(AceiteRegraComissaoEvsRepository aceiteRegraComissaoEvsRepository)
        {
            _aceiteRegraComissaoEvsRepository = aceiteRegraComissaoEvsRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithNotificacaoRepository(NotificacaoRepository notificacaoRepository)
        {
            _notificacaoRepository = notificacaoRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithDocumentoRuleMachinePrePropostaRepository(DocumentoRuleMachinePrePropostaRepository documentoRuleMachinePrePropostaRepository)
        {
            _documentoRuleMachinePrePropostaRepository = documentoRuleMachinePrePropostaRepository;
            return this;
        }

        public PrePropostaWorkflowBase WithDocumentoFormularioRepository(DocumentoFormularioRepository documentoFormularioRepository)
        {
            _documentoFormularioRepository = documentoFormularioRepository;
            return this;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public PrePropostaWorkflowBase CloneAndCasting()
        {
            return (PrePropostaWorkflowBase)Clone();
        }
    }
}
