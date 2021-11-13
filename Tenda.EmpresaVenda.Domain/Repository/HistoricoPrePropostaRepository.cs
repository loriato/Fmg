using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class HistoricoPrePropostaRepository : NHibernateRepository<HistoricoPreProposta>
    {
        public HistoricoPreProposta AtualDaPreProposta(long idPreProposta)
        {
            var historico = Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .OrderByDescending(reg => reg.Inicio)
                .FirstOrDefault();

            return historico;
        }

        public HistoricoPreProposta BuscarEnvioPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.SituacaoInicio == SituacaoProposta.EmElaboracao)
                .Where(x => x.SituacaoTermino == SituacaoProposta.AguardandoAnaliseSimplificada)
                .OrderBy(x => x.Inicio)
                .FirstOrDefault();
        }

        public HistoricoPreProposta BuscarUltimoEnvio(long idPreProposta)
        {
            var situacaoTermino = new List<SituacaoProposta> { SituacaoProposta.AguardandoAnaliseSimplificada, SituacaoProposta.AguardandoAnaliseCompleta };
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.SituacaoTermino != null)
                .Where(x => situacaoTermino.Contains(x.SituacaoTermino.Value))
                .OrderByDescending(x => x.Termino)
                .FirstOrDefault();
        }

        public HistoricoPreProposta BuscarAnaliseInicialPreProposta(long idPreProposta)
        {
            return BuscarAnalisesFinalizadasPreProposta(idPreProposta)
                .OrderBy(x => x.Inicio)
                .FirstOrDefault();
        }

        public HistoricoPreProposta BuscarAnaliseMaisRecentePreProposta(long idPreProposta)
        {
            return BuscarAnalisesFinalizadasPreProposta(idPreProposta)
                .OrderByDescending(x => x.Inicio)
                .FirstOrDefault();
        }

        public HistoricoPreProposta BuscarAnaliseMaisAntigaPreProposta(long idPreProposta)
        {
            return BuscarAnalisesFinalizadasPreProposta(idPreProposta)
                .OrderBy(x => x.Inicio)
                .FirstOrDefault();
        }

        private IQueryable<HistoricoPreProposta> BuscarAnalisesFinalizadasPreProposta(long idPreProposta)
        {
            List<SituacaoProposta> situacoesTermino = new List<SituacaoProposta>
            {
                SituacaoProposta.AnaliseSimplificadaAprovada,
                SituacaoProposta.AguardandoAuditoria,
                SituacaoProposta.Cancelada,
                SituacaoProposta.DocsInsuficientesSimplificado,
                SituacaoProposta.DocsInsuficientesCompleta
            };

            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.SituacaoInicio == SituacaoProposta.EmAnaliseSimplificada)
                .Where(x => x.SituacaoTermino != null)
                .Where(x => situacoesTermino.Contains(x.SituacaoTermino.Value));
        }

        public HistoricoPreProposta BuscarAnaliseAtivaMaisRecente(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Situacao == Situacao.Ativo)
                .OrderByDescending(x => x.Inicio)
                .FirstOrDefault();
        }

        public int EsforcoAnalisesPreProposta(long idPreProposta)
        {
            int totalEmSegundos = Convert.ToInt32(
                BuscarAnalisesFinalizadasPreProposta(idPreProposta)
                .Select(x => new { x.Inicio, Termino = x.Termino.Value })
                .AsEnumerable()
                .Sum(x => x.Termino.Subtract(x.Inicio).TotalSeconds));

            return totalEmSegundos / 60;
        }


        public List<HistoricoPreProposta> HistoricoDaPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .OrderByDescending(reg => reg.CriadoEm)
                .ToList();
        }

        public HistoricoPreProposta BuscarAnaliseSicaq(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.SituacaoInicio == SituacaoProposta.AnaliseSimplificadaAprovada 
                    || x.SituacaoInicio == SituacaoProposta.AnaliseCompletaAprovada
                    || x.SituacaoInicio == SituacaoProposta.FluxoEnviado
                    || x.SituacaoInicio == SituacaoProposta.AguardandoFluxo)
                .Where(x => x.SituacaoTermino == SituacaoProposta.AnaliseSimplificadaAprovada ||
                x.SituacaoTermino == SituacaoProposta.AguardandoIntegracao ||
                x.SituacaoTermino == SituacaoProposta.AguardandoFluxo ||
                x.SituacaoTermino == SituacaoProposta.Condicionada ||
                x.SituacaoTermino == SituacaoProposta.Reprovada ||
                x.SituacaoTermino == SituacaoProposta.SICAQComErro ||
                x.SituacaoTermino == SituacaoProposta.Cancelada)    
                .OrderBy(x => x.Inicio)
                .FirstOrDefault();
        }

        public HistoricoPreProposta HistoricoAnterior(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Situacao == Situacao.Ativo)
                .Select(x => x.Anterior)
                .SingleOrDefault();
        }

        public HistoricoPreProposta BuscarStatusAnterior(long idPreProposta)
        {
            return AtualDaPreProposta(idPreProposta).Anterior;
        }
    }
}
