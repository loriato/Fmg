using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class LojaDto
    {
        public RegionaisRepository _regionaisRepository { get; set; }
        public string IdLoja { get; set; }
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string SapId { get; set; }
        public List<long> Regional { get; set; }
        public DateTime DataIntegracao { get; set; }
        public Situacao Situacao { get; set; }

        public List<long> IdsSituacoes { get; set; }
        public DateTime? DataIntegracaoDe { get; set; }
        public DateTime? DataIntegracaoAte { get; set; }


        public Loja ToModel()
        {
            Loja loja = new Loja
            {
                Nome = Nome,
                NomeFantasia = NomeFantasia,
                SapId = SapId.ToUpper(),
                Regional = _regionaisRepository.FindById(Regional[0]),
                DataIntegracao = DataIntegracao,
                Situacao = Situacao
            };
            return loja;
        }
    }
}
