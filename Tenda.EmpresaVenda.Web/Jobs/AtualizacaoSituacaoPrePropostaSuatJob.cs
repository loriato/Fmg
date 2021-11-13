using Europa.Commons;
using Europa.Data.Model;
using Europa.Extensions;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizacaoSituacaoPrePropostaSuatJob : BaseJob
    {
        public SuatService suatService { get; set; }
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public PropostaSuatRepository _propostaSuatRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }

        protected override void Init()
        {
            suatService = new SuatService();
            _prePropostaRepository = new PrePropostaRepository();
            _prePropostaRepository._session = _session;
            _propostaSuatRepository = new PropostaSuatRepository();
            _propostaSuatRepository._session = _session;
            _logExecucaoRepository = new LogExecucaoRepository(_session);
            _logExecucaoRepository._session = _session;
        }

        public override void Process()
        {
            ITransaction transaction = null;
            try
            {
                var prePropostas = _prePropostaRepository.BuscarPrePropostasIntegradas();

                var dataMinima = ProjectProperties.DataBuscaAtualizacoesPropostas;

                if (DateTime.MinValue.Equals(dataMinima))
                {
                    var modificador = ProjectProperties.DiasBuscaAtualizacoesPropostas;
                    if (modificador == 0)
                    {
                        modificador = 7;
                    }
                    else if (modificador < 0)
                    {
                        modificador *= -1;
                    }

                    dataMinima = DateTime.Today.AddDays(-modificador);
                }

                var dataString = dataMinima.ToDateTimeSeconds();

                WriteLog(TipoLog.Informacao,
                    $"Buscando novas informações das pré-propostas a partir de [{dataString}].");

                var results = suatService.BuscarPropostas(prePropostas.Select(x => x.IdSuat).ToList(), dataMinima);

                WriteLog(TipoLog.Informacao, $"Foram recuperados {results.Count} registros de propostas");

                foreach (var item in results)
                {
                    var preProposta = prePropostas.Where(x => x.IdSuat == item.IdPreProposta).FirstOrDefault();

                    transaction = _session.BeginTransaction();

                    if (preProposta.HasValue())
                    {
                        preProposta.PassoAtualSuat = item.PassoAtualSuat;

                        preProposta.AtualizadoEm = DateTime.Now;
                        _prePropostaRepository.Save(preProposta);
                        item.PropostaSuat.PreProposta = preProposta;
                    }

                    var existente = _propostaSuatRepository.FindByIdSuat(item.PropostaSuat.IdSuat);
                    _session.Evict(existente);

                    var proposta = item.PropostaSuat;

                    if (!existente.IsEmpty())
                    {
                        proposta.Id = existente.Id;
                        proposta.CodigoCliente = existente.CodigoCliente.IsEmpty() ? proposta.CodigoCliente : existente.CodigoCliente;
                        proposta.Fase = existente.Fase.IsEmpty() ? proposta.Fase : existente.Fase;
                        proposta.Sintese = existente.Sintese.IsEmpty() ? proposta.Sintese : existente.Sintese;
                        proposta.Observacao = existente.Observacao.IsEmpty() ? proposta.Observacao : existente.Observacao;
                        if (!proposta.Observacao.IsEmpty() && proposta.Observacao.Length > DatabaseStandardDefinitions.FourThousandLength)
                        {
                            proposta.Observacao = proposta.Observacao.Substring(0, DatabaseStandardDefinitions.FourThousandLength - 10) + "-truncado";
                        }
                        proposta.DataRepasse = existente.DataRepasse.IsEmpty() ? proposta.DataRepasse : existente.DataRepasse;
                        proposta.StatusRepasse = proposta.DataRepasse.IsEmpty() ? "Não" : "Sim";
                        proposta.DataRegistro = existente.DataRegistro.IsEmpty() ? proposta.DataRegistro : existente.DataRegistro;
                        proposta.StatusConformidade = existente.StatusConformidade.IsEmpty() ? proposta.StatusConformidade : existente.StatusConformidade;
                        proposta.DataConformidade = existente.DataConformidade.IsEmpty() ? proposta.DataConformidade : existente.DataConformidade;

                        proposta.Faturado = existente.Faturado;
                        proposta.DataFaturado = existente.DataFaturado;

                        if (existente.AdiantamentoConformidade.HasValue())
                        {
                            proposta.AdiantamentoConformidade = existente.AdiantamentoConformidade;
                        }

                        if (existente.AdiantamentoRepasse.HasValue())
                        {
                            proposta.AdiantamentoRepasse = existente.AdiantamentoRepasse;
                        }
                    }

                    if (proposta.PropostaIdentificada.HasValue())
                    {
                        proposta.PropostaIdentificada = proposta.PropostaIdentificada.PadLeft(10, '0');
                    }                    

                    var nomeLoja = proposta.NomeLoja;
                    nomeLoja = nomeLoja.ToUpper(CultureInfo.CurrentCulture);
                    proposta.NomeLoja = nomeLoja;

                    proposta.AtualizadoEm = DateTime.Now;
                    proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;

                    _propostaSuatRepository.Save(proposta);

                    transaction.Commit();

                    string codigoPreProposta = "Não Localizada";
                    if (preProposta != null)
                    {
                        codigoPreProposta = preProposta.Codigo;
                    }

                    WriteLog(TipoLog.Informacao,
                        string.Format("Pré-Proposta {0} é proposta {1}. Status atual: {2}", codigoPreProposta,
                            item.PropostaSuat.CodigoProposta, item.PassoAtualSuat));
                }

                //Criado para utilização do consolidado de relatório de comissão
                if (ProjectProperties.AtivarRefreshViewRelatorioComissao)
                {
                    WriteLog(TipoLog.Informacao, "Atualizando Relatório de Comissão");
                    _session.CreateSQLQuery("refresh materialized view concurrently vw_rel_comissao")
                        .SetTimeout(600)
                        .ExecuteUpdate();
                    WriteLog(TipoLog.Informacao, "Relatório de Comissão atualizado com sucesso!!");
                }

                WriteLog(TipoLog.Informacao,
                    "Execução do robô de atualização das Pré-propostas finalizada com sucesso.");
            }
            catch (Exception e)
            {
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Rollback();
                }

                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro, String.Format("Erro ao atualizar as pré-propostas: {0}.", e.Message));
            }
        }
    }
}