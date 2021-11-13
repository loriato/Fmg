using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class ConsolidadoPontuacaoFidelidadeJob : BaseJob
    {
        public PropostaSuatRepository _propostaSuatRepository { get; set; }
        public ConsolidadoPontuacaoFidelidadeRepository _consolidadoPontuacaoFidelidadeRepository { get; set; }
        public ConsolidadoPontuacaoFidelidadeService _consolidadoPontuacaoFidelidadeService { get; set; }
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaRepository _pontuacaoFidelidadeEmpresaVendaRepository { get; set; }
        public ValorNominalRepository _valorNominalRepository { get; set; }
        protected override void Init()
        {
            _propostaSuatRepository = new PropostaSuatRepository();
            _propostaSuatRepository._session = _session;

            _consolidadoPontuacaoFidelidadeRepository = new ConsolidadoPontuacaoFidelidadeRepository();
            _consolidadoPontuacaoFidelidadeRepository._session = _session;

            _itemPontuacaoFidelidadeRepository = new ItemPontuacaoFidelidadeRepository();
            _itemPontuacaoFidelidadeRepository._session = _session;

            _empreendimentoRepository = new EmpreendimentoRepository();
            _empreendimentoRepository._session = _session;

            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _session;

            _lojaRepository = new LojaRepository(_session);

            _pontuacaoFidelidadeEmpresaVendaRepository = new PontuacaoFidelidadeEmpresaVendaRepository();
            _pontuacaoFidelidadeEmpresaVendaRepository._session = _session;

            _valorNominalRepository = new ValorNominalRepository();
            _valorNominalRepository._session = _session;

            _consolidadoPontuacaoFidelidadeService = new ConsolidadoPontuacaoFidelidadeService();
            _consolidadoPontuacaoFidelidadeService._session = _session;
            _consolidadoPontuacaoFidelidadeService._itemPontuacaoFidelidadeRepository = _itemPontuacaoFidelidadeRepository;
            _consolidadoPontuacaoFidelidadeService._empreendimentoRepository = _empreendimentoRepository;
            _consolidadoPontuacaoFidelidadeService._empresaVendaRepository = _empresaVendaRepository;
            _consolidadoPontuacaoFidelidadeService._lojaRepository = _lojaRepository;
            _consolidadoPontuacaoFidelidadeService._propostaSuatRepository = _propostaSuatRepository;
            _consolidadoPontuacaoFidelidadeService._consolidadoPontuacaoFidelidadeRepository = _consolidadoPontuacaoFidelidadeRepository;
            _consolidadoPontuacaoFidelidadeService._pontuacaoFidelidadeEmpresaVendaRepository = _pontuacaoFidelidadeEmpresaVendaRepository;
            _consolidadoPontuacaoFidelidadeService._valorNominalRepository = _valorNominalRepository;
        }
        public override void Process()
        {
            var dataCorte = ProjectProperties.DataInicioProgramaFidelidade;

            var consolidado = _consolidadoPontuacaoFidelidadeRepository.ListarFaturados()
                .Select(x => x.IdProposta)
                .ToList();

            var propostasKitCompleto = _propostaSuatRepository.PropostasKitCompletoDataCorte(dataCorte)
                .Where(x => !consolidado.Contains(x.Id))
                .ToList();

            if (propostasKitCompleto.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há propostas a serem avaliadas");
                return;
            }

            var totalPropostas = propostasKitCompleto.Count;
            var tamanhoPagina = 50;
            var indiceAtual = 0;

            var transaction = _session.BeginTransaction();
            
            foreach (var proposta in propostasKitCompleto)
            {
                transaction = _session.BeginTransaction();
                indiceAtual++;
                try
                {
                    if (indiceAtual % tamanhoPagina == 0)
                    {
                        transaction.Commit();
                        WriteLog(TipoLog.Informacao, String.Format("Processados {0:0000} de {1:0000}", indiceAtual, totalPropostas));
                        transaction = _session.BeginTransaction();
                    }

                    _consolidadoPontuacaoFidelidadeService.ContabilizarPontuacao(proposta);
                }
                catch (BusinessRuleException bre)
                {
                    foreach (var erro in bre.Errors)
                    {
                        WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, erro));
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", proposta.Id, proposta.CodigoProposta, e.Message));
                    ExceptionLogger.LogException(e);
                    transaction = _session.BeginTransaction();
                }
                finally
                {
                    if (transaction.IsActive)
                    {
                        transaction.Commit();

                    }
                }
            }
            
            if (transaction.IsActive)
            {
                transaction.Commit();

            }

        }
    }
}