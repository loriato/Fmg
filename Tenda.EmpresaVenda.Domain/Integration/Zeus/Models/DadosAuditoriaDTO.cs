using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class DadosAuditoriaDTO
    {
        public string Usuario { get; set; }
        public string Sistema { get; set; }
        public DateTime DataTransacao { get; set; }

        public override string ToString()
        {
            return Usuario + " | " + Sistema + " | " + DataTransacao;
        }
    }
}
