using Europa.Data.Model;
using System;
using Europa.Extensions;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewProduto : BaseEntity
    {
        public virtual long? IdEmpreendimento { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Cep { get; set; }
        public virtual bool DisponivelCatalogo { get; set; }
        public virtual bool EmpreendimentoVerificado { get; set; }
        public virtual string Informacoes { get; set; }
        public virtual string FichaTecnica { get; set; }
        public virtual int? Sequencia { get; set; }
        public virtual long? IdImagemPrincipal { get; set; }
        
        //Não mapeado
        public virtual string UrlImagemPrincipal { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

        public virtual string FormatarEnderecoCard()
        {
            if (Logradouro.IsEmpty()) return Numero ?? "";
            return !Numero.IsEmpty() ? $"{Logradouro}, {Numero}" : Logradouro;
        }
    }
}
