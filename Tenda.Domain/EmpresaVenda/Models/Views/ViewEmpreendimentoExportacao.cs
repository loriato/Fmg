using System;
using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewEmpreendimentoExportacao: BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Divisao { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual string Regional { get; set; }
        public virtual String CodigoEmpresa { get; set; }
        public virtual String NomeEmpresa { get; set; }
        public virtual String CNPJ { get; set; }
        public virtual String Mancha { get; set; }
        public virtual String RegistroIncorporacao { get; set; }
        public virtual DateTime? DataLancamento { get; set; }
        public virtual DateTime? PrevisaoEntrega { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Estado { get; set; }
        public virtual bool DisponibilizarCatalogo { get; set; }
        public virtual bool DisponivelVenda { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
