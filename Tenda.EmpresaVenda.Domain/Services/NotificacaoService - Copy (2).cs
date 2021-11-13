using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class NotificacaoService : BaseService
    {
        private NotificacaoRepository _notificacaoRepository { get; set; }

        public void MarcarComoLidas(long idUsuario, DateTime? ultimaLeitura, DateTime horarioLeitura)
        {
            var notificacoes = _notificacaoRepository.NaoLidasDoUsuario(idUsuario, ultimaLeitura, horarioLeitura);

            foreach (var notificacao in notificacoes)
            {
                notificacao.DataLeitura = horarioLeitura;
                _notificacaoRepository.Save(notificacao);
            }
            _notificacaoRepository.Flush();
        }
    }
}
