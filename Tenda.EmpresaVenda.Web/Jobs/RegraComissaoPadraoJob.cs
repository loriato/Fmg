using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class RegraComissaoPadraoJob : BaseJob
    {
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private RegraComissaoPadraoRepository _regraComissaoPadraoRepository { get; set; }
        private ItemRegraComissaoPadraoRepository _itemRegraComissaoPadraoRepository { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        private ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        private HistoricoRegraComissaoService _historicoRegraComissaoService { get; set; }
        private HistoricoRegraComissaoRepository _historicoRegraComissaoRepository { get; set; }
        private RegraComissaoPdfService _regraComissaoPdfService { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private RegraComissaoEvsService _regraComissaoEvsService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }         
        private CorretorRepository _corretorRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        protected override void Init()
        {
            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _session;

            _regraComissaoPadraoRepository = new RegraComissaoPadraoRepository();
            _regraComissaoPadraoRepository._session = _session;

            _regraComissaoEvsRepository = new RegraComissaoEvsRepository(_session);

            _itemRegraComissaoPadraoRepository = new ItemRegraComissaoPadraoRepository();
            _itemRegraComissaoPadraoRepository._session = _session;

            _regraComissaoRepository = new RegraComissaoRepository();
            _regraComissaoRepository._session = _session;

            _historicoRegraComissaoRepository = new HistoricoRegraComissaoRepository();
            _historicoRegraComissaoRepository._session = _session;

            _arquivoRepository = new ArquivoRepository();
            _arquivoRepository._session = _session;

            _itemRegraComissaoRepository = new ItemRegraComissaoRepository();
            _itemRegraComissaoRepository._session = _session;

            _historicoRegraComissaoService = new HistoricoRegraComissaoService();
            _historicoRegraComissaoService._session = _session;
            _historicoRegraComissaoService._historicoRegraComissaoRepository = _historicoRegraComissaoRepository;

            _enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository();
            _enderecoEmpreendimentoRepository._session = _session;

            _corretorRepository = new CorretorRepository();
            _corretorRepository._session = _session;

            _notificacaoRepository = new NotificacaoRepository();
            _notificacaoRepository._session = _session;

            _regraComissaoPdfService = new RegraComissaoPdfService();
            _regraComissaoPdfService._regraComissaoEvsRepository = _regraComissaoEvsRepository;
            _regraComissaoPdfService._itemRegraComissaoRepository = _itemRegraComissaoRepository;
            _regraComissaoPdfService._enderecoEmpreendimentoRepository = _enderecoEmpreendimentoRepository;

            _arquivoService = new ArquivoService();
            _arquivoService._session = _session;
            _arquivoService._arquivoRepository = _arquivoRepository;

            _regraComissaoEvsService = new RegraComissaoEvsService();
            _regraComissaoEvsService._session = _session;
            _regraComissaoEvsService._arquivoService = _arquivoService;
            _regraComissaoEvsService._arquivoRepository = _arquivoRepository;
            _regraComissaoEvsService._regraComissaoEvsRepository = _regraComissaoEvsRepository;

            _regraComissaoService = new RegraComissaoService();
            _regraComissaoService._session = _session;
            _regraComissaoService._regraComissaoRepository = _regraComissaoRepository;
            _regraComissaoService._itemRegraComissaoRepository = _itemRegraComissaoRepository;
            _regraComissaoService._regraComissaoEvsRepository = _regraComissaoEvsRepository;
            _regraComissaoService._historicoRegraComissaoService = _historicoRegraComissaoService;
            _regraComissaoService._regraComissaoPdfService = _regraComissaoPdfService;
            _regraComissaoService._arquivoService = _arquivoService;
            _regraComissaoService._regraComissaoEvsService = _regraComissaoEvsService;
            _regraComissaoService._arquivoRepository = _arquivoRepository;
            _regraComissaoService._empresaVendaRepository = _empresaVendaRepository;
            _regraComissaoService._itemRegraComissaoRepository = _itemRegraComissaoRepository;
            _regraComissaoService._corretorRepository = _corretorRepository;
            _regraComissaoService._notificacaoRepository = _notificacaoRepository;
        }

        public override void Process()
        {
            //Lista de EVS com regra de comissão ativa
            var evsComRegraAtiva = _regraComissaoEvsRepository.Queryable()
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo || x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha)
                .Select(x=>x.EmpresaVenda.Id)
                .Distinct()
                .ToList();

            //Lista de EVS sem regra de comissão ativa
            var evsSemRegraAtiva = _empresaVendaRepository.Queryable()
                .Where(x => x.Situacao == Situacao.Ativo)
                .Where(x=>!evsComRegraAtiva.Contains(x.Id))
                .ToList();

            if (evsSemRegraAtiva.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há Empresas de Vendas sem regra de comissão ativa");
                return;
            }

            var totalEvs = evsSemRegraAtiva.Count;
            var indiceAtual = 0;

            WriteLog(TipoLog.Informacao, string.Format("Empresas de Venda com regra de comissão ativa: {0}",evsComRegraAtiva.Count));
            WriteLog(TipoLog.Informacao, string.Format("Empresas de Venda sem regra de comissão ativa: {0}",evsSemRegraAtiva.Count));

            //lista de regionais das EVS que precisão ser processadas
            var regionais = evsSemRegraAtiva.Select(x => x.Estado.ToUpper()).Distinct().ToList();

            //lista das regras de comissão ativas para as regionais das evs
            var regrasPadraoAtiva = _regraComissaoPadraoRepository.Queryable()
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo)
                .Where(reg => regionais.Contains(reg.Regional.ToUpper()))
                .ToList();

            //lista das regionais com regra de comissão padrão ativas
            var regionaisAtiva = regrasPadraoAtiva.Select(x => x.Regional).Distinct().ToList();

            //listando regionais que NÃO possui regra de comissão padrão ativa
            regionais = regionais.Where(x => !regionaisAtiva.Contains(x)).ToList();

            if (regionais.HasValue())
            {
                foreach(var uf in regionais)
                {
                    var cont = evsSemRegraAtiva.Where(x => x.Estado.Equals(uf)).ToList().Count;
                    WriteLog(TipoLog.Informacao, string.Format("Não há regras de comissão padrão ativas para a regional: {0} | Total EVS afetadas: {1}", uf.ToUpper(),cont));
                }
            }

            //removendo evs cujo regionais não possuem regra de comissão padrão ativa
            evsSemRegraAtiva = evsSemRegraAtiva.Where(x => regionaisAtiva.Contains(x.Estado)).ToList();

            if (evsSemRegraAtiva.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há Empresas de Vendas sem regra de comissão ativa");
                return;
            }

            var idRegrasPadraoAtiva = regrasPadraoAtiva.Select(x => x.Id).ToList();

            var itensRegraComissaoPadraoAtiva = _itemRegraComissaoPadraoRepository.Queryable()
                .Where(x=>idRegrasPadraoAtiva.Contains(x.RegraComissaoPadrao.Id))
                .ToList();

            var responsavel = new UsuarioPortal();
            responsavel.Id = ProjectProperties.IdUsuarioSistema;

            var transaction = _session.BeginTransaction();
            foreach (var ev in evsSemRegraAtiva)
            {
                transaction = _session.BeginTransaction();

                var regraPadrao = regrasPadraoAtiva.Where(x => x.Regional.ToUpper().Equals(ev.Estado.ToUpper())).SingleOrDefault();
                var itensPadrao = itensRegraComissaoPadraoAtiva.Where(x => x.RegraComissaoPadrao.Id == regraPadrao.Id).ToList();

                try
                {
                    // atribuindo regra de comissão padrão
                    _regraComissaoService.AtribuirRegraPadrao(ev, regraPadrao, itensPadrao, responsavel);

                    WriteLog(TipoLog.Informacao, string.Format("Regra de comissão padrão {0} atribuida à {1}", regraPadrao.Codigo,ev.NomeFantasia));
                    indiceAtual++;

                    transaction.Commit();
                }
                catch (BusinessRuleException bre)
                {
                    foreach (var erro in bre.Errors)
                    {
                        WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", regraPadrao.Codigo, ev.NomeFantasia, erro));
                    }
                }catch(Exception ex)
                {
                    transaction.Rollback();
                    WriteLog(TipoLog.Erro, String.Format("Erro em {0} | {1}. Mensagem: {2}", regraPadrao.Codigo, ev.NomeFantasia, ex.Message)); 
                    ExceptionLogger.LogException(ex);
                    transaction = _session.BeginTransaction();
                }
            }

            WriteLog(TipoLog.Informacao, string.Format("{0} de {1} Empresas de vendas processadas com sucesso",indiceAtual, totalEvs));

        }
        
    }
}