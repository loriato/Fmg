using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizacaoDisponibilidadeEmpreendimentoJob : BaseJob
    {
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }

        protected override void Init()
        {
            _empreendimentoRepository = new EmpreendimentoRepository();
            _empreendimentoRepository._session = _session;
        }

        public override void Process()
        {
            int idUsuarioProcesso = 1;
            try
            {
                SuatService service = new SuatService();
                WriteLog(TipoLog.Informacao, "Buscando Informações de Empreendimentos");
                var empreendimentosHabilitados = service.EmpreendimentosHabilitadosParaVenda();
                WriteLog(TipoLog.Informacao, "Informações de Empreendimentos Recuperadas");
                WriteLog(TipoLog.Informacao, String.Format("Empreendimentos Habilitados no SUAT: {0}", empreendimentosHabilitados.Count));
                if (!empreendimentosHabilitados.IsEmpty())
                {
                    int disponiveis = _empreendimentoRepository.DisponibilizarParaVenda(empreendimentosHabilitados, idUsuarioProcesso);
                    WriteLog(TipoLog.Informacao, String.Format("{0} empreendimentos marcados como disponíveis", disponiveis));
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro, String.Format("Erro ao atualizar empreendimentos: {0}", e.Message));
            }
        }

    }
}