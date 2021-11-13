using Europa.Extensions;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class HistoricoRegraComissaoService:BaseService 
    {
        public HistoricoRegraComissaoRepository _historicoRegraComissaoRepository { get; set; }
        public HistoricoRegraComissao CriarOuAvancar(RegraComissaoEvs regraComissaoEvs, UsuarioPortal responsavel)
        {
            // Se já existir, devemos finalizar
            FinalizarHistóricoAtual(regraComissaoEvs, responsavel);

            HistoricoRegraComissao novoHistorico = new HistoricoRegraComissao();
            novoHistorico.RegraComissaoEvs = regraComissaoEvs;
            novoHistorico.ResponsavelInicio = responsavel;
            novoHistorico.Situacao = regraComissaoEvs.Situacao;
            novoHistorico.Inicio = DateTime.Now;
            novoHistorico.Status = Situacao.Ativo;
            if(regraComissaoEvs.Situacao == SituacaoRegraComissao.Vencido)
            {
                novoHistorico.ResponsavelTermino = responsavel;
                novoHistorico.Termino = DateTime.Now;
                novoHistorico.Status = Situacao.Suspenso;
            }

            _historicoRegraComissaoRepository.Save(novoHistorico);

            return novoHistorico;
        }
        public bool FinalizarHistóricoAtual(RegraComissaoEvs regraComissaoEvs, UsuarioPortal responsavel)
        {
            HistoricoRegraComissao historico = _historicoRegraComissaoRepository.AtualDaRegra(regraComissaoEvs.Id);
            try
            {
                if (historico != null)
                {
                    historico.Termino = DateTime.Now;
                    historico.Status = Situacao.Suspenso;
                    historico.ResponsavelTermino = responsavel;
                    _historicoRegraComissaoRepository.Save(historico);
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                return false;
            }
        }
        public void DeletarHistorico(long idRegra)
        {
            var historico = _historicoRegraComissaoRepository.FindByIdRegraComissaoEvs(idRegra);

            if (historico.IsEmpty())
            {
                return;
            }

            foreach (var hist in historico)
            {
                _historicoRegraComissaoRepository.Delete(hist);
            }
        }
    }
}
