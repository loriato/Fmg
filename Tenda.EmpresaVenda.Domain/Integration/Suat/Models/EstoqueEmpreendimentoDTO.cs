using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class EstoqueEmpreendimentoDTO
    {
        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string Divisao { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual decimal QtdeDisponivel { get; set; }
        public virtual decimal QtdeReservado { get; set; }
        public virtual decimal QtdeVendido { get; set; }
        public virtual string Caracteristicas { get; set; }
        public virtual decimal QtdeUnidades { get; set; }
        public virtual decimal MenorM2 { get; set; }
        public virtual decimal MaiorM2 { get; set; }
        public virtual DateTime PrevisaoEntrega { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual string TipologiaUnidade { get; set; }
    }
}
