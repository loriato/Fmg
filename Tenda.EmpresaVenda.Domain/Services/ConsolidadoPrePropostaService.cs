using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ConsolidadoPrePropostaService : BaseService
    {
        public ConsolidadoPrePropostaRepository _consolidadoPrePropostaRepository { get; set; }
        public ProponenteRepository _proponenteRepository { get; set; }
        public PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        public HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        public ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }
        public StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }

        public void ProcessarPreProposta(PreProposta preProposta)
        {
            var consolidado = _consolidadoPrePropostaRepository.FindByPrePropostaId(preProposta.Id);
            var idPreProposta = preProposta.Id;

            if (consolidado.IsEmpty())
            {
                consolidado = new ConsolidadoPreProposta();
                consolidado.Id = preProposta.Id;
                consolidado.EmpresaVenda = preProposta.EmpresaVenda;
                consolidado.PreProposta = preProposta;
                consolidado.Regional = preProposta.EmpresaVenda.Estado;
                consolidado.ProponenteUm = preProposta.Cliente;
                int numPrePropostasAnteriores = _prePropostaRepository.QuantidadePrePropostaAnteriorPorCliente(preProposta.Cliente.Id, preProposta.DataElaboracao.Value);
                consolidado.PropostaAnteriorParaCliente = numPrePropostasAnteriores != 0;
                consolidado.NumeroPropostasAnteriores = numPrePropostasAnteriores;
            }

            // O proponente dois pode ser modificado no decorrer do fluxo.
            var proponenteDois = _proponenteRepository.BuscarSegundoProponenteDaPreProposta(idPreProposta);
            if (proponenteDois != null)
            {
                consolidado.ProponenteDois = proponenteDois.Cliente;
            }

            if (consolidado.Envio == null)
            {
                consolidado.Envio = _historicoPrePropostaRepository.BuscarEnvioPreProposta(idPreProposta);
            }
            if (consolidado.AnaliseInicial == null)
            {
                consolidado.AnaliseInicial = _historicoPrePropostaRepository.BuscarAnaliseInicialPreProposta(idPreProposta);
            }

            //Ultimo Envio
            consolidado.UltimoEnvio = _historicoPrePropostaRepository.BuscarUltimoEnvio(idPreProposta);

            //Situação Suat x Evs
            consolidado.SituacaoSuatEvs = SituacaoSuatEvs(preProposta.SituacaoProposta, preProposta.PassoAtualSuat);

            var analiseMaisRecente = _historicoPrePropostaRepository.BuscarAnaliseMaisRecentePreProposta(idPreProposta);
            //consolidado.BreveLancamento = preProposta.BreveLancamento;
            consolidado.AnaliseMaisRecente = analiseMaisRecente;

            if (preProposta.BreveLancamento.HasValue())
            {
                consolidado.BreveLancamento = preProposta.BreveLancamento;
            }

            if (analiseMaisRecente != null)
            {
                consolidado.EsforcoAnaliseMaisRecente = Convert.ToInt32((analiseMaisRecente.Termino.Value.Subtract(analiseMaisRecente.Inicio)).TotalMinutes);
                consolidado.EsforcoTotal = _historicoPrePropostaRepository.EsforcoAnalisesPreProposta(idPreProposta);
            }
            var analiseMaisAntiga = _historicoPrePropostaRepository.BuscarAnaliseMaisAntigaPreProposta(idPreProposta);

            if (analiseMaisAntiga != null)
            {
                consolidado.EsforcoAnaliseMaisAntiga = Convert.ToInt32((analiseMaisAntiga.Termino.Value.Subtract(analiseMaisAntiga.Inicio)).TotalMinutes);
            }

            consolidado.SituacaoAtual = _historicoPrePropostaRepository.BuscarAnaliseAtivaMaisRecente(idPreProposta);
            consolidado.UltimaModificacao = preProposta.AtualizadoEm;
            consolidado.AnaliseSicaq = _historicoPrePropostaRepository.BuscarAnaliseSicaq(idPreProposta);

            var listaDocumentosReprovados = _documentoProponenteRepository.BuscarDocumentosReprovadosPorIdPreProposta(idPreProposta);
            var pendenciasAnaliseBuilder = new StringBuilder();
            var pendenciasParecerBuilder = new StringBuilder();
            foreach (var doc in listaDocumentosReprovados)
            {
                if (doc.Motivo.HasValue())
                {
                    pendenciasAnaliseBuilder.Append(doc.Proponente.Cliente.NomeCompleto).Append(" | ").Append(doc.TipoDocumento.Nome).Append(" : ").AppendLine(doc.Motivo);
                }
            }

            var documentosPreProposta = _documentoProponenteRepository.BuscarDocumentosPorPreProposta(idPreProposta);
            foreach (var doc in documentosPreProposta)
            {
                var parecer = _parecerDocumentoProponenteRepository.BuscarUltimoParecerDocumento(doc.Id);
                if (parecer.HasValue())
                {
                    pendenciasParecerBuilder.Append(doc.Proponente.Cliente.NomeCompleto).Append(" | ").Append(doc.TipoDocumento.Nome).Append(" : ").AppendLine(parecer.Parecer);
                }
            }

            consolidado.PendenciasAnalise = pendenciasAnaliseBuilder.ToString();
            consolidado.PendenciasParecer = pendenciasParecerBuilder.ToString();
            consolidado.DocumentosPendentes = listaDocumentosReprovados.Count;

            consolidado = SomatorioParcelas(consolidado);

            _consolidadoPrePropostaRepository.Save(consolidado);
        }

        public ConsolidadoPreProposta SomatorioParcelas(ConsolidadoPreProposta consolidado)
        {
            var planosPagamento = _planoPagamentoRepository.ListarParcelas(consolidado.Id);
            planosPagamento.ForEach(reg => Session.Evict(reg));

            consolidado.Entrada = planosPagamento.Where(reg => reg.TipoParcela == TipoParcela.Ato).Sum(reg => reg.Total);

            var parcelasPreChaves = new List<TipoParcela>() { TipoParcela.PreChaves, TipoParcela.PreChavesItbi };
            consolidado.PreChaves = planosPagamento.Where(reg => parcelasPreChaves.Contains(reg.TipoParcela)).Sum(reg => reg.Total);

            var parcelasPreChavesIntermediaria = new List<TipoParcela>() { TipoParcela.PreChavesIntermediaria, TipoParcela.PreChavesIntermediariaItbi };
            consolidado.PreChavesIntermediaria = planosPagamento.Where(reg => parcelasPreChavesIntermediaria.Contains(reg.TipoParcela)).Sum(reg => reg.Total);

            consolidado.Fgts = planosPagamento.Where(reg => reg.TipoParcela == TipoParcela.FGTS).Sum(reg => reg.Total);

            consolidado.Subsidio = planosPagamento.Where(reg => reg.TipoParcela == TipoParcela.Subsidio).Sum(reg => reg.Total);

            consolidado.Financiamento = planosPagamento.Where(reg => reg.TipoParcela == TipoParcela.Financiamento).Sum(reg => reg.Total);

            var parcelasPosChaves = new List<TipoParcela>() { TipoParcela.PosChaves, TipoParcela.PosChavesItbi };
            consolidado.PosChaves = planosPagamento.Where(reg => parcelasPosChaves.Contains(reg.TipoParcela)).Sum(reg => reg.Total);

            return consolidado;
        }



        public void RemoverConsolidado(long idPreProposta)
        {
            var consolidado = _consolidadoPrePropostaRepository.FindByPrePropostaId(idPreProposta);
            if (consolidado != null)
            {
                _consolidadoPrePropostaRepository.Delete(consolidado);
            }
        }

        public string SituacaoSuatEvs(SituacaoProposta? situacao,string passoAtualSuat)
        {
            if (situacao.IsEmpty()) return null;

            if (situacao == SituacaoProposta.Integrada)
            {
                var statusSuatStatusEvs = _statusSuatStatusEvsRepository.Queryable()
                    .Where(x => x.DescricaoSuat.Equals(passoAtualSuat))
                    .FirstOrDefault();

                if (statusSuatStatusEvs.HasValue())
                {
                    return statusSuatStatusEvs.DescricaoEvs;
                }

                return "Situação Genérica";

            }

            return situacao.AsString();
        }
    }
}
