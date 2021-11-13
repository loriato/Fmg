using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class LiberarPontuacaoFidelidadeJob : BaseJob
    {
        public PontuacaoFidelidadeRepository _pontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeService _pontuacaoFidelidadeService { get; set; }
        public ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }


        protected override void Init()
        {
            _pontuacaoFidelidadeRepository = new PontuacaoFidelidadeRepository();
            _pontuacaoFidelidadeRepository._session = _session;

            _itemPontuacaoFidelidadeRepository = new ItemPontuacaoFidelidadeRepository();
            _itemPontuacaoFidelidadeRepository._session = _session;

            _corretorRepository = new CorretorRepository();
            _corretorRepository._session = _session;

            _notificacaoRepository = new NotificacaoRepository();
            _notificacaoRepository._session = _session;

            _pontuacaoFidelidadeService = new PontuacaoFidelidadeService();
            _pontuacaoFidelidadeService._session = _session;
            _pontuacaoFidelidadeService._pontuacaoFidelidadeRepository = _pontuacaoFidelidadeRepository;
            _pontuacaoFidelidadeService._itemPontuacaoFidelidadeRepository = _itemPontuacaoFidelidadeRepository;
            _pontuacaoFidelidadeService._corretorRepository = _corretorRepository;
            _pontuacaoFidelidadeService._notificacaoRepository = _notificacaoRepository;
        }

        public override void Process()
        {
            var transaction = _session.BeginTransaction();
            try
            {
                WriteLog(TipoLog.Informacao, "Iniciando finalização de campanhas");
                var pontuacaoVencida = _pontuacaoFidelidadeRepository.CampanhasVencidas();
                var idspontuacaoVencida = pontuacaoVencida.Select(x => x.Id).ToList();
                var itensVencidos = _itemPontuacaoFidelidadeRepository.ItensDePontuacaoFidelidade(idspontuacaoVencida);

                if (pontuacaoVencida.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, string.Format("Não existem Campanhas {0} para serem finalizadas na data {1}. A rotina será finalizada.", GlobalMessages.SituacaoPontuacaoFidelidade_Ativo, DateTime.Now));
                }
                else
                {
                    //Inativando campanas
                    itensVencidos = _pontuacaoFidelidadeService.AlterarSituacaoDeItemPontuacao(itensVencidos, SituacaoPontuacaoFidelidade.Vencido);
                    itensVencidos = _pontuacaoFidelidadeService.AlterarSituacaoDePontuacao(itensVencidos, SituacaoPontuacaoFidelidade.Vencido);

                    WriteLog(TipoLog.Informacao, string.Format("{0} campanha(s) finalizada(s) com sucesso", pontuacaoVencida.Count));
                }

                WriteLog(TipoLog.Informacao, "Ativando campanhas");
                var pontuacaoAguardando = _pontuacaoFidelidadeRepository.CampanhasAguardando();
                var idsPontuacaoAguardando = pontuacaoAguardando.Select(x => x.Id).ToList();
                var itensAguardando = _itemPontuacaoFidelidadeRepository.ItensDePontuacaoFidelidade(idsPontuacaoAguardando);

                if (pontuacaoAguardando.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, string.Format("Não existem Campanhas {0} para serem ativadas na data {1}. A rotina será finalizada.", GlobalMessages.SituacaoPontuacaoFidelidade_AguardandoLiberacao, DateTime.Now));
                }
                else
                {
                    //Atvando campanhas
                    itensAguardando = _pontuacaoFidelidadeService.AlterarSituacaoDeItemPontuacao(itensAguardando, SituacaoPontuacaoFidelidade.Ativo);
                    itensAguardando = _pontuacaoFidelidadeService.AlterarSituacaoDePontuacao(itensAguardando, SituacaoPontuacaoFidelidade.Ativo);

                    _pontuacaoFidelidadeService.NotificarEmpresaVenda(pontuacaoAguardando, itensAguardando);

                    WriteLog(TipoLog.Informacao, string.Format("{0} campanha(s) ativada(s) com sucesso", pontuacaoAguardando.Count));
                }

                //Reativando itens suspensos
                WriteLog(TipoLog.Informacao, "Reativando itens suspensos");
                var idEmpreendimentosAtivos = itensAguardando.Select(x => x.Empreendimento.Id);

                var idEmpreendimentosInativos = itensVencidos
                    .Where(x => !idEmpreendimentosAtivos.Contains(x.Empreendimento.Id))
                    .Select(x => x.Empreendimento.Id);

                var itensSuspensos = _itemPontuacaoFidelidadeRepository.Queryable()
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Suspenso)
                    .Where(x => idEmpreendimentosInativos.Contains(x.Empreendimento.Id))
                    .ToList();

                if (itensSuspensos.HasValue())
                {
                    foreach (var item in itensSuspensos)
                    {
                        item.PontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Ativo;
                        _pontuacaoFidelidadeRepository.Save(item.PontuacaoFidelidade);

                        item.Situacao = SituacaoPontuacaoFidelidade.Ativo;
                        _itemPontuacaoFidelidadeRepository.Save(item);
                    }

                    WriteLog(TipoLog.Informacao, string.Format("{0} itens reativado(s) com sucesso", itensSuspensos.Count));
                }
                else
                {
                    WriteLog(TipoLog.Informacao, "Não existem itens para serem reativados. A rotina será finalizada");
                }
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);
                WriteLog(TipoLog.Erro, err.Message);
            }

            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }        
    }
}