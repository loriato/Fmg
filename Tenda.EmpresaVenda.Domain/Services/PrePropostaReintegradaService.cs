using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PrePropostaReintegradaService : BaseService
    {
        public virtual PrePropostaReintegradaRepository _prePropostaReintegradaRepository { get; set; }

        public void SalvarDadosReintegracao(PreProposta preProposta)
        {
            PrePropostaReintegrada dto = new PrePropostaReintegrada();

            dto.PreProposta = preProposta;
            dto.IdSuat = preProposta.IdSuat;
            dto.IdTorre = preProposta.IdTorre;
            dto.IdUnidadeSuat = preProposta.IdUnidadeSuat;
            dto.NomeTorre = preProposta.NomeTorre;
            dto.ObservacaoTorre = preProposta.ObservacaoTorre;
            dto.PassoAtualSuat = preProposta.PassoAtualSuat;
            dto.IdentificadorUnidadeSuat = preProposta.IdentificadorUnidadeSuat;
            dto.IdBreveLancamento = preProposta.BreveLancamento.Id;

            _prePropostaReintegradaRepository.Save(dto);
        }
    }
}
