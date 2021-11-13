using Europa.Commons;
using NHibernate.Linq;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseSimplificadaAnaliseSimplificadaAprovadaRule : PrePropostaWorkflowBase
    {        
        public EmAnaliseSimplificadaAnaliseSimplificadaAprovadaRule()
        {
            Origem = SituacaoProposta.EmAnaliseSimplificada;
            Destino = SituacaoProposta.AnaliseSimplificadaAprovada;
        }

        public override bool Validate(PreProposta preProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();

            // Verifica se os proponentes possuem apenas aprovados
            var possuiDiferenteAprovado = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == preProposta.Id)
                .Where(x => SituacaoAprovacaoDocumento.Aprovado != x.Situacao)
                .Any();

            if (possuiDiferenteAprovado)
            {
                bre.AddError("Só é possível aprovar uma pré-proposta caso todos os documentos estejam aprovados. Os documentos pendentes são: ").Complete();

                var naoAprovados = _documentoProponenteRepository.Queryable()
                        .Where(x => x.PreProposta.Id == preProposta.Id)
                        .Where(x => SituacaoAprovacaoDocumento.Aprovado != x.Situacao)
                        .Fetch(reg => reg.TipoDocumento)
                        .Fetch(reg => reg.Proponente)
                        .ToList();

                foreach (var documento in naoAprovados)
                {
                    bre.AddError(string.Format(" - Documento {0} do proponente {1}", documento.TipoDocumento.Nome, documento.Proponente.Cliente.NomeCompleto)).Complete();
                }

                bre.ThrowIfHasError();
            }

            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            foreach (var prop in proponentes)
            {
                // Realiza as validações do fluxo EmAnaliseCompleta > AnaliseCompletaAprovada
                var evLoja = preProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja;
                prop.PreProposta.SituacaoProposta = SituacaoProposta.EmAnaliseCompleta;
                var anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, SituacaoProposta.AnaliseCompletaAprovada, evLoja).Validate(prop);
                if (anprResult.IsValid)
                {
                    var possuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

                    if (!possuiFormulario)
                    {
                        bre.AddError("Adicione os formulários necessários").Complete();
                    }
                    bre.ThrowIfHasError();
                }
            }

            return !possuiDiferenteAprovado;
        }
    }
}
