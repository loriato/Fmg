﻿using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class DocsInsuficientesCompletaAguardandoAnaliseCompletaRule : PrePropostaWorkflowBase
    {
        public DocsInsuficientesCompletaAguardandoAnaliseCompletaRule()
        {
            Origem = SituacaoProposta.DocsInsuficientesCompleta;
            Destino = SituacaoProposta.AguardandoAnaliseCompleta;
            Verbo = GlobalMessages.Corrigir;
        }

        public override bool Validate(PreProposta preProposta)
        {
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
                var anprResult = new AnaliseProponenteValidator(_documentoProponenteRepository, _clienteRepository, _tipoDocumentoRepository, _documentoRuleMachinePrePropostaRepository, Destino, EvLoja).Validate(prop);

                // Verifica se existe algum erro e retorna exceção se necessário
                bre.WithFluentValidation(anprResult);
            }

            if (preProposta.BreveLancamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Produto)).Complete();
            }

            if (preProposta.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }

            bre.ThrowIfHasError();

            if (preProposta.EmpresaVenda.TipoEmpresaVenda != TipoEmpresaVenda.Loja)
            {
                long idEmpresaVenda = preProposta.EmpresaVenda.Id;
                var regraEvsVigente = _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);
                if (regraEvsVigente.IsNull())
                {
                    bre.AddError(GlobalMessages.ErroRegraComissao).Complete();
                }
                bre.ThrowIfHasError();

                long idRegraEvsVigente = regraEvsVigente.Id;
                bool possuiAceiteRegraEvsVigente = _aceiteRegraComissaoEvsRepository.BuscarAceiteParaRegraEvsAndEmpresaVenda(idRegraEvsVigente, idEmpresaVenda);
                if (!possuiAceiteRegraEvsVigente)
                {
                    bre.AddError(GlobalMessages.RegrasComissaoPendentesAprovacao).Complete();
                }
                bre.ThrowIfHasError();

                if (preProposta.BreveLancamento.Empreendimento.IsEmpty())
                {
                    return true;
                }

                long idEmpreendimento = preProposta.BreveLancamento.Empreendimento.Id;
                long idRegraComissao = regraEvsVigente.RegraComissao.Id;
                var itemRegraComissao = _itemRegraComissaoRepository.Buscar(idRegraComissao, idEmpresaVenda, idEmpreendimento);
                if (itemRegraComissao.IsEmpty())
                {
                    bre.AddError(GlobalMessages.ErroItemRegraComissao).Complete();
                }
                bre.ThrowIfHasError();
            }

            if (preProposta.IdTorre.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Torre)).Complete();
            }
            bre.ThrowIfHasError();

            return true;
        }
    }
}
