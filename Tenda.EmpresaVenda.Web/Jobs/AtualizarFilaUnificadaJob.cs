using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.Domain.Shared.Models;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizarFilaUnificadaJob : BaseJob
    {
        public PropostaFilaSUATRepository _propostaFilaSUATRepository { get; set; }
        public SuatService _suatService { get; set; }
        public PropostaFilaSUATService _propostaFilaSUATService { get; set; }
        protected override void Init()
        {
            _suatService = new SuatService();

            _propostaFilaSUATRepository = new PropostaFilaSUATRepository();
            _propostaFilaSUATRepository._session = _session;

            _propostaFilaSUATService = new PropostaFilaSUATService();
            _propostaFilaSUATService._session = _session;
            _propostaFilaSUATService._propostaFilaSUATRepository = _propostaFilaSUATRepository;
        }

        public override void Process()
        {
            WriteLog(TipoLog.Informacao, "Atualizar Fila Unificada SUAT X EVS");
            WriteLog(TipoLog.Informacao, "Iniciando processamento");

            LimparTabela();
            
            var filas = ProjectProperties.FilasSUAT.Select(x=>x.IdFila).ToList();

            var filaUnificada = _suatService.AtualizarFilaUnificada(filas,ProjectProperties.IdUsuarioEvsApiSuat);

            if (filaUnificada.IsEmpty())
            {
                WriteLog(TipoLog.Aviso, "Falha na consulta com o SUAT");
                return;
            }

            var totalPropostas = filaUnificada.Propostas.Count;
            var tamanhoPagina = 50;
            var indiceAtual = 0;

            if(filaUnificada.HasValue()&&
                filaUnificada.Filas.HasValue())
            {
                AtualizarParametros(filaUnificada.Filas, filaUnificada.FilasPersonalizadas);
            }
            
            WriteLog(TipoLog.Informacao, "Atualizar propostas");

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}", totalPropostas));

            ITransaction transaction = _session.BeginTransaction();

            foreach (var proposta in filaUnificada.Propostas)
            {
                indiceAtual++;
                try
                {
                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        transaction.Commit();
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalPropostas));
                        transaction = _session.BeginTransaction();
                    }

                    _propostaFilaSUATService.AtualizarFilaUnificada(proposta);

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
            }
            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        public void LimparTabela()
        {
            var dataCorte = _propostaFilaSUATRepository.BuscarUltimaDataAtualizacao();

            var propostas = _propostaFilaSUATRepository.Queryable()
                .Where(x => dataCorte > x.AtualizadoEm)
                .ToList();

            var totalPropostas = propostas.Count;
            var tamanhoPagina = 100;
            var indiceAtual = 0;

            WriteLog(TipoLog.Informacao, "Limpando tabela");

            WriteLog(TipoLog.Informacao, String.Format("Total a ser processado: {0:0000}", totalPropostas));

            var transaction = _session.BeginTransaction();

            foreach (var proposta in propostas)
            {
                indiceAtual++;
                try
                {
                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        transaction.Commit();
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalPropostas));
                        transaction = _session.BeginTransaction();
                    }

                    _propostaFilaSUATRepository.Delete(proposta);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
            }

            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        public void AtualizarParametros(List<FilaAuxiliarDTO> filas,List<FilaPersonalizadaAuxiliarDTO> filasPersonalizadas)
        {
            WriteLog(TipoLog.Informacao, "Atualizando parâmetro emvs_filas_suat");

            var novoParametro = new JavaScriptSerializer().Serialize(filas);

            ProjectProperties.UpdateParameter("emvs_filas_suat", novoParametro);

            foreach (var fila in filasPersonalizadas)
            {
                WriteLog(TipoLog.Informacao, string.Format("Atualizando parâmetro {0}",fila.CodigoIdentificador));

                var parametro = new JavaScriptSerializer().Serialize(fila.FilaPersonalizada);

                ProjectProperties.UpdateParameter(fila.CodigoIdentificador, parametro);

                WriteLog(TipoLog.Informacao, string.Format("Parêmetro {0} atualizado com sucesso",fila.CodigoIdentificador));
            }

            WriteLog(TipoLog.Informacao, "Fim da atualização de parâmetros");
        }
    }
}