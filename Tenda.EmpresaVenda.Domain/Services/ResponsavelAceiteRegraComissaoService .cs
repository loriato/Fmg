using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ResponsavelAceiteRegraComissaoService : BaseService
    {
        ResponsavelAceiteRegraComissaoRepository _responsavelAceiteRegraComissaoRepository { get; set; }
        public void SuspenderResposaveisAtivos(long IdEmpresaVenda)
        {
            var responsaveisAtivos = _responsavelAceiteRegraComissaoRepository.FindResposaveisAtivos(IdEmpresaVenda);
            foreach (var responsavel in responsaveisAtivos)
            {
                SuspenderResponsavelAtivo(responsavel);
            }
        }
        public void SuspenderResponsavelAtivo(ResponsavelAceiteRegraComissao responsavel)
        {
            responsavel.Termino = DateTime.Now;
            responsavel.Situacao = Situacao.Suspenso;
            _responsavelAceiteRegraComissaoRepository.Save(responsavel);
        }
    }
}