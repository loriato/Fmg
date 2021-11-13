using Europa.Commons;
using Europa.Resources;
using NHibernate.Linq;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseCompletaAnaliseCompletaAprovadaRule: PrePropostaWorkflowBase
    {
        public EmAnaliseCompletaAnaliseCompletaAprovadaRule()
        {
            Origem = SituacaoProposta.EmAnaliseCompleta;
            Destino = SituacaoProposta.AnaliseCompletaAprovada;
            Verbo = GlobalMessages.Aguardando;
        }

        public override bool Validate(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();

            var possuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

            if (!possuiFormulario)
            {
                bre.AddError("Adicione os formulários necessários").Complete();
            }

            bre.ThrowIfHasError();

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

            return !possuiDiferenteAprovado;
        }
    }
}
