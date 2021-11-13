using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class HistoricoPrePropostaService : BaseService
    {
        public HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        public ConsolidadoPrePropostaService _consolidadoPrePropostaService { get; set; }
        public UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        public UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }

        /// <summary>
        /// Recebe uma pré-proposta e usa a Situação atual dela para criar um novo registro de histórico
        /// </summary>
        /// <param name="preProposta"></param>
        /// <param name="responsavel"></param>
        public HistoricoPreProposta CriarOuAvancarHistoricoPreProposta(PreProposta preProposta, UsuarioPortal responsavel, HistoricoPreProposta historicoAtual = null, List<Perfil> perfis = null)
        {
            if (historicoAtual.IsEmpty())
            {
                historicoAtual = _historicoPrePropostaRepository.AtualDaPreProposta(preProposta.Id);
            }

            // Se já existir, devemos finalizar
            if (!historicoAtual.IsEmpty())
            {
                historicoAtual.Termino = DateTime.Now;
                historicoAtual.ResponsavelTermino = responsavel;
                historicoAtual.SituacaoTermino = preProposta.SituacaoProposta.Value;
                historicoAtual.Situacao = Situacao.Suspenso;
                historicoAtual.NomePerfilCCAFinal = PopularPerfilCCA(responsavel, historicoAtual, preProposta, perfis);
                _historicoPrePropostaRepository.Save(historicoAtual);
            }

            HistoricoPreProposta novoHistorico = new HistoricoPreProposta();
            novoHistorico.ResponsavelInicio = responsavel;
            novoHistorico.Inicio = DateTime.Now;
            novoHistorico.PreProposta = preProposta;
            novoHistorico.Situacao = Situacao.Ativo;
            novoHistorico.SituacaoInicio = preProposta.SituacaoProposta.Value;
            novoHistorico.Anterior = historicoAtual;

            if (!historicoAtual.IsEmpty())
            {
                novoHistorico.NomePerfilCCAInicial = historicoAtual.NomePerfilCCAFinal;
            }
            else
            {
                novoHistorico.NomePerfilCCAInicial = PopularPerfilCCA(responsavel, novoHistorico, preProposta, perfis);
            }

            _historicoPrePropostaRepository.Save(novoHistorico);

            return novoHistorico;
        }

        public string PopularPerfilCCA(UsuarioPortal responsavel, HistoricoPreProposta historico, PreProposta preProposta, List<Perfil> perfis)
        {
            var cca = _usuarioGrupoCCARepository.Queryable()
                .Where(x => x.Usuario.Id == responsavel.Id)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            var situacoes = new List<SituacaoProposta> {
                SituacaoProposta.EmAnaliseSimplificada,
                SituacaoProposta.DocsInsuficientesSimplificado,
                SituacaoProposta.EmAnaliseCompleta,
                SituacaoProposta.Reprovada,
                SituacaoProposta.Condicionada,
                SituacaoProposta.AguardandoAuditoria,
                SituacaoProposta.DocsInsuficientesCompleta,
                SituacaoProposta.AguardandoIntegracao,
                SituacaoProposta.SICAQComErro,
                SituacaoProposta.AnaliseCompletaAprovada
            };

            if (historico.Anterior.HasValue())
            {
                if (historico.Anterior.SituacaoTermino == SituacaoProposta.AnaliseSimplificadaAprovada)
                {
                    situacoes.Add(SituacaoProposta.AguardandoFluxo);
                }
                if (historico.Anterior.SituacaoTermino == SituacaoProposta.EmAnaliseSimplificada ||
                    historico.Anterior.SituacaoTermino == SituacaoProposta.AnaliseSimplificadaAprovada ||
                    historico.Anterior.SituacaoTermino == SituacaoProposta.EmAnaliseCompleta)
                {
                    situacoes.Add(SituacaoProposta.Cancelada);
                }
                if(historico.Anterior.SituacaoTermino != SituacaoProposta.SICAQComErro)
                {
                    situacoes.Add(SituacaoProposta.AnaliseSimplificadaAprovada);
                }
                if(historico.Anterior.SituacaoTermino == SituacaoProposta.AguardandoFluxo)
                {
                    situacoes.Add(SituacaoProposta.AguardandoAnaliseCompleta);
                }
            }

            if (situacoes.Contains(preProposta.SituacaoProposta.Value))
            {
                if (cca.HasValue())
                {
                    return cca.GrupoCCA.Descricao;
                }
            }
            else
            {

                if (!perfis.IsEmpty())
                {
                    return perfis[0].Nome;
                }
            }
            return null;
        }

        public HistoricoPreProposta RetrocederHistorico(long idPreProposta)
        {
            HistoricoPreProposta historicoAtual = _historicoPrePropostaRepository.AtualDaPreProposta(idPreProposta);

            // Na teoria, esse caso nunca pode acontecer, por conta da validação de primeiro passo (logo a seguir)
            if (historicoAtual == null) { return null; }

            // O histórico sem anterior é o primeiro histórico, não pode ser retrocedido
            if (historicoAtual.Anterior == null) { return historicoAtual; }

            var novoAtual = historicoAtual.Anterior;
            novoAtual.Situacao = Situacao.Ativo;
            novoAtual.ResponsavelTermino = null;
            novoAtual.Termino = null;
            novoAtual.SituacaoTermino = null;

            _historicoPrePropostaRepository.Save(novoAtual);
            _consolidadoPrePropostaService.RemoverConsolidado(idPreProposta);
            _historicoPrePropostaRepository.Delete(historicoAtual);

            return novoAtual;

        }
    }
}
