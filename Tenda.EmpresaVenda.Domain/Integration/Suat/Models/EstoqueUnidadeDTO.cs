using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class EstoqueUnidadeDTO
    {
        public virtual long IdEmpreendimento {get;set;}
        public virtual string NomeEmpreendimento {get;set;}
        public virtual string Divisao {get;set;}
        public virtual long IdTorre {get;set;}
        public virtual string NomeTorre {get;set;}
        public virtual long IdUnidade {get;set;}
        public virtual string IdSapTorre {get;set;}
        public virtual string NomeUnidade {get;set;}
        public virtual string Caracteristicas {get;set;}
        public virtual decimal Metragem {get;set;}
        public virtual string Andar {get;set;}
        public virtual string Prumada {get;set;}
        public virtual string IdSapUnidade {get;set;}
        public virtual DateTime? DataEntregaObra {get;set;}
        public virtual string TipologiaUnidade {get;set;}
        public virtual bool Disponivel {get;set;}
        public virtual bool Reservada {get;set;}
        public virtual bool Vendida {get;set;}
    }
}
