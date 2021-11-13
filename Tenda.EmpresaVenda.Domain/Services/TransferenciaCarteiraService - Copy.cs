using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class TransferenciaCarteiraService : BaseService
    {
        private TransferenciaCarteiraRepository _transferenciaCarteiraRepository { get; set; }

        public void Salvar(PreProposta preProposta,UsuarioPortal viabilizadorOrigem, UsuarioPortal viabilizadorDestino)
        {
            var transferencia = new TransferenciaCarteira();

            transferencia.PreProposta = preProposta;
            transferencia.ViabilizadorOrigem = viabilizadorOrigem;
            transferencia.ViabilizadorDestino = viabilizadorDestino;

            _transferenciaCarteiraRepository.Save(transferencia);
        }
    }
}
