using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Banco
{
    public class BancoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public Situacao Situacao { get; set; }

        public BancoDto FromDomain(Tenda.Domain.EmpresaVenda.Models.Banco model)
        {
            Id = model.Id;
            Codigo = model.Codigo;
            Nome = model.Nome;
            Sigla = model.Sigla;
            Situacao = model.Situacao;

            return this;
        }

        public override string ToString()
        {
            return Codigo + " - " + Sigla + " - " + Nome;
        }
    }
}
