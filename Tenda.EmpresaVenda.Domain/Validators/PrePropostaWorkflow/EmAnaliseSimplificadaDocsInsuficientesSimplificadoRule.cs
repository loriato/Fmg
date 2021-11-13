using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseSimplificadaDocsInsuficientesSimplificadoRule : PrePropostaWorkflowBase
    {
        public EmAnaliseSimplificadaDocsInsuficientesSimplificadoRule()
        {
            Origem = SituacaoProposta.EmAnaliseSimplificada;
            Destino = SituacaoProposta.DocsInsuficientesSimplificado;
        }

        public override bool Validate(PreProposta preProposta)
        {
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

            List<SituacaoAprovacaoDocumento> situacoes = new List<SituacaoAprovacaoDocumento>
            {
                SituacaoAprovacaoDocumento.Aprovado,
                SituacaoAprovacaoDocumento.Pendente
            };

            var proponentesId = proponentes.Select(reg => reg.Id).ToList();

            // Verifica se os proponentes possuem apenas aprovados e pendenciados
            var somenteAprovadoPendenciado = !_documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == preProposta.Id)
                .Where(x => proponentesId.Contains(x.Proponente.Id))
                .Where(x => !situacoes.Contains(x.Situacao))
                .Any();

            // Pode deixar pendenciar se houver no mínimo um pendenciado
            var existePendenciado = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == preProposta.Id)
                .Where(x => proponentesId.Contains(x.Proponente.Id))
                .Where(x => x.Situacao == SituacaoAprovacaoDocumento.Pendente)
                .Any();

            if (somenteAprovadoPendenciado && existePendenciado) {

                Notificacao notificacao = new Notificacao()
                {
                    Titulo = string.Format(GlobalMessages.Notificacao_DocsInsuficiente_Titulo,preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                    Conteudo = GlobalMessages.Notificacao_DocsInsuficiente_Conteudo,
                    Usuario = preProposta.Corretor.Usuario,
                    EmpresaVenda = preProposta.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);

                return true;
            }

            List<SituacaoAprovacaoDocumento> documentosPendentesAvaliacao = new List<SituacaoAprovacaoDocumento>() {
                SituacaoAprovacaoDocumento.Anexado,
                SituacaoAprovacaoDocumento.Informado
            };

            var naoAvaliados = _documentoProponenteRepository.Queryable()
                    .Where(x => x.PreProposta.Id == preProposta.Id)
                    .Where(x => documentosPendentesAvaliacao.Contains(x.Situacao))
                    .Fetch(reg => reg.TipoDocumento)
                    .Fetch(reg => reg.Proponente)
                    .ToList();


            if (!naoAvaliados.IsEmpty())
            {
                BusinessRuleException err = new BusinessRuleException();
                err.AddError("Só é possível pendenciar uma pré-proposta caso todos os documentos tenham sido avaliados. Os documentos que ainda não foram avaliados são: ").Complete();

                foreach (var documento in naoAvaliados)
                {
                    err.AddError(string.Format(" - Documento {0} do proponente {1}", documento.TipoDocumento.Nome, documento.Proponente.Cliente.NomeCompleto)).Complete();
                }

                err.ThrowIfHasError();
            }


            BusinessRuleException bre = new BusinessRuleException();
            bre.AddError("Só é possível pendênciar uma pré-proposta caso exista ao menos um documento pendente. No momento não existem documentos pendênciados para a pré-proposta").Complete();
            bre.ThrowIfHasError();

            return false;
        }
    }
}
