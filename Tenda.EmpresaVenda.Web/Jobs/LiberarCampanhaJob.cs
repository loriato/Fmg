using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public  class LiberarCampanhaJob : BaseJob
    {
        private RegraComissaoService _regraComissaoService { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        protected override void Init()
        {
            _regraComissaoService = new RegraComissaoService();
            _regraComissaoService._session = _session;

            _regraComissaoService._regraComissaoEvsRepository = new RegraComissaoEvsRepository(_session);

            _regraComissaoService._regraComissaoRepository = new RegraComissaoRepository();
            _regraComissaoService._regraComissaoRepository._session = _session;

            _regraComissaoService._corretorRepository = new CorretorRepository();
            _regraComissaoService._corretorRepository._session = _session;

            _regraComissaoService._notificacaoRepository = new NotificacaoRepository();
            _regraComissaoService._notificacaoRepository._session = _session;

            _regraComissaoService._historicoRegraComissaoService = new HistoricoRegraComissaoService();
            _regraComissaoService._historicoRegraComissaoService._session = _session;

            _regraComissaoService._historicoRegraComissaoService._historicoRegraComissaoRepository = new HistoricoRegraComissaoRepository();
            _regraComissaoService._historicoRegraComissaoService._historicoRegraComissaoRepository._session = _session;

            _regraComissaoRepository = new RegraComissaoRepository();
            _regraComissaoRepository._session = _session;

        }
        public override void Process()
        {
            LimparCampanha();
            AtivarCamapanha();
            InativarCampanha();
        }

        private void AtivarCamapanha()
        {
            var campanhaAguardando = _regraComissaoRepository.CampanhasAguardandoLiberacao();

            if (campanhaAguardando.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, string.Format("Não existem Campanhas {0} para serem ativas na data {1}. A rotina será finalizada.", SituacaoRegraComissao.AguardandoLiberacao, DateTime.Now.Date));
                return;
            }

            try
            {
                foreach (var campanha in campanhaAguardando)
                {
                    try
                    {
                        WriteLog(TipoLog.Informacao, string.Format("Liberando Camapanha {0}", campanha.Descricao));
                        _regraComissaoService.AtivarCampanhaRobo(campanha);
                        WriteLog(TipoLog.Informacao, string.Format("Camapanha {0} liberada com sucesso", campanha.Descricao));
                    }
                    catch (BusinessRuleException bre)
                    {
                        foreach (var erro in bre.Errors)
                        {
                            WriteLog(TipoLog.Erro, erro);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);
                WriteLog(TipoLog.Erro, err.Message);
            }
        }
        
        private void InativarCampanha()
        {
            var campanhasVencidas = _regraComissaoRepository.CampanhasAguardandoInativacao();

            if (campanhasVencidas.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, string.Format("Não existem Campanhas {0} para serem finalizadas na data {1}. A rotina será finalizada.", GlobalMessages.SituacaoRegraComissao_Ativo, DateTime.Now));
                return;
            }

            try
            {
                foreach (var campanha in campanhasVencidas)
                {
                    try
                    {
                        WriteLog(TipoLog.Informacao, string.Format("Inativando Camapanha {0}", campanha.Descricao));
                        _regraComissaoService.InativarCampanha(campanha);
                        WriteLog(TipoLog.Informacao, string.Format("Camapanha {0} inativada com sucesso", campanha.Descricao));
                    }
                    catch (BusinessRuleException bre)
                    {
                        foreach (var erro in bre.Errors)
                        {
                            WriteLog(TipoLog.Erro, erro);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);
                WriteLog(TipoLog.Erro, err.Message);
            }
        }

        public void LimparCampanha()
        {
            _regraComissaoService.LimparCamapanhasComErro();
        }
    }
}